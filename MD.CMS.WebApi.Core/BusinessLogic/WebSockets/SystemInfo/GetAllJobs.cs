using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using MD.CMS.WebApi.Core.Properties;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MD.Tools.Helpers.Core.Logging;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.BaseDataAccess.Plugins.Core;

namespace MD.CMS.WebApi.Core.BusinessLogic.WebSockets.SystemInfo
{
    public class GetAllJobs : IOmegaWebSocket
    {
        #region Attributes
        private IEnumerable<PluginJob> jobs = new List<PluginJob>();
        #endregion

        #region Properties

        public IEnumerable<string> UrlsToBindTo => new List<string>() { "SystemInfo/GetAllJobs" };

        public int MilisecondDelay => (int)Settings.Default.SystemInfoGetAllJobsInterval.TotalMilliseconds;
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
                jobs = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.SystemInfoController.GetNewInstance().Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetAllPluginJobs();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                typeof(GetAllJobs).Log(error);
                context.Result = System.Net.HttpStatusCode.InternalServerError;
            }
        }

        public async Task<Stream> OnSendAsync(OmegaSocketContext context)
        {
            try
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jobs)));
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                typeof(GetAllJobs).Log(error);
                context.Result = System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }

        public IOmegaWebSocket Clone()
        {
            return new GetAllJobs();
        }
        #endregion
    }
}