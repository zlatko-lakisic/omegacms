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
        /// Gets the Double value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Double ToDouble(this string value, Double defaultValue)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            Double? result = null;
            Double temp;
            if (Double.TryParse(value, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the Double value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The style.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Double ToDouble(this string value, Double defaultValue, NumberStyles style)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            Double? result = null;
            Double temp;
            if (Double.TryParse(value, style, CultureInfo.InvariantCulture, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the Double value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="provider">The provider.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Double ToDouble(this string value, Double defaultValue, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            Double? result = null;
            Double temp;
            if (Double.TryParse(value, NumberStyles.Float, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the Double value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Double ToDouble(this string value, Double defaultValue, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            Double? result = null;
            Double temp;
            if (Double.TryParse(value, style, provider, out temp)) result = temp;
            return result ?? defaultValue;
        }

        /// <summary>
        /// Gets the Double value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted</returns>
        public static Double? ToDouble(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            Double? result = null;
            Double temp;
            if (Double.TryParse(value, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Gets the Double value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The number style thats expected</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Double? ToDouble(this string value, NumberStyles style)
        {
            if (string.IsNullOrEmpty(value)) return null;
            Double? result = null;
            Double temp;
            if (Double.TryParse(value, style, CultureInfo.InvariantCulture, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Gets the Double value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Double? ToDouble(this string value, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return null;
            Double? result = null;
            Double temp;
            if (Double.TryParse(value, NumberStyles.Float, provider, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Gets the Double value stored in the string
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="style">The number style thats expected</param>
        /// <param name="provider">The format provider usually used for culture specific formating</param>
        /// <returns>
        /// The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted
        /// </returns>
        public static Double? ToDouble(this string value, NumberStyles style, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return null;
            Double? result = null;
            Double temp;
            if (Double.TryParse(value, style, provider, out temp)) result = temp;
            return result;
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A list of <see ref="Double" /></returns>
        public static IList<Double> ToDoubleList(this string value)
        {
            List<Double> ids = new List<Double>();
            foreach (string s in ToList(value, DefaultListDelimiters))
            {
                Double id = 0;
                if (Double.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="Double"/></returns>
        public static IList<Double> ToDoubleList(this string value, params char[] delimiters)
        {
            List<Double> ids = new List<Double>();
            foreach (string s in ToList(value, delimiters))
            {
                Double id = 0;
                if (Double.TryParse(s, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <returns>A list of <see ref="Double"/></returns>
        public static IList<Double> ToDoubleList(this string value, NumberStyles style)
        {
            return value.ToDoubleList(style, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="Double"/></returns>
        public static IList<Double> ToDoubleList(this string value, NumberStyles style, params char[] delimiters)
        {
            List<Double> ids = new List<Double>();
            foreach (string s in ToList(value, delimiters))
            {
                Double id = 0;
                if (Double.TryParse(s, style, CultureInfo.InvariantCulture, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="Double"/></returns>
        public static IList<Double> ToDoubleList(this string value, IFormatProvider provider)
        {
            return value.ToDoubleList(provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="Double"/></returns>
        public static IList<Double> ToDoubleList(this string value, IFormatProvider provider, params char[] delimiters)
        {
            List<Double> ids = new List<Double>();
            foreach (string s in ToList(value, delimiters))
            {
                Double id = 0;
                if (Double.TryParse(s, NumberStyles.Float, provider, out id)) ids.Add(id);
            }
            return ids.AsReadOnly();
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>A list of <see ref="Double" /></returns>
        public static IList<Double> ToDoubleList(this string value, NumberStyles style, IFormatProvider provider)
        {
            return value.ToDoubleList(style, provider, DefaultListDelimiters);
        }

        /// <summary>
        /// Parsed values from a comma or semicolon delimited list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of <see ref="Double"/></returns>
        public static IList<Double> ToDoubleList(this string value, NumberStyles style, IFormatProvider provider, params char[] delimiters)
        {
            List<Double> ids = new List<Double>();
            foreach (string s in ToList(value, delimiters))
            {
                Double id = 0;
                if (Double.TryParse(s, style, provider, out id)) ids.Add(id);
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
        public static string ToDelimitedString(this IEnumerable<Double> list, string delimiter)
        {
            return ToDelimitedString<Double>(list, delimiter, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
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
        public static string ToDelimitedString(this IEnumerable<Double> list, string delimiter, IFormatProvider formatter, string format)
        {
            return ToDelimitedString<Double>(list, delimiter, formatter, format);
        }

        /// <summary>
        /// Converts enumerable to the string list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<Double> list)
        {
            return ToStrings<Double>(list, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
        }


        /// <summary>
        /// Converts enumerable to the string list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<Double> list, IFormatProvider formatter, string format)
        {
            return ToStrings<Double>(list, formatter, format);
        }
    }
}
