using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ContentTypeDefinitionFolder")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Folder")]
    public class ContentTypeDefinitionFolderController : BaseLoggedOnWebApiController
    {

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetByFolder")]
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
        public async Task<IActionResult> Post([FromBody]Folder<Content> folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Folder<Content> newfolder = new Folder<Content>();
            newfolder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(folder);

            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteByFolderIdAsync(newfolder.Id);
            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteByFolderIdAsync(newfolder.Id);


            foreach (ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition in folder.ContentTypeDefinitions)
            {
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(newfolder.Id, contentTypeDefinition);
            }          

            foreach (FolderMetaDataField item in folder.MetaDataFields)
            {
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).AssignMetaDataFieldToFolderAsync(newfolder.Id, item);
            }
            IEnumerable<FolderMetaDataField> used = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetUsedMetaDataFieldsByFolderAsync(newfolder.Id);

            foreach (FolderMetaDataField item in used)
            {
                newfolder.MetaDataFields.Add(item);
            }

            foreach (FolderMediaContentMetaDataField item in folder.FolderMediaContentMetaDataField)
            {
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).AssignMetaDataFieldToFolderAsync(newfolder.Id, item);
            }
            IEnumerable<FolderMetaDataField> usedfield = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetUsedMetaDataFieldsByFolderAsync(newfolder.Id);

            foreach (FolderMetaDataField item in usedfield)
            {
                FolderMediaContentMetaDataField toAdd = new FolderMediaContentMetaDataField()
                {
                    Name = item.Name,
                    IsRequired = item.IsRequired,
                    Checked = item.Checked
                };
                newfolder.FolderMediaContentMetaDataField.Add(toAdd);
            }

            Folder<Content> c = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(newfolder.Id);


            if (c == null)
                return BadRequest();


            return Ok(newfolder);

        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder, PermissionAccessTypeEnum.Delete)]
        [Route("[action]")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetByFolder")]
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
        public async Task<IActionResult> Delete([FromBody]Folder<Content> folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ContentTypeDefinitionFolder result = new ContentTypeDefinitionFolder();
            foreach (ContentTypeDefinition<ContentTypeDefinitionField> contenttypedefinition in folder.ContentTypeDefinitions)
            {
                result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(folder, contenttypedefinition);
            }

            Folder<Content> c = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(folder.Id);


            if (c == null)
                return BadRequest();

            return Ok(c);

        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByFolder")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentTypeDefinitionFolder GetByFolder")]
        public async Task<IActionResult> GetByFolder(long id)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (folder == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAsync(folder.Id));

        }

    }
}