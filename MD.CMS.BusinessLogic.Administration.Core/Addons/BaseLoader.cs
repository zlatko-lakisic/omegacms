using MD.CMS.BusinessLogic.Core;
using MD.Tools.Helpers.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Administration.Core.Addons
{
    public abstract class BaseLoader
    {
        private readonly RequestDelegate _next;
        protected List<IAdminByte> _filesToIntercept;
        protected List<IAdminJavaScript> _scriptsToIntercept;
        protected List<IAdminHtml> _htmlToIntercept;
        protected List<IAdminCss> _cssToIntercept;

        public BaseLoader()
        {
            InitLists();
        }

        public BaseLoader(RequestDelegate next)
        {
            _next = next;
            InitLists();
        }

        public async Task Invoke(HttpContext context)
        {
            InitLists();
            if (ShouldProcess(context))
            {
                await Process(context);
                return;
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        void InitLists()
        {
            if (_filesToIntercept == null)
            {
                _filesToIntercept = new List<IAdminByte>();
            }
            if (_scriptsToIntercept == null)
            {
                _scriptsToIntercept = new List<IAdminJavaScript>();
            }
            if (_htmlToIntercept == null)
            {
                _htmlToIntercept = new List<IAdminHtml>();
            }
            if (_cssToIntercept == null)
            {
                _cssToIntercept = new List<IAdminCss>();
            }
        }

        bool ShouldProcess(HttpContext context)
        {
            foreach (IAdminByte file in _filesToIntercept)
            {
                if (context.Request.Path.Value.ToLowerInvariant().EndsWith(file.Url.ToLowerInvariant()))
                {
                    return true;
                }
            }

            foreach (IAdminCss css in _cssToIntercept)
            {
                if (context.Request.Path.Value.ToLowerInvariant().EndsWith(css.Url.ToLowerInvariant()))
                {
                    return true;
                }
            }

            foreach (IAdminJavaScript script in _scriptsToIntercept)
            {
                if (context.Request.Path.Value.ToLowerInvariant().EndsWith(script.Url.ToLowerInvariant()))
                {
                    return true;
                }
            }

            foreach (IAdminHtml html in _htmlToIntercept)
            {
                if (context.Request.Path.Value.ToLowerInvariant().EndsWith(html.Url.ToLowerInvariant()))
                {
                    return true;
                }
            }
            return false;
        }

        async Task Process(HttpContext context)
        {
            foreach (IAdminByte file in _filesToIntercept)
            {
                if (context.Request.Path.Value.ToLowerInvariant().EndsWith(file.Url.ToLowerInvariant()))
                {
                    MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue(MimeTypes.GetMimeType(string.Format("test.{0}", file.Extension)));
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = mediaType.ToString();
                    await context.Response.Body.WriteAsync(file.FileContent, 0, file.FileContent.Length);
                }
            }

            foreach (IAdminCss css in _cssToIntercept)
            {
                if (context.Request.Path.Value.ToLowerInvariant().EndsWith(css.Url.ToLowerInvariant()))
                {
                    MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("text/css");
                    mediaType.Encoding = Encoding.UTF8;
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = mediaType.ToString();
                    await context.Response.WriteAsync(css.Code);
                }
            }

            foreach (IAdminJavaScript script in _scriptsToIntercept)
            {
                if (context.Request.Path.Value.ToLowerInvariant().EndsWith(script.Url.ToLowerInvariant()))
                {
                    MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("text/javascript");
                    mediaType.Encoding = Encoding.UTF8;
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = mediaType.ToString();
                    await context.Response.WriteAsync(script.Code);
                }
            }

            foreach (IAdminHtml html in _htmlToIntercept)
            {
                if (context.Request.Path.Value.ToLowerInvariant().EndsWith(html.Url.ToLowerInvariant()))
                {
                    MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("text/html");
                    mediaType.Encoding = Encoding.UTF8;
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = mediaType.ToString();
                    await context.Response.WriteAsync(html.Code);
                }
            }
        }
    }
}
