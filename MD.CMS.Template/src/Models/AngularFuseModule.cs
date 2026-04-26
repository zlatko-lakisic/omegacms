using MD.CMS.Template.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web;

namespace MD.CMS.Template.Modules
{
    public class AngularFuseModule : IHttpModule
    {
        public void Dispose()
        {
            //nothing
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            if (
                !Settings.Default.AngularFuseModuleExclusionList.Cast<string>().Any(exclusion => context.Request.Url.LocalPath.ToLowerInvariant().Contains(exclusion.ToLowerInvariant())) &&
                (
                    (!context.Request.Url.LocalPath.Equals("/") && context.Request.Url.LocalPath.EndsWith("/")) || 
                    (!context.Request.Url.LocalPath.Equals("/") && string.IsNullOrEmpty(Path.GetExtension(context.Request.FilePath)))
                )
            )
            {
                string url = Settings.Default.AngularFuseModuleRedirectTo;
                if (context.Request.QueryString.Count > 0)
                {
                    url = string.Format("{0}?{1}", url, string.Join("&", context.Request.QueryString.AllKeys.Select(key => string.Format("{0}={1}", key, context.Request.QueryString[key]))));
                }
                context.RewritePath(url);
            }
        }
    }
}
