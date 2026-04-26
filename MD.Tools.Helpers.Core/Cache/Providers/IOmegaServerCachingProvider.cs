using MD.Tools.Helpers.Core.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MD.Tools.Helpers.Core.Caching.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOmegaServerCachingProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<OmegaCachingObject>> GetAllFromCache();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Task<OmegaCachingObject> GetFromCacheAsync(string cacheKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheObject"></param>
        /// <returns></returns>
        Task<bool> StoreToCacheAsync(string cacheKey, OmegaCachingObject cacheObject);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regexPattern"></param>
        /// <returns></returns>
        Task<bool> InvalidateCacheAsync(string regexPattern);
        /// <summary>
        /// 
        /// </summary>
        string ProviderName { get; }
        /// <summary>
        /// 
        /// </summary>
        IConfigParsable Config { get; }
    }
}
