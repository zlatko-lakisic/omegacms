using System.Threading.Tasks;
using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.IO;
using System.Text;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.Logging;

namespace MD.CMS.WebApi.Core.BusinessLogic.WebSockets.Permissions
{
    public class UserPermissionsSocket : IOmegaWebSocket
    {
        #region Attributes
        private List<UserPermissions> permissions = new List<UserPermissions>();
        #endregion

        #region Properties
        public IEnumerable<string> UrlsToBindTo => new List<string>() { "Permissions/UserPermissionsSocket" };

        public int MilisecondDelay => default;
        #endregion

        #region Methods
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
            try
            {
                MD.CMS.BusinessLogic.Core.DataAccess.Entities.User user = await WebSocketHelpers.GetLoggedOnUserAsync(WebSocketHelpers.GetIsAdministration(context.QueryStrings), new StreamReader(context.Body).ReadToEnd());
                if (user != null)
                {
                    permissions.AddRange(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(WebSocketHelpers.GetIsAdministration(context.QueryStrings)).GetAllPermissionsByUserAsync(user));
                }
            }
            catch (Exception error)
            {
                typeof(UserPermissionsSocket).Log(error);
                context.Result = System.Net.HttpStatusCode.InternalServerError;
            }
        }

        public async Task<Stream> OnSendAsync(OmegaSocketContext context)
        {
            try
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(permissions)));
            }
            catch (Exception error)
            {
                typeof(UserPermissionsSocket).Log(error);
                context.Result = System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }

        public IOmegaWebSocket Clone()
        {
            return new UserPermissionsSocket();
        }
        #endregion
    }
}