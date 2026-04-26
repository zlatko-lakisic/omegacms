using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.ApiGatewayManagementApi;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.Message;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.User;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.SystemInfo;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.Permissions;
using Microsoft.Extensions.Configuration;
using MD.Tools.Helpers.Core.Plugins;
using MD.CMS.WebApi.Core.Properties;
using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using MD.Tools.Helpers.Core.Config;
using Amazon.Runtime;
using Newtonsoft.Json;
using MD.CMS.BusinessLogic.AwsLambda.Core.Containers;
using System.Collections;
using System.Linq;
using MD.CMS.BusinessLogic.Aws.Core.FileProviders.S3;
using MD.CMS.BusinessLogic.Aws.Core.ConfigParsers;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace MD.CMS.WebApi.Sockets.Core.AwsLambda
{
    public class Functions : IAwsStartupSockets
    {
        static Dictionary<string, IWebSocketHandler> _webSockets;
        static IConfigurationRoot configuration;
        static MD.CMS.WebApi.Core.Startup _startup;
        static bool loadedInitialConfiguration = false;
        static bool loadedConfigurationAndPlugins = false;
        static bool loadedSockets = false;

        public Functions()
        {
            Tools.Helpers.Core.FileProvider.DynamicFileProvider.AddFileProvider<AWSS3FileProvider>();
        }

        private void LogError<T>(T error)
            where T : Exception
        {
            Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.Functions Exception {error.HResult} Message: {error.Message}");
            Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.Functions Exception {error.HResult} StackTrace: {error.StackTrace}");
            Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.Functions Exception {error.HResult} Data: {error.Data}");
        }
        private void LogError<E>(E error, string message, params object[] param)
            where E : Exception
        {
            LogError<E>(error);
            Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.Functions Exception Detail {string.Format(message, param)}");
        }

        private void LogInfo(string message, params object[] param)
        {
            if (Properties.Settings.Default.DebugMode)
            {
                Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.Functions: {string.Format(message, param)}");
            }
        }

        private async Task SetupCmsAssets(string path)
        {
            MD.Tools.Helpers.Core.Properties.HelperSettings.Default.VerboseLoggingReflectionEnabled = false;
            MD.Tools.Helpers.Core.Properties.HelperSettings.Default.AwsCloudWatchLoggerIsEnabled = true;
            MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TraceSwitches = new Dictionary<string, string>();
            MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TraceSwitches.Add("AwsCloudWatchLogger", "4");
            MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TempAssembliesFolder = $"/tmp/{MD.CMS.BusinessLogic.AwsLambda.Core.Properties.Settings.Default.AppReferencePath.Split(".dll.").First()}";

            if (string.IsNullOrEmpty(path))
            {
                path = string.Empty;
            }

            LogInfo("Loading lambda parser");
            ConfigParser.Providers.Add(new LambdaConfigParser());

            if (!loadedInitialConfiguration)
            {
                LogInfo("Setting up configuration");
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{path}appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
                configuration = configurationBuilder.Build();
                loadedInitialConfiguration = true;
            }


            if (!loadedConfigurationAndPlugins)
            {
                LogInfo("Running startup");
                _startup = new WebApi.Core.Startup(configuration, $"{ReflectionHelper.GetDefaultPluginPath};{(!path.StartsWith("/") ? "/" : "")}{path}{(!path.EndsWith("/") ? "/" : "")}");
                LogInfo("Running preload configurations");
                _startup.PreloadConfigurations();
                LogInfo("Running preload plugins");
                _startup.PreloadPlugins();
                loadedConfigurationAndPlugins = true;
            }

            if (!loadedSockets)
            {
                LogInfo("Setting up socket handlers");
                _webSockets = new Dictionary<string, IWebSocketHandler>();
                RegisterWebSocket(new WebSocketHandler<GetUnreadByUserSocket>());
                RegisterWebSocket(new WebSocketHandler<ValidateTokenSocket>());
                RegisterWebSocket(new WebSocketHandler<GetAllJobs>());
                RegisterWebSocket(new WebSocketHandler<UserPermissionsSocket>());
                RegisterWebSocket(new WebSocketHandler<ProfileTypePermissionsSocket>());

                LogInfo("Adding socket handlers from plugins");
                foreach (IOmegaWebSocket pluginSocketObject in PluginLoader<IOmegaWebSocket>.GetAll(Settings.Default.PluginsFileProviderType, Settings.Default.PluginsDirectory))
                {
                    RegisterWebSocket(new PluginWebSocketHandler(pluginSocketObject));
                }

                LogInfo("Added {0} socket handlers from plugins", _webSockets.Count - 5);
                loadedSockets = true;
            }
        }

        public async Task<APIGatewayProxyResponse> OnConnectHandler(APIGatewayProxyRequest request, ILambdaContext context, string path = null, IDictionary environmentalVariables = null)
        {
            try
            {
                /* SetupCmsAssets(path);

                 string domainName = request.RequestContext.DomainName;
                 string stage = request.RequestContext.Stage;
                 string endpoint = $"https://{domainName}/{stage}";


                 LogInfo($"Now logging for request {endpoint}/{request.Path}");
                 LogInfo($"Request Id {request.RequestContext.RequestId}");

                 foreach (IWebSocketHandler socket in _webSockets)
                 {
                     await socket.OnConnectHandler(request, context);
                 }*/
            }
            catch (Exception e)
            {
                LogError(e);
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = $"Failed to send message: {e.Message}, stacktrace: {e.StackTrace}"
                };
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "Connected."
            };
        }
    
        public async Task<APIGatewayProxyResponse> SendMessageHandler(APIGatewayProxyRequest request, ILambdaContext context, string path = null, IDictionary environmentalVariables = null)
        {
            try
            {
                await SetupCmsAssets(path);
                WebSocketMessage message = GetSocketMessage(request, context);

                string domainName = request.RequestContext.DomainName;
                string stage = request.RequestContext.Stage;
                string endpoint = $"https://{domainName}/{stage}";


                LogInfo($"Now logging for request {endpoint}/{request.Path}");
                LogInfo($"Request Id {request.RequestContext.RequestId}");

                if (!string.IsNullOrEmpty(Settings.Default.BaseApiPath))
                {
                    if (!Settings.Default.BaseApiPath.StartsWith("/"))
                    {
                        endpoint = $"{endpoint}/";
                    }
                    endpoint = $"{endpoint}{Settings.Default.BaseApiPath}";
                }

                LogInfo("Starting {0}", endpoint);

                LogInfo("Workingh with {0} sockets", _webSockets.Count);
                foreach (KeyValuePair<string, IWebSocketHandler> socket in _webSockets)
                {
                    if (message != null &&
                        message.data != null &&
                        message.data.address != null &&
                        !string.IsNullOrEmpty(message.data.address) &&
                        message.data.address.ToLowerInvariant().Contains(socket.Key.ToLowerInvariant()))
                    {
                        socket.Value.GatewayApi = new AmazonApiGatewayManagementApiClient(new AmazonApiGatewayManagementApiConfig
                        {
                            ServiceURL = endpoint
                        });
                        await socket.Value.SendMessageHandler(request.RequestContext.ConnectionId, message);
                    }
                }
            }
            catch (AmazonServiceException e)
            {
                LogError(e);
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = $"Failed to send message: {e.Message}, stacktrace: {e.StackTrace}"
                };
            }
            catch (Exception e)
            {
                LogError(e);
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = $"Failed to send message: {e.Message}, stacktrace: {e.StackTrace}"
                };
            }
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "Connection OK."
            };
        }

        public async Task<APIGatewayProxyResponse> OnDisconnectHandler(APIGatewayProxyRequest request, ILambdaContext context, string path = null, IDictionary environmentalVariables = null)
        {
            try
            {
                /*SetupCmsAssets(path);

                string domainName = request.RequestContext.DomainName;
                string stage = request.RequestContext.Stage;
                string endpoint = $"https://{domainName}/{stage}";

                LogInfo($"Now logging for request {endpoint}/{request.Path}");
                LogInfo($"Request Id {request.RequestContext.RequestId}");

                foreach (IWebSocketHandler socket in _webSockets)
                {
                    await socket.OnDisconnectHandler(request, context);
                }*/
            }
            catch (Exception e)
            {
                LogError(e);
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = $"Failed to send message: {e.Message}, stacktrace: {e.StackTrace}"
                };
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "Disconnected."
            };
        }

        private void RegisterWebSocket(IWebSocketHandler socket)
        {
            foreach(string url in socket.RegisteredUrls)
            {
                if (!_webSockets.ContainsKey(url))
                {
                    _webSockets.Add(url, socket);
                }
            }
        }
        private WebSocketMessage GetSocketMessage(APIGatewayProxyRequest request, ILambdaContext context)
        {
            WebSocketMessage message = new WebSocketMessage();
            if (!string.IsNullOrEmpty(request.Body))
            {
                try
                {
                    string body = request.Body.Substring(0);
                    message = JsonConvert.DeserializeObject<WebSocketMessage>(body);
                }
                catch (JsonSerializationException error)
                {
                    LogError(error, "A JsonSerializationException exception occured while setting up the socket context, the request info is: \n{0}", JsonConvert.SerializeObject(request));
                }
                catch (Exception error)
                {
                    LogError(error, "A general exception occured while setting up the socket context, the request info is: \n{0}", JsonConvert.SerializeObject(request));
                }
            }
            return message;
        }
    }
}