using System.IO;
using MD.CMS.BusinessLogic.WebApi.Core.Addons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MD.CMS.WebApi.Core.Hosted
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            LocalEnvLoader.Load();
            if (string.IsNullOrEmpty(WebApiAddonAppBuilder.Default.WebApiSystemVersion))
            {
                WebApiAddonAppBuilder.Default.WebApiSystemVersion = System.Reflection.Assembly.GetAssembly(typeof(Program))!.GetName().Version!.ToString();
            }
            if (string.IsNullOrEmpty(WebApiAddonAppBuilder.Default.WorkingDirectory))
            {
                WebApiAddonAppBuilder.Default.WorkingDirectory = Directory.GetCurrentDirectory();
            }

            Tools.Licensing.ClientKey.SaveToFile(WebApiAddonAppBuilder.Default.WorkingDirectory);
            Tools.Licensing.ClientId.SaveToFile(WebApiAddonAppBuilder.Default.WebApiSystemVersion, WebApiAddonAppBuilder.Default.ClientKey, WebApiAddonAppBuilder.Default.WorkingDirectory);

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
                        options.Limits.MaxConcurrentConnections = 900;
                    });
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseIIS();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
