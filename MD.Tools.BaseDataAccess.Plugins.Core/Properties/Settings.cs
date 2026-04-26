using MD.Tools.BaseDataAccess.Plugins.Core.Caching;
using MD.Tools.Helpers.Core.Caching.Providers;
using MD.Tools.Helpers.Core.Config;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Properties
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
        private StringCollection _dataAccessPlugins;
        private Dictionary<string, string> _dataAccessPluginSettings;
        private string _baseDataAccessPluginsDirectory;
        private int _baseDataAccessPluginsFileProviderType;
        private string _pluginJobManagerQueueSettings;
        private OmegaCacheSettings _dataCacheSettings;
        private string _cacheSourceName;
        #endregion

        #region Properties
        public StringCollection DataAccessPlugins { get => _dataAccessPlugins; set => _dataAccessPlugins = value; }
        public Dictionary<string, string> DataAccessPluginSettings { get => _dataAccessPluginSettings; set => _dataAccessPluginSettings = value; }
        public string SectionName => "MD.Tools.BaseDataAccess.Plugins.Core";
        public string BaseDataAccessPluginsDirectory { get => _baseDataAccessPluginsDirectory; set => _baseDataAccessPluginsDirectory = value; }
        public int BaseDataAccessPluginsFileProviderType { get => _baseDataAccessPluginsFileProviderType; set => _baseDataAccessPluginsFileProviderType = value; }
        public string PluginJobManagerQueueSettings { get => _pluginJobManagerQueueSettings; set => _pluginJobManagerQueueSettings = value; }
        public OmegaCacheSettings DataCacheSettings { get => _dataCacheSettings; set => _dataCacheSettings = value; }
        public string CacheSourceName { get => _cacheSourceName; set => _cacheSourceName = value; }
        #endregion

        #region Methods
        public Settings()
        {
            _cacheSourceName = "Business Logic";
            _dataAccessPlugins = new StringCollection();
            _dataAccessPluginSettings = new Dictionary<string, string>();
            _baseDataAccessPluginsDirectory = string.Empty;
            _pluginJobManagerQueueSettings = string.Empty;
            if (_dataCacheSettings == null)
            {
                _dataCacheSettings = new OmegaCacheSettings()
                {
                    DefaultCacheProvider = new MemoryCacheProvider().ProviderName,
                    Entities = new OmegaCacheSettings.Entity[]
                    {
                    new OmegaCacheSettings.Entity()
                    {
                        MappedEntity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.AttributeTypeDefinition,
                        Enabled = true,
                        Methods = new OmegaCacheSettings.Entity.Method[]
                        {
                            new OmegaCacheSettings.Entity.Method()
                            {
                                MappedMethod = Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Methods.GetById.GetIntValue(),
                                TimeSpan = 8600,
                                Enabled = true
                            },
                            new OmegaCacheSettings.Entity.Method()
                            {
                                MappedMethod = Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Methods.GetAll.GetIntValue(),
                                TimeSpan = 8600,
                                Enabled = true
                            },
                            new OmegaCacheSettings.Entity.Method()
                            {
                                MappedMethod = Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Methods.GetByInputType.GetIntValue(),
                                TimeSpan = 8600,
                                Enabled = true
                            }
                        }
                    }
                    }
                };
            }
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
                case "DataCacheSettings":
                    DataCacheSettings = JsonConvert.DeserializeObject<OmegaCacheSettings>(stringValue);
                    break;
            }
        }
        #endregion
    }
}
