using MD.CMS.BusinessLogic.Core.DataAccess;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;
using System.Linq;

namespace MD.CMS.WebApi.Core.Controllers
{
    [TokenAuth]
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Menu")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Menu")]
    public class MenuController : BaseLoggedOnWebApiController
    {

        //GET: ws/Menu/GetById/1        
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            bool fillParent = false;
            bool fillContents = true;
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers.GetValue("fillParent")))
                fillParent = HttpContext.Request.Headers.GetValue("fillParent").ToBoolean(false);

            Menu menu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, fillParent, fillContents: fillContents);

            if (menu == null)
                return NotFound();

            //menu.Items = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMenuId(menu.Id);
            return Ok(menu);
        }


        //GET: ws/Menu/GetByParentId/1      
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("GetByParentId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu GetByParentId")]
        public async Task<IActionResult> GetByParentId(long id)
        {
            int depth = 0;
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers.GetValue("depth")))
                depth = HttpContext.Request.Headers.GetValue("depth").ToInt32(default(int));

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdAsync(id, depth));
        }


        //GET: ws/Menu/GetByContent/1       
        //id - contentId
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("GetByContent")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu GetByContent")]
        public async Task<IActionResult> GetByContent(string id)
        {
            int depth = int.MaxValue;
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers.GetValue("depth")))
                depth = HttpContext.Request.Headers.GetValue("depth").ToInt32(int.MaxValue);

            Content content = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { id }
            })).FirstOrDefault(); 

            if (content == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByContentAsync(content, depth));
        }

        //GET: ws/Menu/GetAll
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu GetAll")]
        public async Task<IActionResult> GetAll()
        {
            int lcid = DataAccessSettings.SelectedLcid;
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(lcid));
        }

        //GET: ws/Menu/GetHierarchyByParentId/1      
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("GetHierarchyByParentId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu GetHierarchyByParentId")]
        public async Task<IActionResult> GetHierarchyByParentId(long id)
        {
            int depth = int.MaxValue;
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers.GetValue("depth")))
                depth = HttpContext.Request.Headers.GetValue("depth").ToInt32(int.MaxValue);

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetHierarchyByParentIdAsync(id, depth));
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [ActionName("GetMenuByPath")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu GetMenuByPath")]
        public async Task<IActionResult> GetMenuByPath([FromBody]GenericJsonSingleObject<long> obj)
        {
            int lcid = DataAccessSettings.SelectedLcid;


            Menu menu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetMenuByPathAsync(obj.ValueName, true, true, lcid);
            if (menu != null)
            {
                menu.Children = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdAsync(menu.Id, 1, lcid);
                menu.Items = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMenuIdAsync(menu.Id, lcid);
                menu.Contents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMenuAsync(menu);
                return Ok(menu);
            }
            return NotFound();
        }

        [HttpGet]
        [ActionName("PaginationGetMenuByPath")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu PaginationGetMenuByPath")]
        public async Task<IActionResult> PaginationGetMenuPath([FromQuery] string path, [FromQuery] long pageIndex, [FromQuery] int pageSize, [FromQuery] string searchTerm, [FromQuery] string sortString = "Order ASC")
        {
            int lcid = DataAccessSettings.SelectedLcid;
            Menu menu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetMenuByPathAsync(path, true, true, lcid);
            if (menu != null)
            {
                CMS.BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<Menu> childrenPagination = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdWithPaginationAsync(menu.Id, pageIndex, pageSize, sortString, searchTerm, lcid);
                menu.Children = childrenPagination.Items;
                menu.ChildrenTotalCount = childrenPagination.TotalCount;
                CMS.BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<MenuContent> itemsPagination = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMenuIdWithPaginationAsync(menu.Id, pageIndex, pageSize, searchTerm, lcid, sortString);
                menu.Items = itemsPagination.Items;
                menu.ItemsTotalCount = itemsPagination.TotalCount;
                return Ok(menu);
            }
            return NotFound();
        }

        [HttpGet]
        [ActionName("GetByParentIdWithPagination")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu GetByParentIdWithPagination")]
        public async Task<IActionResult> GetByParentIdWithPagination([FromQuery] long parentId, [FromQuery] long pageIndex, [FromQuery] int pageSize, [FromQuery] string searchTerm, [FromQuery] string sortString = "Order ASC")
        {
            int lcid = DataAccessSettings.SelectedLcid;
            Menu menu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(parentId);
            if (menu != null)
            {
                CMS.BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<Menu> childrenPagination = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdWithPaginationAsync(menu.Id, pageIndex, pageSize, sortString, searchTerm, lcid);
                return Ok(childrenPagination);
            }
            return NotFound();
        }


        //GET: ws/Menu/MenuSearchByName
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [ActionName("MenuSearchByName")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu MenuSearchByName")]
        public async Task<IActionResult> MenuSearchByName([FromQuery] string searchTerm, [FromQuery] int currentPage, [FromQuery] int pageSize, [FromQuery] string orderColumn, [FromQuery] bool reverseOrder)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).MenuSearchByNameAsync(searchTerm, currentPage, pageSize, orderColumn, reverseOrder));
        }

        //GET: ws/Menu/Search
        [HttpGet]
        [Lcid]
        [Route("[action]")]
        [ActionName("Search")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu Search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm, [FromQuery] int lcid, [FromQuery] long parentId, [FromQuery] bool recursion)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).MenusSearchAsync(searchTerm, lcid, parentId, recursion));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetByParentIdCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Menu GetByParentIdCount")]
        public async Task<IActionResult> GetByParentIdCount([FromQuery] long menuId, [FromQuery] int lcid, [FromQuery] string searchTerm)
        {
            return Ok(MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdCountAsync(menuId, lcid, searchTerm));
        }

        //POST: ws/Menu/Save
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [Lcid]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetMenuByPath")]
        [OmegaInvalidateCache("PaginationGetMenuByPath")]
        [OmegaInvalidateCache("MenuSearchByName")]
        [OmegaInvalidateCache("GetByParentIdCount")]      
        [OmegaInvalidateCache("GetByMenuId", typeof(MenuContentController))]
        [OmegaInvalidateCache("GetByMenuIdCount", typeof(MenuContentController))]
        [OmegaInvalidateCache("PaginationGetByMenuId", typeof(MenuContentController))]
        [OmegaInvalidateCache("MenuContentGetContentByMenu", typeof(ContentController))]
        public async Task<IActionResult> Post([FromBody]Menu menu)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);

            if (!await MenuExist(menu.ParentId))
                return BadRequest("Menu parent doesn't exist");

            if (menu.ParentId == menu.Id)
                return BadRequest();

            Menu newMenu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(menu);
                      

            if (newMenu == null)
                return BadRequest();

            return Ok(newMenu);
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Write)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("UpdateChildren")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetMenuByPath")]
        [OmegaInvalidateCache("PaginationGetMenuByPath")]
        [OmegaInvalidateCache("MenuSearchByName")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("GetByMenuId", typeof(MenuContentController))]
        [OmegaInvalidateCache("GetByMenuIdCount", typeof(MenuContentController))]
        [OmegaInvalidateCache("PaginationGetByMenuId", typeof(MenuContentController))]
        [OmegaInvalidateCache("MenuContentGetContentByMenu", typeof(ContentController))]
        public async Task<IActionResult> UpdateChildren([FromBody]Menu menu, long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (menu.ParentId == menu.Id)
                return BadRequest();

            long order = id;
            if (menu != null && menu.Children != null)
            {
                for (var i = 0; i < menu.Children.Count; i++)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateAsync(menu.Children[i], order);
                    order++;
                }

            }

            return Ok(menu);
        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("DeleteContent")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetMenuByPath")]
        [OmegaInvalidateCache("PaginationGetMenuByPath")]
        [OmegaInvalidateCache("MenuSearchByName")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("PaginationGetMenuByPath")]
        [OmegaInvalidateCache("GetByMenuId", typeof(MenuContentController))]
        [OmegaInvalidateCache("GetByMenuIdCount", typeof(MenuContentController))]
        [OmegaInvalidateCache("PaginationGetByMenuId", typeof(MenuContentController))]
        [OmegaInvalidateCache("MenuContentGetContentByMenu", typeof(ContentController))]
        public async Task<IActionResult> DeleteContent(string id, GenericJsonSingleObject<long> obj)
        {
            Menu menu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetMenuByPathAsync(obj.ValueName);
            Content content = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { id }
            })).FirstOrDefault();
            List<MenuContent> menuContent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMenuIdAsync(menu.Id);

            if (menu != null && menuContent != null)
            {

                MenuContent result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteMenu1Async(content.Id, content.LCID, content.DateCreated, menu);

                if (result == null)
                    return Ok(new GenericResponse { Success = true });
            }


            throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("Contents from {0} are not deleted successfully", menu.Name));
        }

        //DELETE: ws/Menu/Delete/1
        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [Lcid]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetMenuByPath")]
        [OmegaInvalidateCache("PaginationGetMenuByPath")]
        [OmegaInvalidateCache("MenuSearchByName")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("PaginationGetMenuByPath")]
        [OmegaInvalidateCache("GetByMenuId", typeof(MenuContentController))]
        [OmegaInvalidateCache("GetByMenuIdCount", typeof(MenuContentController))]
        [OmegaInvalidateCache("PaginationGetByMenuId", typeof(MenuContentController))]
        public async Task<IActionResult> Delete(long id)
        {
            Menu menu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (menu != null)
            {
                bool success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(menu);
                if (!success)
                {
                    throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("{0} menu is not deleted. Please try again.", menu.Name));
                }
            }
            return Ok();
        }


        //POST: ws/Menu/AssignContentToMenu/1/1
        //id=menuId, id2 = contentId
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu, PermissionAccessTypeEnum.Write)]
        [Route("[action]/{id?}/{id2?}")]
        [Lcid]
        [ActionName("AssignContentToMenu")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetMenuByPath")]
        [OmegaInvalidateCache("PaginationGetMenuByPath")]
        [OmegaInvalidateCache("MenuSearchByName")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("PaginationGetMenuByPath")]
        [OmegaInvalidateCache("GetByMenuId", typeof(MenuContentController))]
        [OmegaInvalidateCache("GetByMenuIdCount", typeof(MenuContentController))]
        [OmegaInvalidateCache("PaginationGetByMenuId", typeof(MenuContentController))]
        public async Task<IActionResult> AssignContentToMenu(long id, string id2)
        {
            if (id2 == default(long).ToString())
                return BadRequest();

            Menu menu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            Content content = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { id2 }
            })).FirstOrDefault();

            if (menu == null || content == null)
                return BadRequest();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).AssignContentToMenuAsync(menu, content))
                return Ok();

            return BadRequest();
        }


        private async Task<bool> MenuExist(long id)
        {
            Menu menu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (menu == null)
                return false;

            return true;
        }

    }
}
