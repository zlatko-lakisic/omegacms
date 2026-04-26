using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.Tools.Helpers.Core.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace MD.CMS.WebApi.Core.Properties
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
        private string _fileUpload;
        private StringCollection _dMZUrls;
        private string _googleApiKey;
        private string _uploadFile;
        private string _templateDirectory;
        private string _entityAliasingBase;
        private StringCollection _addonControllerAssemblies;
        private string _templateDirectoryRoot;
        private TimeSpan _userTokenRefreshCheckInterval;
        private TimeSpan _permissionsRefreshCheckInterval;
        private TimeSpan _unreadMessagesCheckInterval;
        private TimeSpan _systemInfoGetAllJobsInterval;
        private TimeSpan _systemInfoPerformanceInterval;
        private string _baseFolder;
        private int _kestrelPort;
        private bool _enableCors;
        private StringCollection _corsOrigins;
        private string _pluginsDirectory;
        private int _pluginsFileProviderType;
        private string _baseApiPath;
        #endregion

        #region Properties
        public string ContentRootPath { get => _contentRootPath; set => _contentRootPath = value; }
        public string WebRootPath { get => _webRootPath; set => _webRootPath = value; }
        public string FileUpload { get => _fileUpload; set => _fileUpload = value; }
        public StringCollection DMZUrls { get => _dMZUrls; set => _dMZUrls = value; }
        public string GoogleApiKey { get => _googleApiKey; set => _googleApiKey = value; }
        public string UploadFile { get => _uploadFile; set => _uploadFile = value; }
        public string TemplateDirectory { get => _templateDirectory; set => _templateDirectory = value; }
        public string EntityAliasingBase { get => _entityAliasingBase; set => _entityAliasingBase = value; }
        public StringCollection AddonControllerAssemblies { get => _addonControllerAssemblies; set => _addonControllerAssemblies = value; }
        public string TemplateDirectoryRoot { get => _templateDirectoryRoot; set => _templateDirectoryRoot = value; }
        public TimeSpan UserTokenRefreshCheckInterval { get => _userTokenRefreshCheckInterval; set => _userTokenRefreshCheckInterval = value; }
        public TimeSpan UnreadMessagesCheckInterval { get => _unreadMessagesCheckInterval; set => _unreadMessagesCheckInterval = value; }
        public TimeSpan SystemInfoGetAllJobsInterval { get => _systemInfoGetAllJobsInterval; set => _systemInfoGetAllJobsInterval = value; }
        public TimeSpan SystemInfoPerformanceInterval { get => _systemInfoPerformanceInterval; set => _systemInfoPerformanceInterval = value; }
        public string SectionName => "MD.CMS.WebApi.Core";
        public string BaseFolder { get => _baseFolder; set => _baseFolder = value; }
        public int KestrelPort { get => _kestrelPort; set => _kestrelPort = value; }
        public bool EnableCors { get => _enableCors; set => _enableCors = value; }
        public StringCollection CorsOrigins { get => _corsOrigins; set => _corsOrigins = value; }
        public string PluginsDirectory { get => _pluginsDirectory; set => _pluginsDirectory = value; }
        public TimeSpan PermissionsRefreshCheckInterval { get => _permissionsRefreshCheckInterval; set => _permissionsRefreshCheckInterval = value; }
        public int PluginsFileProviderType { get => _pluginsFileProviderType; set => _pluginsFileProviderType = value; }
        public string BaseApiPath { get => _baseApiPath; set => _baseApiPath = value; }
        #endregion

        #region Methods
        public Settings()
        {
            FileUpload = "~/UploadedFile";
            DMZUrls = new StringCollection();
            GoogleApiKey = "";
            UploadFile = "~/FileUpload";
            TemplateDirectory = "~/TemplateDirectory";
            EntityAliasingBase = "/ws/Entities/";
            AddonControllerAssemblies = new StringCollection();
            TemplateDirectoryRoot = "../MD.CMS.Administration/src/";
            PermissionsRefreshCheckInterval = TimeSpan.Parse("00:00:10");
            UserTokenRefreshCheckInterval = TimeSpan.Parse("00:00:10");
            UnreadMessagesCheckInterval = TimeSpan.Parse("00:00:10");
            SystemInfoGetAllJobsInterval = TimeSpan.Parse("00:00:10");
            SystemInfoPerformanceInterval = TimeSpan.Parse("00:00:10");
            EnableCors = false;
            CorsOrigins = new StringCollection();
            PluginsDirectory = string.Empty;
            BaseApiPath = string.Empty;
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
