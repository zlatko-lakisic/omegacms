using MD.Tools.BaseDataAccess.PluginMethods.Core.Helpers;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.BaseDataAccess.Plugins.Core.Properties;
using MD.Tools.Helpers.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MD.Tools.BaseDataAccess.PluginMethods.Core.DataAccess
{
    public class BaseDataAccessPluginsContainer : IPluginContainer
    {
        #region Attributes
        private IEnumerable<IBaseDataAccessPlugin> _plugins;
        private static object _lock = new object();
        private static SemaphoreSlim @lock = new SemaphoreSlim(1);
        #endregion

        #region Properties

        public IBaseDataAccessPlugin DefaultPlugin
        {
            get { return _plugins.LastOrDefault(); }
        }

        public IBaseDataAccessPlugin SearchPlugin
        {
            get 
            { 
                return _plugins.FirstOrDefault(); 
            }
        }

        public IEnumerable<IBaseDataAccessPlugin> Plugins
        {
            get { return _plugins; }
        }
        #endregion

        #region Methods
        public BaseDataAccessPluginsContainer()
        {

        }

        internal async Task RegisterPluginsAsync()
        {
            await @lock.WaitAsync();
            try
            {
                RegisterPluginsPrivate();
            }
            finally
            {
                @lock.Release();
            }
        }

        internal void RegisterPlugins()
        {
            lock (_lock)
            {
                RegisterPluginsPrivate();
            }
        }

        private void RegisterPluginsPrivate()
        {
            try
            {
                List<IBaseDataAccessPlugin> sortedPlugins = new List<IBaseDataAccessPlugin>();
                Logger.LogInformation($"Loading IBaseDataAccessPlugin from: {Settings.Default.BaseDataAccessPluginsDirectory} with provider type {Settings.Default.BaseDataAccessPluginsFileProviderType}");
                IEnumerable<IBaseDataAccessPlugin> unsortedPlugins = MD.Tools.Helpers.Core.Plugins.PluginLoader<IBaseDataAccessPlugin>.GetAll(Settings.Default.BaseDataAccessPluginsFileProviderType, Settings.Default.BaseDataAccessPluginsDirectory);
                Logger.LogInformation($"{unsortedPlugins.Count()} IBaseDataAccessPlugin loaded from: {Settings.Default.BaseDataAccessPluginsDirectory} with provider type {Settings.Default.BaseDataAccessPluginsFileProviderType}");
                unsortedPlugins = unsortedPlugins.Where(plugin => Settings.Default.DataAccessPlugins.Cast<string>().Any(pluginNameString => plugin.PluginName.ToLowerInvariant().Contains(pluginNameString.ToLowerInvariant()))).ToList();

                foreach (string pluginName in Settings.Default.DataAccessPlugins.Cast<string>())
                {
                    if (unsortedPlugins.Any(plugin => string.CompareOrdinal(pluginName, plugin.PluginName).Equals(0)))
                    {
                        sortedPlugins.Add(unsortedPlugins.First(plugin => string.CompareOrdinal(pluginName, plugin.PluginName).Equals(0)));
                    }
                }
                _plugins = sortedPlugins;
                foreach (IBaseDataAccessPlugin plugin in _plugins)
                {
                    PluginHelper.LoadPluginSettings(plugin);
                }
                if (_plugins.Any() && SearchPlugin.EventHandlers != null && SearchPlugin.EventHandlers.Any())
                {
                    DefaultPlugin.EventHandlers = SearchPlugin.EventHandlers;
                }
            }
            catch (Exception error)
            {
                Logger.Log("An error occured while loading data access plugins", error);
            }
        }


        public IBaseDataAccessPlugin GetAppropriatePluginForMethod(Method method)
		{
			return _plugins.FirstOrDefault(p => p.HasMethod(method));
		}

		public IBaseDataAccessPlugin GetAppropriatePluginForMethod(DataBoundMethod method)
		{
			return _plugins.FirstOrDefault(p => p.IsDataBoundFieldPlugin && p.DatabaseType == method.DatabaseType);
		}

		public IEnumerable<IBaseDataAccessPlugin> GetAllDataboundPlugins()
		{
			return _plugins.Where(plugin => plugin.IsDataBoundFieldPlugin);
		}
		#endregion
	}

    public static class BaseDataAccessPlugins
    {
        #region Attributes
        private static BaseDataAccessPluginsContainer _container = new BaseDataAccessPluginsContainer();
        #endregion

        #region Properties
        /// <summary>
        /// Plugin Container
        /// </summary>
        public static BaseDataAccessPluginsContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new BaseDataAccessPluginsContainer();
                }
                if (_container.Plugins == null || !_container.Plugins.Any())
                {
                    throw new Exception("No BaseDataAccess Plugins were registered!");
                }
                return _container; 
            }
            set { _container = value; }
        }
        #endregion

        #region Methods
        public static async Task InitializeAsync()
        {
            if (_container == null)
            {
                _container = new BaseDataAccessPluginsContainer();
            }
            if (_container.Plugins == null || !_container.Plugins.Any())
            {
                await _container.RegisterPluginsAsync();
            }
        }
        public static void Initialize()
        {
            if (_container == null)
            {
                _container = new BaseDataAccessPluginsContainer();
            }
            if (_container.Plugins == null || !_container.Plugins.Any())
            {
                _container.RegisterPlugins();
            }
        }
        #endregion
    }
}
