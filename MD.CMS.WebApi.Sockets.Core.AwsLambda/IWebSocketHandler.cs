using Amazon.ApiGatewayManagementApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Sockets.Core.AwsLambda
{
    public interface IWebSocketHandler
    {
        Task OnConnectHandler(string requestId, WebSocketMessage message);
        Task SendMessageHandler(string requestId, WebSocketMessage message);
        Task OnDisconnectHandler(string requestId, WebSocketMessage message);
        IAmazonApiGatewayManagementApi GatewayApi { set; }
        IEnumerable<string> RegisteredUrls { get; }
        IWebSocketHandler Clone();
    }
}
