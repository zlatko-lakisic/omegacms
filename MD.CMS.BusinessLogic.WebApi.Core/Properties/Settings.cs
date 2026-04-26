using MD.Tools.Helpers.Core.Config;
using Microsoft.Extensions.Configuration;
using System;
using Newtonsoft.Json;
using MD.CMS.BusinessLogic.WebApi.Core.Caching;

namespace MD.CMS.BusinessLogic.WebApi.Core.Properties
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
        private string _authenticateHeaderName;
        private string _lCIDHeaderName;
        private string _secureApiAesSalt;
        private string _secureApiHash;
        private string _isAdministrationHeaderName;
        private string _entityAliasBase;
        private TimeSpan _sessionTimeout;
        private OmegaCacheSettings _outputCacheSettings;
        private string _cacheSource;
        #endregion

        #region Properties
        public string AuthenticateHeaderName { get => _authenticateHeaderName; set => _authenticateHeaderName = value; }
        public string LCIDHeaderName { get => _lCIDHeaderName; set => _lCIDHeaderName = value; }
        public string SecureApiAesSalt { get => _secureApiAesSalt; set => _secureApiAesSalt = value; }
        public string SecureApiHash { get => _secureApiHash; set => _secureApiHash = value; }
        public string IsAdministrationHeaderName { get => _isAdministrationHeaderName; set => _isAdministrationHeaderName = value; }
        public string EntityAliasBase { get => _entityAliasBase; set => _entityAliasBase = value; }
        public OmegaCacheSettings OutputCacheSettings { get => _outputCacheSettings; set => _outputCacheSettings = value; }
        public string SectionName => "MD.CMS.BusinessLogic.WebApi.Core";

        public string CacheSource { get => _cacheSource; set => _cacheSource = value; }
        #endregion

        #region Methods
        public Settings()
        {
            CacheSource = "WebApi";
            AuthenticateHeaderName = "authorization";
            LCIDHeaderName = "LCID";
            SecureApiAesSalt = "pE/jJ4HtvzxQTUOlADk/vg==";
            SecureApiHash = "SHA1";
            IsAdministrationHeaderName = "administration";
            EntityAliasBase = "Custom";
            OutputCacheSettings = new OmegaCacheSettings();
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
            switch (sectionName)
            {
                case "OutputCacheSettings":
                    OutputCacheSettings = JsonConvert.DeserializeObject<OmegaCacheSettings>(stringValue);
                    break;
            }
        }
        #endregion
    }
}
