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
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ContentTypeDefinitionField")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content Type")]
    public class ContentTypeDefinitionFieldController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionField GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            ContentTypeDefinitionField contentTypeDefinitionField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (contentTypeDefinitionField == null)
                return NotFound();

            return Ok(contentTypeDefinitionField);
        }



        //id = contentTypeDefinitionId
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByContentTypeDefinition")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionField GetByContentTypeDefinition")]
        public async Task<IEnumerable<ContentTypeDefinitionField>> GetByContentTypeDefinition(long id)
        {
            ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync<ContentTypeDefinitionField>(id);
            if (contentTypeDefinition == null)
                return null;

            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByContentTypeDefinitionIdAsync(contentTypeDefinition.Id);
        }

        
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionField, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByContentTypeDefinition")]
        [OmegaInvalidateCache("GetById", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetAll", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetAllVersion", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetByAll", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("MenuContentGetContentByMenu", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetByFolderId", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("PaginationGetByFolderId", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetBySearchTerm", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("Translate", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        public async Task<IActionResult> Post([FromBody]ContentTypeDefinitionField contentTypeDefinitionField)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync<ContentTypeDefinitionField>(contentTypeDefinitionField.ContentTypeDefinitionId);
            AttributeTypeDefinition attributeTypeDefinition = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.AttributeTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(contentTypeDefinitionField.AttributeTypeDefinitionId);

            if (contentTypeDefinition == null || attributeTypeDefinition == null)
                return BadRequest();

            contentTypeDefinitionField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(contentTypeDefinitionField);

            if (contentTypeDefinition != null)
              return Ok(contentTypeDefinitionField);

            return BadRequest();
        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionField, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByContentTypeDefinition")]
        [OmegaInvalidateCache("GetById", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetAll", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetAllVersion", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetByAll", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("MenuContentGetContentByMenu", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetByFolderId", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("PaginationGetByFolderId", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetBySearchTerm", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("Translate", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        public async Task<IActionResult> Delete(long id)
        {
            ContentTypeDefinitionField contentTypeDefinitionField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (contentTypeDefinitionField == null)
                return NotFound();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(contentTypeDefinitionField))
                return Ok(new GenericResponse { Success = true });

            return BadRequest();
        }
    }
}
