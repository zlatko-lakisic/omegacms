using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.Helpers.Core.Extensions.IEnumerable
{
    /// <summary>
    /// 
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IEnumerable<T> Join<T>(this IEnumerable<T> left, IEnumerable<T> right)
        {
            List<T> newList = new List<T>(left);
            newList.AddRange(right);
            return newList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToClone"></param>
        /// <returns></returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
