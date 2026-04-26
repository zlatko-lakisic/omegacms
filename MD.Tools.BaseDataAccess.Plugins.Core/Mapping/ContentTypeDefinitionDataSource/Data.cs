using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource
{
    public class Data
    {
        #region Columns
        public class Columns
        {
			#region Properties
			public static string DataSourceId
			{
				get { return "DataSourceId"; }
			}
			public static string ContentTypeDefinitionId
			{
				get { return "contentTypeDefinitionId"; }
			}
			public static string ConnectionString
			{
				get { return "ConnectionString"; }
			}
			public static string IsDeleted
			{
				get { return "IsDeleted"; }
			}
			#endregion
		}
        #endregion
        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.DataSourceId));
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.ConnectionString));
            table.Columns.Add(new DataColumn(Columns.IsDeleted));

            return table;
        }
        #endregion
    }
}
