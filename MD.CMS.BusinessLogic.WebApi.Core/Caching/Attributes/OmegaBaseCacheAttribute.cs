using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using MD.Tools.Helpers.Core.Caching;
using System;
using System.Linq;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes
{
    public abstract class OmegaBaseCacheAttribute : Attribute
    {
        #region Attributes
        #endregion

        #region Properties
        public virtual string CacheProvider { get; set; }
        /// <summary>
        /// Output cache name
        /// </summary>
        public virtual string OutputCacheName { get; set; }
        /// <summary>
        /// Client Output cache name
        /// </summary>
        internal virtual string ClientOutputCacheName
        {
            get
            {
                return $"Client-{OutputCacheName}";
            }
        }
        /// <summary>
        /// Server Output cache name
        /// </summary>
        internal virtual string ServerOutputCacheName
        {
            get
            {
                return $"Server-{OutputCacheName}";
            }
        }
        internal virtual string ControllerName
        {
            get
            {
                return OutputCacheName.Split(' ').FirstOrDefault();
            }
        }
        internal virtual string MethodName
        {
            get
            {
                return OutputCacheName.Split(' ').LastOrDefault();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual OmegaCacheProfile GetClientCacheProfile()
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
                CachingProvider = OmegaCacheController.GetNewInstance().CachingProviders[!string.IsNullOrEmpty(CacheProvider) ? CacheProvider : Settings.Default.OutputCacheSettings.DefaultCacheProvider],
                ControllerName = ControllerName,
                MethodName = MethodName
            };

            return profile;
        }
        #endregion
    }
}
