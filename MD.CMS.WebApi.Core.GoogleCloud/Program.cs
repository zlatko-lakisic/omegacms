using MD.CMS.BusinessLogic.GoogleCloud.Core.FileProviders;
using MD.CMS.BusinessLogic.WebApi.Core.Addons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace MD.CMS.WebApi.Core.GoogleCloud
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(WebApiAddonAppBuilder.Default.WebApiSystemVersion))
            {
                WebApiAddonAppBuilder.Default.WebApiSystemVersion = System.Reflection.Assembly.GetAssembly(typeof(Program)).GetName().Version.ToString();
            }
            if (string.IsNullOrEmpty(WebApiAddonAppBuilder.Default.WorkingDirectory))
            {
                WebApiAddonAppBuilder.Default.WorkingDirectory = Directory.GetCurrentDirectory();
            }

            Tools.Helpers.Core.FileProvider.DynamicFileProvider.AddFileProvider<GoogleCloudStorageFileProvider>();

            MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TraceSwitches = new System.Collections.Generic.Dictionary<string, string>()
            {
                { "NetCoreLogger", "4" },
                { "GoogleCloudLogger", "4" }
            };

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
                    var url = $"http://0.0.0.0:{port}";
                    webBuilder
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var env = hostingContext.HostingEnvironment;
                        config.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                              .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                              .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
                              .AddJsonFile($"hosting.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                        config.AddEnvironmentVariables();
                    })
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.AddEventSourceLogger();
                    })
                    .ConfigureKestrel(options =>
                    {
                        options.AddServerHeader = false;
                        options.Limits.MaxResponseBufferSize = long.MaxValue;
                        options.Limits.MaxRequestBodySize = long.MaxValue;
                        options.Limits.MaxConcurrentConnections = 900;
                    })
                    .UseStartup<Startup>()
                    .UseUrls(url);
                });
        }
    }
}
