using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ContentTypeDefinition")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content Type")]
    public class ContentTypeDefinitionController : BaseLoggedOnWebApiController
    {

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinition GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync<ContentTypeDefinitionField>(id, fillFields: true, transformExpression: false);
            if (contentTypeDefinition == null)
                return NotFound();

            return Ok(contentTypeDefinition);
        }


        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByFolder")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinition GetByFolder")]
        public async Task<IEnumerable<ContentTypeDefinition<ContentTypeDefinitionField>>> GetByFolder(long id)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (folder == null)
                return null;

            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAsync<Content, ContentTypeDefinitionField>(folder);

        }

        //GET: ws/ContentTypeDefinition/GetAll
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinition GetAll")]
        public async Task<IEnumerable<ContentTypeDefinition<ContentTypeDefinitionField>>> GetAll()
        {
            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync<ContentTypeDefinitionField>();
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("PaginationGetAll")]
        // [MdOutputCacheAttribute(OutputCacheType = MdOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinition PaginationGetAll")]
        public async Task<IActionResult> PaginationGetAll([FromQuery]int currentPageIndex, [FromQuery]int maxNumberOfRows, [FromQuery]string searchTerm, [FromQuery]string searchColumn, [FromQuery]string sort = null)
        {
            if (String.IsNullOrEmpty(sort))
            {
                sort = "Name ASC";
            }
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = HttpUtility.UrlDecode(searchTerm);
            searchColumn = HttpUtility.UrlDecode(searchColumn);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllWithPaginationAsync<ContentTypeDefinitionField>(currentPageIndex, maxNumberOfRows, searchTerm, searchColumn, sort));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{searchTerm?}/{searchColumn?}")]
        [ActionName("GetAllCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinition GetAllCount")]
        public async Task<int> GetAllCount(string searchTerm, string searchColumn)
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = HttpUtility.UrlDecode(searchTerm);
            searchColumn = HttpUtility.UrlDecode(searchColumn);
            int count = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SelectAllCountAsync(searchTerm, searchColumn);
            return count;
        }


        //POST: ws/ContentTypeDefinition/Save
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByFolder")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("PaginationGetAll")]
        [OmegaInvalidateCache("GetAllCount")]
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
        [OmegaInvalidateCache("GetByContentTypeDefinition", typeof(ContentTypeDefinitionFieldController))]
        [OmegaInvalidateCache("PaginationGetFolderByPath", typeof(FolderController))] //because folder can own some content types assigned
        [OmegaInvalidateCache("Search")]
        [ActionName("Save")]
        public async Task<IActionResult> Post([FromBody]ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest();
            if (contentTypeDefinition.IsEditable == false)
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "This content type is not editable");
            }
            ContentTypeDefinition<ContentTypeDefinitionField> newContentType = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(contentTypeDefinition);

            return Ok(newContentType);
        }

        //DELETE: ws/ContentTypeDefinition/Delete/1
        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByFolder")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("PaginationGetAll")]
        [OmegaInvalidateCache("GetAllCount")]
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
        [OmegaInvalidateCache("PaginationGetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("GetByContentTypeDefinition", typeof(ContentTypeDefinitionFieldController))]
        [OmegaInvalidateCache("Search")]
        public async Task<IActionResult> Delete(long id)
        {
            ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync<ContentTypeDefinitionField>(id);
            if (contentTypeDefinition == null)
                return NotFound();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(contentTypeDefinition))
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinition Search")]
        [Route("[action]")]
        [ActionName("Search")]
        public async Task<IActionResult> Search(string searchTerm, string searchColumn)
        {
            List<ContentTypeDefinition<ContentTypeDefinitionField>> searchResults = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().Caller(await GetLoggedOnUser()).SearchAsync<ContentTypeDefinitionField>(searchTerm, searchColumn);
            return Ok(searchResults);
        }

	}
}
