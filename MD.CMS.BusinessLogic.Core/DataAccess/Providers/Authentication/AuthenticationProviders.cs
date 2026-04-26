using MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication.BuiltIn;
using MD.Tools.Helpers.Core.FileProvider;
using MD.Tools.Helpers.Core.Plugins;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication
{
    internal class AuthenticationProviders : ConcurrentDictionary<string, IAuthenticationProvider>
    {
        #region Attributes
        private static string _defaultName;
        private static AuthenticationProviders _registered = new AuthenticationProviders();
        #endregion

        #region Properties
        internal static AuthenticationProviders Registered 
        { 
            get
            {
                if(_registered == null)
                {
                    _registered = new AuthenticationProviders();
                }
                return _registered;
            }
        }
        internal static IAuthenticationProvider Default
        {
            get
            {
                if (!_registered.ContainsKey(_defaultName))
                {
                    throw new ArgumentOutOfRangeException(nameof(_defaultName), $"The requested default authentication provider {_defaultName} is not registered within the system.");
                }

                return _registered[_defaultName];
            }
        }
        internal static void LoadProviders(int provider = (int)FileProviderEnum.Hosted, string path = null)
        {
            foreach (IAuthenticationProvider obj in PluginLoader<IAuthenticationProvider>.GetAll(provider, path))
            {
                if (!Registered.ContainsKey(obj.ProviderName))
                {
                    if(Properties.Settings.Default.EnabledAuthenticationProviders != null)
                    {
                        if (Properties.Settings.Default.EnabledAuthenticationProviders.Contains(obj.ProviderName))
                        {
                            Registered.TryAdd(obj.ProviderName, obj);
                        }
                    } 
                    else
                    {
                        Registered.TryAdd(obj.ProviderName, obj);
                    }
                }
            }
        }
        #endregion

        #region Methods
        internal AuthenticationProviders() : base()
        {
            IAuthenticationProvider defaultProvider = new BuiltInAuthenticationProvider();
            _defaultName = defaultProvider.ProviderName;

            TryAdd(_defaultName, defaultProvider);
        }
        #endregion
    }
}
