using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportDefinitionController : BaseController<ReportDefinitionController>
    {
        [Obsolete("Deprecated", true)]
        public ReportDefinition Save(ReportDefinition definition)
        {
            return SaveAsync(definition).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ReportDefinition> GetAll(string sort = "Name ASC")
        {
            return GetAllAsync(sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<ReportDefinition> GetAllWithPagination(long pageIndex, long pageSize, string searchTerm, string searchColumn, string sort)
        {
            return GetAllWithPaginationAsync(pageIndex, pageSize, searchTerm, searchColumn, sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public long GetAllCount(string searchTerm, string searchColumn)
        {
            return GetAllCountAsync(searchTerm, searchColumn).Result;
        }

        [Obsolete("Deprecated", true)]
        public ReportDefinition GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(ReportDefinition obj)
        {
            return DeleteAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ReportDefinition> Search(string searchTerm, string searchColumn)
        {
            return SearchAsync(searchTerm, searchColumn).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Entity> GetAllEntities()
        {
            return GetAllEntitiesAsync().Result;
        }
    }
}
