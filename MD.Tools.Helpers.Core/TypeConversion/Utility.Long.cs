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
        /// Gets the long value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static long ToLong(this string value, long defaultValue)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            long? result = null;
            long temp;
            if (long.TryParse(value, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the long value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The style.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static long ToLong(this string value, long defaultValue, NumberStyles style)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            long? result = null;
            long temp;
            if (long.TryParse(value, style, CultureInfo.InvariantCulture, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the long value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="provider">The provider.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static long ToLong(this string value, long defaultValue, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            long? result = null;
            long temp;
            if (long.TryParse(value, NumberStyles.Integer, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the long value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static long ToLong(this string value, long defaultValue, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            long? result = null;
            long temp;
            if (long.TryParse(value, style, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the long value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted</returns>
        public static long? ToLong(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            long? result = null;
            long temp;
            if (long.TryParse(value, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Gets the long value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static long? ToLong(this string value, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return null;
            long? result = null;
            long temp;
            if (long.TryParse(value, style, provider, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A list of <see ref="long" /></returns>
        public static IList<long> ToLongList(this string value)
        {
            List<long> ids = new List<long>();
            foreach (string s in ToList(value, DefaultListDelimiters))
            {
                long id = 0;
                if (long.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="long"/></returns>
        public static IList<long> ToLongList(this string value, params char[] delimiters)
        {
            List<long> ids = new List<long>();
            foreach (string s in ToList(value, delimiters))
            {
                long id = 0;
                if (long.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <returns>A list of <see ref="long"/></returns>
        public static IList<long> ToLongList(this string value, NumberStyles style)
        {
            return value.ToLongList(style, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="long"/></returns>
        public static IList<long> ToLongList(this string value, NumberStyles style, params char[] delimiters)
        {
            List<long> ids = new List<long>();
            foreach (string s in ToList(value, delimiters))
            {
                long id = 0;
                if (long.TryParse(s, style, CultureInfo.InvariantCulture, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="long"/></returns>
        public static IList<long> ToLongList(this string value, IFormatProvider provider)
        {
            return value.ToLongList(provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="long"/></returns>
        public static IList<long> ToLongList(this string value, IFormatProvider provider, params char[] delimiters)
        {
            List<long> ids = new List<long>();
            foreach (string s in ToList(value, delimiters))
            {
                long id = 0;
                if (long.TryParse(s, NumberStyles.Integer, provider, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="long" /></returns>
        public static IList<long> ToLongList(this string value, NumberStyles style, IFormatProvider provider)
        {
            return value.ToLongList(style, provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="long"/></returns>
        public static IList<long> ToLongList(this string value, NumberStyles style, IFormatProvider provider, params char[] delimiters)
        {
            List<long> ids = new List<long>();
            foreach (string s in ToList(value, delimiters))
            {
                long id = 0;
                if (long.TryParse(s, style, provider, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        ///// <summary>
        ///// Converts the Enumerable collection to a delimited string.
        ///// </summary>
        ///// <param name="list">The list.</param>
        ///// <param name="delimiter">The delimiter.</param>
        ///// <returns>
        ///// A string delimiting the items in the list use invariant standard string formatting
        ///// </returns>
        //public static string ToDelimitedString(this IEnumerable<long> list, string delimiter)
        //{
        //    return ToDelimitedString<long>(list, delimiter, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
        //}

        ///// <summary>
        ///// Converts the Enumerable collection to a delimited string.
        ///// </summary>
        ///// <param name="list">The list.</param>
        ///// <param name="delimiter">The delimiter.</param>
        ///// <param name="formatter">The formatter.</param>
        ///// <param name="format">The format.</param>
        ///// <returns>
        ///// A string delimiting the items in the list using the provided formatting options
        ///// </returns>
        //public static string ToDelimitedString(this IEnumerable<long> list, string delimiter, IFormatProvider formatter, string format)
        //{
        //    return ToDelimitedString<long>(list, delimiter, formatter, format);
        //}
    }
}
