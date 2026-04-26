using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Linq;

namespace MD.Tools.Helpers.Core.Caching.CacheKeys
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultCacheKeyGenerator : ICacheKeyGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        protected virtual string MakeBaseKey(string controllerName, string actionName)
        {
            return $"{controllerName}-{actionName}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual string FormatParameters(string separator, params string[] parameters)
        {
            if(parameters == null || !parameters.Any())
            {
                return string.Empty;
            }

            return string.Join(separator, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseKey"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual string MakeCacheKey(string baseKey, params string[] parameters)
        {
            string key = $"{baseKey}";
            string param = FormatParameters("-", parameters);
            return string.Format(CultureInfo.InvariantCulture, "{0}-{1}", key, param);
        }
    }
}
