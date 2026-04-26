using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDataSourceController : BaseController<ContentTypeDataSourceController>
    {
		[Obsolete("Deprecated", true)]
        public ContentTypeDataSource GetById(long dataSourceId)
		{
			return GetByIdAsync(dataSourceId).Result;
        }

		[Obsolete("Deprecated", true)]
		public IEnumerable<ContentTypeDataSource> GetByContentTypeDefinitionId(long contentTypeDefinitionId)
		{
			return GetByContentTypeDefinitionIdAsync(contentTypeDefinitionId).Result;
		}

		[Obsolete("Deprecated", true)]
		public ContentTypeDataSource Save(ContentTypeDataSource contentTypeDataSource)
		{
			return SaveAsync(contentTypeDataSource).Result;
		}

		[Obsolete("Deprecated", true)]
		public bool Delete(ContentTypeDataSource contentTypeDataSource)
		{
			return DeleteAsync(contentTypeDataSource).Result;
		}

		[Obsolete("Deprecated", true)]
		public dynamic GetDataStructure(string type, string connectionString, string field = "")
		{
			return GetDataStructureAsync(type, connectionString, field).Result;
		}

		[Obsolete("Deprecated", true)]
		public IEnumerable<string> GetAllDatabaseTypes()
		{
			return GetAllDatabaseTypesAsync().Result;
		}
	}
}
