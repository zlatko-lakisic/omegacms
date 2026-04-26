using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.WebApi.Core.Properties;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MD.Tools.Helpers.Core.Logging;

namespace MD.CMS.WebApi.Core.BusinessLogic.WebSockets.Message
{
    public class GetUnreadByUserSocket : IOmegaWebSocket
    {
        #region Properties
        public IEnumerable<string> UrlsToBindTo => new List<string>() { "message/GetUnreadByUserSocket" };
        private MD.CMS.BusinessLogic.Core.DataAccess.Entities.User user;
        public int MilisecondDelay => (int)Settings.Default.UnreadMessagesCheckInterval.TotalMilliseconds;
        #endregion

        #region Method
        public async Task<Stream> OnBroadcastAsync(OmegaSocketContext context)
        {
            return null;
        }

        public async Task OnCloseAsync(OmegaSocketContext context)
        {
        }

        public async Task OnConnectedAsync(OmegaSocketContext context)
        {
        }

        public async Task OnReceiveAsync(OmegaSocketContext context)
        {
            user = await WebSocketHelpers.GetLoggedOnUserAsync(context.QueryStrings);
        }

        public async Task<Stream> OnSendAsync(OmegaSocketContext context)
        {
            try
            {
                List<MD.CMS.BusinessLogic.Core.DataAccess.Entities.Message> messages = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().Caller(user).DefaultPlugin(WebSocketHelpers.GetIsAdministration(context.QueryStrings)).GetUnreadByUserAsync(user);
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messages)));
            }
            catch (Exception error)
            {
                typeof(GetUnreadByUserSocket).Log(error);
                context.Result = System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }

        public IOmegaWebSocket Clone()
        {
            return new GetUnreadByUserSocket();
        }
        #endregion
    }
}