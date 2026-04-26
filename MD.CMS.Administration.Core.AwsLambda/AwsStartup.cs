using MD.CMS.BusinessLogic.AwsLambda.Core.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using Microsoft.Extensions.Logging;
using MD.Tools.Helpers.Core.Plugins;
using System.Collections.Generic;
using System.Linq;

namespace MD.CMS.Administration.Core.AwsLambda
{
    public class AwsStartup : IAwsStartup
    {
        public void Configure(IWebHostBuilder builder, string path, IDictionary environmentalVariables)
        {
            builder.UseStartup<Startup>();
        }
        public IWebHostBuilder UseAwsStartup(IWebHostBuilder hostBuilder, string path, IDictionary environmentalVariables)
        {
            try
            {
                MD.Tools.Helpers.Core.Properties.HelperSettings.Default.VerboseLoggingReflectionEnabled = false;
                MD.Tools.Helpers.Core.Properties.HelperSettings.Default.AwsCloudWatchLoggerIsEnabled = true;
                MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TraceSwitches = new Dictionary<string, string>();
                MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TraceSwitches.Add("AwsCloudWatchLogger", "4");
                MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TempAssembliesFolder = $"/tmp/{MD.CMS.BusinessLogic.AwsLambda.Core.Properties.Settings.Default.AppReferencePath.Split(".dll.").First()}";

                Startup.StartupPath = $"{ReflectionHelper.GetDefaultPluginPath};{(!path.StartsWith("/") ? "/" : "")}{path}{(!path.EndsWith("/") ? "/" : "")}";

                string webRoot = $"{(!path.StartsWith("/") ? "/" : "")}{path}{(!path.EndsWith("/") ? "/" : "")}wwwroot/";

                return hostBuilder
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile($"{path}appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"{path}appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"{path}.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"{path}hosting.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseStartup<Startup>()
                .UseWebRoot(webRoot)
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                    options.Limits.MaxResponseBufferSize = long.MaxValue;
                    options.Limits.MaxRequestBodySize = long.MaxValue;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(JsonConvert.SerializeObject(e));
                throw;
            }
        }
    }
}
