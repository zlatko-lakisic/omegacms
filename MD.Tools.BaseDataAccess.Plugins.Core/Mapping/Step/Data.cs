using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Step
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
            public static string ChainId
            {
                get { return "ChainId"; }
            }

            public static string Order
            {
                get { return "Order"; }
            }
            public static string ComboOperator
            {
                get { return "ComboOperator"; }
            }
          
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.StepId));
            table.Columns.Add(new DataColumn(Columns.ChainId));
            table.Columns.Add(new DataColumn(Columns.Order));
            table.Columns.Add(new DataColumn(Columns.ComboOperator));

            return table;
        }
        #endregion
    }
}