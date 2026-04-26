using MD.Tools.Helpers.Core.Config;
using MD.Tools.Helpers.Core.FileProvider;
using MD.Tools.Helpers.Core.Plugins;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.Tests
{
    public class Startup
    {
        public static async Task Init()
        {
            if (Directory.Exists($"{Directory.GetCurrentDirectory()}\\plugins"))
            {
                Directory.Delete($"{Directory.GetCurrentDirectory()}\\plugins", true);
            }
            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\plugins");
            File.WriteAllBytes($"{Directory.GetCurrentDirectory()}\\plugins\\MySqlConnector.dll", Properties.Resources.MySqlConnector);
            File.WriteAllBytes($"{Directory.GetCurrentDirectory()}\\plugins\\MD.Tools.BaseDataAccess.Plugins.MySql.Core.dll", Properties.Resources.MD_Tools_BaseDataAccess_Plugins_MySql_Core);

            if (File.Exists($"{Directory.GetCurrentDirectory()}\\appsettings.json"))
            {
                File.Delete($"{Directory.GetCurrentDirectory()}\\appsettings.json");
            }
            File.WriteAllBytes($"{Directory.GetCurrentDirectory()}\\appsettings.json", Properties.Resources.appsettings);
            IConfigurationRoot iConfig = GetIConfigurationRoot(Directory.GetCurrentDirectory());

            foreach (IConfigParsable obj in PluginLoader<IConfigParsable>.GetAll())
            {
                obj.GetStaticInstance().Parse(iConfig.GetSection("Config"));
            }

            foreach (IConfigParsable obj in PluginLoader<IPluginConfigParsable>.GetAll((int)FileProviderEnum.Hosted, Directory.GetCurrentDirectory()))
            {
                obj.GetStaticInstance().Parse(iConfig.GetSection("Config"));
            }
        }

        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
