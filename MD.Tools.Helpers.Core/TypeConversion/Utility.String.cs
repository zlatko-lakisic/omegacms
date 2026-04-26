using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace MD.Tools.Helpers.Core.TypeConversion
{
    /// <summary>
    /// Defines the item to string transformation
    /// </summary>
    public delegate string ObjectFormattingFunction<T>(T item);

    public static partial class Utility
    {
        /// <summary>
        /// The characters that are used to split lists
        /// </summary>
        private static readonly char[] DefaultListDelimiters = new char[] { ',', ';' };

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A list of strings within the given value</returns>
        public static IList<string> ToList(this string value)
        {
            return value.ToList(DefaultListDelimiters);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>A list of strings within the given value</returns>
        public static IList<string> ToList(this string value, params char[] delimiters)
        {
            if (delimiters == null || delimiters.Length == 0) throw new ArgumentException("You must provide as least one delimiter", nameof(delimiters));
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(value))
            {
                string[] idlist = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
                if (idlist != null) list.AddRange(idlist);
            }
            return list.AsReadOnly();
        }

        /// <summary>
        /// Url encodes the given string and escapes any ' or " characters
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToXssSafeString(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            char[] xssCharacters = new char[] { '\'', '\"' };
            if (value.IndexOfAny(xssCharacters) > -1)
            {
                foreach (char c in xssCharacters)
                {
                    value = value.Replace(c.ToString(CultureInfo.InvariantCulture), "\\" + c.ToString(CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
                }
            }
            return System.Web.HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Returns a version of the original string with only allowed url characters
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns>Returns a sanitized string replaces accented characters with nearest english equivalent and
        /// removes all non-alphanumerical characters while allowing commong url path characters</returns>
        public static string ToSimplePathFormat(this string original)
        {
            if (string.IsNullOrEmpty(original)) return string.Empty;
            return original
                .ToSimpleCharacters()
                .Replace(' ', '_')
                .ToAlphanumeric('_', '-', '/', '.');
        }

        /// <summary>
        /// Removes any non-alphanumeric characters (unless specified in the Allowed Symbols)
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="allowedSymbols">The allowed symbols.</param>
        /// <returns></returns>
        public static string ToAlphanumeric(this string original, params char[] allowedSymbols)
        {
            if (string.IsNullOrEmpty(original)) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (char c in original)
            {
                if (char.IsLetterOrDigit(c) || allowedSymbols.Contains(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Replaces Accented Characters with Closest Equivalents
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns></returns>
        public static string ToSimpleCharacters(this string original)
        {
            if (string.IsNullOrEmpty(original)) return string.Empty;
            string stFormD = original.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    if (Lookup.ContainsKey(stFormD[ich]))
                    {
                        sb.Append(Lookup[stFormD[ich]]);
                    }
                    else
                    {
                        sb.Append(stFormD[ich]);
                    }
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        private static object _lock = new object();

        private static Dictionary<char, string> _lookup;
        private static Dictionary<char, string> Lookup
        {
            get
            {
                if (_lookup == null)
                {
                    lock (_lock)
                    {
                        if (_lookup == null)
                        {
                            _lookup = new Dictionary<char, string>();
                            _lookup[char.ConvertFromUtf32(230)[0]] = "ae";//_lookup['æ']="ae";
                            _lookup[char.ConvertFromUtf32(198)[0]] = "Ae";//_lookup['Æ']="Ae";
                            _lookup[char.ConvertFromUtf32(240)[0]] = "o";//_lookup['ð']="o";
                        }
                    }
                }
                return _lookup;
            }
        }

        /// <summary>
        /// Converts the Enumerable collection to a delimited string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>A string delimiting the items in the list use invariant standard string formatting</returns>
        public static string ToDelimitedString<T>(this IEnumerable<T> list, string delimiter)
        {
            return ToDelimitedString<T>(list, delimiter, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
        }



        /// <summary>
        /// Converts the Enumerable collection to a string Enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> list)
        {
            return ToStrings<T>(list, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
        }


        /// <summary>
        /// Toes the delimited string.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string ToDelimitedString(this IEnumerable<string> list, string delimiter)
        {
            if (list == null) return string.Empty;
            return string.Join(delimiter, list.ToArray());
        }


        /// <summary>
        /// Converts the Enumerable collection to a delimited string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A string delimiting the items in the list using the provided formatting options
        /// </returns>
        public static string ToDelimitedString<T>(this IEnumerable<T> list, string delimiter, IFormatProvider formatter, string format)
        {
            if (list == null) return string.Empty;
            List<string> strings = new List<string>();
            string formatToUse = "{0}";
            if (!string.IsNullOrEmpty(format)) formatToUse = string.Concat("{0:", format, "}");
            foreach (T item in list)
            {
                strings.Add(string.Format(formatter, formatToUse, item));
            }
            return string.Join(delimiter, strings.ToArray());
        }

        /// <summary>
        /// Converts the Enumerable collection to a string Enumerable
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string ToDelimitedString(this IEnumerable list, string delimiter)
        {
            return ToDelimitedString(list, delimiter, System.Globalization.CultureInfo.InvariantCulture, string.Empty);
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
        public static string ToDelimitedString(this IEnumerable list, string delimiter, IFormatProvider formatter, string format)
        {
            if (list == null) return string.Empty;
            List<string> strings = new List<string>();
            string formatToUse = "{0}";
            if (!string.IsNullOrEmpty(format)) formatToUse = string.Concat("{0:", format, "}");
            foreach (object item in list)
            {
                strings.Add(string.Format(formatter, formatToUse, item));
            }
            return string.Join(delimiter, strings.ToArray());
        }

        /// <summary>
        /// Toes the strings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> list, string format)
        {
            return ToStrings(list, CultureInfo.InvariantCulture, format);
        }

        /// <summary>
        /// Converts enumerable to strings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> list, IFormatProvider formatter, string format)
        {
            if (list != null)
            {
                string formatToUse = "{0}";
                if (!string.IsNullOrEmpty(format)) formatToUse = string.Concat("{0:", format, "}");
                foreach (T item in list)
                {
                    yield return string.Format(formatter, formatToUse, item);
                }
            }
            yield break;
        }

        /// <summary>
        /// Converts the each of the items in the list to a string using the provided function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="formatFunction">The format function.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> list, ObjectFormattingFunction<T> formatFunction)
        {
            if (formatFunction == null) throw new ArgumentNullException(nameof(formatFunction));
            if (list != null)
            {
                foreach (T item in list)
                    yield return formatFunction(item);
            }
            yield break;
        }

        /// <summary>
        /// Converts a indiviualt string to an enumerable
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToStrings(this string item)
        {
            if (string.IsNullOrEmpty(item)) item = string.Empty;
            yield return item;
        }

        /// <summary>
        /// Populates the template using the provided arguments and the invariante culture
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="args">The args.</param>
        public static string ToFormattedString(this string template, params object[] args)
        {
            return template.ToFormattedString(CultureInfo.InvariantCulture, args);
        }

        /// <summary>
        /// Populates the template using the provided arguments usign the provided formatter
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="args">The args.</param>
        public static string ToFormattedString(this string template, IFormatProvider formatter, params object[] args)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;
            if (!args.Any()) return template;
            return string.Format(formatter, template, args);
        }

        /// <summary>
        /// Converts the list into a set of formatted strings.  The list item is always parameter 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="template">The template.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToFormattedStrings<T>(this IEnumerable<T> list, string template, params object[] arguments)
        {
            return ToFormattedStrings(list, CultureInfo.InvariantCulture, template, arguments);
        }

        /// <summary>
        /// Converts the list into a set of formatted strings.  The list item is always parameter 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="template">The template.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToFormattedStrings<T>(this IEnumerable<T> list, IFormatProvider formatter, string template, params object[] arguments)
        {
            if (list != null)
            {
                List<object> args = new List<object>(arguments);
                args.Insert(0, new object());
                foreach (T item in list)
                {
                    if (item != null)
                    {
                        args[0] = item;
                        yield return string.Format(formatter, template, args.ToArray());
                    }
                }
            }
            yield break;
        }

        /// <summary>
        /// Converts the string to title case.
        /// </summary>
        /// <param name="original">The original string.</param>
        /// <returns></returns>
        public static string ToTitleCase(this string original)
        {
            return original.ToTitleCase(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the string to title case.
        /// </summary>
        /// <param name="original">The original string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string ToTitleCase(this string original, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            return original.ToTitleCase(culture.TextInfo);
        }
        /// <summary>
        /// Converts the string to title case.
        /// </summary>
        /// <param name="original">The original string.</param>
        /// <param name="formatting">The formatting.</param>
        /// <returns></returns>
        public static string ToTitleCase(this string original, TextInfo formatting)
        {
            if (string.IsNullOrEmpty(original)) return string.Empty;
            if (formatting == null) throw new ArgumentNullException(nameof(formatting));
            return formatting.ToTitleCase(original);
        }

        /// <summary>
        /// Toes the name value collection.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        public static System.Collections.Specialized.NameValueCollection ToNameValueCollection(this string queryString)
        {
            return System.Web.HttpUtility.ParseQueryString(queryString);
        }

        /// <summary>
        /// Toes the compressed base64 string.
        /// </summary>
        /// <param name="decompressed">The decompressed.</param>
        /// <returns></returns>
        public static string ToCompressedBase64String(this string decompressed)
        {
            byte[] data = Convert.FromBase64String(decompressed);

            using (MemoryStream output = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(output, CompressionMode.Compress, true))
                {
                    gzip.Write(data, 0, data.Length);
                }
                return Convert.ToBase64String(output.ToArray());
            }

        }

        /// <summary>
        /// Froms the compressed base64 string.
        /// </summary>
        /// <param name="compressed">The compressed.</param>
        /// <returns></returns>
        public static string FromCompressedBase64String(this string compressed)
        {
            byte[] data = Convert.FromBase64String(compressed);
            using (MemoryStream input = new MemoryStream(data))
            {
                using (MemoryStream output = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true))
                    {
                        byte[] buff = new byte[64];
                        int read = -1;
                        read = gzip.Read(buff, 0, buff.Length);
                        while (read > 0)
                        {
                            output.Write(buff, 0, read);
                            read = gzip.Read(buff, 0, buff.Length);
                        }
                    }
                    return Convert.ToBase64String(output.ToArray());
                }
            }
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, char value)
        {
            return IndexOf(sb, value, 0);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, char value, int startIndex)
        {
            if(sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }

            int length = sb.Length;
            for (int i = startIndex; i < length; i++)
            {
                if (sb[i] == value) return i;
            }
            return -1;//Not Found
        }

    }
}
