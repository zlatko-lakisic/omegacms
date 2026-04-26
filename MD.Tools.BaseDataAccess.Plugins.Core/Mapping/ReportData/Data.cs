using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportData
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ReportSchedulerId
            {
                get { return "ReportSchedulerId"; }
            }
            
            public static string DateCreated
            {
                get { return "DateCreated"; }
            }
            public static string Data
            {
                get { return "Data"; }
            }

            #endregion
        }
        #endregion
        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.ReportSchedulerId));
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.Data));
            return table;
        }
        #endregion
    }
}
