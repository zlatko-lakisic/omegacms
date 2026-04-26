using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System.Collections.Generic;
using MD.Tools.Helpers.Core.Caching;
using System;

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
        /// 
        [Obsolete("Deprecated", true)]
        public IEnumerable<KeyValuePair<string, IEnumerable<OmegaCachingObject>>> GetAllDataCache()
        {
            return GetAllDataCacheAsync().Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [Obsolete("Deprecated", true)]
#pragma warning disable CA1822 // Mark members as static
        public bool InvalidateDataCache(string providerName, string cacheKey)
#pragma warning restore CA1822 // Mark members as static
        {
            return InvalidateDataCacheAsync(providerName, cacheKey).Result;
        }
    }
}