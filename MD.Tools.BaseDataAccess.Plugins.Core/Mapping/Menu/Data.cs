using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Menu
{
    public class Data
    {

        #region Columns
        public class Columns
        {
            #region Properties
            public static string Name
            {
                get { return "menuName"; }
            }
            public static string LCID
            {
                get { return "lcid"; }
            }
            public static string Description
            {
                get { return "description"; }
            }
            public static string MenuPath
            {
                get { return "MenuPath"; }
            }
            public static string ParentId
            {
                get { return "parentId"; }
            }
            public static string MenuId
            {
                get { return "menuId"; }
            }
            public static string Options
            {
                get { return "options"; }
            }
            public static string MenuCount { get { return "menuCount"; } }
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
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.LCID));
            table.Columns.Add(new DataColumn(Columns.Description));
            table.Columns.Add(new DataColumn(Columns.MenuPath));
            table.Columns.Add(new DataColumn(Columns.ParentId));
            table.Columns.Add(new DataColumn(Columns.MenuId));
            table.Columns.Add(new DataColumn(Columns.MenuCount));
            table.Columns.Add(new DataColumn(Columns.Options));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        #endregion

    }
}
