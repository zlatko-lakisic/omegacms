using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "LCID")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "General")]
    public class LCIDController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.LCID, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "LCID GetById")]
        public IActionResult GetById(int id = default(int))
        {
            LCID lcid = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.LcidController.GetNewInstance().GetById(id);

            if (lcid == null)
                return NotFound();

            return Ok(lcid);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.LCID, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "LCID GetAll")]
        public IEnumerable<LCID> GetAll()
        {
            return MD.CMS.BusinessLogic.Core.DataAccess.Controllers.LcidController.GetNewInstance().GetAll();
        }
    }
}
