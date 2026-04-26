using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Profile
{
  public  class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ProfileTypeId
            {
                get { return "profileTypeId"; }
            }


         
            public static string UserId
            {
                get { return "userId"; }
            }



            #endregion



        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.ProfileTypeId));
            table.Columns.Add(new DataColumn(Columns.UserId));
           



            return table;
        }
        #endregion
    }
}
