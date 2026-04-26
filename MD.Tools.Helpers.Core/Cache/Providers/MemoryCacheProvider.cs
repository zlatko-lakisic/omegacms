using MD.Tools.Helpers.Core.Config;
using MD.Tools.Helpers.Core.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MD.Tools.Helpers.Core.Caching.Providers
{
    /// <summary>
    /// 
    /// </summary>
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class MemoryCacheProvider : IOmegaServerCachingProvider
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private ConcurrentDictionary<string, OmegaCachingObject> _cacheStore = new ConcurrentDictionary<string, OmegaCachingObject>();
        private SemaphoreSlim @lock = new SemaphoreSlim(1);

        /// <summary>
        /// 
        /// </summary>
        public string ProviderName => "MemoryCacheProvider";

        /// <summary>
        /// 
        /// </summary>
        public IConfigParsable Config => null;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IEnumerable<OmegaCachingObject>> GetAllFromCache()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                return _cacheStore.Select(c => c.Value);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(MemoryCacheProvider).Log(error);
            }
            return new List<OmegaCachingObject>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<OmegaCachingObject> GetFromCacheAsync(string cacheKey)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentException($"'{nameof(cacheKey)}' cannot be null or empty", nameof(cacheKey));
            }

            try
            {
                if (_cacheStore.ContainsKey(cacheKey))
                {
                    return _cacheStore[cacheKey];
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(MemoryCacheProvider).Log(error);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regexPattern"></param>
        /// <returns></returns>
        public async Task<bool> InvalidateCacheAsync(string regexPattern)
        {
            if (string.IsNullOrEmpty(regexPattern))
            {
                throw new ArgumentException($"'{nameof(regexPattern)}' cannot be null or empty", nameof(regexPattern));
            }

            bool resultValue = false;
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            await @lock.WaitAsync();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            try
            {
                IEnumerable<string> results = (from result in _cacheStore
                                               where Regex.Match(result.Key, regexPattern, RegexOptions.IgnoreCase).Success
                                               select result).Select(res => res.Key);
                if (results.Any())
                {
                    foreach (string key in results)
                    {
                        OmegaCachingObject obj = new OmegaCachingObject();
                        _cacheStore.TryRemove(key, out obj);
                    }
                    resultValue = true;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(MemoryCacheProvider).Log(error);
            }
            finally
            {
                @lock.Release();
            }
            return resultValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheObject"></param>
        /// <returns></returns>
        public async Task<bool> StoreToCacheAsync(string cacheKey, OmegaCachingObject cacheObject)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentException($"'{nameof(cacheKey)}' cannot be null or empty", nameof(cacheKey));
            }

            if (cacheObject is null)
            {
                throw new ArgumentNullException(nameof(cacheObject));
            }

            bool result = false;
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            await @lock.WaitAsync();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            try
            {
                if (_cacheStore.ContainsKey(cacheKey))
                {
                    _cacheStore[cacheKey] = cacheObject;
                    result = true;
                }
                else
                {
                    _cacheStore.TryAdd(cacheKey, cacheObject);
                    result = true;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(MemoryCacheProvider).Log(error);
            }
            finally
            {
                @lock.Release();
            }
            return result;
        }
    }
}
