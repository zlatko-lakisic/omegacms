using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;

namespace MD.Tools.Helpers.Core.TypeConversion
{
    public static partial class Utility
    {

        /// <summary>
        /// Merges missing values from the source collection into the target
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void MergeFrom(this NameValueCollection target, NameValueCollection source)
        {
            MergeFrom(target, source, Enumerable.Empty<string>());
        }

        /// <summary>
        /// Merges missing values from the source collection into the target
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <param name="ignoreKeys">The ignore keys.</param>
        public static void MergeFrom(this NameValueCollection target, NameValueCollection source, IEnumerable<string> ignoreKeys)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (source == null) throw new ArgumentNullException(nameof(source));
            foreach (string key in source.Keys)
            {
                if (ignoreKeys.Contains(key, StringComparer.OrdinalIgnoreCase)) continue;
                if (string.IsNullOrEmpty(target[key])) target[key] = source[key];
            }
        }

       
        /// <summary>
        /// Merges the values from the source into the target
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void MergeFrom<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
        {
            MergeFrom(target, source, Enumerable.Empty<TKey>());
        }

        /// <summary>
        /// Merges the values from the source into the target
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <param name="ignoreKeys">The ignore keys.</param>
        public static void MergeFrom<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source, IEnumerable<TKey> ignoreKeys)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target.IsReadOnly) throw new NotSupportedException("Target dictionary is readonly");
            foreach (TKey key in source.Keys)
            {
                if (ignoreKeys.Contains(key) || target.ContainsKey(key)) continue;
                target[key] = source[key];
            }
        }
               

        /// <summary>
        /// Chains the two enumerables
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original">The original.</param>
        /// <param name="additional">The additional.</param>
        /// <returns>A combined enumeration</returns>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> original, IEnumerable<T> additional)
        {
            if (original != null)
            {
                foreach(T item in original)
                    yield return item;
            }
            if (additional != null)
            {
                foreach (T item in additional)
                    yield return item;
            }
            yield break;
        }

    }
}
