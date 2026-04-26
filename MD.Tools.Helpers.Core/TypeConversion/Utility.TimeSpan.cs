using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace MD.Tools.Helpers.Core.TypeConversion
{
  
    public static partial class Utility
    {

        /// <summary>
        /// Gets the TimeSpan value stored in the resource file
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static TimeSpan ToTimeSpan(this string value, TimeSpan defaultValue)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            TimeSpan? result = null;
            TimeSpan temp;
            if (TimeSpan.TryParse(value, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the TimeSpan value stored in the resource file
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static TimeSpan ToTimeSpan(this string value, TimeSpan defaultValue, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            TimeSpan? result = null;
            TimeSpan temp;
            if (TimeSpan.TryParse(value, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the TimeSpan value stored in the resource file
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted</returns>
        public static TimeSpan? ToTimeSpan(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            TimeSpan? result = null;
            TimeSpan temp;
            if (TimeSpan.TryParse(value, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Gets the TimeSpan value stored in the resource file
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static TimeSpan? ToTimeSpan(this string value, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return null;
            TimeSpan? result = null;
            TimeSpan temp;
            if (TimeSpan.TryParse(value, provider, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Converts the Enumerable collection to a delimited string.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>
        /// A string delimiting the items in the list use invariant standard string formatting
        /// </returns>
        public static string ToDelimitedString(this IEnumerable<TimeSpan> list, string delimiter)
        {
            return ToDelimitedString<TimeSpan>(list, delimiter, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
        }

        /// <summary>
        /// Converts the Enumerable collection to a delimited string.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A string delimiting the items in the list using the provided formatting options
        /// </returns>
        public static string ToDelimitedString(this IEnumerable<TimeSpan> list, string delimiter, IFormatProvider formatter, string format)
        {
            return ToDelimitedString<TimeSpan>(list, delimiter, formatter, format);
        }


        /// <summary>
        /// Converts enumerable to the string list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<TimeSpan> list)
        {
            return ToStrings<TimeSpan>(list, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
        }


        /// <summary>
        /// Converts enumerable to the string list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<TimeSpan> list, IFormatProvider formatter, string format)
        {
            return ToStrings<TimeSpan>(list,  formatter, format);
        }
    }
}
