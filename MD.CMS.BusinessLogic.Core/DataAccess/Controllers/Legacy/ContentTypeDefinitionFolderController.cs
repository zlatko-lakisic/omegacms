using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFolderController : BaseController<ContentTypeDefinitionFolderController>
    {
        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionFolder Save(long folderId, ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition)
        {
            return SaveAsync(folderId, contentTypeDefinition).Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionFolder Delete(Folder<Content> obj, ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition)
        {
            return DeleteAsync(obj, contentTypeDefinition).Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionFolder DeleteAll(long folderId, long ContentTypeDefinitionId)
        {
            return DeleteAllAsync(folderId, ContentTypeDefinitionId).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ContentTypeDefinitionFolder> GetByFolder(long id)
        {
            return GetByFolderAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteAllByFolderId(long folderId)
        {
            return DeleteAllByFolderIdAsync(folderId).Result;
        }
    }
}
