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
        /// Gets the short value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static short ToShort(this string value, short defaultValue)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            short? result = null;
            short temp;
            if (short.TryParse(value, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the short value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The style.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static short ToShort(this string value, short defaultValue, NumberStyles style)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            short? result = null;
            short temp;
            if (short.TryParse(value, style, CultureInfo.InvariantCulture, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the short value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="provider">The provider.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static short ToShort(this string value, short defaultValue, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            short? result = null;
            short temp;
            if (short.TryParse(value, NumberStyles.Integer, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the short value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static short ToShort(this string value, short defaultValue, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            short? result = null;
            short temp;
            if (short.TryParse(value, style, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the short value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted</returns>
        public static short? ToShort(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            short? result = null;
            short temp;
            if (short.TryParse(value, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Gets the short value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static short? ToShort(this string value, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return null;
            short? result = null;
            short temp;
            if (short.TryParse(value, style, provider, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A list of <see ref="short" /></returns>
        public static IList<short> ToShortList(this string value)
        {
            List<short> ids = new List<short>();
            foreach (string s in ToList(value, DefaultListDelimiters))
            {
                short id = 0;
                if (short.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="short"/></returns>
        public static IList<short> ToShortList(this string value, params char[] delimiters)
        {
            List<short> ids = new List<short>();
            foreach (string s in ToList(value, delimiters))
            {
                short id = 0;
                if (short.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <returns>A list of <see ref="short"/></returns>
        public static IList<short> ToShortList(this string value, NumberStyles style)
        {
            return value.ToShortList(style, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="short"/></returns>
        public static IList<short> ToShortList(this string value, NumberStyles style, params char[] delimiters)
        {
            List<short> ids = new List<short>();
            foreach (string s in ToList(value, delimiters))
            {
                short id = 0;
                if (short.TryParse(s, style, CultureInfo.InvariantCulture, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="short"/></returns>
        public static IList<short> ToShortList(this string value, IFormatProvider provider)
        {
            return value.ToShortList(provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="short"/></returns>
        public static IList<short> ToShortList(this string value, IFormatProvider provider, params char[] delimiters)
        {
            List<short> ids = new List<short>();
            foreach (string s in ToList(value, delimiters))
            {
                short id = 0;
                if (short.TryParse(s, NumberStyles.Integer, provider, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="short" /></returns>
        public static IList<short> ToShortList(this string value, NumberStyles style, IFormatProvider provider)
        {
            return value.ToShortList(style, provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="short"/></returns>
        public static IList<short> ToShortList(this string value, NumberStyles style, IFormatProvider provider, params char[] delimiters)
        {
            List<short> ids = new List<short>();
            foreach (string s in ToList(value, delimiters))
            {
                short id = 0;
                if (short.TryParse(s, style, provider, out id)) ids.Add(id);
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
        //public static string ToDelimitedString(this IEnumerable<short> list, string delimiter)
        //{
        //    return ToDelimitedString<short>(list, delimiter, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
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
        //public static string ToDelimitedString(this IEnumerable<short> list, string delimiter, IFormatProvider formatter, string format)
        //{
        //    return ToDelimitedString<short>(list, delimiter, formatter, format);
        //}
    }
}
