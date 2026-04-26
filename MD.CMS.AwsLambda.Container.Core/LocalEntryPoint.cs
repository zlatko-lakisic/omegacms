using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MD.CMS.BusinessLogic.AwsLambda.Core.Properties;

namespace MD.CMS.AwsLambda.Container.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalEntryPoint
    {
        /// <summary>
        /// Default main application entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Settings.Default.ParseConfig();
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddAWSProvider();

                    // When you need logging below set the minimum level. Otherwise the logging framework will default to Informational for external providers.
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //AwsStartupTools.GetAwsStartup(Settings.Default.WebAppPath, Settings.Default.AppReferencePath).UseAwsStartup(webBuilder, Settings.Default.WebAppPath, Environment.GetEnvironmentVariables());
                });
    }
}