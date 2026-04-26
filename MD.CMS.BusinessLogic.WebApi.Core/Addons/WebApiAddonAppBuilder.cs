using MD.Tools.Licensing;

namespace MD.CMS.BusinessLogic.WebApi.Core.Addons
{
    public class WebApiAddonAppBuilder : ILicensingSettings
    {
        #region Attributes
        private static WebApiAddonAppBuilder _default;
        #endregion

        #region Properties
        public static WebApiAddonAppBuilder Default
        {
            get
            {
                if(_default == null)
                {
                    _default = new WebApiAddonAppBuilder();
                }
                return _default;
            }
        }
        public string WebApiSystemVersion { get; set; }

        public ComponentEnum LicensingComponent => ComponentEnum.WebApi;

        public string WorkingDirectory { get; set; }

        public string ClientId => Tools.Licensing.ClientId.GetClientId(WebApiSystemVersion, ClientKey);

        public string ClientKey => Tools.Licensing.ClientKey.ReadClientKeyFile(WorkingDirectory);
        #endregion
    }
}
