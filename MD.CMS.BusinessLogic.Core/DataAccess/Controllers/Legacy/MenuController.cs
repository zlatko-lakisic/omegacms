using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MenuController : BaseController<MenuController>
    {
        [Obsolete("Deprecated", true)]
        public Menu GetById(long id, bool fillParent = false, bool fillContents = false)
        {
            return GetByIdAsync(id, fillParent, fillContents).Result;
        }

        [Obsolete("Deprecated", true)]
        public Menu GetMenuByPath(string path = "", bool fillParent = false, bool fillAllParents = false, int lcid = 0)
        {
            return GetMenuByPathAsync(path, fillParent, fillAllParents, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Menu> GetByContentId(long id)
        {
            return GetByContentIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Menu> GetByParentId(long id, int depth = 0, int lcid = 0)
        {
            return GetByParentIdAsync(id, depth, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Menu> GetByContent(Content content, int depth = int.MaxValue)
        {
            return GetByContentAsync(content, depth).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<Menu> GetAll(int lcid = default(int))
        {
            return GetAllAsync(lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public EntityHierarchycalCollection<Menu> GetHierarchyByParentId(long id, int depth = int.MaxValue)
        {
            return GetHierarchyByParentIdAsync(id, depth).Result;
        }

        [Obsolete("Deprecated", true)]
        public Menu Save(Menu menu)
        {
            return SaveAsync(menu).Result;
        }

        [Obsolete("Deprecated", true)]
        public Menu Update(Menu menu, long order)
        {
            return UpdateAsync(menu, order).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(Menu obj)
        {
            return DeleteAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteParentId(Menu obj)
        {
            return DeleteParentIdAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool AssignContentToMenu(Menu menu, Content content)
        {
            return AssignContentToMenuAsync(menu, content).Result;
        }

        [Obsolete("Deprecated", true)]
        public void GetAssignedContentItems(ref Menu menu)
        {

        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<Menu> MenuSearchByName(string searchTerm, int currentPage, int pageSize, string orderColumn, bool reverseOrder)
        {
            return MenuSearchByNameAsync(searchTerm, currentPage, pageSize, orderColumn, reverseOrder).Result;
        }

        /// <summary>
        /// Method for searching menus by search word found in menu name
        /// </summary>
        /// <param name="searchTerm">String to find in menu name</param>
        /// <param name="lcid">LCID for menu</param>
        /// <param name="parentId">Parent Menu</param>
        /// <param name="recursion">Search child menus also?</param>
        /// <returns>List of menus containing search query in their name</returns>
        [Obsolete("Deprecated", true)]
        public List<Menu> MenusSearch(string searchTerm, int lcid, long parentId, bool recursion)
        {
            return MenusSearchAsync(searchTerm, lcid, parentId, recursion).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<Menu> GetByParentIdWithPagination(long id, long currentPageIndex, long maxNumberOfRows, string sortString, string searchTerm, int lcid = default(int))
        {
            return GetByParentIdWithPaginationAsync(id, currentPageIndex, maxNumberOfRows, sortString, searchTerm, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public int GetByParentIdCount(long menuId, int lcid, string searchTerm)
        {
            return GetByParentIdCountAsync(menuId, lcid, searchTerm).Result;
        }
    }
}
