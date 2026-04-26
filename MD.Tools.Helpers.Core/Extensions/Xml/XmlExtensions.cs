using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MD.Tools.Helpers.Core.Extensions.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childNodeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetString(this XmlNode node, string childNodeName, string defaultValue)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            string value = node.SelectSingleNode(childNodeName).InnerText;

            if (string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }

            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childNodeName"></param>
        /// <returns></returns>
        public static string GetString(this XmlNode node, string childNodeName)
        {
            return GetString(node, childNodeName, string.Empty);
        }
    }
}
