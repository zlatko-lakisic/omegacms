using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class TemplateController<T> : BaseController<TemplateController<T>> where T : Content
    {
        [Obsolete("Deprecated", true)]
        public List<Template> GetAll(string sort = "Name ASC")
        {
            return GetAllAsync(sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public long GetAllCount(string searchTerm, string searchColumn)
        {
            return GetAllCountAsync(searchTerm, searchColumn).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<Template> GetAllWithPagination(string sort, long pageIndex, long pageSize, string searchTerm, string searchColumn)
        {
            return GetAllWithPaginationAsync(sort, pageIndex, pageSize, searchTerm, searchColumn).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Template> GetByFolder<T>(Folder<T> folder) where T : Content
        {
            return GetByFolderAsync<T>(folder).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Template> GetByParentId(long id)
        {
            return GetByParentIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public Template GetByContent(Content content)
        {
            return GetByContentAsync(content).Result;
        }

        [Obsolete("Deprecated", true)]
        public Template GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public Template Save(Template template)
        {
            return SaveAsync(template).Result;
        }

        [Obsolete("Deprecated", true)]
        public Template Update(Template template)
        {
            return UpdateAsync(template).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(Template template)
        {
            return DeleteAsync(template).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool AssignTemplateToFolder(Template template, Folder<T> folder)
        {
            return AssignTemplateToFolderAsync(template, folder).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteConnectionWithFolder(Template template, Folder<T> folder)
        {
            return DeleteConnectionWithFolderAsync(template, folder).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteByFolder(Folder<T> folder)
        {
            return DeleteByFolderAsync(folder).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool AssignTemplateToContent(Template template, Content content)
        {
            return AssignTemplateToContentAsync(template, content).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Template> Search(string searchTerm, string searchColumn)
        {
            return SearchAsync(searchTerm, searchColumn).Result;
        }
    }
}
