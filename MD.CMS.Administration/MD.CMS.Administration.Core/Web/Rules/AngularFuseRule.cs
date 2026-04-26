using MD.CMS.Administration.Core.Properties;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;

namespace MD.CMS.Administration.Core.Web.Rules
{
    public class AngularFuseRule : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            ResourceSet resourceSet = Resources.SupportedLanguages.ResourceManager.GetResourceSet(CultureInfo.GetCultureInfo(BusinessLogic.Core.Properties.Settings.Default.DefaultLcid), true, true);
            IEnumerable<DictionaryEntry> languages = resourceSet.Cast<DictionaryEntry>();
            string selectedLanguage = context.HttpContext.Request.Path.Value.Trim('/').Split('/').FirstOrDefault();
            if (!languages.Select(item => item.Key.ToString()).Contains(selectedLanguage))
            {
                selectedLanguage = languages.First().Key.ToString();
            }

            if (
                !Settings.Default.AngularFuseModuleExclusionList.Cast<string>().Any(exclusion => context.HttpContext.Request.Path.ToString().ToLowerInvariant().Contains(exclusion.ToLowerInvariant())) &&
                (
                    (!context.HttpContext.Request.Path.ToString().Equals("/") && context.HttpContext.Request.Path.ToString().EndsWith("/")) ||
                    (!context.HttpContext.Request.Path.ToString().Equals("/") && string.IsNullOrEmpty(Path.GetExtension(context.HttpContext.Request.Path.ToString())))
                )
            )
            {
                List<KeyValuePair<string, StringValues>> queryStrings = context.HttpContext.Request.Query.AsEnumerable().ToList();
                queryStrings.Add(new KeyValuePair<string, StringValues>("lang", new StringValues(selectedLanguage)));

                string url = string.Empty;
                if (queryStrings.Count > 0)
                {
                    url = string.Format("?{0}", string.Join("&", queryStrings.Select(item => string.Format("{0}={1}", item.Key, item.Value.ToString()))));
                }
            }
        }
    }
}
