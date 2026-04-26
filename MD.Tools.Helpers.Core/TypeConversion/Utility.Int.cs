using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace MD.Tools.Helpers.Core.TypeConversion
{
    /// <summary>
    /// Encapsulates helper methods for string parsing and manipulation
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// Gets the int value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static int ToInt(this string value, int defaultValue)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            int? result = null;
            int temp;
            if (int.TryParse(value, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the int value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The style.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static int ToInt(this string value, int defaultValue, NumberStyles style)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            int? result = null;
            int temp;
            if (int.TryParse(value, style, CultureInfo.InvariantCulture, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the int value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="provider">The provider.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static int ToInt(this string value, int defaultValue, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            int? result = null;
            int temp;
            if (int.TryParse(value, NumberStyles.Integer, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the int value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static int ToInt(this string value, int defaultValue, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            int? result = null;
            int temp;
            if (int.TryParse(value, style, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the int value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted</returns>
        public static int? ToInt(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            int? result = null;
            int temp;
            if (int.TryParse(value, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Gets the int value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static int? ToInt(this string value, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return null;
            int? result = null;
            int temp;
            if (int.TryParse(value, style, provider, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A list of <see ref="int" /></returns>
        public static IList<int> ToIntList(this string value)
        {
            List<int> ids = new List<int>();
            foreach (string s in ToList(value, DefaultListDelimiters))
            {
                int id = 0;
                if (int.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="int"/></returns>
        public static IList<int> ToIntList(this string value, params char[] delimiters)
        {
            List<int> ids = new List<int>();
            foreach (string s in ToList(value, delimiters))
            {
                int id = 0;
                if (int.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <returns>A list of <see ref="int"/></returns>
        public static IList<int> ToIntList(this string value, NumberStyles style)
        {
            return value.ToIntList(style, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="int"/></returns>
        public static IList<int> ToIntList(this string value, NumberStyles style, params char[] delimiters)
        {
            List<int> ids = new List<int>();
            foreach (string s in ToList(value, delimiters))
            {
                int id = 0;
                if (int.TryParse(s, style, CultureInfo.InvariantCulture, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="int"/></returns>
        public static IList<int> ToIntList(this string value, IFormatProvider provider)
        {
            return value.ToIntList(provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="int"/></returns>
        public static IList<int> ToIntList(this string value, IFormatProvider provider, params char[] delimiters)
        {
            List<int> ids = new List<int>();
            foreach (string s in ToList(value, delimiters))
            {
                int id = 0;
                if (int.TryParse(s, NumberStyles.Integer, provider, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="int" /></returns>
        public static IList<int> ToIntList(this string value, NumberStyles style, IFormatProvider provider)
        {
            return value.ToIntList(style, provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="int"/></returns>
        public static IList<int> ToIntList(this string value, NumberStyles style, IFormatProvider provider, params char[] delimiters)
        {
            List<int> ids = new List<int>();
            foreach (string s in ToList(value, delimiters))
            {
                int id = 0;
                if (int.TryParse(s, style, provider, out id)) ids.Add(id);
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
        //public static string ToDelimitedString(this IEnumerable<int> list, string delimiter)
        //{
        //    return ToDelimitedString<int>(list, delimiter, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
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
        //public static string ToDelimitedString(this IEnumerable<int> list, string delimiter, IFormatProvider formatter, string format)
        //{
        //    return ToDelimitedString<int>(list, delimiter, formatter, format);
        //}
    }
}
