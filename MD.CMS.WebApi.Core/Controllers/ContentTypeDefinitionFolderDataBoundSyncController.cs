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
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ContentTypeDefinitionFolderDataBoundSync")]
	[Route("[controller]")]
	[ApiController]
	[ApiExplorerSettings(GroupName = "Folder")]
	public class ContentTypeDefinitionFolderDataBoundSyncController : BaseLoggedOnWebApiController
    {

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundSync, PermissionAccessTypeEnum.Read)]
		[Route("[action]/{id?}/{id2?}")]
		[ActionName("GetByFolderAndContentTypeDefinitionId")]
        //[MdOutputCacheAttribute(OutputCacheType = MdOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionFolderDataBoundCondition GetByFolderAndContentTypeDefinitionId")]
        public async Task<IActionResult> GetByFolderAndContentTypeDefinitionId(long id, long id2)
        {
            ContentTypeDefinitionFolderDataBoundSync newData = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderDataBoundSyncController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAndContentTypeDefinitionIdAsync(id, id2);

            if(newData == null)
            {
                return NotFound();
            }

            return Ok(newData);
		}

		[HttpPost]
		[Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundSync, PermissionAccessTypeEnum.Write)]
		[Route("[action]")]
		[OmegaInvalidateCache("GetByFolderAndContentTypeDefinitionId")]
		[ActionName("Save")]
		public async Task<IActionResult> Post([FromBody]ContentTypeDefinitionFolderDataBoundSync data)
		{
            ContentTypeDefinitionFolderDataBoundSync newData = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderDataBoundSyncController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(data);

			return Ok(newData);
		}

		[HttpDelete]
		[Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundSync, PermissionAccessTypeEnum.Delete)]
		[Route("[action]/{id?}/{id2?}")]
		[OmegaInvalidateCache("GetByFolderAndContentTypeDefinitionId")]
		public async Task<IActionResult> Delete(long id, long id2)
		{
			if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderDataBoundSyncController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(id, id2))
				return Ok();

			return BadRequest();
		}
	}
}
