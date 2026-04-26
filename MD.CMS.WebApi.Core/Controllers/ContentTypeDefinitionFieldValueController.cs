using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.Tools.Helpers.Core.Data;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;
using System.Linq;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ContentTypeDefinitionFieldValue")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content Type")]
    public class ContentTypeDefinitionFieldValueController : BaseLoggedOnWebApiController
    {
        //id = contentId
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByContent")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionFieldValue GetByContent")]
        public async Task<IEnumerable<ContentTypeDefinitionFieldValue>> GetByContent(string id)
        {
            Content content = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { id }
            })).FirstOrDefault(); 

            if (content == null)
                return null;

            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByContentAsync(content);
        }
        //id = contentId
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id}")]
        [ActionName("GetByContentId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionFieldValue GetByContentId")]
        public async Task<IEnumerable<ContentTypeDefinitionFieldValue>> GetByContentId(string id)
        {
            Content content = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { id }
            })).FirstOrDefault();

            if (content == null)
                return null;

            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByContentIdAsync(content.Id, content.LCID, content.DateCreated);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{value?}/{contentTypeDefinitionId?}/{contentTypeDefinitionFieldId?}/{comparer?}/{transform?}")]
        [ActionName("GetByValue")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionFieldValue GetByValue")]
        public async Task<IEnumerable<ContentTypeDefinitionFieldValue>> GetByValue(string value, long contentTypeDefinitionId = default, long contentTypeDefinitionFieldId = default, ComparerTypeEnum comparer = ComparerTypeEnum.Equals, DataTransformEnum transform = DataTransformEnum.ToString)
        {
            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByValueAsync(value, contentTypeDefinitionId, contentTypeDefinitionFieldId, comparer, transform);
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("GetByContentId")]
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
        public async Task<IActionResult> Post([FromBody]ContentTypeDefinitionFieldValue contentTypeDefinitionFieldValue)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Content content = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { contentTypeDefinitionFieldValue.ContentId }
            })).FirstOrDefault();
            ContentTypeDefinition<ContentTypeDefinitionFieldValue> contentTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync<ContentTypeDefinitionFieldValue>(contentTypeDefinitionFieldValue.ContentTypeDefinitionId);
            ContentTypeDefinitionField contentTypeDefinitionField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(contentTypeDefinitionFieldValue.Id);

            if (content == null || contentTypeDefinition == null || contentTypeDefinitionField == null)
                return BadRequest();
            

            if (contentTypeDefinitionFieldValue.DateCreated == default(DateTime))
                contentTypeDefinitionFieldValue.DateCreated = DateTime.UtcNow;


            ContentTypeDefinitionFieldValue field = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(contentTypeDefinitionFieldValue);
                       

            return Ok(field);
        }
    }
}
