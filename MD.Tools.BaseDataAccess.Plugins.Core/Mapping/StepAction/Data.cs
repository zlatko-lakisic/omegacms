using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ActionId
            {
                get { return "ActionId"; }
            }
            public static string StepId
            {
                get { return "StepId"; }
            }

            public static string UserId
            {
                get { return "UserId"; }
            }
            public static string Type
            {
                get { return "Type"; }
            }
            public static string Action
            {
                get { return "Action"; }
            }
            public static string RedirectTo
            {
                get { return "RedirectTo"; }
            }
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.ActionId));
            table.Columns.Add(new DataColumn(Columns.StepId));
            table.Columns.Add(new DataColumn(Columns.UserId));
            table.Columns.Add(new DataColumn(Columns.Type));
                 table.Columns.Add(new DataColumn(Columns.Action));
            table.Columns.Add(new DataColumn(Columns.RedirectTo));

            return table;
        }
        #endregion
    }
}