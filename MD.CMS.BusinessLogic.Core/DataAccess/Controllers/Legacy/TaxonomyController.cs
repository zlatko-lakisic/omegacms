using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using System;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class TaxonomyController : BaseController<TaxonomyController>
    {
        [Obsolete("Deprecated", true)]
        public Taxonomy GetById(long id, bool fillParent = false)
        {
            return GetByIdAsync(id, fillParent).Result;
        }

        [Obsolete("Deprecated", true)]
        public Taxonomy GetTaxonomyByPath(string path = "", bool fillParent = false, bool fillAllParents = false, int lcid = 0)
        {
            return GetTaxonomyByPathAsync(path, fillParent, fillAllParents, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Taxonomy> GetByContentId(long id)
        {
            return GetByContentIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Taxonomy> GetByParentId(long id, int depth = 0, int lcid = 0, bool loadContents = false)
        {
            return GetByParentIdAsync(id, depth, lcid, loadContents).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<Taxonomy> GetByParentIdWithPagination(long id, long pageIndex, long pageSize, string searchTerm, int depth = int.MaxValue, int lcid = default(int))
        {
            return GetByParentIdWithPaginationAsync(id, pageIndex, pageSize, searchTerm, depth, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Taxonomy> Search(string searchTerm, long parentId, bool recursive)
        {
            return SearchAsync(searchTerm, parentId, recursive).Result;
        }

        [Obsolete("Deprecated", true)]
        public int GetByParentIdCount(long parentId, int lcid, string searchTerm)
        {
            return GetByParentIdCountAsync(parentId, lcid, searchTerm).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Taxonomy> TaxonomyContentGetTaxonomyByContent(Content content)
        {
            return TaxonomyContentGetTaxonomyByContentAsync(content).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Taxonomy> GetByContent(Content content, int depth = int.MaxValue)
        {
            return GetByContentAsync(content, depth).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Taxonomy> GetAll(int lcid = default(int))
        {
            return GetAllAsync(lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public EntityHierarchycalCollection<Taxonomy> GetHierarchyByParentId(long id, int depth = int.MaxValue, bool loadContents = false)
        {
            return GetHierarchyByParentIdAsync(id, depth, loadContents).Result;
        }

        [Obsolete("Deprecated", true)]
        public Taxonomy Save(Taxonomy taxonomy)
        {
            return SaveAsync(taxonomy).Result;
        }

        [Obsolete("Deprecated", true)]
        public Taxonomy Update(Taxonomy taxonomy, long order)
        {
            return UpdateAsync(taxonomy, order).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(Taxonomy obj)
        {
            return DeleteAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool AssignContentToTaxonomy(Taxonomy taxonomy, Content content)
        {
            return AssignContentToTaxonomyAsync(taxonomy, content).Result;
        }

        [Obsolete("Deprecated", true)]
        public void GetAssignedContentItems(Taxonomy taxonomy)
        {
            Task.Run(async () => {
                await GetAssignedContentItemsAsync(taxonomy); }).Wait();
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<Taxonomy> TaxonomySearchByName(string searchTerm, int currentPage, int pageSize, string orderColumn, bool reverseOrder)
        {
            return TaxonomySearchByNameAsync(searchTerm, currentPage, pageSize, orderColumn, reverseOrder).Result;
        }
    }
}
