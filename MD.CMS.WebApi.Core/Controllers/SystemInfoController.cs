using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Threading.Tasks;
using MD.Tools.BaseDataAccess.PluginMethods.Core.DataAccess;
using MD.CMS.WebApi.Core.Models;

namespace MD.CMS.WebApi.Core.Controllers
{
    [TokenAuth]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "General")]
    public class SystemInfoController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Route("[action]/{id?}")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.SystemInfo, PermissionAccessTypeEnum.Delete)]
        public async Task<IActionResult> RemoveJob(string id)
        {
            MD.CMS.BusinessLogic.Core.DataAccess.Controllers.SystemInfoController.GetNewInstance().Caller(await GetLoggedOnUser()).RemovePluginJob(id);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.SystemInfo, PermissionAccessTypeEnum.Read)]
        public async Task<IActionResult> GetCacheSize()
        {
            /*var statsField = typeof(MemoryCache).GetField("_stats", BindingFlags.NonPublic | BindingFlags.Instance);
            var statsValue = statsField.GetValue(MemoryCache.Default);
            var monitorField = statsValue.GetType().GetField("_cacheMemoryMonitor", BindingFlags.NonPublic | BindingFlags.Instance);
            var monitorValue = monitorField.GetValue(statsValue);
            var sizeField = monitorValue.GetType().GetField("_sizedRef", BindingFlags.NonPublic | BindingFlags.Instance);
            var sizeValue = sizeField.GetValue(monitorValue);
            var approxProp = sizeValue.GetType().GetProperty("ApproximateSize", BindingFlags.NonPublic | BindingFlags.Instance);
            return Ok((long)approxProp.GetValue(sizeValue, null));*/
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Init()
        {
            BaseDataAccessPlugins.Initialize();
            return Ok(InitModel.Default);
        }
    }
}
