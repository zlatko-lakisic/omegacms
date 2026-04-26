using System.Collections.Generic;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using System.IO;
using System.Text;
using System;
using Newtonsoft.Json;
using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using System.Collections.Concurrent;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets;

namespace MD.CMS.WebApi.Sockets.Core.AwsLambda
{
    public abstract class BaseWebSocketHandler<T> : IWebSocketHandler
        where T : IOmegaWebSocket
    {
        #region Attributes
        protected T _socketObject;
        private OmegaSocketContext _omegaSocket;
        private IAmazonApiGatewayManagementApi _gatewayApi;
        private string _requestId;
        #endregion

        #region Properties
        public IAmazonApiGatewayManagementApi GatewayApi { set => _gatewayApi = value; }
        public IEnumerable<string> RegisteredUrls => _socketObject.UrlsToBindTo;
        #endregion

        #region Methods
        private void LogError<E>(E error)
            where E : Exception
        {
            Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.{typeof(BaseWebSocketHandler<T>).Name} Exception {error.HResult} Message: {error.Message}");
            Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.{typeof(BaseWebSocketHandler<T>).Name} Exception {error.HResult} StackTrace: {error.StackTrace}");
            Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.{typeof(BaseWebSocketHandler<T>).Name} Exception {error.HResult} Data: {error.Data}");
        }
        private void LogError<E>(E error, string message, params object[] param)
            where E : Exception
        {
            LogError<E>(error);
            Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.{typeof(BaseWebSocketHandler<T>).Name} Exception Detail {string.Format(message, param)}");
        }

        private void LogInfo(string message, params object[] param)
        {
            if (Properties.Settings.Default.DebugMode)
            {
                Console.WriteLine($"MD.CMS.WebApi.Sockets.Core.AwsLambda.{typeof(BaseWebSocketHandler<T>).Name}: {string.Format(message, param)}");
            }
        }

        public BaseWebSocketHandler(T socketObject)
        {
            _socketObject = socketObject;
        }

        public BaseWebSocketHandler(BaseWebSocketHandler<T> obj)
        {
            _socketObject = obj._socketObject;
            _gatewayApi = obj._gatewayApi;
            _omegaSocket = obj._omegaSocket;
            _requestId = obj._requestId;
        }

        private void SetupContext(string requestId, WebSocketMessage message)
        {
            _requestId = requestId;
            _omegaSocket = new OmegaSocketContext();
            _omegaSocket.QueryStrings = new ConcurrentDictionary<string, string>();
            if (message != null)
            {
                try
                {
                    if (message.data.queryStrings != null)
                    {
                        _omegaSocket.QueryStrings = message.data.queryStrings;
                    }

                    _omegaSocket.Body = new MemoryStream(Encoding.UTF8.GetBytes(message.data.data.ToString()));
                }
                catch (Exception error)
                {
                    LogError(error, "A general exception occured while setting up the socket context, the request message is: \n{0}", JsonConvert.SerializeObject(message));
                }
            }
        }

        public async Task OnConnectHandler(string requestId, WebSocketMessage message)
        {
            SetupContext(requestId, message);
            await _socketObject.OnConnectedAsync(_omegaSocket);
        }

        public async Task SendMessageHandler(string requestId, WebSocketMessage message)
        {
            try
            {
                SetupContext(requestId, message);

                SocketModel socketModel = JsonConvert.DeserializeObject<SocketModel>(JsonConvert.SerializeObject(message.data));

                _omegaSocket.Body = new MemoryStream(Encoding.UTF8.GetBytes(socketModel.message));

                await _socketObject.OnReceiveAsync(_omegaSocket);

                try
                {
                    while (true)
                    {
                        using (Stream stream = await _socketObject.OnSendAsync(_omegaSocket))
                        {
                            if (stream != null)
                            {
                                string response = new StreamReader(stream).ReadToEnd();

                                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(response));

                                PostToConnectionRequest postConnectionRequest = new PostToConnectionRequest
                                {
                                    ConnectionId = _requestId,
                                    Data = memoryStream
                                };

                                await _gatewayApi.PostToConnectionAsync(postConnectionRequest);
                            }
                        }
                        if (_socketObject.MilisecondDelay.Equals(default))
                        {
                            break;
                        }

                        await Task.Delay(_socketObject.MilisecondDelay).ConfigureAwait(true);
                    }
                }
                catch (ForbiddenException error)
                {
                    LogError(error);
                    throw;
                }
                catch (GoneException error)
                {
                    LogError(error);
                    throw;
                }
                catch (LimitExceededException error)
                {
                    LogError(error);
                    throw;
                }
                catch (PayloadTooLargeException error)
                {
                    LogError(error);
                    throw;
                }
                catch (Exception error)
                {
                    LogError(error);
                    throw;
                }
            }
            catch (Exception error)
            {
                LogError(error);
                throw;
            }
        }

        public async Task OnDisconnectHandler(string requestId, WebSocketMessage message)
        {
            SetupContext(requestId, message);
            await _socketObject.OnCloseAsync(_omegaSocket);
        }

        public virtual IWebSocketHandler Clone()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
