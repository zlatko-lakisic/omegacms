using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class FolderMediaContentMetaDataFieldController : BaseController<FolderMediaContentMetaDataFieldController>
    {
        [Obsolete("Deprecated", true)]
        public FolderMediaContentMetaDataField FolderMetaDataFieldGetByIds(long folderId, long metaDataFieldId)
        {
            return FolderMetaDataFieldGetByIdsAsync(folderId, metaDataFieldId).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<FolderMediaContentMetaDataField> GetAll()
        {
            return GetAllAsync().Result;
        }

        [Obsolete("Deprecated", true)]
        public List<FolderMediaContentMetaDataField> GetMediaContentMetaDataFieldByFolder(long folderId)
        {
            return GetMediaContentMetaDataFieldByFolderAsync(folderId).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<FolderMetaDataField> GetUsedMetaDataFieldsByFolder(long folderId)
        {
            return GetUsedMetaDataFieldsByFolderAsync(folderId).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteByFolderId(long folderId)
        {
            return DeleteByFolderIdAsync(folderId).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool AssignMetaDataFieldToFolder(long folderId, FolderMediaContentMetaDataField folderMetaDataField)
        {
            return AssignMetaDataFieldToFolderAsync(folderId, folderMetaDataField).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<FolderMediaContentMetaDataField> GetByFolderId(long id)
        {
            return GetByFolderIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<FolderMediaContentMetaDataField> GetByFolder<T>(Folder<T> folder) where T : Content, new()
        {
            return GetByFolderAsync<T>(folder).Result;
        }
    }
}
