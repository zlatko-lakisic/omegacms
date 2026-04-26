using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.Helpers.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : ISingleton<T>
       where T : class, new()
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        private static T _instance;
        /// <summary>
        /// 
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T GetSingletonInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static T GetNewInstance()
        {
            return new T();
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
