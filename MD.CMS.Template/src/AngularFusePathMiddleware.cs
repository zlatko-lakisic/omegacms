#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

namespace MD.CMS.Template
{
    /// <summary>
    /// Replaces the legacy IIS (AngularFuse) module used for the Fuse template.
    /// </summary>
    public sealed class AngularFusePathMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _redirectToPath;
        private readonly string[] _exclusionFragments;

        public AngularFusePathMiddleware(RequestDelegate next, IOptions<AngularFusePathOptions> options)
        {
            _next = next;
            var o = options.Value;
            _redirectToPath = (o.RedirectTo ?? "/").TrimStart('~');
            if (string.IsNullOrEmpty(_redirectToPath)) _redirectToPath = "/";
            _exclusionFragments = o.ExclusionPathFragments?.Select(s => s?.ToLowerInvariant() ?? string.Empty)
                .Where(s => s.Length > 0).ToArray() ?? Array.Empty<string>();
        }

        public Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "/";
            if (string.Equals(path, "/", StringComparison.OrdinalIgnoreCase)) return _next(context);

            var pLower = path.ToLowerInvariant();
            if (_exclusionFragments.Any(ex => pLower.Contains(ex))) return _next(context);

            if (!(path.Length > 1 && path.EndsWith('/')) && !string.IsNullOrEmpty(Path.GetExtension(path)))
                return _next(context);

            var f = context.Features.Get<IHttpRequestFeature>();
            if (f is null) return _next(context);

            f.Path = new PathString(_redirectToPath);
            f.QueryString = context.Request.QueryString.Value ?? string.Empty;
            return _next(context);
        }
    }

    public sealed class AngularFusePathOptions
    {
        public string? RedirectTo { get; set; } = "/";
        public string[]? ExclusionPathFragments { get; set; } = Array.Empty<string>();
    }
}
