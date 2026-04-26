using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace MD.Tools.Helpers.Core.Config
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigParser
    {
        private static List<IConfigParserProvier> _providers = new List<IConfigParserProvier>(new IConfigParserProvier []{ new DefaultConfigParser() });

        /// <summary>
        /// 
        /// </summary>
        public static List<IConfigParserProvier> Providers { get => _providers; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingsObject"></param>
        /// <param name="section"></param>
        public static void ParseConfig<T>(T settingsObject, IConfigurationSection section)
            where T: IConfigParsable
        {
            foreach(IConfigParserProvier provider in _providers.OrderBy(p => p.Order))
            {
                provider.ParseConfig<T>(settingsObject, section);
            }
        }
    }
}
