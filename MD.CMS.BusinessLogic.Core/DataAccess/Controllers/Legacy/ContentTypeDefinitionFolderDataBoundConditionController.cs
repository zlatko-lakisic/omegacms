using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFolderDataBoundConditionController : BaseController<ContentTypeDefinitionFolderDataBoundConditionController>
    {
		[Obsolete("Deprecated", true)]
		public ContentTypeDefinitionFolderDataBoundCondition Save(ContentTypeDefinitionFolderDataBoundCondition contentTypeDefinition)
		{
			return SaveAsync(contentTypeDefinition).Result;
		}

		[Obsolete("Deprecated", true)]
		public IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> GetByFolderAndContentTypeDefinitionId(long folderId, long contentTypeId)
		{
			return GetByFolderAndContentTypeDefinitionIdAsync(folderId, contentTypeId).Result;
		}

		[Obsolete("Deprecated", true)]
		public bool DeleteAll(long folderId, long contentTypeId)
		{
			return DeleteAllAsync(folderId, contentTypeId).Result;
		}

		[Obsolete("Deprecated", true)]
		public bool Delete(long folderId, long contentTypeId, long fieldId)
		{
			return DeleteAsync(folderId, contentTypeId, fieldId).Result;
		}
    }
}
