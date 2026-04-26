using Microsoft.Extensions.Configuration;

namespace MD.Tools.Helpers.Core.Config
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigParserProvier
    {
        /// <summary>
        /// 
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingsObject"></param>
        /// <param name="section"></param>
        void ParseConfig<T>(T settingsObject, IConfigurationSection section) where T : IConfigParsable;
    }
}
