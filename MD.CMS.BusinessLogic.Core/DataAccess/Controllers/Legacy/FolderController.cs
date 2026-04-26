using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using System;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class FolderController<T> : BaseController<FolderController<T>>
        where T : Content, new()
    {
        [Obsolete("Deprecated", true)]
        public Folder<T> GetById(long id, bool fillParent = false)
        {
            return GetByIdAsync(id, fillParent).Result;
        }

        [Obsolete("Deprecated", true)]
        public Folder<T> GetFolderByPath(string path = "", bool fillParent = false, bool fillAllParents = false)
        {
            return GetFolderByPathAsync(path, fillParent, fillAllParents).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Folder<T>> GetRoots()
        {
            return GetRootsAsync().Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<Folder<T>> GetByParentId(long id, int depth = int.MaxValue, bool fillContents = false)
        {
            return GetByParentIdAsync(id, depth, fillContents).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<Folder<T>> GetByParentIdWithPagination(long id, int currentPageIndex, int maxNumberOfRows, string searchTerm, int depth = int.MaxValue, bool fillContents = false)
        {
            return GetByParentIdWithPaginationAsync(id, currentPageIndex, maxNumberOfRows, searchTerm, depth, fillContents).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Folder<T>> Search(string searchTerm, long parentId, bool recursive)
        {
            return SearchAsync(searchTerm, parentId, recursive).Result;
        }

        [Obsolete("Deprecated", true)]
        public int GetByParentIdCount(long parentId, string searchTerm)
        {
            return GetByParentIdCountAsync(parentId, searchTerm).Result;
        }

        [Obsolete("GetHierarchyByParentId is deprecated, please use GetHierarchyByParentIdAsync instead.")]
        public EntityHierarchycalCollection<Folder<T>> GetHierarchyByParentId(long id, int depth = int.MaxValue)
        {
            return GetHierarchyByParentIdAsync(id, depth).Result;
        }

        [Obsolete("Deprecated", true)]
        public void GetChilds(long ParentId, Folder<T> folder)
        {
            Task.Run(async () => {
                await GetChildsAsync(ParentId, folder); }).Wait();
        }

        [Obsolete("Deprecated", true)]
        public void InsertForChildrens(Folder<T> folder, Folder<T> inheritedChildren)
        {
            Task.Run(async () =>
            {
                await InsertForChildrensAsync(folder, inheritedChildren);
            }).Wait();
        }

        [Obsolete("Deprecated", true)]
        public Folder<T> Save(Folder<T> folder)
        {
            return SaveAsync(folder).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(Folder<T> folder)
        {
            return DeleteAsync(folder).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteByParentId(Folder<T> obj)
        {
            return DeleteByParentIdAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool IsAuthorized(User user, Folder<Content> folder, RWDPermissionType permission)
        {
            return IsAuthorizedAsync(user, folder, permission).Result;
        }
    }
}
