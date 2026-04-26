using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.Serializer;
using Newtonsoft.Json;
using System;
using System.Text;

namespace MD.Tools.Helpers.Core.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public class OmegaCachingObject
    {
        #region Attributes
        private string _cacheSource;
        private string _cacheKey;
        private TimeSpan _timeout;
        private DateTime _cacheTime;
        private string _cacheValue;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public string CacheKey { get => _cacheKey; set => _cacheKey = value; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Timeout { get => _timeout; set => _timeout = value; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CacheTime { get => _cacheTime; set => _cacheTime = value; }
        /// <summary>
        /// 
        /// </summary>
        public string CacheValue { get => _cacheValue; set => _cacheValue = value; }
        /// <summary>
        /// 
        /// </summary>
        public int ByteSize { 
            get 
            {
                if (!string.IsNullOrEmpty(_cacheValue))
                {
                    try
                    {
                        return UTF8Encoding.UTF8.GetByteCount(_cacheValue);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        //Silent Fail
                    }
                }
                return default;
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        public string CacheSource { get => _cacheSource; set => _cacheSource = value; }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public OmegaCachingObject()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _cacheTime = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return OmegaJsonSerializer.SerializeObject(this);
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
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        public static OmegaCachingObject FromString(string serializedObject)
        {
            try
            {
                return OmegaJsonSerializer.DeserializeObject<OmegaCachingObject>(serializedObject);
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
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
        #endregion
    }
}
