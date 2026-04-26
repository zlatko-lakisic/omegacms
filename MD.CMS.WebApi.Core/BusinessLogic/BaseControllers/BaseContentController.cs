using MD.CMS.BusinessLogic.WebApi.Core.ActionResults;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using MD.CMS.WebApi.Core.Controllers;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.BusinessLogic.BaseControllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Content")]
    public abstract class BaseContentController<T> : MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<T>
        where T : MD.CMS.BusinessLogic.Core.DataAccess.Entities.Content, new()
    {
        #region Web Methods

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("SelectAllCount")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllVersion")]
        [OmegaInvalidateCache("GetByAll")]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy")]
        [OmegaInvalidateCache("MenuContentGetContentByMenu")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("PaginationGetByFolderId")]
        [OmegaInvalidateCache("GetBySearchTerm")]
        [OmegaInvalidateCache("Translate")]
        [OmegaInvalidateCache("SelectByContentTypeDefinitionCount")]
        [OmegaInvalidateCache("GetByFolderIdCount")]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("GetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("PaginationGetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("GetByContent", typeof(ContentTypeDefinitionFieldValueController))]
        [OmegaInvalidateCache("GetByContentId", typeof(ContentTypeDefinitionFieldValueController))]
        [OmegaInvalidateCache("GetByMenuId", typeof(MenuContentController))]
        [OmegaInvalidateCache("GetAll", typeof(ContentAliasController))]  
        public override Task<ContentActionResult<T>> Post([FromBody]T content)
        {
            return base.Post(content);
        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("SelectAllCount")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllVersion")]
        [OmegaInvalidateCache("GetByAll")]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy")]
        [OmegaInvalidateCache("MenuContentGetContentByMenu")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("PaginationGetByFolderId")]
        [OmegaInvalidateCache("GetBySearchTerm")]
        [OmegaInvalidateCache("Translate")]
        [OmegaInvalidateCache("SelectByContentTypeDefinitionCount")]
        [OmegaInvalidateCache("GetByFolderIdCount")]
        [OmegaInvalidateCache("GetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("PaginationGetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("GetByContent", typeof(ContentTypeDefinitionFieldValueController))]
        [OmegaInvalidateCache("GetByContentId", typeof(ContentTypeDefinitionFieldValueController))]
        [OmegaInvalidateCache("GetByMenuId", typeof(MenuContentController))]
        public override Task<ContentActionResult<T>> Delete(string id)
        {
            return base.Delete(id);
        }


        //id + content.Id, // id2 = dateCreated
        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("DeleteByAll")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("SelectAllCount")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllVersion")]
        [OmegaInvalidateCache("GetByAll")]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy")]
        [OmegaInvalidateCache("MenuContentGetContentByMenu")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("PaginationGetByFolderId")]
        [OmegaInvalidateCache("GetBySearchTerm")]
        [OmegaInvalidateCache("Translate")]
        [OmegaInvalidateCache("SelectByContentTypeDefinitionCount")]
        [OmegaInvalidateCache("GetByFolderIdCount")]
        [OmegaInvalidateCache("GetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("PaginationGetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("GetByContent", typeof(ContentTypeDefinitionFieldValueController))]
        [OmegaInvalidateCache("GetByContentId", typeof(ContentTypeDefinitionFieldValueController))]
        [OmegaInvalidateCache("GetByMenuId", typeof(MenuContentController))]
        public override async Task<ContentActionResult<T>> DeleteByAll(string id)
        {

            return base.GetByIdAsync(await base.DataAccess_DeleteByAllAsync(id));
        }

        #endregion
    }
}