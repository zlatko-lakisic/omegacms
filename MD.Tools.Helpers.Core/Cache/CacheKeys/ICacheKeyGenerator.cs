using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Utilities.Encoders;
using System.Net.Http.Headers;

namespace MD.Tools.Helpers.Core.Caching.CacheKeys
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICacheKeyGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseKey"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string MakeCacheKey(string baseKey, params string[] parameters);
    }
}
