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
    public class ProfileTypePermissionsSocket : IOmegaWebSocket
    {
        #region Attributes
        private string _tokenData;
        private List<ProfileTypePermissions> permissions = new List<ProfileTypePermissions>();
        #endregion

        #region Properties

        public IEnumerable<string> UrlsToBindTo => new List<string>() { "Permissions/ProfileTypePermissionsSocket" };

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
            MD.CMS.BusinessLogic.Core.DataAccess.Entities.User loggedOnUser = await WebSocketHelpers.GetLoggedOnUserAsync(WebSocketHelpers.GetIsAdministration(context.QueryStrings), new StreamReader(context.Body).ReadToEnd());
            if (loggedOnUser != null)
            {
                foreach (MD.CMS.BusinessLogic.Core.DataAccess.Entities.ProfileType profile in loggedOnUser.ProfileTypes)
                {
                    permissions.AddRange(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(WebSocketHelpers.GetIsAdministration(context.QueryStrings)).GetAllPermissionsByProfileTypeAsync(profile));
                }
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
                typeof(ProfileTypePermissionsSocket).Log(error);
                context.Result = System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }

        public IOmegaWebSocket Clone()
        {
            return new ProfileTypePermissionsSocket();
        }
        #endregion
    }
}