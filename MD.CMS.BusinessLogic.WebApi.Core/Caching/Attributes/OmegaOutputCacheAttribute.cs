using System.Linq;
using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Filters;
using MD.Tools.Helpers.Core.Caching;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class OmegaOutputCacheAttribute : OmegaBaseCacheAttribute, IFilterFactory, IOrderedFilter
    {
        #region Helpers
        /// <summary>
        /// Output cache type enum
        /// </summary>
        public enum CacheType
        {
            None,
            Controller,
            Method
        }
        #endregion

        #region Attributes
        private int _clientDuration;
        private bool _clientEnabled;
        private int _serverDuration;
        private bool _serverEnabled;
        #endregion

        #region Properties
        public bool IsReusable => true;
        public int Order { get; set; }
        /// <summary>
        /// Output cache type
        /// </summary>
        public CacheType OutputCacheType { get; set; }
        public bool ClientEnabled { get => _clientEnabled; set => _clientEnabled = value; }
        public int ClientDuration { get => _clientDuration; set => _clientDuration = value; }
        public int ServerDuration { get => _serverDuration; set => _serverDuration = value; }
        public bool ServerEnabled { get => _serverEnabled; set => _serverEnabled = value; }
        internal override string ControllerName
        {
            get
            {
                return OutputCacheType == CacheType.Controller ? OutputCacheName : OutputCacheName.Split(' ').FirstOrDefault();
            }
        }
        internal override string MethodName
        {
            get
            {
                return OutputCacheType == CacheType.Controller ? string.Empty : OutputCacheName.Split(' ').LastOrDefault();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override OmegaCacheProfile GetClientCacheProfile()
        {
            OmegaCacheProfile profile = base.GetClientCacheProfile();
            OmegaCacheSettings.Controller controller = Settings.Default.OutputCacheSettings.GetController(ControllerName);
            if (controller != null)
            {
                switch (OutputCacheType)
                {
                    case CacheType.Controller:
                        profile.Client = new OmegaCacheProfile.CacheProfile()
                        {
                            Enabled = _clientEnabled || controller.Enabled && controller.ClientTimeSpan > 0,
                            Duration = _clientDuration.Equals(default) ? controller.ClientTimeSpan : default
                        };
                        profile.Server = new OmegaCacheProfile.CacheProfile()
                        {
                            Enabled = _serverEnabled || controller.Enabled && controller.ServerTimeSpan > 0,
                            Duration = _serverDuration.Equals(default) ? controller.ServerTimeSpan : default
                        };
                        profile.CachingProvider = OmegaCacheController.GetNewInstance().CachingProviders[!string.IsNullOrEmpty(controller.CacheProvider) ? controller.CacheProvider : Settings.Default.OutputCacheSettings.DefaultCacheProvider];
                        break;
                    case CacheType.Method:
                        OmegaCacheSettings.Controller.Method method = controller.GetMethod(MethodName);
                        profile.Client = new OmegaCacheProfile.CacheProfile()
                        {
                            Enabled = method != null && (_clientEnabled || method.Enabled && method.ClientTimeSpan > 0),
                            Duration = method != null && _clientDuration.Equals(default) ? method.ClientTimeSpan : default
                        };
                        profile.Server = new OmegaCacheProfile.CacheProfile()
                        {
                            Enabled = method != null && (_serverEnabled || method.Enabled && method.ServerTimeSpan > 0),
                            Duration = method != null && _serverDuration.Equals(default) ? method.ServerTimeSpan : default
                        };
                        if (method != null && !string.IsNullOrEmpty(method.CacheProvider))
                        {
                            profile.CachingProvider = OmegaCacheController.GetNewInstance().CachingProviders[!string.IsNullOrEmpty(method.CacheProvider) ? method.CacheProvider : Settings.Default.OutputCacheSettings.DefaultCacheProvider];
                        }
                        else
                        {
                            profile.CachingProvider = OmegaCacheController.GetNewInstance().CachingProviders[!string.IsNullOrEmpty(controller.CacheProvider) ? controller.CacheProvider : Settings.Default.OutputCacheSettings.DefaultCacheProvider];
                        }
                        break;
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

            return new OmegaOutputCacheFilter(GetClientCacheProfile(), loggerFactory);
        }
        #endregion
    }
}