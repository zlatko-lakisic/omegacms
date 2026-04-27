using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.CMS.BusinessLogic.Core.DataAccess;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options;
using MD.CMS.BusinessLogic.Core.Properties;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Folder")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Folder")]
    public class FolderController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder Get")]
        public virtual async Task<BasePaginationEntity<Folder<Content>>> Get([FromQuery] FolderRequestOptions options)
        {
            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Execute(options);
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder Post")]
        public virtual async Task<BasePaginationEntity<Folder<Content>>> Post([FromBody] FolderRequestOptions options)
        {
            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Execute(options);
        }

        //id2 = fillParent
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder GetById")]
        public async Task<IActionResult> GetById(long id, bool id2 = false)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, id2);

            if (folder != null)
            {
                if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(await GetLoggedOnUser(), folder, RWDPermissionType.Read))
                {
                    return Ok(folder);
                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Lcid]
        [Route("[action]")]
        [ActionName("GetByRequest")]
        [OmegaOutputCacheAttribute(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder GetByRequest")]
        public virtual async Task<IActionResult> GetByRequest([FromBody] FolderRequestOptions request)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Execute(request));
        }

        //id2 = depth
        //id = parentId
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetByParentId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder GetByParentId")]
        public async Task<IActionResult> GetByParentId(long id, int id2 = int.MaxValue)
        {
            IEnumerable<Folder<Content>> result = new List<Folder<Content>>();
            Folder<Content> parent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, false);
            if (parent != null)
            {
                if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(await GetLoggedOnUser(), parent, RWDPermissionType.Read))
                {
                    result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdAsync(id, id2);
                }
            }
            return Ok(result);
        }


        //id2 = depth
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetHierarchyByParentId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder GetHierarchyByParentId")]
        public async Task<IEnumerable<Folder<Content>>> GetHierarchyByParentId(long id, int id2 = 3)
        {
            return CastFoldersToDynamic(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetHierarchyByParentIdAsync(id, id2));
        }

        private IEnumerable<Folder<Content>> CastFoldersToDynamic(IEnumerable<Folder<Content>> folders)
        {
            return folders.Select(f => {
                Folder<Content> folder = new Folder<Content>();
                folder.Id = f.Id;
                folder.Name = f.Name;
                folder.ParentId = f.ParentId;
                folder.FolderPath = f.FolderPath;
                folder.Children = CastFoldersToDynamic(f.Children != null ? f.Children : new List<Folder<Content>>()).ToList();
                return folder;
            });
        }

        //id - searchTerm
        //id2 - parentid
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}/{id3?}")]
        [ActionName("Search")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder Search")]
        public async Task<IEnumerable<Folder<Content>>> Search(string id, long id2, bool id3)
        {
            string searchTerm = id;
            long parentId = id2;
            bool recursive = id3;
            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm, parentId, recursive);
        }

        //id = path
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetFolderByPath")]
        [TokenAuth]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder GetFolderByPath")]
        public async Task<IActionResult> GetFolderByPath([FromQuery]string id, [FromQuery]bool loadContents = false)
        {

            int lcid = DataAccessSettings.SelectedLcid;
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetFolderByPathAsync(id, true, true);
            if (folder != null)
            {
                folder.Children = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdAsync(folder.Id, 0)).ToList();
                if (loadContents)
                {
                    folder.Contents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdAsync(folder.Id, true, lcid);
                    folder.MediaContent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdAsync(folder.Id, lcid);
                }
                return Ok(folder);
            }
            return NotFound();
        }

        [HttpGet]
        [ActionName("GetFolderWithPaginationByPath")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder GetFolderWithPaginationByPath")]
        public async Task<IActionResult> GetFolderWithPaginationByPath([FromQuery]string path, [FromQuery]int pageIndex, [FromQuery]int pageSize, [FromQuery]string searchTerm, [FromQuery]bool fillContents, [FromQuery]bool fillMediaContents)
        {
            try
            {
                int lcid = DataAccessSettings.SelectedLcid;
                Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetFolderByPathAsync(path, true, true);
                if (folder != null)
                {
                    await Task.WhenAll(new List<Task> {
                        Task.Run(async () => {
                            BasePaginationEntity<Folder<Content>> childrenPagination = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdWithPaginationAsync(folder.Id, pageIndex, pageSize, searchTerm, 0);
                            folder.Children = childrenPagination.Items;
                            folder.ChildrenTotalCount = childrenPagination.TotalCount;
                        }),
                        Task.Run(async () => {
                            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(await GetLoggedOnUser(), folder, RWDPermissionType.Read))
                            {
                                if (fillContents)
                                {
                                    ContentTypeDefinition<ContentTypeDefinitionField> type = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAsync<Content, ContentTypeDefinitionField>(folder)).FirstOrDefault();
                                    BasePaginationEntity<Content> contentsPagination = new BasePaginationEntity<Content>();
                                    if (type != null && type.Fields.Any(field => field.DataBound))
                                    {
                                        contentsPagination = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.DataBoundContentController<Content>.GetNewInstance().DefaultPlugin(this.IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderWithPaginationAsync(folder, 0, pageSize, "");
                                    }
                                    else
                                    {
                                        contentsPagination = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(this.IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderWithPaginationAsync(folder, 0, pageSize, "", lcid: lcid);
                                    }
                                    folder.Contents = contentsPagination.Items;
                                    folder.ContentsTotalCount = contentsPagination.TotalCount;
                                }
                                if (fillMediaContents)
                                {
                                    BasePaginationEntity<MediaContent> mediaContentsPagination = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdWithPaginationAsync(folder.Id, 0, pageSize, "", lcid: lcid);
                                    folder.MediaContent = mediaContentsPagination.Items;
                                    folder.MediaContentTotalCount = mediaContentsPagination.TotalCount;
                                }
                            }
                        })
                    });
                    return Ok(folder);
                }
                return NotFound();
            }
            catch (Exception error)
            {
                MD.Tools.Helpers.Core.Logging.Logger.Log(error);
                throw;
            }
        }

        [HttpGet]
        [ActionName("GetByParentIdWithPagination")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder GetByParentIdWithPagination")]
        public async Task<IActionResult> GetByParentIdWithPagination([FromQuery] long parentId, [FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string searchTerm)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(parentId);
            if (folder != null)
            {
               return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdWithPaginationAsync(folder.Id, pageIndex, pageSize, searchTerm, 0));
            }
            return NotFound();
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("ContentTypeDefinitionsByFolder")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder ContentTypeDefinitionsByFolder")]
        public async Task<IEnumerable<ContentTypeDefinition<ContentTypeDefinitionField>>> ContentTypeDefinitionsByFolder(long id)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            IEnumerable<ContentTypeDefinition<ContentTypeDefinitionField>> content = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAsync<Content, ContentTypeDefinitionField>(folder);

            if (content == null)
                NotFound();

            return content;
        }

        //id = parentId
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{folderId}/{searchTerm}")]
        [ActionName("GetByParentIdCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Folder GetByParentIdCount")]
        public async Task<int> GetByParentIdCount(long folderId, string searchTerm)
        {
            int folderCount = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdCountAsync(folderId, searchTerm);
            return folderCount;
        }

        //id = folderId
        

        
        //id = assignedFieldsString
        [HttpPost]
        [ActionName("Save")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetFolderByPath")]
        [OmegaInvalidateCache("PaginationGetFolderByPath")]
        [OmegaInvalidateCache("GetFoldersForPaginationByParentId")]
        [OmegaInvalidateCache("ContentTypeDefinitionsByFolder")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("GetUsedFolderMetaDataField")]
        [OmegaInvalidateCache("GetAll", typeof (ContentTypeDefinitionController))]
        [OmegaInvalidateCache("GetAll", typeof (TemplateController))]
        [OmegaInvalidateCache("GetByFolder", typeof(ContentTypeDefinitionController))]
        [OmegaInvalidateCache("GetByFolder", typeof(TemplateController))]
        [OmegaInvalidateCache("GetByFolderId", typeof(MetaDataFieldController))]
        [OmegaInvalidateCache("GetAllProfileTypesWithPermissions", typeof(ProfileTypeController))]       
        public async Task<IActionResult> Post([FromBody]Folder<Content> folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await FolderExists(folder.ParentId))
                return BadRequest("Parent doesn't exist");

            Folder<Content> newFolder = new Folder<Content>();
            bool isAuthorized = false;

            if (folder.IsNew)
            {
                Folder<Content> parent = new Folder<Content>();
                if (folder.ParentId != default(long))
                {
                    parent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(folder.ParentId);
                    if (parent != null)
                    {
                        isAuthorized = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(await GetLoggedOnUser(), parent, RWDPermissionType.Write);
                    }
                }
                else
                {
                    User loggedOnUser = await GetLoggedOnUser();
                    isAuthorized = loggedOnUser != null && loggedOnUser.Id == Settings.Default.RootId();
                }
            }
            else
            {
                isAuthorized = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(await GetLoggedOnUser(), folder, RWDPermissionType.Write);
            }

            if (isAuthorized)
            {
                newFolder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(folder);
            }
            else
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "403");
            }


            return Ok(newFolder);
        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetFolderByPath")]
        [OmegaInvalidateCache("GetFoldersForPaginationByParentId")]
        [OmegaInvalidateCache("PaginationGetFolderByPath")]
        [OmegaInvalidateCache("ContentTypeDefinitionsByFolder")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("GetUsedFolderMetaDataField")]
        public async Task<IActionResult> Delete(long id)
        {

            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (folder == null)
                throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("Folder does not exist ", folder.Name));

            if (folder != null)
            {
                if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(await GetLoggedOnUser(), folder, RWDPermissionType.Delete))
                {
                    bool success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(folder);
                    if (!success)
                    {
                        throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("{0} folder is not deleted. Please try again.", folder.Name));
                    }
                }
                else
                    throw new HttpException((int)HttpStatusCode.Forbidden, "403");
            }


            return Ok();
        }

        private async Task<bool> FolderExists(long folderId)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(folderId);
            if (folder == null)
                return false;

            return true;
        }

    }
}
