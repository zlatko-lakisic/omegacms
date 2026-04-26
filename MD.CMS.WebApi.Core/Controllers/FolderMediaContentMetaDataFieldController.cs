using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "FolderMediaContentMetaDataField")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Folder")]
    public class FolderMediaContentMetaDataFieldController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMediaContentMetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetByIds")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "FolderMediaContentMetaDataField GetByIds")]
        public async Task<IActionResult> GetByIds(long id, long id2)
        {
            FolderMediaContentMetaDataField folderMetaDataField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).FolderMetaDataFieldGetByIdsAsync(id, id2);

            if (folderMetaDataField == null)
                return NotFound();

            return Ok(folderMetaDataField);
        }


        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMediaContentMetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "FolderMediaContentMetaDataField GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync());
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMediaContentMetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByFolderId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "FolderMediaContentMetaDataField GetByFolderId")]
        public async Task<IActionResult> GetByFolderId(long id)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (folder == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAsync(folder));
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetMediaContentMetaDataFieldByFolder")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "FolderMediaContentMetaDataField GetMediaContentMetaDataFieldByFolder")]
        public async Task<IActionResult> GetMediaContentMetaDataFieldByFolder(long id)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (folder == null)
                return NotFound();

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetMediaContentMetaDataFieldByFolderAsync(id));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMediaContentMetaDataField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetUsedFolderMediaContentMetaDataField")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "FolderMediaContentMetaDataField GetUsedFolderMediaContentMetaDataField")]
        public async Task<IActionResult> GetUsedFolderMediaContentMetaDataField(int id)
        {
            if (id != 0)
            {
                IEnumerable<FolderMetaDataField> list = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetUsedMetaDataFieldsByFolderAsync(id);
                if (list != null)
                    return Ok(list);
            }

            return null;
            //return BadRequest();
        }
    }
}