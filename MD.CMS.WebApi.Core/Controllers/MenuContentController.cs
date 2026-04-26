using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;
using System.Linq;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "MenuContent")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Menu")]
    public class MenuContentController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByMenuId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MenuContent GetByMenuId")]
        public async Task<IActionResult> GetByMenuId(long id)
        {
            return Ok(MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMenuIdAsync(id));

        }

        //id2 = lcid
        [HttpGet]
        [ActionName("GetByMenuIdCount")]
        [Route("[action]/{menuId}/{lcid}/{searchTerm}")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MenuContent GetByMenuIdCount")]
        public async Task<IActionResult> GetByMenuIdCount(long menuId, int lcid, string searchTerm)
        {
            int menuContentCount = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMenuIdCountAsync(menuId, searchTerm, lcid);
            return Ok(menuContentCount);
        }

        [HttpGet]
        [ActionName("PaginationGetByMenuId")]
        [Route("[action]/{menuId}/{lcid}/{currentPageIndex}/{maxNumberOfRows}/{searchTerm}/{sort?}")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MenuContent PaginationGetByMenuId")]
        public virtual async Task<IActionResult> PaginationGetByMenuId(long menuId, int lcid, int currentPageIndex, int maxNumberOfRows, string searchTerm, string sort="Order ASC")
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMenuIdWithPaginationAsync(menuId, currentPageIndex, maxNumberOfRows, searchTerm, lcid, sort));
        }

        [HttpGet]
        [ActionName("Search")]
        [Route("[action]/{searchTerm}/{lcid}/{menuId}")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "MenuContent Search")]
        public virtual async Task<IActionResult> PaginationGetByMenuId(string searchTerm, int lcid, long menuId)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).MenuContentsSearchAsync(searchTerm,lcid,menuId));
        }

        [HttpDelete]
        //id2 = menuId    
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetByMenuId")]
        [OmegaInvalidateCache("GetByMenuIdCount")]
        [OmegaInvalidateCache("PaginationGetByMenuId")]
        public async Task<IActionResult> Delete(long id, long id2)
        {
            IEnumerable<MenuContent> menuContents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMenuIdAsync(id2);
            MenuContent menuContent = new MenuContent();


            foreach (MenuContent mc in menuContents)
            {
                if (mc.Id == id)
                {
                    menuContent = mc;
                }
            }

            //    if (menuContent == null)
            //        return NotFound();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(menuContent))
                return Ok();

            return BadRequest();
        }

        //POST: ws/MenuContent/Delete
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetByMenuId")]
        [OmegaInvalidateCache("GetByMenuIdCount")]
        [OmegaInvalidateCache("PaginationGetByMenuId")]
        public async Task<IActionResult> Deletemenu([FromBody]Menu menu)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            MenuContent result = new MenuContent();
            foreach (Content content in menu.Contents)
            {
                result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteMenuAsync(content, menu);
            }



            Menu c = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(menu.Id);

            return Ok(new GenericResponse { Success = true });

        }


        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent, PermissionAccessTypeEnum.Write)]
        [Route("[action]/{id?}")]
        [ActionName("Update")]
        [OmegaInvalidateCache("GetByMenuId")]
        [OmegaInvalidateCache("GetByMenuIdCount")]
        [OmegaInvalidateCache("PaginationGetByMenuId")]
        public async Task<IActionResult> Update([FromBody]Menu menu, int id)
        {
            //id is pageindex
            //we have to know which page is curentlly showed and based on that change order
            int order = id;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (menu != null && menu.Items != null)
            {
                MenuContent result = new MenuContent();
                for (var i = 0; i < menu.Items.Count; i++)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateAsync(menu.Items[i], menu, order);
                    order++;
                }

            }
            if (menu == null)
                return BadRequest();

            return Ok(menu);
        }

        //POST: ws/MenuContent/Delete
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetByMenuId")]
        [OmegaInvalidateCache("GetByMenuIdCount")]
        [OmegaInvalidateCache("PaginationGetByMenuId")]
        public async Task<IActionResult> Post([FromBody]Content content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            MenuContent result = new MenuContent();
            for (var i = 0; i < content.Menu.Count; i++)
            {
                int order = i;
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().Caller(await GetLoggedOnUser()).SaveAsync(content, content.Menu[i], order);
            }
            //foreach (Menu menu in content.Menu)
            //{
            //    result = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Save(content, menu);
            //}

            Content c = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { content.Id }
            })).FirstOrDefault(); 

            //menu = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Save(menu);

            if (c == null)
                return BadRequest();

            return Ok(c);
        }
    }
}
