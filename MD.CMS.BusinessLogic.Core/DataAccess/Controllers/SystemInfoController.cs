using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.BaseDataAccess.Plugins.Core;


namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public class SystemInfoController : BaseController<SystemInfoController>
    {
        public IEnumerable<PluginJob> GetAllPluginJobs()
        {
            return PluginJobManager.GetAllJobs();
        }

        public void RemovePluginJob(string jobId)
        {
            PluginJobManager.RemoveJob(jobId);
        }
    }
}
