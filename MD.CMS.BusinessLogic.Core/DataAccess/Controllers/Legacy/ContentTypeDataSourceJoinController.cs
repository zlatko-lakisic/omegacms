using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDataSourceJoinController : BaseController<ContentTypeDataSourceJoinController>
    {
		[Obsolete("Deprecated", true)]
        public IEnumerable<ContentTypeDataSourceJoin> GetById(long rightDataSourceId)
		{
			return GetByIdAsync(rightDataSourceId).Result;
        }

		[Obsolete("Deprecated", true)]
		public ContentTypeDataSourceJoin Save(ContentTypeDataSourceJoin contentTypeDataSourceJoin)
		{
			return SaveAsync(contentTypeDataSourceJoin).Result;
		}

		[Obsolete("Deprecated", true)]
		public bool Delete(ContentTypeDataSourceJoin ContentTypeDefinitionDataSourceJoin)
		{
			return DeleteAsync(ContentTypeDefinitionDataSourceJoin).Result;
		}
	}
}
