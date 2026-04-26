using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MD.Tools.Helpers.Core.TypeAttributes;
using System.Globalization;

namespace MD.Tools.Helpers.Core.Extensions.EnumExt
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the string value of the enumeration if available, in all other cases returns an empty string
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum enumeration)
        {
            if(enumeration == null)
            {
                throw new ArgumentNullException(nameof(enumeration));
            }

            string valueToReturn = default(string);

            Type type = enumeration.GetType();
            FieldInfo fi = type.GetField(enumeration.ToString());
            if (fi != null)
            {
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs != null && attrs.Any())
                {
                    valueToReturn = attrs[0].Value;
                }
            }
            return valueToReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumByStringValue<T>(this string value)
            where T : Enum
        {
            foreach (FieldInfo field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(StringValueAttribute)) is StringValueAttribute attribute)
                {
                    if (string.CompareOrdinal(attribute.Value, value).Equals(0))
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException("Not found.", nameof(value));
        }

        /// <summary>
        /// Gets the int value of the enumeration
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetIntValue(this Enum enumeration, int defaultValue)
        {
            if(enumeration == null)
            {
                return defaultValue;
            }
            else
            {
                return Convert.ToInt32(enumeration, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets the int value of the enumeration
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static int GetIntValue(this Enum enumeration)
        {
            return GetIntValue(enumeration, default(int));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static T GetByStringValue<T>(string stringValue)
            where T : Enum
        {
            foreach(T value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                if(string.Compare(value.GetStringValue(), stringValue, true, CultureInfo.InvariantCulture).Equals(0))
                {
                    return value;
                }
            }
            return Enum.GetValues(typeof(T)).Cast<T>().First();
        }
    }
}
