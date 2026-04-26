using MD.CMS.WebApi.Core.Hosted.BusinessLogic.Middleware;
using System;
using System.Collections.Concurrent;

namespace MD.CMS.WebApi.Core.Hosted.BusinessLogic
{
    public class WebSocketServerConnectionManager
    {
        private ConcurrentDictionary<string, ThreadSafeSocket> _sockets = new ConcurrentDictionary<string, ThreadSafeSocket>();

        public string AddSocket(ThreadSafeSocket socket)
        {
            _sockets.TryAdd(socket.Id, socket);
            Console.WriteLine("WebSocketServerConnectionManager-> AddSocket: WebSocket added with ID: " + socket.Id);
            return socket.Id;
        }

        public ConcurrentDictionary<string, ThreadSafeSocket> GetAllSockets()
        {
            return _sockets;
        }
    }
}