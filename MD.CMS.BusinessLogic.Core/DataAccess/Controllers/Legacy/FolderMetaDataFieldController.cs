using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class FolderMetaDataFieldController : BaseController<FolderMetaDataFieldController>
    {
        [Obsolete("Deprecated", true)]
        public FolderMetaDataField FolderMetaDataFieldGetByIds(long folderId, long metaDataFieldId)
        {
            return FolderMetaDataFieldGetByIdsAsync(folderId, metaDataFieldId).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<FolderMetaDataField> GetAll()
        {
            return GetAllAsync().Result;
        }

        [Obsolete("Deprecated", true)]
        public List<FolderMetaDataField> GetOnlyUsed(long folderId)
        {
            return GetOnlyUsedAsync(folderId).Result;
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
        public bool AssignMetaDataFieldToFolder(long folderId, FolderMetaDataField folderMetaDataField)
        {
            return AssignMetaDataFieldToFolderAsync(folderId, folderMetaDataField).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<FolderMetaDataField> GetByFolderId(long id)
        {
            return GetByFolderIdAsync(id).Result;
        }
    }

}
