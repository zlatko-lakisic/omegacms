using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using MD.CMS.BusinessLogic.Administration.Core.Addons;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.Helpers.Core.Logging;
using Microsoft.AspNetCore.Http;

namespace MD.CMS.Administration.Core.Models
{
    public class Layout
    {
        private HttpContext _context;

        public Layout(HttpContext context)
        {
            _context = context;
        }

        public bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public bool IsProduction
        {
            get
            {
                return Settings.Default.ProductionMode;
            }
        }

        public IEnumerable<string> AddonJavascriptBodyUrls
        {
            get
            {
                return string.Join("***", Startup.AdminAddonLoaders.Select(loader =>
                    string.Join("***", loader.ScriptsToIntercept.Where(script => script.Location == AdminResourceLocationEnum.Body).Select(script => script.Url))
                )).Split("***").Where(url =>
                    !string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url)
                );
            }
        }

        public IEnumerable<string> AddonJavascriptHeadUrls
        {
            get
            {
                return string.Join("***", Startup.AdminAddonLoaders.Select(loader =>
                    string.Join("***", loader.ScriptsToIntercept.Where(script => script.Location == AdminResourceLocationEnum.Head).Select(script => script.Url))
                )).Split("***").Where(url =>
                    !string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url)
                );
            }
        }

        public IEnumerable<string> AddonCssUrls
        {
            get
            {
                return string.Join("***", Startup.AdminAddonLoaders.Select(loader => 
                    string.Join("***", loader.CssToIntercept.Select(css => css.Url))
                )).Split("***").Where(url => 
                    !string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url)
                );
            }
        }

        public string UILanguage
        {
            get
            {
                string language = CultureInfo.CurrentUICulture.Name;
                try
                {
                    ResourceSet resourceSet = Resources.SupportedLanguages.ResourceManager.GetResourceSet(CultureInfo.GetCultureInfo(Settings.Default.DefaultLcid), true, true);
                    IEnumerable<DictionaryEntry> languages = resourceSet.Cast<DictionaryEntry>();
                    language = languages.First().Key.ToString();
                    string lang = _context.Request.Query["lang"];
                    if (string.IsNullOrEmpty(lang))
                    {
                        lang = CultureInfo.GetCultureInfo(Settings.Default.DefaultLcid).Name;
                    }
                    if (!string.IsNullOrEmpty(lang) && languages.Count(l => string.Compare(l.Key.ToString().Replace("_", "-", true, CultureInfo.InvariantCulture), lang, true).Equals(0)).Equals(1))
                    {
                        language = lang;
                    }
                    return language;
                }
                catch (Exception e)
                {
                    typeof(Layout).Log("Error occured while setting UI language in Layout model.", e);
                }
                return language;
            }
        }

        public string DefaultCultureName
        {
            get
            {
                return CultureInfo.GetCultureInfo(Settings.Default.DefaultLcid).Name;
            }
        }

        public string UploadsBase
        {
            get
            {
                string result = Properties.Settings.Default.BaseFolder + Settings.Default.FileUploadPath;
                if (!result.EndsWith("/", true, CultureInfo.InvariantCulture))
                {
                    result += "/";
                }
                return result;
            }
        }

        public string GoogleMapsApiKey
        {
            get
            {
                return Settings.Default.GoogleMapsJsKey;
            }
        }

        public int CurrentLcid
        {
            get
            {
                try
                {
                    int currentCultureLcid = Settings.Default.DefaultLcid;
                    int queryStringLcid = Settings.Default.DefaultLcid;
                    if (!string.IsNullOrEmpty(_context.Request.Query["lcid"]) && int.TryParse(_context.Request.Query["lcid"], out currentCultureLcid))
                    {
                        CultureInfo defaultCulture = CultureInfo.GetCultureInfo(queryStringLcid);
                        return defaultCulture.LCID;
                    }
                    else
                    {
                        CultureInfo defaultCulture = CultureInfo.GetCultureInfo(Settings.Default.DefaultLcid);
                        return defaultCulture.LCID;
                    }
                }
                catch
                {
                    //Silent Fail
                }
                return Settings.Default.DefaultLcid;
            }
        }

        public string CMSVersion
        {
            get
            {
                if (IsProduction)
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(CMSVersionString));
                        return (new Guid(hash)).ToString();
                    }
                }
                else
                {
                    return CMSVersionString;
                }
            }
        }

        public string CMSVersionString
        {
            get
            {
                if (string.IsNullOrEmpty(AdminAddonAppBuilder.Default.AdminSystemVersion))
                {
                    AdminAddonAppBuilder.Default.AdminSystemVersion = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString();
                }
                return AdminAddonAppBuilder.Default.AdminSystemVersion;
            }
        }

        public string CMSNameString
        {
            get
            {
                if (string.IsNullOrEmpty(AdminAddonAppBuilder.Default.AdminSystemName))
                {
                    AdminAddonAppBuilder.Default.AdminSystemName = Properties.Resources.MDCMSAdministrationName;
                }
                return AdminAddonAppBuilder.Default.AdminSystemName;
            }
        }

        public string CMSTitleString
        {
            get
            {
                if (string.IsNullOrEmpty(AdminAddonAppBuilder.Default.AdminSystemTitle))
                {
                    AdminAddonAppBuilder.Default.AdminSystemTitle = Properties.Resources.MDCMSAdministrationTitle;
                }
                return AdminAddonAppBuilder.Default.AdminSystemTitle;
            }
        }

        public AdminAddonAppBuilder.Icons CMSIconSettings
        {
            get
            {
                if (AdminAddonAppBuilder.Default.IconSettings == null)
                {
                    AdminAddonAppBuilder.Default.IconSettings = new AdminAddonAppBuilder.Icons()
                    {
                        AppleTouchIcon = Properties.Resources.AppleTouchIcon,
                        Icon32x32 = Properties.Resources.Icon32x32,
                        Icon16x16 = Properties.Resources.Icon16x16,
                        Manifest = Properties.Resources.Manifest,
                        MaskIcon = Properties.Resources.MaskIcon,
                        MsApplicationConfig = Properties.Resources.MsApplicationConfig,
                        ShortcutIcon = Properties.Resources.ShortcutIcon,
                        ThemeColor = Properties.Resources.ThemeColor
                    };
                }
                return AdminAddonAppBuilder.Default.IconSettings;
            }
        }

        public int ResponseCode
        {
            get
            {
                return _context.Response.StatusCode;
            }
        }


        public string BaseFolder
        {
            get
            {
                string result = Properties.Settings.Default.BaseFolder;
                if (!result.EndsWith("/", true, CultureInfo.InvariantCulture))
                {
                    result += "/";
                }
                return result;
            }
        }

        public string GetResources
        {
            get
            {
                return Handlers.ResourcesHandler.GetResources(UILanguage);
            }
        }

        public string ApiBase
        {
            get
            {
                return Properties.Settings.Default.ApiBase;
            }
        }

        public string AuthenticateHeaderName
        {
            get
            {
                return MD.CMS.BusinessLogic.WebApi.Core.Properties.Settings.Default.AuthenticateHeaderName;
            }
        }

        public double SessionTimeout
        {
            get
            {
                return MD.CMS.BusinessLogic.Core.Properties.Settings.Default.SessionTimeout.TotalMilliseconds;
            }
        }

        public StringCollection EnabledAuthenticatonProviders
        {
            get
            {
                return BusinessLogic.Core.Properties.Settings.Default.EnabledAuthenticationProviders;
            }
        }

        public string UiDebugMode => Properties.Settings.Default.UiDebugMode.ToString().ToLowerInvariant();
    }
}
