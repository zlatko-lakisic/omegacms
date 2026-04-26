using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ContentTypeDefinitionDatasourceJoinController")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content Type")]
    public class ContentTypeDefinitionDatasourceJoinController : BaseLoggedOnWebApiController
    {

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSourceJoin, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetById/{id?}")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionDatasourceJoinController GetById")]
        public async Task<IActionResult> GetById(long id)
        {
			return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceJoinController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id));
        }

        //POST: ws/ContentTypeDefinition/Save
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSourceJoin, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById")]
        [ActionName("Save")]
        public async Task<IActionResult> Post([FromBody]ContentTypeDataSourceJoin data)
        {
			ContentTypeDataSourceJoin newData = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceJoinController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(data);

            return Ok(newData);
        }
		
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSourceJoin, PermissionAccessTypeEnum.Delete)]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById")]
		public async Task<IActionResult> Delete([FromBody]ContentTypeDataSourceJoin data)
        {
            if (await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceJoinController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(data))
                return Ok();

            return BadRequest();
		}
	}
}
