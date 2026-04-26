using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication.BuiltIn;
using MD.Tools.Helpers.Core.Config;
using MD.Tools.Helpers.Core.TypeConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;

namespace MD.CMS.BusinessLogic.Core.Properties
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
        private int _defaultLcid;
        private XmlDocument _availableLcid;
        private bool _productionMode;
        private bool _inProcSessions;
        private string _sessionDomain;
        private TimeSpan _sessionTimeout;
        private string _googleApiTranslationKey;
        private string _googleMapsJsKey;
        private int _emailPort;
        private string _emailHost;
        private bool _emailEnableSsl;
        private string _username;
        private string _password;
        private int _emailTimeout;
        private string _emailBody;
        private string _emailSubject;
        private string _emailRecipient;
        private string _administrationAngularRootModuleRegisterUrl;
        private string _administrationAddonsModuleCode;
        private int _numberOfSystemMessageFolders;
        private string _pluginJobHashFileLocation;
        private string _fileUploadPath;
        private int _fileUploadProvider;
        private List<EntityPermission> _systemUserPermissions;
        private Dictionary<string, string> _rootAdminAccount;
        private User _rootAdmin;
        private StringCollection _enabledAuthenticationProviders;
        #endregion

        #region Properties
        public int DefaultLcid { get => _defaultLcid; set => _defaultLcid = value; }
        public XmlDocument AvailableLcid { get => _availableLcid; set => _availableLcid = value; }
        public bool ProductionMode { get => _productionMode; set => _productionMode = value; }
        public bool InProcSessions { get => _inProcSessions; set => _inProcSessions = value; }
        public string SessionDomain { get => _sessionDomain; set => _sessionDomain = value; }
        public TimeSpan SessionTimeout { get => _sessionTimeout; set => _sessionTimeout = value; }
        public string GoogleApiTranslationKey { get => _googleApiTranslationKey; set => _googleApiTranslationKey = value; }
        public string GoogleMapsJsKey { get => _googleMapsJsKey; set => _googleMapsJsKey = value; }
        public int EmailPort { get => _emailPort; set => _emailPort = value; }
        public string EmailHost { get => _emailHost; set => _emailHost = value; }
        public bool EmailEnableSsl { get => _emailEnableSsl; set => _emailEnableSsl = value; }
        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
        public int EmailTimeout { get => _emailTimeout; set => _emailTimeout = value; }
        public string EmailBody { get => _emailBody; set => _emailBody = value; }
        public string EmailSubject { get => _emailSubject; set => _emailSubject = value; }
        public string EmailRecipient { get => _emailRecipient; set => _emailRecipient = value; }
        public string AdministrationAngularRootModuleRegisterUrl { get => _administrationAngularRootModuleRegisterUrl; set => _administrationAngularRootModuleRegisterUrl = value; }
        public string AdministrationAddonsModuleCode { get => _administrationAddonsModuleCode; set => _administrationAddonsModuleCode = value; }
        public int NumberOfSystemMessageFolders { get => _numberOfSystemMessageFolders; set => _numberOfSystemMessageFolders = value; }
        public string PluginJobHashFileLocation { get => _pluginJobHashFileLocation; set => _pluginJobHashFileLocation = value; }
        public string FileUploadPath { get => _fileUploadPath; set => _fileUploadPath = value; }
        public List<EntityPermission> SystemUserPermissions { get => _systemUserPermissions; set => _systemUserPermissions = value; }
        public Dictionary<string, string> RootAdminAccount { set => _rootAdminAccount = value; }
        public int FileUploadProvider { get => _fileUploadProvider; set => _fileUploadProvider = value; }
        public string SectionName => "MD.CMS.BusinessLogic.Core";

        public StringCollection EnabledAuthenticationProviders 
        { 
            get 
            { 
                if(_enabledAuthenticationProviders == null)
                {
                    _enabledAuthenticationProviders = new StringCollection();
                }

                if (!_enabledAuthenticationProviders.Cast<string>().Any())
                {
                    _enabledAuthenticationProviders.Add(BuiltInAuthenticationProvider.GetProviderName());
                }

                return _enabledAuthenticationProviders;
            } 
            set => _enabledAuthenticationProviders = value; 
        }
        #endregion

        #region Methods
        public Settings()
        {
            _productionMode = true;
            InProcSessions = true;
            _administrationAddonsModuleCode = "(function () {'use strict';angular.module('app.addons', [#modulesGoHere#]);})();";
            _administrationAngularRootModuleRegisterUrl = "scripts/app/addons.module.js";
            _systemUserPermissions = Enum.GetValues(typeof(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)).Cast<Tools.BaseDataAccess.Plugins.Core.Mapping.Entities>().Select(obj =>
            {
                return new EntityPermission()
                {
                    Object = obj,
                    Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User,
                    AccessTypes = new HashSet<PermissionAccessTypeEnum>(new List<PermissionAccessTypeEnum> { PermissionAccessTypeEnum.Read })
                };
            }).ToList();
            _rootAdminAccount = null;
            _enabledAuthenticationProviders = null;
        }

        public void Parse(IConfigurationSection section)
        {
            ConfigParser.ParseConfig(this, section);
            using (RNGCryptoServiceProvider rngCrypt = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[4];       // int32 takes 4 bytes in C#
                rngCrypt.GetBytes(tokenBuffer);

                _rootAdmin = new User();
                _rootAdmin.Id = (-BitConverter.ToInt32(tokenBuffer, 0)).ToString(CultureInfo.InvariantCulture);
                if (_rootAdminAccount != null && _rootAdminAccount.ContainsKey("Username") && _rootAdminAccount.ContainsKey("Password"))
                {
                    _rootAdmin.Username = _rootAdminAccount["Username"];
                    _rootAdmin.AdministrationAllowed = true;
                    _rootAdmin.Token = Guid.NewGuid().ToString();
                    _rootAdmin.ReferenceId = _rootAdmin.Id;
                    _rootAdmin.AuthenticationProvider = DataAccess.Providers.Authentication.BuiltIn.BuiltInAuthenticationProvider.GetProviderName();
                }
            }
        }

        public IConfigParsable GetStaticInstance()
        {
            return Settings.Default;
        }

        public string RootId()
        {
            if(_rootAdmin != null)
            {
                return _rootAdmin.Id;
            }
            return null;
        }

        internal User RootAdmin()
        {
            return _rootAdmin;
        }

        internal string RootAdminPassword()
        {
            return _rootAdminAccount["Password"];
        }

        internal IEnumerable<EntityPermission> RootEntityPermissions()
        {
            return Enum.GetValues(typeof(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)).Cast<Tools.BaseDataAccess.Plugins.Core.Mapping.Entities>().Select(obj =>
            {
                return new EntityPermission()
                {
                    Object = obj,
                    Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User,
                    AccessTypes = new HashSet<PermissionAccessTypeEnum>(new List<PermissionAccessTypeEnum> {
                        PermissionAccessTypeEnum.Read,
                        PermissionAccessTypeEnum.Write,
                        PermissionAccessTypeEnum.Delete
                    })
                };
            });
        }

        public void ParseComplexType(string sectionName, string stringValue)
        {
            //Do Nothing
        }
        #endregion
    }
}
