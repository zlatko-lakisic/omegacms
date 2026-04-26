using MD.Tools.Helpers.Core.Caching.Providers;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.Serializer;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace MD.Tools.Helpers.Core.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public class OmegaCacheController : Singleton<OmegaCacheController>
    {
        #region Attributes
        private ConcurrentDictionary<string, IOmegaServerCachingProvider> _cachingProviders;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public ConcurrentDictionary<string, IOmegaServerCachingProvider> CachingProviders => _cachingProviders;
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public OmegaCacheController()
        {
            _cachingProviders = new ConcurrentDictionary<string, IOmegaServerCachingProvider>();
            AddCachingProvider(new MemoryCacheProvider());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public void AddCachingProvider(IOmegaServerCachingProvider provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            try
            {
                if (!_cachingProviders.ContainsKey(provider.ProviderName))
                {
                    _cachingProviders.TryAdd(provider.ProviderName, provider);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(OmegaCacheController).Log(error);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<T> GetFromCacheAsync<T>(IOmegaServerCachingProvider provider, string cacheKey)
        {
            string cacheValue = await GetFromCacheAsync(provider, cacheKey).ConfigureAwait(true);
            try
            {
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return OmegaJsonSerializer.DeserializeObject<T>(cacheValue);
                }
            }
            catch (JsonReaderException error)
            {
                typeof(OmegaCacheController).Log(error);
            }
            catch (JsonSerializationException error)
            {
                typeof(OmegaCacheController).Log(error);
            }
            catch (JsonWriterException error)
            {
                typeof(OmegaCacheController).Log(error);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(OmegaCacheController).Log(error);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<string> GetFromCacheAsync(IOmegaServerCachingProvider provider, string cacheKey)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentException($"'{nameof(cacheKey)}' cannot be null or empty", nameof(cacheKey));
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string result = default;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            try
            {
                OmegaCachingObject cacheObject = await provider.GetFromCacheAsync(cacheKey).ConfigureAwait(true);
                if (cacheObject != null)
                {
                    if (cacheObject.CacheTime.Add(cacheObject.Timeout) > DateTime.Now)
                    {
                        result = cacheObject.CacheValue;
                    }
                    else
                    {
                        await InvalidateCacheAsync(provider, cacheKey).ConfigureAwait(false);
                    }
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(OmegaCacheController).Log(error);
            }

#pragma warning disable CS8603 // Possible null reference return.
            return result;
#pragma warning restore CS8603 // Possible null reference return.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
#pragma warning disable CA1822 // Mark members as static
        public async Task<DateTime> GetDateFromCacheAsync(IOmegaServerCachingProvider provider, string cacheKey)
#pragma warning restore CA1822 // Mark members as static
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentException($"'{nameof(cacheKey)}' cannot be null or empty", nameof(cacheKey));
            }


            OmegaCachingObject cacheObject = await provider.GetFromCacheAsync(cacheKey).ConfigureAwait(true);
            if (cacheObject != null)
            {
                return cacheObject.CacheTime;
            }

            return DateTime.MinValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="cacheKey"></param>
        /// <param name="useWildcard"></param>
        /// <returns></returns>
#pragma warning disable CA1822 // Mark members as static
        public async Task<bool> InvalidateCacheAsync(IOmegaServerCachingProvider provider, string cacheKey, bool useWildcard = false)
#pragma warning restore CA1822 // Mark members as static
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentException($"'{nameof(cacheKey)}' cannot be null or empty", nameof(cacheKey));
            }

            try
            {
                string regexPattern = cacheKey.Replace(@"\", @"\\", true, CultureInfo.InvariantCulture).
                                            Replace(".", @"\.", true, CultureInfo.InvariantCulture).
                                            Replace("/", @"\/", true, CultureInfo.InvariantCulture).
                                            Replace("$", @"\$", true, CultureInfo.InvariantCulture);
                if (useWildcard)
                {
                    regexPattern = $"^{regexPattern}(.*)";
                }
                return await provider.InvalidateCacheAsync(regexPattern).ConfigureAwait(true);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(OmegaCacheController).Log(error);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="timeout"></param>
        /// <param name="cacheSource"></param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <returns></returns>
        public async Task<bool> AddToCacheAsync<T>(IOmegaServerCachingProvider provider, TimeSpan timeout, string cacheSource, string cacheKey, T cacheValue)
        {
            if (cacheValue is null)
            {
                throw new ArgumentNullException(nameof(cacheValue));
            }

            try
            {
                return await AddToCacheAsync(provider, timeout, cacheSource, cacheKey, OmegaJsonSerializer.SerializeObject(cacheValue)).ConfigureAwait(true);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(OmegaCacheController).Log(error);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="timeout"></param>
        /// <param name="cacheSource"></param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <returns></returns>
#pragma warning disable CA1822 // Mark members as static
        public async Task<bool> AddToCacheAsync(IOmegaServerCachingProvider provider, TimeSpan timeout, string cacheSource, string cacheKey, string cacheValue)
#pragma warning restore CA1822 // Mark members as static
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if ((timeout - new TimeSpan(0)).Ticks == 0)
            {
                throw new ArgumentException($"'{nameof(timeout)}' cannot be zero", nameof(timeout));
            }

            if (string.IsNullOrEmpty(cacheSource))
            {
                throw new ArgumentException($"'{nameof(cacheSource)}' cannot be null or empty", nameof(cacheSource));
            }

            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentException($"'{nameof(cacheKey)}' cannot be null or empty", nameof(cacheKey));
            }

            if (string.IsNullOrEmpty(cacheValue))
            {
                throw new ArgumentException($"'{nameof(cacheValue)}' cannot be null or empty", nameof(cacheValue));
            }

            try
            {
                return await provider.StoreToCacheAsync(cacheKey, new OmegaCachingObject()
                {
                    CacheSource = cacheSource,
                    CacheKey = cacheKey,
                    CacheValue = cacheValue,
                    Timeout = timeout
                }).ConfigureAwait(true);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(OmegaCacheController).Log(error);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
#pragma warning disable CA1822 // Mark members as static
        public async Task<IEnumerable<OmegaCachingObject>> GetAllCache(string provider)
#pragma warning restore CA1822 // Mark members as static
        {
            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            try
            {
                if (_cachingProviders.ContainsKey(provider))
                {
                    return await _cachingProviders[provider].GetAllFromCache().ConfigureAwait(true);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(OmegaCacheController).Log(error);
            }
            return new List<OmegaCachingObject>();
        }
        #endregion
    }
}
