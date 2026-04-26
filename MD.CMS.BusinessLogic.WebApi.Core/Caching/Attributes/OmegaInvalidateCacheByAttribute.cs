using MD.CMS.BusinessLogic.WebApi.Core.Caching.Filters;
using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using MD.Tools.Helpers.Core.Caching;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OmegaInvalidateCacheByAttribute : OmegaBaseCacheAttribute, IFilterFactory, IOrderedFilter
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsReusable => true;
        public int Order { get; set; }
        #endregion

        #region Methods
        private OmegaCacheProfile CreateCacheProfile(string cacheProvider, string controllerName, string methodName = null)
        {
            OmegaCacheProfile profile = new OmegaCacheProfile()
            {
                Client = new OmegaCacheProfile.CacheProfile()
                {
                    Enabled = false,
                    Duration = default
                },
                Server = new OmegaCacheProfile.CacheProfile()
                {
                    Enabled = false,
                    Duration = default
                },
                CachingProvider = OmegaCacheController.GetNewInstance().CachingProviders[!string.IsNullOrEmpty(cacheProvider) ? cacheProvider : Settings.Default.OutputCacheSettings.DefaultCacheProvider],
                ControllerName = controllerName,
                MethodName = methodName
            };

            return profile;
        }
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            OmegaCacheProfile baseProfile = GetClientCacheProfile();

            List<OmegaCacheProfile> cacheProfilesToClear = new List<OmegaCacheProfile>();
            if (Settings.Default.OutputCacheSettings != null && Settings.Default.OutputCacheSettings.Controllers != null)
            {
                foreach (OmegaCacheSettings.Controller controller in Settings.Default.OutputCacheSettings.Controllers)
                {
                    if (controller.InvalidateBy != null && controller.InvalidateBy.Any(cacheName => string.CompareOrdinal(cacheName, OutputCacheName).Equals(default)))
                    {
                        cacheProfilesToClear.Add(CreateCacheProfile(controller.CacheProvider, controller.Name));
                    }

                    if (controller.Methods != null)
                    {
                        foreach (OmegaCacheSettings.Controller.Method method in controller.Methods)
                        {
                            if (method.InvalidateBy != null && method.InvalidateBy.Any(cacheName => string.CompareOrdinal(cacheName, OutputCacheName).Equals(default)))
                            {
                                cacheProfilesToClear.Add(CreateCacheProfile(method.CacheProvider, controller.Name, method.Name));
                            }
                        }
                    }
                }
            }

            return new OmegaIvalidateCacheByFilter(cacheProfilesToClear, loggerFactory);
        }
        #endregion
    }
}