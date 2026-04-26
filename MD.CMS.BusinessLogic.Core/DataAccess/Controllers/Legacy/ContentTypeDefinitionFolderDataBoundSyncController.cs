using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFolderDataBoundSyncController : BaseController<ContentTypeDefinitionFolderDataBoundSyncController>
    {
        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionFolderDataBoundSync GetByFolderAndContentTypeDefinitionId(long folderId, long contentTypeDefinitionId)
        {
            return GetByFolderAndContentTypeDefinitionIdAsync(folderId, contentTypeDefinitionId).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<ContentTypeDefinitionFolderDataBoundSync> GetAll()
        {
            return GetAllAsync().Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionFolderDataBoundSync Save(ContentTypeDefinitionFolderDataBoundSync obj)
        {
            return SaveAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(long folderId, long contentTypeDefinitionId)
        {
            return DeleteAsync(folderId, contentTypeDefinitionId).Result;
        }
	}
}
