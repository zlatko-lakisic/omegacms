using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition
{
    public class Data
    {

        #region Columns
        public class Columns
        {
            #region Properties
            public static string ContentTypeDefinitionId { get { return "ContentTypeDefinitionId"; } }
            public static string FolderId { get { return "FolderId"; } }
            public static string ContentTypeDefinitionFieldId { get { return "ContentTypeDefinitionFieldId"; } }
            public static string Value { get { return "Value"; } }
            public static string Comparer { get { return "Comparer"; } }
			#endregion
		}
        #endregion
        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionFieldId));
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.FolderId));
            table.Columns.Add(new DataColumn(Columns.Value));          
            table.Columns.Add(new DataColumn(Columns.Comparer));

			return table;
        }
        #endregion
    }
}

