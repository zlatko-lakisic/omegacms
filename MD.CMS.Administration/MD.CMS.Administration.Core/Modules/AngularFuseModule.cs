using MD.CMS.Administration.Core.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace MD.CMS.Administration.Core.Modules
{
    public class AngularFuseModule
    {
        private readonly RequestDelegate _next;

        public AngularFuseModule(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context_BeginRequest(context);

            await _next.Invoke(context);
        }

        private void context_BeginRequest(HttpContext context)
        {
            ResourceSet resourceSet = Resources.SupportedLanguages.ResourceManager.GetResourceSet(CultureInfo.GetCultureInfo(BusinessLogic.Core.Properties.Settings.Default.DefaultLcid), true, true);
            IEnumerable<DictionaryEntry> languages = resourceSet.Cast<DictionaryEntry>();
            string selectedLanguage = context.Request.Path.Value.Trim('/').Split('/').FirstOrDefault();

            if(!languages.Select(item => item.Key.ToString()).Contains(selectedLanguage))
            {
                selectedLanguage = languages.First().Key.ToString();
            }

            if (
                !Settings.Default.AngularFuseModuleExclusionList.Cast<string>().Any(exclusion => context.Request.Path.ToString().ToLowerInvariant().Contains(exclusion.ToLowerInvariant())) &&
                (
                    (!context.Request.Path.ToString().Equals("/") && context.Request.Path.ToString().EndsWith("/")) ||
                    (!context.Request.Path.ToString().Equals("/") && string.IsNullOrEmpty(Path.GetExtension(context.Request.Path.ToString())))
                )
            )
            {
                List<KeyValuePair<string, StringValues>> queryStrings = context.Request.Query.AsEnumerable().ToList();
                queryStrings.Add(new KeyValuePair<string, StringValues>("lang", new StringValues(selectedLanguage)));

                string url = Settings.Default.AngularFuseModuleRedirectTo;
                if (queryStrings.Count > 0)
                {
                    url = string.Format("{0}?{1}", url, string.Join("&", queryStrings.Select(item => string.Format("{0}={1}", item.Key, item.Value.ToString()))));
                }
                context.Request.Path = url;
            }
        }
    }
}
