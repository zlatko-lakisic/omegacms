using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.Serializer;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Hosted.BusinessLogic
{
    public class ThreadSafeSocket
    {
        private WebSocket _socket;
        public string Id { get => _id; }

        public bool IsRunning { get => _isRunning; }
        public WebSocket Socket { get => _socket; }

        private SemaphoreSlim @lock = new SemaphoreSlim(1); 
        private IOmegaWebSocket _socketObject;
        private string _id;
        private CancellationToken _cancellationToken;
        private OmegaSocketContext _socketContext;
        private bool _isRunning;

        public ThreadSafeSocket(WebSocket socket, CancellationToken cancellationToken, Microsoft.AspNetCore.Http.HttpContext context, IOmegaWebSocket socketObject)
        {
            _socketContext = new OmegaSocketContext(context);
            _socket = socket;
            _socketObject = socketObject;
            _id = _socketContext.ConnectionId;
            _cancellationToken = cancellationToken;
        }

        public async Task ConnectedAsync()
        {
            await @lock.WaitAsync();
            try
            {
                await _socketObject.OnConnectedAsync(_socketContext);
                if (!await HandleSocketResultAsync())
                {
                    return;
                }
            }
            catch (WebSocketException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
            finally
            {
                @lock.Release();
            }
        }

        public async Task CloseAsync()
        {
            await @lock.WaitAsync();
            try
            {
                await _socketObject.OnCloseAsync(_socketContext);
                if (!await HandleSocketResultAsync())
                {
                    return;
                }
            }
            catch (WebSocketException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
            finally
            {
                @lock.Release();
            }
        }

        public async Task ReceiveAsync(byte[] buffer, Action<string, byte[], ThreadSafeSocket> handleMessage)
        {
            SocketModel socketModel = await ReceiveStreamAsync(buffer);

            if (socketModel != null)
            {
                _socketContext.Body = new MemoryStream(Encoding.UTF8.GetBytes(socketModel.message));
                await @lock.WaitAsync();
                try
                {
                    await _socketObject.OnReceiveAsync(_socketContext);
                    if (!await HandleSocketResultAsync())
                    {
                        return;
                    }
                }
                catch (WebSocketException error)
                {
                    throw;
                }
                catch (Exception error)
                {
                    throw;
                }
                finally
                {
                    @lock.Release();
                }
                handleMessage(socketModel.connectionId, buffer, this);
            }
        }

        public async Task SendAsync(OmegaSocketContext context)
        {
            await @lock.WaitAsync();
            try
            {
                while (true)
                {
                    using (Stream stream = await _socketObject.OnSendAsync(context))
                    {
                        if (stream != null)
                        {
                            string result = new StreamReader(stream).ReadToEnd();
                            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(OmegaJsonSerializer.SerializeObject(new SocketModel() { connectionId = _id, message = result })));
                            await _socket.SendAsync(memoryStream.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);
                            if (!await HandleSocketResultAsync())
                            {
                                return;
                            }
                        } 
                        else
                        {
                            break;
                        }
                    }
                    if (_socketObject.MilisecondDelay.Equals(default))
                    {
                        break;
                    }

                    await Task.Delay(_socketObject.MilisecondDelay).ConfigureAwait(true);
                }
            }
            catch (WebSocketException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
            finally
            {
                @lock.Release();
            }
        }

        public async Task BroadcastAsync(OmegaSocketContext context)
        {
            await @lock.WaitAsync();
            try
            {
                while (true)
                {
                    using (Stream stream = await _socketObject.OnBroadcastAsync(context))
                    {
                        if (stream != null)
                        {
                            string result = new StreamReader(stream).ReadToEnd();
                            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(OmegaJsonSerializer.SerializeObject(new SocketModel() { connectionId = _id, message = result })));
                            await _socket.SendAsync(memoryStream.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);
                            if (!await HandleSocketResultAsync())
                            {
                                return;
                            }
                        }
                    }
                    if (_socketObject.MilisecondDelay.Equals(default))
                    {
                        break;
                    }

                    await Task.Delay(_socketObject.MilisecondDelay).ConfigureAwait(true);
                }
            }
            catch (WebSocketException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
            finally
            {
                @lock.Release();
            }
        }

        public async Task Run()
        {
            /*using (Socket)
            {
                _isRunning = true;
                try
                {
                    string socketId = Id.ToString();
                    string broadcastMessage = string.Empty;

                    if (_socketObject.CatchOnConnected)
                    {
                        await _socketObject.OnConnected(_socketContext);
                    }

                    while (Socket.State == WebSocketState.Open)
                    {
                        await @lock.WaitAsync();
                        try
                        {
                            if (_cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }

                            _socketContext.Body = await ReceiveStreamAsync(Socket, _cancellationToken);

                            if (_socketObject.CatchOnReceive)
                            {
                                await _socketObject.OnReceive(_socketContext);
                                if (!(await HandleSocketResultAsync(Socket, _cancellationToken, _socketContext)))
                                {
                                    break;
                                }
                            }

                            if (_socketObject.CatchOnSendAsync)
                            {
                                await _socketObject.OnSendAsync(_socketContext);
                                if (!(await HandleSocketResultAsync(Socket, _cancellationToken, _socketContext)))
                                {
                                    break;
                                }
                            }

                            if (_socketObject.CatchOnBroadcast)
                            {
                                await _socketObject.OnBroadcast(_socketContext);
                                if (!(await HandleSocketResultAsync(Socket, _cancellationToken, _socketContext)))
                                {
                                    break;
                                }
                            }
                        }
                        catch (WebSocketException error)
                        {
                            throw;
                        }
                        catch (Exception error)
                        {
                            throw;
                        }
                        finally
                        {
                            @lock.Release();
                        }
                    }

                    if (_socketObject.CatchOnClose)
                    {
                        await _socketObject.OnClose(_socketContext);
                    }

                    if (Socket.State == WebSocketState.Open)
                    {
                        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", _cancellationToken);
                    }
                }
                catch (WebSocketException error)
                {
                    Logger.Log(error);
                    _socketFinishedTcs.SetException(error);
                }
                catch (Exception error)
                {
                    Logger.Log(error);
                    _socketFinishedTcs.SetException(error);
                }
                finally
                {
                    if (Socket != null && (Socket.State == WebSocketState.Open || Socket.State == WebSocketState.CloseReceived || Socket.State == WebSocketState.CloseSent))
                    {
                        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", _cancellationToken);
                    }
                    _isRunning = false;

                    if (!_isRunning)
                    {
                        _socketFinishedTcs.SetResult("Closed gradefully.");
                    }
                }
            }*/
        }

        private async Task<bool> HandleSocketResultAsync()
        {
            switch (_socketContext.Result)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    typeof(ThreadSafeSocket).LogVerbose("The user making the call is unauthorised, closing thread");
                    await _socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "The user making the call is unauthorised.", _cancellationToken);
                    return false;
                case System.Net.HttpStatusCode.InternalServerError:
                    typeof(ThreadSafeSocket).LogVerbose("The server experienced an error and the socket is closing, closing thread");
                    await _socket.CloseAsync(WebSocketCloseStatus.InternalServerError, "The server experienced an error and the socket is closing.", _cancellationToken);
                    return false;
            }
            return true;
        }

        protected async Task SendStreamAsync(Stream stream)
        {
            if (_socket.State == WebSocketState.Open)
            {
                string result = new StreamReader(stream).ReadToEnd();
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(OmegaJsonSerializer.SerializeObject(new SocketModel() { connectionId = _id, message = result })));
                await _socket.SendAsync(memoryStream.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private async Task<SocketModel> ReceiveStreamAsync(byte[] buffer)
        {
            if (_socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string resultValue = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    return OmegaJsonSerializer.DeserializeObject<SocketModel>(resultValue, null);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    typeof(ThreadSafeSocket).LogVerbose("Close message type received, closing thread");
                    await _socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                }
            }
            return null;
        }
    }
}
