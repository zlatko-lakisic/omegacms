using System.IO;
using MD.CMS.BusinessLogic.Administration.Core.Addons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MD.CMS.Administration.Core.Hosted
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            LocalEnvLoader.Load();
            if (string.IsNullOrEmpty(AdminAddonAppBuilder.Default.AdminSystemVersion))
            {
                AdminAddonAppBuilder.Default.AdminSystemVersion = System.Reflection.Assembly.GetAssembly(typeof(Program))!.GetName().Version!.ToString();
            }
            if (string.IsNullOrEmpty(AdminAddonAppBuilder.Default.WorkingDirectory))
            {
                AdminAddonAppBuilder.Default.WorkingDirectory = Directory.GetCurrentDirectory();
            }

            Tools.Licensing.ClientKey.SaveToFile(AdminAddonAppBuilder.Default.WorkingDirectory);
            Tools.Licensing.ClientId.SaveToFile(AdminAddonAppBuilder.Default.AdminSystemVersion, AdminAddonAppBuilder.Default.ClientKey, AdminAddonAppBuilder.Default.WorkingDirectory);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("hosting.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"hosting.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddEventSourceLogger();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.AddServerHeader = false;
                        options.Limits.MaxResponseBufferSize = long.MaxValue;
                        options.Limits.MaxRequestBodySize = long.MaxValue;
                    });
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseIIS();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
