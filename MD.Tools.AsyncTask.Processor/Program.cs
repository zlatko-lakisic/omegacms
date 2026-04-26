using MD.Tools.AsyncTask.Processor.Properties;
using MD.Tools.Helpers.Core.Config;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using DotNetEnv;

namespace MD.Tools.AsyncTask.Processor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            foreach (var p in new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), ".env"),
                Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".env")),
            })
            {
                if (File.Exists(p))
                {
                    Env.Load(p);
                    break;
                }
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((logging) =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    typeof(Program).LogVerbose($"Loading base configurations...");
                    DateTime statrt = DateTime.Now;
                    int pluginCount = 0;
                    try
                    {
                        foreach (IConfigParsable obj in PluginLoader<IConfigParsable>.GetAll())
                        {
                            obj.GetStaticInstance().Parse(configuration.GetSection("Config"));
                            pluginCount++;
                        }
                    }
                    catch (Exception e)
                    {
                        typeof(Program).Log(e);
                    }
                    typeof(Program).LogVerbose($"{pluginCount} configurations found and loaded. Took {DateTime.Now.Subtract(statrt).TotalMilliseconds} ms.");
                    typeof(Program).LogVerbose($"Loading other configurations from {Settings.Default.PluginsDirectory}...");
                    statrt = DateTime.Now;
                    pluginCount = 0;
                    try
                    {
                        foreach (IPluginConfigParsable obj in PluginLoader<IPluginConfigParsable>.GetAll(path: Settings.Default.PluginsDirectory))
                        {
                            obj.GetStaticInstance().Parse(configuration.GetSection("Config"));
                            pluginCount++;
                        }
                    }
                    catch (Exception e)
                    {
                        typeof(Program).Log(e);
                    }
                    typeof(Program).LogVerbose($"{pluginCount} other configurations found and loaded. Took {DateTime.Now.Subtract(statrt).TotalMilliseconds} ms.");

                    services.AddHostedService<AsyncTaskWorkerWorker>();
                });

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                builder = builder.UseWindowsService();
            }
            else
            {
                builder = builder.UseSystemd();
            }

            return builder;
        }
    }
}
