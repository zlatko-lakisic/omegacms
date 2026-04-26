using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Culture
{
 public   class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string IsoCode
            {
                get { return "isoCode"; }
            }
            public static string Code
            {
                get { return "code"; }
            }
            public static string LCID
            {
                get { return "lcid"; }
            }           
            public static string Name
            {
                get { return "name"; }
            }
           
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.IsoCode));
            table.Columns.Add(new DataColumn(Columns.Code));
            table.Columns.Add(new DataColumn(Columns.LCID));
            table.Columns.Add(new DataColumn(Columns.Name));

            return table;
        }
        #endregion
    }
}
