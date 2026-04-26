using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string FolderId
            {
                get { return "folderId"; }
            }
            public static string ParentId
            {
                get { return "parentId"; }
            }
            public static string Name
            {
                get { return "name"; }
            }
            public static string Description
            {
                get { return "description"; }
            }          
            public static string Inherit
            {
                get { return "inherit"; }
            }
            public static string FolderPath
            {
                get { return "folderPath"; }
            }
            public static string FolderCount { get { return "folderCount"; } }
            public static string TotalCount { get { return "TotalCount"; } }
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.FolderId));
            table.Columns.Add(new DataColumn(Columns.ParentId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.Description));
            table.Columns.Add(new DataColumn(Columns.Inherit));
            table.Columns.Add(new DataColumn(Columns.FolderPath));
            table.Columns.Add(new DataColumn(Columns.FolderCount));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        #endregion
    }
}
