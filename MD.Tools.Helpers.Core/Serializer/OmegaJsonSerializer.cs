using MD.Tools.Helpers.Core.Logging;
using Newtonsoft.Json;
using System;

namespace MD.Tools.Helpers.Core.Serializer
{
    /// <summary>
    /// 
    /// </summary>
    public static class OmegaJsonSerializer
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object? DeserializeObject(string value, object? defaultValue = null)
        {
            try
            {
                return JsonConvert.DeserializeObject(value, new JsonSerializerSettings() { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (JsonReaderException error)
            {
                typeof(JsonSerializer).Log(error);
            }
            catch (JsonSerializationException error)
            {
                typeof(JsonSerializer).Log(error);
            }
            catch (JsonWriterException error)
            {
                typeof(JsonSerializer).Log(error);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(JsonSerializer).Log(error);
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return DeserializeObject<T>(value, default);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value, T defaultValue)
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return obj == null ? defaultValue : obj;
            }
            catch (JsonReaderException error)
            {
                typeof(JsonSerializer).Log(error);
            }
            catch (JsonSerializationException error)
            {
                typeof(JsonSerializer).Log(error);
            }
            catch (JsonWriterException error)
            {
                typeof(JsonSerializer).Log(error);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(JsonSerializer).Log(error);
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public static string SerializeObject(object? value, string defaultValue = null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            try
            {
                return JsonConvert.SerializeObject(value, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (JsonReaderException error)
            {
                typeof(JsonSerializer).Log(error);
            }
            catch (JsonSerializationException error)
            {
                typeof(JsonSerializer).Log(error);
            }
            catch (JsonWriterException error)
            {
                typeof(JsonSerializer).Log(error);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(JsonSerializer).Log(error);
            }
            return defaultValue;
        }
        #endregion
    }
}
