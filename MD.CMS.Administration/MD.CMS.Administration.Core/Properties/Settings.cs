using MD.Tools.Helpers.Core.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace MD.CMS.Administration.Core.Properties
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
        private string _contentRootPath;
        private string _webRootPath;
        private string _virtualPath;
        private string _angularFuseModuleRedirectTo;
        private StringCollection _angularFuseModuleExclusionList;
        private string _baseFolder;
        private string _uploadsRootPath;
        private string _apiBase;
        private int _kestrelPort;
        private string _pluginsDirectory;
        private int _pluginsFileProviderType;
        private bool _uiDebugMode;
        private Dictionary<string, Func<string, string>> _interceptRedirect;
        #endregion

        #region Properties
        public string ContentRootPath { get => _contentRootPath; set => _contentRootPath = value; }
        public string WebRootPath { get => _webRootPath; set => _webRootPath = value; }
        public string VirtualPath { get => _virtualPath; set => _virtualPath = value; }
        public StringCollection AngularFuseModuleExclusionList { get => _angularFuseModuleExclusionList; set => _angularFuseModuleExclusionList = value; }
        public string AngularFuseModuleRedirectTo { get => _angularFuseModuleRedirectTo; set => _angularFuseModuleRedirectTo = value; }
        public string SectionName => "MD.CMS.Administration.Core";

        public string DefaultUILanguage
        {
            get
            {
                ResourceSet resourceSet = Administration.Core.Resources.SupportedLanguages.ResourceManager.GetResourceSet(CultureInfo.GetCultureInfo(BusinessLogic.Core.Properties.Settings.Default.DefaultLcid), true, true);
                IEnumerable<DictionaryEntry> languages = resourceSet.Cast<DictionaryEntry>();
                string language = languages.First().Key.ToString();
                string lang = CultureInfo.GetCultureInfo(BusinessLogic.Core.Properties.Settings.Default.DefaultLcid).Name;
                if (!string.IsNullOrEmpty(lang) && languages.Count(l => string.Compare(l.Key.ToString().Replace("_", "-", System.StringComparison.InvariantCultureIgnoreCase), lang, true).Equals(0)).Equals(1))
                {
                    language = lang;
                }
                return language;
            }
        }

        public string BaseFolder { get => _baseFolder; set => _baseFolder = value; }
        public string UploadsRootPath { get => _uploadsRootPath; set => _uploadsRootPath = value; }
        public string ApiBase { get => _apiBase; set => _apiBase = value; }
        public int KestrelPort { get => _kestrelPort; set => _kestrelPort = value; }
        public string PluginsDirectory { get => _pluginsDirectory; set => _pluginsDirectory = value; }
        public int PluginsFileProviderType { get => _pluginsFileProviderType; set => _pluginsFileProviderType = value; }
        public bool UiDebugMode { get => _uiDebugMode; set => _uiDebugMode = value; }
        public Dictionary<string, Func<string, string>> InterceptRedirect { get => _interceptRedirect; set => _interceptRedirect = value; }
        #endregion

        #region Methods
        public Settings()
        {
            _angularFuseModuleRedirectTo = "";
            _angularFuseModuleExclusionList = new StringCollection();
            _baseFolder = string.Empty;
            _pluginsDirectory = string.Empty;
            _uiDebugMode = false;
            _interceptRedirect = new Dictionary<string, Func<string, string>>();
            _interceptRedirect.Add(@".*worker-javascript\.js$", (string url) => {
                if (!url.ToLowerInvariant().Contains("scripts/plugins/ace"))
                {
                    return "/scripts/plugins/ace/worker-javascript.js";
                }
                return url;
            });
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
