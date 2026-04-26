using MD.Tools.Helpers.Core.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Specialized;

namespace MD.Tools.AsyncTask.Processor.Properties
{
    public class Settings : IConfigParsable
    {
        private static Settings defaultInstance = new Settings();
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


        #region Attributes
        private string _pluginsDirectory;
        private TimeSpan _period;
        private string _serviceName;
        #endregion

        #region Properties
        public string SectionName => "MD.Tools.AsyncTask.Processor";
        public string PluginsDirectory { get => _pluginsDirectory; set => _pluginsDirectory = value; }
        public TimeSpan Period { get => _period; set => _period = value; }
        public string ServiceName { get => _serviceName; set => _serviceName = value; }
        #endregion

        #region Methods
        public Settings()
        {
            PluginsDirectory = string.Empty;
            Period = new TimeSpan(0, 5, 0);
            ServiceName = "Omega Async Task Processor";
        }

        public void Parse(IConfigurationSection section)
        {
            ConfigParser.ParseConfig(this, section);
        }

        public IConfigParsable GetStaticInstance()
        {
            return Settings.Default;
        }

        public void ParseComplexType(string sectionName, string stringValue)
        {
            //Do Nothing
        }
        #endregion
    }
}
