using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Caching;
using MD.Tools.BaseDataAccess.Plugins.Core.Caching;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DataCacheController : BaseController<DataCacheController>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<KeyValuePair<string, IEnumerable<OmegaCachingObject>>>> GetAllDataCacheAsync()
        {
            OmegaCacheSettings dataCacheSettings = MD.Tools.BaseDataAccess.Plugins.Core.Properties.Settings.Default.DataCacheSettings;
            Dictionary<string, IEnumerable<OmegaCachingObject>> result = new Dictionary<string, IEnumerable<OmegaCachingObject>>();

            List<string> providers = new List<string>();
            AddProvidersFromSettings(providers, MD.Tools.BaseDataAccess.Plugins.Core.Properties.Settings.Default.DataCacheSettings);

            return await Task.WhenAll(providers.Select(async (provider) => {
                return new KeyValuePair<string, IEnumerable<OmegaCachingObject>>(provider, await OmegaCacheController.Instance.GetAllCache(provider));
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providers"></param>
        /// <param name="settings"></param>
#pragma warning disable CA1822 // Mark members as static
        public void AddProvidersFromSettings(List<string> providers, OmegaCacheSettings settings)
#pragma warning restore CA1822 // Mark members as static
        {
            if (!providers.Any(provider => string.CompareOrdinal(settings.DefaultCacheProvider, provider).Equals(default)))
            {
                providers.Add(settings.DefaultCacheProvider);
            }

            foreach (OmegaCacheSettings.Entity entity in settings.Entities)
            {
                if (!string.IsNullOrEmpty(entity.CacheProvider) && !providers.Any(provider => string.CompareOrdinal(entity.CacheProvider, provider).Equals(default)))
                {
                    providers.Add(entity.CacheProvider);
                }

                foreach (OmegaCacheSettings.Entity.Method method in entity.Methods)
                {
                    if (!string.IsNullOrEmpty(method.CacheProvider) && !providers.Any(provider => string.CompareOrdinal(method.CacheProvider, provider).Equals(default)))
                    {
                        providers.Add(method.CacheProvider);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
#pragma warning disable CA1822 // Mark members as static
        public async Task<bool> InvalidateDataCacheAsync(string providerName, string cacheKey)
#pragma warning restore CA1822 // Mark members as static
        {
            if (string.IsNullOrEmpty(providerName))
            {
                throw new System.ArgumentException($"'{nameof(providerName)}' cannot be null or empty", nameof(providerName));
            }

            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new System.ArgumentException($"'{nameof(cacheKey)}' cannot be null or empty", nameof(cacheKey));
            }

            return await OmegaCacheController.Instance.InvalidateCacheAsync(OmegaCacheController.Instance.CachingProviders[providerName], cacheKey);
        }
    }
}