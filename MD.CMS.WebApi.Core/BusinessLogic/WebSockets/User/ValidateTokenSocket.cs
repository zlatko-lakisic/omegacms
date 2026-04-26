using MD.CMS.BusinessLogic.WebApi.Core.Session;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MD.Tools.Helpers.Core.Logging;

namespace MD.CMS.WebApi.Core.BusinessLogic.WebSockets.User
{
    public class ValidateTokenSocket : IOmegaWebSocket
    {
        #region Attributes
        private string _tokenData;
        private MD.CMS.BusinessLogic.Core.DataAccess.Entities.User result;
        #endregion

        #region Properties

        public IEnumerable<string> UrlsToBindTo => new List<string>() { "User/ValidateTokenSocket" };

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
            if (context.Body != null)
            {
                try
                {
                    _tokenData = new StreamReader(context.Body).ReadToEnd();
                    MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session session = await SessionTable.GetLoggedOnSessionAsync(_tokenData);
                    if (session != null && session.UserId != "0")
                    {
                        if (session.DateAdded.Add(MD.CMS.BusinessLogic.Core.Properties.Settings.Default.SessionTimeout) < DateTime.Now)
                        {
                            typeof(ValidateTokenSocket).LogVerbose($"Session has expired, closing validatetokensocket...");
                            context.Result = System.Net.HttpStatusCode.Unauthorized;
                        }

                        result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(WebSocketHelpers.GetIsAdministration(context.QueryStrings)).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetByIdAsync(session.UserId, true, true);
                        
                        if (result == null || result.Id.Equals(default(long)))
                        {
                            typeof(ValidateTokenSocket).LogVerbose($"Session object is not null but user could not be retreived");
                            context.Result = System.Net.HttpStatusCode.Unauthorized;
                        }
                    } 
                    else
                    {
                        typeof(ValidateTokenSocket).LogVerbose($"Session object is{(session == null ? "" : " not ")} null and session id is {(session == null ? "0" : session.SessionId)}, token data is {_tokenData}");
                        context.Result = System.Net.HttpStatusCode.Unauthorized;
                    }
                }
                catch (Exception error)
                {
                    typeof(ValidateTokenSocket).Log(error);
                    context.Result = System.Net.HttpStatusCode.InternalServerError;
                }
            }
        }

        public async Task<Stream> OnSendAsync(OmegaSocketContext context)
        {
            if(result == null)
            {
                result = new CMS.BusinessLogic.Core.DataAccess.Entities.User();
            }
            return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)));
        }

        public IOmegaWebSocket Clone()
        {
            return new ValidateTokenSocket();
        }
        #endregion
    }
}