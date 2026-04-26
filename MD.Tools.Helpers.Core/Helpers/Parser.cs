using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace MD.Tools.Helpers.Core.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Parser<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public delegate T ParserFunction(string value);

        /// <summary>
        /// 
        /// </summary>
        public static readonly ParserFunction Parse = GetFunction();
        private static ParserFunction GetFunction()
        {
            Type t = typeof(T);
            MethodInfo m = t.GetMethod("Parse", new Type[] { typeof(string) });
            ParserFunction d = (ParserFunction)ParserFunction.CreateDelegate(typeof(ParserFunction), m);
            return d;        
        }
    }
}
