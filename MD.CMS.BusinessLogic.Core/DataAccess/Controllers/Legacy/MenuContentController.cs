using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MenuContentController : BaseController<MenuContentController>
    {
        [Obsolete("Deprecated", true)]
        public MenuContent Save(Content obj, Menu menu,int order)
        {
            return SaveAsync(obj, menu, order).Result;
        }

        [Obsolete("Deprecated", true)]
        public MenuContent Update(MenuContent obj, Menu menu, int order)
        {
            return UpdateAsync(obj, menu, order).Result;
        }

        [Obsolete("Deprecated", true)]
        public MenuContent DeleteMenu(Content obj, Menu menu)
        {
            return DeleteMenuAsync(obj, menu).Result;
        }

        [Obsolete("Deprecated", true)]
        public void SaveMenuContent(Menu menu, Content content)
        {
            Task.Run(async () => {
                await SaveMenuContentAsync(menu, content); }).Wait();
        }

        [Obsolete("Deprecated", true)]
        public List<MenuContent> GetByMenuId(long id, int lcid = default(int))
        {
            return GetByMenuIdAsync(id, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(MenuContent menuContent)
        {
            return DeleteAsync(menuContent).Result;
        }

        [Obsolete("Deprecated", true)]
        public MenuContent DeleteMenu1(string contentId, int lcid, string dateCreated, Menu newMenu)
        {
            return DeleteMenu1Async(contentId, lcid, dateCreated, newMenu).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteAllbyMenuId(Menu menu)
        {
            return DeleteAllbyMenuIdAsync(menu).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MenuContent> GetByContent(Content obj)
        {
            return GetByContentAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public int GetByMenuIdCount(long menuId, string searchTerm, int lcid = default(int))
        {
            return GetByMenuIdCountAsync(menuId, searchTerm, lcid).Result;
        }

        /// <summary>
        /// Search for menu contents by given keyword
        /// </summary>
        /// <param name="searchTerm">Word to search for in menu content title</param>
        /// <param name="menuId">Menu where search is done</param>
        /// <param name="lcid">Content language</param>
        /// <returns></returns>
        [Obsolete("Deprecated", true)]
        public List<MenuContent> MenuContentsSearch(string searchTerm, int lcid, long menuId)
        {
            return MenuContentsSearchAsync(searchTerm, lcid, menuId).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<MenuContent> GetByMenuIdWithPagination(long id, long currentPageIndex, long maxNumberOfRows, string searchTerm, int lcid = default(int), string sort = "Order ASC")
        {
            return GetByMenuIdWithPaginationAsync(id, currentPageIndex, maxNumberOfRows, searchTerm, lcid, sort).Result;
        }
    }
}
