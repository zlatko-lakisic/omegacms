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
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "FolderMetaDataField")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Folder")]
    public class FolderMetaDataFieldController : BaseLoggedOnWebApiController
    {
        //id - folderId, id2 - metaDataFieldId
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetByIds")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "FolderMetaDataField GetByIds")]
        public async Task<IActionResult> GetByIds(long id, long id2)
        {
            FolderMetaDataField folderMetaDataField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).FolderMetaDataFieldGetByIdsAsync(id, id2);

            if (folderMetaDataField == null)
                return NotFound();

            return Ok(folderMetaDataField);
        }


        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "FolderMetaDataField GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync());
        }

        //edin
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByFolderId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "FolderMetaDataField GetByFolderId")]
        public async Task<IActionResult> GetByFolderId(long id)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdAsync(id));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetUsedFolderMetaDataField")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "FolderMetaDataField GetUsedFolderMetaDataField")]
        public async Task<IActionResult> GetUsedFolderMetaDataField(int id)
        {
            if (id != 0)
            {
                IEnumerable<FolderMetaDataField> list = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetUsedMetaDataFieldsByFolderAsync(id);
                if (list != null)
                    return Ok(list);
            }

            return null;
            //return BadRequest();
        }
    }
}
