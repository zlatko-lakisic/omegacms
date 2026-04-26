using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.CMS.BusinessLogic.WebApi.Core.Extensions
{
    public static class WebCollectionExtensions
    {
        public static bool ContainsKeyName(this IEnumerable<KeyValuePair<string, StringValues>> keyValuePairs, string headerName)
        {
            if (keyValuePairs is null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentException("message", nameof(headerName));
            }

            return keyValuePairs.Any(keyValuePair => string.CompareOrdinal(keyValuePair.Key.ToLowerInvariant(), headerName.ToLowerInvariant()).Equals(0));
        }

        public static string GetValue(this IEnumerable<KeyValuePair<string, StringValues>> keyValuePairs, string headerName, string defaultValue = "")
        {
            if (keyValuePairs is null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentException("message", nameof(headerName));
            }

            foreach(KeyValuePair<string, StringValues> keyValuePair in keyValuePairs)
            {
                if(string.CompareOrdinal(keyValuePair.Key.ToLowerInvariant(), headerName.ToLowerInvariant()).Equals(0))
                {
                    return keyValuePair.Value;
                }
            }

            return defaultValue;
        }
    }
}
