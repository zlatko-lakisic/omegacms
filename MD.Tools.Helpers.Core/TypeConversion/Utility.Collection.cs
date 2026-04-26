using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace MD.Tools.Helpers.Core.TypeConversion
{
    public static partial class Utility
    {
        /// <summary>
        /// Determines whether the target dictionary contains all of the key/value pairs held within the reference
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="reference">The reference.</param>
        /// <returns>
        /// 	<c>true</c> if [contains all keys and values] [the specified target]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAllKeysAndValues(this NameValueCollection target, NameValueCollection reference)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (reference == null) throw new ArgumentNullException(nameof(reference));
            foreach (string qsk in reference.Keys)
            {
                if (!string.IsNullOrEmpty(target[qsk]) || string.Equals(reference[qsk], target[qsk], StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the target dictionary contains all of the key/value pairs held within the reference
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="reference">The reference.</param>
        /// <returns>
        /// 	<c>true</c> if [contains all keys and values] [the specified target]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAllKeysAndValues<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> reference) where TValue : class
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (reference == null) throw new ArgumentNullException(nameof(reference));
            foreach (TKey key in reference.Keys)
            {
                if (!target.ContainsKey(key)
                    || (target[key] == default(TValue) && reference[key] != default(TValue))
                    || !target[key].Equals(reference[key])) return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether the target dictionary contains all of the key/value pairs held within the reference
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>
        /// 	<c>true</c> if [contains all keys and values] [the specified target]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAllKeysAndValues<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> reference, IEqualityComparer<TValue> comparer)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (reference == null) throw new ArgumentNullException(nameof(reference));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            foreach (TKey key in reference.Keys)
            {
                if (!target.ContainsKey(key) || !comparer.Equals(target[key], reference[key])) return false;
            }
            return true;
        }

        
    }
}
