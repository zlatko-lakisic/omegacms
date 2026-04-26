using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder
{
    public class Data
    {

        #region Columns
        public class Columns
        {
            #region Properties
            public static string MessageFolderId
            {
                get { return "MessageFolderId"; }
            }
            public static string Name
            {
                get { return "Name"; }
            }
            public static string Icon
            {
                get { return "Icon"; }
            }
            public static string AuthorId
            {
                get { return "AuthorId"; }
            }
            public static string IsGlobal
            {
                get { return "IsGlobal"; }
            }          
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.MessageFolderId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.Icon));
            table.Columns.Add(new DataColumn(Columns.AuthorId));
            table.Columns.Add(new DataColumn(Columns.IsGlobal));          
            return table;
        }
        #endregion
    }
}
