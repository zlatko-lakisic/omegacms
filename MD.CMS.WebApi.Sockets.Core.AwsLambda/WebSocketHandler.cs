using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;

namespace MD.CMS.WebApi.Sockets.Core.AwsLambda
{
    public class WebSocketHandler<T> : BaseWebSocketHandler<T>
        where T : IOmegaWebSocket, new()
    {
        #region Methods
        public WebSocketHandler() : base(new T())
        {
        }
        public WebSocketHandler(WebSocketHandler<T> obj) : base(obj)
        {
        }

        public override IWebSocketHandler Clone()
        {
            return new WebSocketHandler<T>(this);
        }
        #endregion
    }
}
