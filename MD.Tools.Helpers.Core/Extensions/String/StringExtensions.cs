using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MD.Tools.Helpers.Core.Extensions.StringExt
{
    /// <summary>
    /// Set of extensions methods that will be bound to the string class
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a given string to an integer, in case of failure a default integer value will be returned
        /// </summary>
        /// <param name="value">String value to convert</param>
        /// <returns>Converted integer or default integer value</returns>
        public static int ToInt(this string value)
        {
            int returnValue = default(int);

            int.TryParse(value, out returnValue);

            return returnValue;
        }

        /// <summary>
        /// Converts a given string to a boolean, in case of failure a default boolean value will be returned
        /// </summary>
        /// <param name="value">String value to convert</param>
        /// <returns>Converted boolean or default boolean value</returns>
        public static bool ToBoolean(this string value)
        {
            bool returnValue = default(bool);

            bool.TryParse(value, out returnValue);

            return returnValue;
        }

        /// <summary>
        /// Return the default string.Empty value if string is null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Default(this string value)
        {
            string returnValue = string.IsNullOrEmpty(value) ? string.Empty : value;
            return returnValue;
        }

        /// <summary>
        /// Convert string to date time
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultDateTime"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value, DateTime defaultDateTime)
        {
            DateTime dateTime = defaultDateTime;
            DateTime.TryParse(value, out dateTime);
            return dateTime;
        }

        /// <summary>
        /// Convert string to date time
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            return ToDateTime(value, DateTime.Now);
        }

        /// <summary>
        /// Test if a string is a guid
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsGuid(this string s)
        {
            Guid value = Guid.Empty;
            return Guid.TryParse(s, out value);
        }

        /// <summary>
        /// Get safe string value, if null returs empty string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Safe(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetFirstNCharacters(this string str, int n)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            if (str.Length <= n)
                return str;

            return str.Substring(0, n);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSafeString(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = string.Empty;
            }
            return str;
        }
    }
}