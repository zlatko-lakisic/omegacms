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
        /// Returns the culture into for the given lcid
        /// </summary>
        /// <param name="lcid">The lcid.</param>
        /// <returns>a culture Info class</returns>
        public static CultureInfo ToCultureInfo(this int lcid)
        {
            return new CultureInfo(lcid);
        }

        /// <summary>
        /// Gets the Int32 value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Int32 ToInt32(this string value, Int32 defaultValue)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            Int32? result = null;
            Int32 temp;
            if (Int32.TryParse(value, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the Int32 value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The style.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Int32 ToInt32(this string value, Int32 defaultValue, NumberStyles style)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            Int32? result = null;
            Int32 temp;
            if (Int32.TryParse(value, style, CultureInfo.InvariantCulture, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the Int32 value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="provider">The provider.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Int32 ToInt32(this string value, Int32 defaultValue, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            Int32? result = null;
            Int32 temp;
            if (Int32.TryParse(value, NumberStyles.Integer, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the Int32 value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Int32 ToInt32(this string value, Int32 defaultValue, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            Int32? result = null;
            Int32 temp;
            if (Int32.TryParse(value, style, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the Int32 value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted</returns>
        public static Int32? ToInt32(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            Int32? result = null;
            Int32 temp;
            if (Int32.TryParse(value, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Gets the Int32 value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Int32? ToInt32(this string value, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return null;
            Int32? result = null;
            Int32 temp;
            if (Int32.TryParse(value, style, provider, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A list of <see ref="Int32" /></returns>
        public static IList<Int32> ToInt32List(this string value)
        {
            List<Int32> ids = new List<Int32>();
            foreach (string s in ToList(value, DefaultListDelimiters))
            {
                Int32 id = 0;
                if (Int32.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="Int32"/></returns>
        public static IList<Int32> ToInt32List(this string value, params char[] delimiters)
        {
            List<Int32> ids = new List<Int32>();
            foreach (string s in ToList(value, delimiters))
            {
                Int32 id = 0;
                if (Int32.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <returns>A list of <see ref="Int32"/></returns>
        public static IList<Int32> ToInt32List(this string value, NumberStyles style)
        {
            return value.ToInt32List(style, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="Int32"/></returns>
        public static IList<Int32> ToInt32List(this string value, NumberStyles style, params char[] delimiters)
        {
            List<Int32> ids = new List<Int32>();
            foreach (string s in ToList(value, delimiters))
            {
                Int32 id = 0;
                if (Int32.TryParse(s, style, CultureInfo.InvariantCulture, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="Int32"/></returns>
        public static IList<Int32> ToInt32List(this string value, IFormatProvider provider)
        {
            return value.ToInt32List(provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="Int32"/></returns>
        public static IList<Int32> ToInt32List(this string value, IFormatProvider provider, params char[] delimiters)
        {
            List<Int32> ids = new List<Int32>();
            foreach (string s in ToList(value, delimiters))
            {
                Int32 id = 0;
                if (Int32.TryParse(s, NumberStyles.Integer, provider, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="Int32" /></returns>
        public static IList<Int32> ToInt32List(this string value, NumberStyles style, IFormatProvider provider)
        {
            return value.ToInt32List(style, provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="Int32"/></returns>
        public static IList<Int32> ToInt32List(this string value, NumberStyles style, IFormatProvider provider, params char[] delimiters)
        {
            List<Int32> ids = new List<Int32>();
            foreach (string s in ToList(value, delimiters))
            {
                Int32 id = 0;
                if (Int32.TryParse(s, style, provider, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Converts the Enumerable collection to a delimited string.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>
        /// A string delimiting the items in the list use invariant standard string formatting
        /// </returns>
        public static string ToDelimitedString(this IEnumerable<Int32> list, string delimiter)
        {
            return ToDelimitedString<Int32>(list, delimiter, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
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
        public static string ToDelimitedString(this IEnumerable<Int32> list, string delimiter, IFormatProvider formatter, string format)
        {
            return ToDelimitedString<Int32>(list, delimiter, formatter, format);
        }

        /// <summary>
        /// Converts enumerable to the string list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<Int32> list)
        {
            return ToStrings<Int32>(list,  System.Globalization.CultureInfo.InvariantCulture, string.Empty);
        }


        /// <summary>
        /// Converts enumerable to the string list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<Int32> list,  IFormatProvider formatter, string format)
        {
            return ToStrings<Int32>(list,  formatter, format);
        }
    }
}
