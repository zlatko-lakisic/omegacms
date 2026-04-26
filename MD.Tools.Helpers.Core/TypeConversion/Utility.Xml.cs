using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MD.Tools.Helpers.Core.TypeConversion
{
    public  static partial class Utility
    {

        /// <summary>
        /// Reads the inner XML.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public static string ReadInnerXml(this XNode parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            var reader = parent.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }

        /// <summary>
        /// Reads the outer XML.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public static string ReadOuterXml(this XNode parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            var reader = parent.CreateReader();
            reader.MoveToContent();
            return reader.ReadOuterXml();
        }

        /// <summary>
        /// Reads the inner text.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public static string ReadInnerText(this XNode parent)
        {
            if (parent==null) throw new ArgumentNullException(nameof(parent));
            if (typeof(XElement).IsAssignableFrom(parent.GetType()))
            {
                return ((XElement)parent).Value;
            }
            var reader = parent.CreateReader();
            reader.MoveToContent();
            return reader.ReadContentAsString();
        }
    }
}
