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
        /// Gets the enum.
        /// </summary>
        /// <typeparam name="T">The enum type that is epected</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The enum represented by the value</returns>
        /// <exception cref="ArgumentException">Thrown when <typeparamref name="T"/> is not an enum</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c> or empty</exception>
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
            if (string.IsNullOrEmpty(value)) return defaultValue;

            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (Exception)
            { }// invalid enum to swallow

            return defaultValue;
        }


    }
}
