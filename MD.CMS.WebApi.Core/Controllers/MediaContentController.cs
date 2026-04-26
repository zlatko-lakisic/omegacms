using MD.CMS.BusinessLogic.Core.DataAccess;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "MediaContent")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Media Content")]
    public class MediaContentController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            // int lcid = Int32.Parse(HttpContext.Request.Headers["LCID"]);
            int lcid = DataAccessSettings.SelectedLcid;

            MediaContent mediacontent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, lcid);
            if (mediacontent != null)
            {
                if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(mediacontent, await GetLoggedOnUser(), RWDPermissionType.Read))
                {
                    return Ok(mediacontent);
                }
                else 
                {
                    throw new HttpException((int)HttpStatusCode.Forbidden, "403");
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("GetByIdWithMetaData")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent GetById")]
        public async Task<IActionResult> GetByIdWithMetaData(long id)
        {
            // int lcid = Int32.Parse(HttpContext.Request.Headers["LCID"]);
            int lcid = DataAccessSettings.SelectedLcid;

            MediaContent mediacontent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, lcid, true);
            if (mediacontent != null)
            {
                if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(mediacontent, await GetLoggedOnUser(), RWDPermissionType.Read))
                {
                    return Ok(mediacontent);
                }
                else
                {
                    throw new HttpException((int)HttpStatusCode.Forbidden, "403");
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent GetAll")]
        public async Task<IActionResult> GetAll()
        {

            int lcid = DataAccessSettings.SelectedLcid;
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(lcid));
        }


        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{folderId}/{lcid}/{pageIndex}/{pageSize}/{searchTerm}/{sort}")]
        [Lcid]
        [ActionName("GetWithPaginationByFolderId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent GetWithPaginationByFolderId")]
        public async Task<IActionResult> GetWithPaginationByFolderId(long folderId, int lcid, long pageIndex, long pageSize, string searchTerm, string sort = "Name ASC")
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdWithPaginationAsync(folderId, pageIndex, pageSize, searchTerm, lcid: lcid, sort: sort));

        }
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetByFolderIdCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent GetByFolderIdCount")]
        public async Task<IActionResult> GetByFolderIdCount(long folderId, int lcid, string searchTerm)
        {
            int mediaContentCount = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdCountAsync(folderId, lcid, searchTerm);
            return Ok(mediaContentCount);
        }

        //id = lcid
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("SelectAllCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent SelectAllCount")]
        public async Task<IActionResult> SelectAllCount(int id = default(int))
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SelectAllCountAsync(id));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByFolderId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent GetByFolderId")]
        public async Task<IActionResult> GetByFolderId(long id)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdAsync(id, lcid));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByFileType")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent GetByFileType")]
        public async Task<IActionResult> GetByFileType(long id)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFileTypeAsync(id, lcid));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("SearchByFileType")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent SearchByFileType")]
        public async Task<IActionResult> SearchByFileType(string id, int id2)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchByFileTypeAsync(id, id2, lcid));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{searchTerm}")]
        [ActionName("Search")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MediaContent Search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            List<MediaContent> searchResults = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm, lcid);
            return Ok(searchResults);
        }
        [HttpPost]
        [ActionName("Save")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetWithPaginationByFolderId")]
        [OmegaInvalidateCache("GetByFolderIdCount")]
        [OmegaInvalidateCache("SelectAllCount")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("GetByFileType")]
        [OmegaInvalidateCache("Search")]
        [OmegaInvalidateCache("GetByMediaContentId", typeof(MediaContentController))]
        [OmegaInvalidateCache("GetByMediaContentId", typeof(MediaContentMetaDataFieldValuesController))]
        public async Task<IActionResult> Post([FromBody]MediaContent mediaContent)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            int lcid = DataAccessSettings.SelectedLcid;
            MediaContent newContent = new MediaContent();
            mediaContent.LCID = lcid;

            bool isAuthorized = false;
            if (mediaContent.IsNew)
            {
                Folder<Content> folder = new Folder<Content>();
                if (mediaContent.FolderId != default(long))
                {
                    folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(mediaContent.FolderId);
                    if (folder != null)
                    {
                        isAuthorized = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(await GetLoggedOnUser(), folder, RWDPermissionType.Write);
                    }
                }
            }
            else
            {
                isAuthorized = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(mediaContent, await GetLoggedOnUser(), RWDPermissionType.Write);
            }

            if (isAuthorized)
            {
                newContent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(mediaContent);
                if (newContent != null)
                    return Ok(newContent);
                else
                    throw new HttpException((int)HttpStatusCode.NotFound);
            }
            else
            {
                throw new HttpException((int)HttpStatusCode.Forbidden);
            }
        }



        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetWithPaginationByFolderId")]
        [OmegaInvalidateCache("GetByFolderIdCount")]
        [OmegaInvalidateCache("SelectAllCount")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("GetByMediaContentId", typeof(MediaContentController))]
        [OmegaInvalidateCache("GetByMediaContentId", typeof(MediaContentMetaDataFieldValuesController))]
        [OmegaInvalidateCache("PaginationGetFolderByPath", typeof(FolderController))]
        public async Task<IActionResult> Delete(long id)
        {
            MediaContent mediaContent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (mediaContent == null)
                throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("MediaContent does not exist ", mediaContent.Name));

            if (mediaContent != null)
            {
                if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(mediaContent, await GetLoggedOnUser(), RWDPermissionType.Delete))
                {
                    bool success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(mediaContent);
                    if (!success)
                    {
                        throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("{0} mediaContent is not deleted. Please try again.", mediaContent.Name));
                    }
                }
                else
                {
                    throw new HttpException((int)HttpStatusCode.Forbidden);
                }
            }


            return Ok(new GenericResponse { Success = true });
        }
    }
}