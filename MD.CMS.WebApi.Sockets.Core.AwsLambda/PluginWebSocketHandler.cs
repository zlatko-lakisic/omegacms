using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;

namespace MD.CMS.WebApi.Sockets.Core.AwsLambda
{
    public class PluginWebSocketHandler : BaseWebSocketHandler<IOmegaWebSocket>
    {
        #region Methods
        public PluginWebSocketHandler(IOmegaWebSocket socketHandler) : base(socketHandler)
        {
        }
        public PluginWebSocketHandler(PluginWebSocketHandler obj) : base(obj)
        {
        }

        public override IWebSocketHandler Clone()
        {
            return new PluginWebSocketHandler(this);
        }
        #endregion
    }
}
