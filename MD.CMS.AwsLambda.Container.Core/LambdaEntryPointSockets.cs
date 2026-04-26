using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using MD.CMS.BusinessLogic.AwsLambda.Core.Containers;
using MD.CMS.BusinessLogic.AwsLambda.Core.Properties;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace MD.CMS.AwsLambda.Container.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class LambdaEntryPointSockets
    {
        #region Methods
        private IAwsStartupSockets Loader()
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

            return AwsStartupTools.GetAwsStartupSockets(Settings.Default.WebAppPath, Settings.Default.AppReferencePath);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LambdaEntryPointSockets()
        {
        }

        /// <summary>
        /// OnConnect event handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<APIGatewayProxyResponse> OnConnectHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                return Loader().OnConnectHandler(request, context, Settings.Default.WebAppPath, Environment.GetEnvironmentVariables());
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

        /// <summary>
        /// SendMessage event handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<APIGatewayProxyResponse> SendMessageHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                return Loader().SendMessageHandler(request, context, Settings.Default.WebAppPath, Environment.GetEnvironmentVariables());
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

        /// <summary>
        /// OnDisconnect event handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<APIGatewayProxyResponse> OnDisconnectHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                return Loader().OnDisconnectHandler(request, context, Settings.Default.WebAppPath, Environment.GetEnvironmentVariables());
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
        #endregion
    }
}
