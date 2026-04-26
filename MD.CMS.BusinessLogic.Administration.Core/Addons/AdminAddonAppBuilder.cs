using MD.Tools.Licensing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Text;

namespace MD.CMS.BusinessLogic.Administration.Core.Addons
{
    public class AdminAddonAppBuilder : ILicensingSettings
    {
        #region Icon Class
        public class Icons
        {
            #region Attributes
            private string appleTouchIcon;
            private string icon32x32;
            private string icon16x16;
            private string manifest;
            private string maskIcon;
            private string shortcutIcon;
            private string msApplicationConfig;
            private string themeColor;
            #endregion

            #region Properties
            public string AppleTouchIcon { get => appleTouchIcon; set => appleTouchIcon = value; }
            public string Icon32x32 { get => icon32x32; set => icon32x32 = value; }
            public string Icon16x16 { get => icon16x16; set => icon16x16 = value; }
            public string Manifest { get => manifest; set => manifest = value; }
            public string MaskIcon { get => maskIcon; set => maskIcon = value; }
            public string ShortcutIcon { get => shortcutIcon; set => shortcutIcon = value; }
            public string MsApplicationConfig { get => msApplicationConfig; set => msApplicationConfig = value; }
            public string ThemeColor { get => themeColor; set => themeColor = value; }
            #endregion
        }
        #endregion

        #region Attributes
        private IApplicationBuilder _applicationBuilder;
        private string _adminSystemName;
        private string _adminSystemVersion;
        private string _adminSystemTitle;
        private Icons _iconSettings;
        private static AdminAddonAppBuilder _default;
        #endregion

        #region Properties
        public static AdminAddonAppBuilder Default
        {
            get
            {
                if(_default == null)
                {
                    _default = new AdminAddonAppBuilder();
                }
                return _default;
            }
        }
        public IApplicationBuilder ApplicationBuilder { get => _applicationBuilder; set => _applicationBuilder = value; }
        public string AdminSystemVersion { get => _adminSystemVersion; set => _adminSystemVersion = value; }
        public string AdminSystemName { get => _adminSystemName; set => _adminSystemName = value; }
        public string AdminSystemTitle { get => _adminSystemTitle; set => _adminSystemTitle = value; }
        public Icons IconSettings { get => _iconSettings; set => _iconSettings = value; }

        public ComponentEnum LicensingComponent => ComponentEnum.Administration;

        public string WorkingDirectory { get; set; }

        public string ClientId => Tools.Licensing.ClientId.GetClientId(AdminSystemVersion, ClientKey);

        public string ClientKey => Tools.Licensing.ClientKey.ReadClientKeyFile(WorkingDirectory);
        #endregion

        #region Methods
        public AdminAddonAppBuilder()
        {
        }
        public AdminAddonAppBuilder(IApplicationBuilder app)
        {
            _applicationBuilder = app;
        }
        #endregion
    }
}
