using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.StepUser
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string StepId
            {
                get { return "StepId"; }
            }
            public static string UserId
            {
                get { return "UserId"; }
            }

          
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.StepId));
            table.Columns.Add(new DataColumn(Columns.UserId));

            return table;
        }
        #endregion
    }
}