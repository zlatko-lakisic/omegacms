using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.Helpers.Core.TypeAttributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class StringValueAttribute : System.Attribute
    {
        private string _value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            _value = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get { return _value; }
        }

    }
}
