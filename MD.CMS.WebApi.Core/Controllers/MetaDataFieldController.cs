using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "MetaDataField")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Metadata")]
    public class MetaDataFieldController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MetaDataField GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync());
        }


        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MetaDataField GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            MetaDataField metaDataField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (metaDataField == null)
                return NotFound();

            return Ok(metaDataField);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByFolderId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MetaDataField GetByFolderId")]
        public async Task<IActionResult> GetByFolderId(long id)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (folder == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAsync(folder));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("MetaDataMediaContentGetByFolderId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MetaDataField MetaDataMediaContentGetByFolderId")]
        public async Task<IActionResult> MetaDataMediaContentGetByFolderId(long id)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (folder == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).MetaDataMediaContentGetByFolderIdAsync(folder));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByFolder")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MetaDataField GetByFolder")]
        public async Task<IActionResult> GetByFolder(long id)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (folder == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAsync(folder));
        }


        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("PaginationGetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MetaDataField PaginationGetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int currentPageIndex, [FromQuery] int maxNumberOfRows, [FromQuery] string searchTerm, [FromQuery] string searchColumn, [FromQuery] string sort = null)
        {
            if (string.IsNullOrEmpty(sort))
            {
                sort = "Name ASC";
            }
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            searchColumn = System.Web.HttpUtility.UrlDecode(searchColumn);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllWithPaginationAsync(currentPageIndex, maxNumberOfRows, searchTerm, searchColumn, sort));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAllCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MetaDataField GetAllCount")]
        public async Task<IActionResult> GetAllCount([FromQuery] string searchTerm, [FromQuery] string searchColumn)
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            searchColumn = System.Web.HttpUtility.UrlDecode(searchColumn);
            int count = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SelectAllCountAsync(searchTerm, searchColumn);
            return Ok(count);
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("MetaDataMediaContentGetByFolderId")]
        [OmegaInvalidateCache("GetByFolder")]
        [OmegaInvalidateCache("PaginationGetAll")]
        [OmegaInvalidateCache("Search")]
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
        [OmegaInvalidateCache("GetById", typeof(FolderController))]
        [OmegaInvalidateCache("GetByParentId", typeof(FolderController))]
        [OmegaInvalidateCache("GetHierarchyByParentId", typeof(FolderController))]
        [OmegaInvalidateCache("GetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("PaginationGetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("ContentTypeDefinitionsByFolder", typeof(FolderController))]
        [OmegaInvalidateCache("GetByParentIdCount", typeof(FolderController))]
        [OmegaInvalidateCache("GetUsedFolderMetaDataField", typeof(FolderController))]
        [OmegaInvalidateCache("GetUsedFolderMediaContentMetaDataField", typeof(FolderController))]
        public async Task<IActionResult> Post([FromBody]MetaDataField metaDataField)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            metaDataField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(metaDataField);

            if (metaDataField == null)
                return BadRequest();

            return Ok(metaDataField);
        }


        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("MetaDataMediaContentGetByFolderId")]
        [OmegaInvalidateCache("GetByFolder")]
        [OmegaInvalidateCache("PaginationGetAll")]
        [OmegaInvalidateCache("Search")]
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
        [OmegaInvalidateCache("GetById", typeof(FolderController))]
        [OmegaInvalidateCache("GetByParentId", typeof(FolderController))]
        [OmegaInvalidateCache("GetHierarchyByParentId", typeof(FolderController))]
        [OmegaInvalidateCache("GetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("PaginationGetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("ContentTypeDefinitionsByFolder", typeof(FolderController))]
        [OmegaInvalidateCache("GetByParentIdCount", typeof(FolderController))]
        [OmegaInvalidateCache("GetUsedFolderMetaDataField", typeof(FolderController))]
        [OmegaInvalidateCache("GetUsedFolderMediaContentMetaDataField", typeof(FolderController))]
        public async Task<IActionResult> Delete(long id)
        {
            MetaDataField metaDataField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (metaDataField == null)
                return NotFound();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(metaDataField))
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("Search")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MetaDataField Search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm, [FromQuery] string searchColumn = "All")
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "No search term recieved");
            }
            List<MetaDataField> results = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm, searchColumn);
            return Ok(results);
        }

    }
}
