using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class TaxonomyContentController : BaseController<TaxonomyContentController>
    {
        [Obsolete("Deprecated", true)]
        public TaxonomyContent Save(Content obj, Taxonomy taxonomy,int order)
        {
            return SaveAsync(obj, taxonomy, order).Result;
        }

        [Obsolete("Deprecated", true)]
        public TaxonomyContent Update(Content obj, Taxonomy taxonomy, int order)
        {
            return UpdateAsync(obj, taxonomy, order).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteTaxonomy(Content obj, Taxonomy taxonomy)
        {
            return DeleteTaxonomyAsync(obj, taxonomy).Result;
        }

        [Obsolete("Deprecated", true)]
        public void SaveTaxonomyContent(Taxonomy taxonomy, Content content)
        {
            Task.Run(async () => {
                await SaveTaxonomyContentAsync(taxonomy, content); }).Wait();
        }

        [Obsolete("Deprecated", true)]
        public List<TaxonomyContent> GetByTaxonomyId(long id, int lcid = default(int))
        {
            return GetByTaxonomyIdAsync(id, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public int GetByTaxonomyIdCount(long taxonomyId, string searchTerm, int lcid = default(int))
        {
            return GetByTaxonomyIdCountAsync(taxonomyId, searchTerm, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<TaxonomyContent> GetByTaxonomyIdWithPagination(long id, long currentPageIndex, long maxNumberOfRows, string searchTerm, int lcid = default(int), string sort = "Order ASC")
        {
            return GetByTaxonomyIdWithPaginationAsync(id, currentPageIndex, maxNumberOfRows, searchTerm, lcid, sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<TaxonomyContent> Search(string searchTerm, long parentId, int lcid)
        {
            return SearchAsync(searchTerm, parentId, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(TaxonomyContent taxonomyContent)
        {
            return DeleteAsync(taxonomyContent).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteAllByTaxonomyId(Taxonomy taxonomy)
        {
            return DeleteAllByTaxonomyIdAsync(taxonomy).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteContentTaxonomy(int id, int id2)
        {
            return DeleteContentTaxonomyAsync(id, id2).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<TaxonomyContent> GetByContent(Content obj)
        {
            return GetByContentAsync(obj).Result;
        }
    }
}
