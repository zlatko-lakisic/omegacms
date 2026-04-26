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
        /// Gets the Guid value stored in the resource file
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted</returns>
        public static Guid? ToGuid(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                return new Guid(value);
            }
            catch (FormatException)
            {
                return null;
            }
            catch (OverflowException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the Guid value stored in the resource file
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value if the the given value cannot be parsed</param>
        /// <returns>The setting stored in the brand/locale specific resource file or <c>null</c> if the value cannot be converted</returns>
        public static Guid ToGuid(this string value, Guid defaultValue)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            try
            {
                return new Guid(value);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
            catch (OverflowException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Converts the Enumerable collection to a delimited string.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>
        /// A string delimiting the items in the list use invariant standard string formatting
        /// </returns>
        public static string ToDelimitedString(this IEnumerable<Guid> list, string delimiter)
        {
            return ToDelimitedString<Guid>(list, delimiter, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
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
        public static string ToDelimitedString(this IEnumerable<Guid> list, string delimiter, IFormatProvider formatter, string format)
        {
            return ToDelimitedString<Guid>(list, delimiter, formatter, format);
        }

        /// <summary>
        /// Converts enumerable to the string list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<Guid> list)
        {
            return ToStrings<Guid>(list,  System.Globalization.CultureInfo.InvariantCulture, string.Empty);
        }


        /// <summary>
        /// Converts enumerable to the string list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<Guid> list, IFormatProvider formatter, string format)
        {
            return ToStrings<Guid>(list,  formatter, format);
        }
    }
}
