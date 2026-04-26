using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ContentTypeDefinitionFolderDataBoundCondition")]
	[Route("[controller]")]
	[ApiController]
	[ApiExplorerSettings(GroupName = "Folder")]
	public class ContentTypeDefinitionFolderDataBoundConditionController : BaseLoggedOnWebApiController
    {

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundCondition, PermissionAccessTypeEnum.Read)]
		[Route("[action]/{id?}/{id2?}")]
		[ActionName("GetByFolderAndContentTypeDefinitionId")]
        //[MdOutputCacheAttribute(OutputCacheType = MdOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionFolderDataBoundCondition GetByFolderAndContentTypeDefinitionId")]
        public async Task<IActionResult> GetByFolderAndContentTypeDefinitionId(long id, long id2)
        {
			return Ok(await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAndContentTypeDefinitionIdAsync(id, id2));
		}

		[HttpPost]
		[Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundCondition, PermissionAccessTypeEnum.Write)]
		[Route("[action]")]
		[OmegaInvalidateCache("GetByFolderAndContentTypeDefinitionId")]
		[ActionName("Save")]
		public async Task<IActionResult> Post([FromBody]ContentTypeDefinitionFolderDataBoundCondition data)
		{
			ContentTypeDefinitionFolderDataBoundCondition newData = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(data);

			return Ok(newData);
		}

		[HttpPost]
		[Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundCondition, PermissionAccessTypeEnum.Write)]
		[Route("[action]")]
		[OmegaInvalidateCache("GetByFolderAndContentTypeDefinitionId")]
		[ActionName("SaveAll")]
		public async Task<IActionResult> Post([FromBody]ContentTypeDefinitionFolderDataBoundCondition[] data)
		{
			List<ContentTypeDefinitionFolderDataBoundCondition> newData = new List<ContentTypeDefinitionFolderDataBoundCondition>();
			foreach(ContentTypeDefinitionFolderDataBoundCondition condition in data)
			{
				newData.Add(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(condition));
			}

			return Ok(newData);
		}

		[HttpDelete]
		[Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundCondition, PermissionAccessTypeEnum.Read)]
		[Route("[action]/{id?}/{id2?}")]
		[OmegaInvalidateCache("GetByFolderAndContentTypeDefinitionId")]
		public async Task<IActionResult> DeleteAll(long id, long id2)
		{
			if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAllAsync(id, id2))
				return Ok();

			return BadRequest();
		}

		[HttpDelete]
		[Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundCondition, PermissionAccessTypeEnum.Read)]
		[Route("[action]/{id?}/{id2?}/{id3?}")]
		[OmegaInvalidateCache("GetByFolderAndContentTypeDefinitionId")]
		public async Task<IActionResult> Delete(long id, long id2, long id3)
		{
			if (await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(id, id2, id3))
				return Ok();

			return BadRequest();
		}
	}
}
