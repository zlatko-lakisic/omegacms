using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder
{
  public  class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ContentTypeDefinitionId
            {
                get { return "contentTypeDefinitionId"; }
            }
            public static string FolderId
            {
                get { return "folderId"; }
            }
            public static string Name
            {
                get { return "name"; }
            }           
            public static string Description
            {
                get { return "description"; }
            }
            public static string Options 
            {
                get { return "options"; }
            }
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.FolderId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.Description));
            table.Columns.Add(new DataColumn(Columns.Options));
            return table;
        }
        #endregion
    }
}
