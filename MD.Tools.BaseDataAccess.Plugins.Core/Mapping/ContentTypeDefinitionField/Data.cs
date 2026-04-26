using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField
{
    public class Data
    {

        #region Columns
        public class Columns
        {
            #region Properties
            public static string AttributeTypeDefinitionId { get { return "AttributeTypeDefinitionId"; } }
            public static string ContentTypeDefinitionFieldId { get { return "ContentTypeDefinitionFieldId"; } }
            public static string ContentTypeDefinitionId { get { return "ContentTypeDefinitionId"; } }
            public static string DefaultValue { get { return "DefaultValue"; } }
            public static string Delimiter { get { return "Delimiter"; } }
            public static string ListValue { get { return "ListValue"; } }
            public static string Name { get { return "Name"; } }
            public static string Options { get { return "Options"; } }
            public static string Order { get { return "Order"; } }
			public static string Description { get { return "Description"; } }
			public static string DataBound { get { return "DataBound"; } }
			public static string DataSourceId { get { return "DataSourceId"; } }
			public static string DataSourceField { get { return "DataSourceField"; } }
            public static string DataBoundReadOnly { get { return "DataBoundReadOnly"; } }
            public static string IsDataBoundPrimaryKey { get { return "IsDataBoundPrimaryKey"; } }
            #endregion
        }
        #endregion
        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.AttributeTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionFieldId));
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.DefaultValue));
            table.Columns.Add(new DataColumn(Columns.Delimiter));          
            table.Columns.Add(new DataColumn(Columns.ListValue));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.Options));
            table.Columns.Add(new DataColumn(Columns.Order));
			table.Columns.Add(new DataColumn(Columns.Description));
			table.Columns.Add(new DataColumn(Columns.DataBound));
			table.Columns.Add(new DataColumn(Columns.DataSourceId));
			table.Columns.Add(new DataColumn(Columns.DataSourceField));
			table.Columns.Add(new DataColumn(Columns.DataBoundReadOnly));
            table.Columns.Add(new DataColumn(Columns.IsDataBoundPrimaryKey));

			return table;
        }
        #endregion
    }
}

