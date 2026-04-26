using MD.Tools.Helpers.Core.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MD.CMS.WebApi.Sockets.Core.AwsLambda.Properties
{
    public class Settings : IConfigParsable
    {
        #region Attributes
        private static Settings defaultInstance = new Settings();
        private bool _debugMode;
        #endregion

        #region Properties
        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
            set
            {
                defaultInstance = value;
            }
        }
        public string SectionName => "MD.CMS.WebApi.Sockets.Core.AwsLambda";

        public bool DebugMode { get => _debugMode; set => _debugMode = value; }
        #endregion

        #region Methods
        public IConfigParsable GetStaticInstance()
        {
            return defaultInstance;
        }

        public void Parse(IConfigurationSection section)
        {
            ConfigParser.ParseConfig(this, section);
        }

        public void ParseComplexType(string sectionName, string stringValue)
        {
            //Do Nothing
        }
        #endregion
    }
}
