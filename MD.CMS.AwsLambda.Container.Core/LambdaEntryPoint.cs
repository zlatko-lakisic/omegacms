using System;
using MD.CMS.BusinessLogic.AwsLambda.Core.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;
using MD.CMS.BusinessLogic.AwsLambda.Core.Properties;
using Amazon.Lambda.AspNetCoreServer;

namespace MD.CMS.AwsLambda.Container.Core
{
    /// <summary>
    /// This class extends from APIGatewayProxyFunction which contains the method FunctionHandlerAsync which is the 
    /// actual Lambda function entry point. The Lambda handler field should be set to
    /// 
    /// MD.CMS.AwsLambda.Container.Core::MD.CMS.AwsLambda.Container.Core.LambdaEntryPoint::FunctionHandlerAsync
    /// </summary>
    public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        /// <summary>
        /// The builder has configuration, logging and Amazon API Gateway already configured. The startup class
        /// needs to be configured in this method using the UseStartup() method.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Init(IWebHostBuilder builder)
        {
            try
            {
                Settings.Default.ParseConfig();

                if (string.IsNullOrEmpty(Settings.Default.WebAppPath))
                {
                    throw new ArgumentOutOfRangeException(nameof(Settings.Default.WebAppPath));
                }

                if (Settings.Default.DebugMode)
                {
                    Console.WriteLine("Writing environmental variables..");
                    Console.WriteLine(JsonConvert.SerializeObject(Environment.GetEnvironmentVariables()));
                }

                foreach(string mimeType in Settings.Default.SupportedMimeTypes)
                {
                    RegisterResponseContentEncodingForContentType(mimeType, ResponseContentEncoding.Base64);
                }

                IAwsStartup startup = AwsStartupTools.GetAwsStartup(Settings.Default.WebAppPath, Settings.Default.AppReferencePath);

                builder.UseAwsStartup(startup, Settings.Default.WebAppPath, Environment.GetEnvironmentVariables());
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(JsonConvert.SerializeObject(e));
                throw;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(JsonConvert.SerializeObject(e));
                throw;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(JsonConvert.SerializeObject(e));
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(JsonConvert.SerializeObject(e));
                throw;
            }
        }
    }
}