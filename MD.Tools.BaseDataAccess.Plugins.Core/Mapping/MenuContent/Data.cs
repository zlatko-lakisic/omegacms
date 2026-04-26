using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent
{
    public class Data
    {

        #region Columns
        public class Columns
        {
            #region Properties
            public static string MenuId
            {
                get { return "menuId"; }
            }
            public static string ContentId
            {
                get { return "contentId"; }
            }
            public static string LCID
            {
                get { return "lcid"; }
            }
            public static string DateCreated
            {
                get { return "dateCreated"; }
            }
            public static string Title
            {
                get { return "title"; }
            }
            public static string MenuContentCount
            {
                get { return "menuContentCount"; }
            }
            public static string TotalCount
            {
                get { return "TotalCount"; }
            }

            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.MenuId));
            table.Columns.Add(new DataColumn(Columns.LCID));
            table.Columns.Add(new DataColumn(Columns.ContentId));
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.Title));
            table.Columns.Add(new DataColumn(Columns.MenuContentCount));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        #endregion

    }
}
