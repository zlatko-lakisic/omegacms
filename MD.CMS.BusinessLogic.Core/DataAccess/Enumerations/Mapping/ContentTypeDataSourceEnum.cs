using MD.Tools.Helpers.Core.TypeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum ContentTypeDataSourceEnum
    {
        [StringValue("DataSourceId")]
        DataSourceId,
        [StringValue("ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("ConnectionString")]
        ConnectionString,
		[StringValue("DatabaseType")]
		DatabaseType
	}
    internal enum ContentTypeDataSourceParamatersEnum
    {
        [StringValue("_DataSourceId")]
        DataSourceId,
        [StringValue("_ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
		[StringValue("_ConnectionString")]
		ConnectionString,
		[StringValue("_DatabaseType")]
		DatabaseType
	}

    internal enum ContentTypeDataSourceSPEnum
    {
        [StringValue("ContentTypeDataSource_GetById")]
        Select,
        [StringValue("ContentTypeDataSource_DeleteById")]
        Delete,
        [StringValue("ContentTypeDataSource_Update")]
        Update,
        [StringValue("ContentTypeDataSource_Insert")]
        Insert,
        [StringValue("ContentTypeDataSource_GetByContentTypeId")]
        SelectByContentTypeDefinitionId
    }
}
