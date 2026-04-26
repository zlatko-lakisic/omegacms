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
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ContentTypeDefinitionDatasourceController")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content Type")]
    public class ContentTypeDefinitionDatasourceController : BaseLoggedOnWebApiController
    {

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionDatasourceController GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            ContentTypeDataSource contentTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (contentTypeDefinition == null)
                return NotFound();

            return Ok(contentTypeDefinition);
        }


        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByContentTypeDefinitionId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionDatasourceController GetByContentTypeDefinitionId")]
        public async Task<IActionResult> GetByContentTypeDefinitionId(long id)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByContentTypeDefinitionIdAsync(id));

        }

        //POST: ws/ContentTypeDefinition/Save
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByContentTypeDefinitionId")]
        [ActionName("Save")]
        public async Task<IActionResult> Post([FromBody] ContentTypeDataSource data)
        {
            ContentTypeDataSource newData = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(data);

            return Ok(newData);
        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByContentTypeDefinitionId")]
        public async Task<IActionResult> Delete(long id)
        {
            ContentTypeDataSource contentTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (contentTypeDefinition == null)
                return NotFound();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(contentTypeDefinition))
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        public async Task<IActionResult> GetDataStructure([FromBody] ContentTypeDataSource data)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceController.GetNewInstance().Caller(await GetLoggedOnUser()).GetDataStructureAsync(data.DbType, data.ConnectionString));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDatabaseTypes()
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDataSourceController.GetNewInstance().Caller(await GetLoggedOnUser()).GetAllDatabaseTypesAsync());
        }

    }
}
