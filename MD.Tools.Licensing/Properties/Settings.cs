using MD.Tools.Helpers.Core.Config;
using Microsoft.Extensions.Configuration;

namespace MD.Tools.Licensing.Properties
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
        private string _licenseKeyFilePath;
        private string _clientKeyFilePath;
        private string _serverKeyFilePath;
        #endregion

        #region Properties
        public string SectionName => "MD.Tools.Licensing";

        public string LicenseKeyFilePath { get => _licenseKeyFilePath; set => _licenseKeyFilePath = value; }
        public string ClientKeyFilePath { get => _clientKeyFilePath; set => _clientKeyFilePath = value; }
        public string ServerKeyFilePath { get => _serverKeyFilePath; set => _serverKeyFilePath = value; }
        #endregion

        #region Methods
        public Settings()
        {
            _licenseKeyFilePath = "~/license.lic";
            _clientKeyFilePath = "~/client.key";
            _serverKeyFilePath = "~/server.key";
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
