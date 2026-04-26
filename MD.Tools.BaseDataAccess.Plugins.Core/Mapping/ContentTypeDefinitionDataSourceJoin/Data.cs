using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin
{
    public class Data
    {
        #region Columns
        public class Columns
        {
			#region Properties
			public static string RightDataSourceId
			{
				get { return "RightDataSourceId"; }
			}
			public static string LeftRightDataSourceJoinType
			{
				get { return "LeftRightDataSourceJoinType"; }
			}
			public static string LeftFieldId
			{
				get { return "LeftFieldId"; }
			}
			public static string RightFieldId
			{
				get { return "RightFieldId"; }
			}
			#endregion
		}
        #endregion
        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.RightDataSourceId));
            table.Columns.Add(new DataColumn(Columns.LeftRightDataSourceJoinType));
            table.Columns.Add(new DataColumn(Columns.LeftFieldId));
            table.Columns.Add(new DataColumn(Columns.RightFieldId));

            return table;
        }
        #endregion
    }
}
