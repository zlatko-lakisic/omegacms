using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using System.Collections.Generic;
using MD.Tools.Helpers.Core.Caching;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Linq;
using MD.CMS.BusinessLogic.WebApi.Core.Caching;
using System.Web;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [TokenAuth]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "General")]
    public class CacheController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetDataCache()
        {
            Dictionary<string, IEnumerable<OmegaCachingObject>> result = new Dictionary<string, IEnumerable<OmegaCachingObject>>();

            List<string> providers = new List<string>();
            DataCacheController.Instance.AddProvidersFromSettings(providers, MD.Tools.BaseDataAccess.Plugins.Core.Properties.Settings.Default.DataCacheSettings);
            AddProvidersFromSettings(providers, MD.CMS.BusinessLogic.WebApi.Core.Properties.Settings.Default.OutputCacheSettings);

            return Ok((await Task.WhenAll(providers.Select(async provider => new KeyValuePair<string, IEnumerable<OmegaCachingObject>>(provider, await OmegaCacheController.Instance.GetAllCache(provider))))).
                                Select(obj => new CacheResponse() { ProviderName = obj.Key, CacheObjects = obj.Value }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providers"></param>
        /// <param name="settings"></param>
#pragma warning disable CA1822 // Mark members as static
        private void AddProvidersFromSettings(List<string> providers, OmegaCacheSettings settings)
#pragma warning restore CA1822 // Mark members as static
        {
            if (!providers.Any(provider => string.CompareOrdinal(settings.DefaultCacheProvider, provider).Equals(default)))
            {
                providers.Add(settings.DefaultCacheProvider);
            }

            foreach (OmegaCacheSettings.Controller controller in settings.Controllers)
            {
                if (!string.IsNullOrEmpty(controller.CacheProvider) && !providers.Any(provider => string.CompareOrdinal(controller.CacheProvider, provider).Equals(default)))
                {
                    providers.Add(controller.CacheProvider);
                }

                foreach (OmegaCacheSettings.Controller.Method method in controller.Methods)
                {
                    if (!string.IsNullOrEmpty(method.CacheProvider) && !providers.Any(provider => string.CompareOrdinal(method.CacheProvider, provider).Equals(default)))
                    {
                        providers.Add(method.CacheProvider);
                    }
                }
            }
        }

        [HttpGet]
        [Route("[action]/{id?}/{id2?}")]
        public async Task<IActionResult> InvalidateDataCache(string id, string id2)
        {
            await DataCacheController.GetNewInstance().Caller(await GetLoggedOnUser()).InvalidateDataCacheAsync(id, HttpUtility.UrlDecode(id2));
            return Ok();
        }
    }
}
