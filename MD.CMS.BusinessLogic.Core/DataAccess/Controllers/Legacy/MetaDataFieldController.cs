using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MetaDataFieldController : BaseController<MetaDataFieldController>
    {
        [Obsolete("Deprecated", true)]
        public MetaDataField GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MetaDataField> GetByFolder<T>(Folder<T> folder) where T : Content, new()
        {
            return GetByFolderAsync<T>(folder).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MetaDataField> GetByFolderId(long folderId)
        {
            return GetByFolderIdAsync(folderId).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MetaDataField> GetAll()
        {
            return GetAllAsync().Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(MetaDataField obj)
        {
            return DeleteAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public MetaDataField Save(MetaDataField obj)
        {
            return SaveAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<MetaDataField> GetAllWithPagination(int currentPageIndex, int maxNumberOfRows, string searchTerm, string searchColumn, string sort = "Name ASC")
        {
            return GetAllWithPaginationAsync(currentPageIndex, maxNumberOfRows, searchTerm, searchColumn, sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public int SelectAllCount(string searchTerm, string searchColumn)
        {
            return SelectAllCountAsync(searchTerm, searchColumn).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<MetaDataField> MetaDataMediaContentGetByFolderId<T>(Folder<T> folder) where T : Content, new()
        {
            return MetaDataMediaContentGetByFolderIdAsync(folder).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MetaDataField> Search(string searchTerm, string searchColumn)
        {
            return SearchAsync(searchTerm, searchColumn).Result;
        }
    }
}
