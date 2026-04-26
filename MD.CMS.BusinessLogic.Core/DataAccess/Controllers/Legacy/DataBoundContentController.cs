using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class DataBoundContentController<T> : ContentController<T, DataBoundContentController<T>>, Tools.BaseDataAccess.Core.Interfaces.IBaseControllerSettings
        where T : Content, new()
    {
        [Obsolete("Deprecated", true)]
        public T GetById(string id, long contentTypeId = default(long), IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> extraConditions = null)
        {
            return GetByIdAsync(id, contentTypeId, extraConditions).Result;
        }

        [Obsolete("Deprecated", true)]
        public override BasePaginationEntity<T> GetByFolderIdWithPagination(long id, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", bool loadAuthor = false, int lcid = 0, string sort = "Title ASC", bool loadFields = false)
        {
            return GetByFolderIdWithPaginationAsync(id, currentPageIndex, maxNumberOfRows, searchTerm, sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public BasePaginationEntity<T> GetByFolderIdWithPagination(long id, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", string sort = "Title ASC")
        {
            return GetByFolderWithPaginationAsync(FolderController<T>.GetNewInstance().Caller(UserMakingTheCall).GetByIdAsync(id).Result, currentPageIndex, maxNumberOfRows, searchTerm, sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public BasePaginationEntity<T> GetByFolderWithPagination(Folder<T> folder, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", string sort = "Title ASC")
        {
            return GetByFolderWithPaginationAsync(folder, currentPageIndex, maxNumberOfRows, searchTerm, sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public string SaveWithReturnId(T content)
        {
            return SaveWithReturnIdAsync(content).Result;
        }

        [Obsolete("Deprecated", true)]
        public override T Save(T content)
        {
            return SaveAsync(content).Result;
        }

        [Obsolete("Deprecated", true)]
        public override bool Delete(T obj)
        {
            return DeleteAsync(obj).Result;
        }
    }
}
