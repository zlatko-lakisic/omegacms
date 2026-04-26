using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.Helpers.Core.Config
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigParsable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        void Parse(IConfigurationSection section);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="stringValue"></param>
        void ParseComplexType(string sectionName, string stringValue);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IConfigParsable GetStaticInstance();
        /// <summary>
        /// 
        /// </summary>
        string SectionName { get; }
    }
}
