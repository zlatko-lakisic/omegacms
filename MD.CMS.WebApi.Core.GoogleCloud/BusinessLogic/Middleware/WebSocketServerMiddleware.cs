using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using MD.Tools.Helpers.Core.Logging;
using Microsoft.AspNetCore.Http;

namespace MD.CMS.WebApi.Core.GoogleCloud.BusinessLogic.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;

        private WebSocketServerConnectionManager _manager;

        public WebSocketServerMiddleware(RequestDelegate next, WebSocketServerConnectionManager manager)
        {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                IOmegaWebSocket socketObj = Startup.Sockets.Select(kvp => kvp.Value).FirstOrDefault(socket => {
                    return socket.UrlsToBindTo.Any(url => context.Request.Path.ToString().ToLowerInvariant().Contains(url.ToLowerInvariant()));
                });

                if (socketObj != null) {

                    OmegaSocketContext omegaSocketContext = new OmegaSocketContext(context);

                    ThreadSafeSocket threadSafeSocket = new ThreadSafeSocket(webSocket, context.RequestAborted, context, socketObj.Clone());

                    _manager.AddSocket(threadSafeSocket);
                    await threadSafeSocket.ConnectedAsync();

                    await Receive(threadSafeSocket, async (id, buffer, threadSafeSocket) =>
                    {
                        KeyValuePair<string, ThreadSafeSocket> threadSafeSocketKvp = _manager.GetAllSockets().FirstOrDefault(s => s.Key == id);

                        if (threadSafeSocket != null)
                        {
                            await threadSafeSocketKvp.Value.SendAsync(omegaSocketContext);
                        }
                        else
                        {
                            foreach (KeyValuePair<string, ThreadSafeSocket> sock in _manager.GetAllSockets())
                            {
                                if (sock.Value.Socket.State == WebSocketState.Open)
                                {
                                    await sock.Value.BroadcastAsync(omegaSocketContext);
                                }
                            }
                        }
                    });
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task Receive(ThreadSafeSocket socket, Action<string, byte[], ThreadSafeSocket> handleMessage)
        {
            try
            {
                byte[] buffer = new byte[1024 * 4];
                while (socket.Socket.State == WebSocketState.Open)
                {
                    await socket.ReceiveAsync(buffer, handleMessage);
                }
            }
            catch (WebSocketException error)
            {
                typeof(WebSocketServerMiddleware).Log(error);
            }
            catch (Exception error)
            {
                typeof(WebSocketServerMiddleware).Log(error);
            }
            finally
            {
                await socket.CloseAsync();
                ThreadSafeSocket sock;
                _manager.GetAllSockets().TryRemove(socket.Id, out sock);
            }
        }
    }
}