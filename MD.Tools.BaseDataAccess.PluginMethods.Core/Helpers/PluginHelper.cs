using MD.Tools.BaseDataAccess.Plugins.Core;
using System;
using System.Linq;

namespace MD.Tools.BaseDataAccess.PluginMethods.Core.Helpers
{
    public class PluginHelper
    {
        public static void LoadPluginSettings<T>(T plugin)
            where T : IBaseDataAccessPlugin
        {
            if (Plugins.Core.Properties.Settings.Default.DataAccessPluginSettings.Keys.Any(key => string.CompareOrdinal(key, plugin.PluginName).Equals(0)))
            {
                string settings = Plugins.Core.Properties.Settings.Default.DataAccessPluginSettings[plugin.PluginName].ToString();
                plugin.PluginSettings = settings;
            }
        }
    }
}