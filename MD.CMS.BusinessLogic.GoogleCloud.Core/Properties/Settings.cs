using MD.Tools.Helpers.Core.Config;
using Microsoft.Extensions.Configuration;

namespace MD.CMS.BusinessLogic.GoogleCloud.Core.Properties
{
    /// <summary>
    /// Application settings
    /// </summary>
    public class Settings : IConfigParsable
    {
        #region Attributes
        private static Settings defaultInstance = new Settings();
        private string _projectId;
        private string _logId;
        private string _product;
        #endregion

        #region Properties
        /// <summary>
        /// Default static settings instance
        /// </summary>
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

        public string ProjectId { get => _projectId; set => _projectId = value; }
        public string LogId { get => _logId; set => _logId = value; }

        public string SectionName => "MD.CMS.BusinessLogic.GoogleCloud.Core";

        public string Product { get => _product; set => _product = value; }
        #endregion

        #region Methods
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Settings()
        {
        }
        public void Parse(IConfigurationSection section)
        {
            ConfigParser.ParseConfig(this, section);
        }

        public void ParseComplexType(string sectionName, string stringValue)
        {
            //Do Nothing
        }

        public IConfigParsable GetStaticInstance()
        {
            return Settings.Default;
        }
        #endregion
    }
}
