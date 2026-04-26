using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MD.CMS.Administration.Core.Modules
{
    public class InterceptorRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public InterceptorRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string url = context.Request.Path.Value;

            foreach(var matcher in Properties.Settings.Default.InterceptRedirect)
            {
                string pattern = matcher.Key;
                string input = url;
                Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string rewriteUrl = matcher.Value(url);
                    context.Request.Path = rewriteUrl;
                    break;
                }
            }
            await _next.Invoke(context);
        }
    }
}
