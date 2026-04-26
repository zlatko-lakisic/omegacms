using MD.CMS.BusinessLogic.WebApi.Core.Caching.Filters;
using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using MD.Tools.Helpers.Core.Caching;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OmegaInvalidateCacheAttribute : OmegaBaseCacheAttribute, IFilterFactory, IOrderedFilter
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsReusable => true;
        public int Order { get; set; }
        #endregion

        #region Methods
        public OmegaInvalidateCacheAttribute(string cacheName, [CallerFilePath] string callerFilePath = null)
        {
            if (!string.IsNullOrEmpty(callerFilePath)) {
                string callerTypeName = Path.GetFileNameWithoutExtension(callerFilePath);
                SetOutputCacheName(cacheName, callerTypeName);
            }
            else
            {
                SetOutputCacheName(cacheName);
            }
        }
        public OmegaInvalidateCacheAttribute(string cacheName, Type type)
        {
            SetOutputCacheName(cacheName, type.Name);
        }

        private void SetOutputCacheName(string cacheName, string memberName = default)
        {
            if (!string.IsNullOrEmpty(memberName))
            {
                memberName = memberName.Replace("BaseContentController", "ContentController").Replace("Controller", string.Empty);
                OutputCacheName = $"{memberName} {cacheName}";
            }
            else
            {
                OutputCacheName = cacheName;
            }
        }
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override OmegaCacheProfile GetClientCacheProfile()
        {
            OmegaCacheProfile profile = base.GetClientCacheProfile();
            OmegaCacheSettings.Controller controller = Settings.Default.OutputCacheSettings.GetController(ControllerName);
            if (controller != null)
            {
                profile.Client = new OmegaCacheProfile.CacheProfile()
                {
                    Enabled = controller.Enabled && controller.ClientTimeSpan > 0,
                    Duration = controller.ClientTimeSpan
                };
                profile.Server = new OmegaCacheProfile.CacheProfile()
                {
                    Enabled = controller.Enabled && controller.ServerTimeSpan > 0,
                    Duration = controller.ServerTimeSpan
                };
                profile.CachingProvider = OmegaCacheController.GetNewInstance().CachingProviders[!string.IsNullOrEmpty(controller.CacheProvider) ? controller.CacheProvider : Settings.Default.OutputCacheSettings.DefaultCacheProvider];
                OmegaCacheSettings.Controller.Method method = controller.GetMethod(MethodName);
                profile.Client = new OmegaCacheProfile.CacheProfile()
                {
                    Enabled = method != null && method.Enabled && method.ClientTimeSpan > 0,
                    Duration = method != null ? method.ClientTimeSpan : default
                };
                profile.Server = new OmegaCacheProfile.CacheProfile()
                {
                    Enabled = method != null && method.Enabled && method.ServerTimeSpan > 0,
                    Duration = method != null ? method.ServerTimeSpan : default
                };
                if (method != null && !string.IsNullOrEmpty(method.CacheProvider))
                {
                    profile.CachingProvider = OmegaCacheController.GetNewInstance().CachingProviders[!string.IsNullOrEmpty(method.CacheProvider) ? method.CacheProvider : Settings.Default.OutputCacheSettings.DefaultCacheProvider];
                }
                else
                {
                    profile.CachingProvider = OmegaCacheController.GetNewInstance().CachingProviders[!string.IsNullOrEmpty(controller.CacheProvider) ? controller.CacheProvider : Settings.Default.OutputCacheSettings.DefaultCacheProvider];
                }
            }

            return profile;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            return new OmegaInvalidateCacheFilter(GetClientCacheProfile(), loggerFactory);
        }
        #endregion
    }
}