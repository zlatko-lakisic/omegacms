using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue
{
   public class Data
    {

        #region Columns
        public class Columns
        {
            #region Properties
            public static string Id
            {
                get { return "id"; }
            }
            public static string ProfileTypeFieldId
            {
                get { return "ProfileTypeFieldId"; }
            }
            public static string ProfileTypeId
            {
                get { return "ProfileTypeId"; }
            }
            public static string UserId
            {
                get { return "userId"; }
            }
            public static string Name
            {
                get { return "name"; }
            }
            public static string Value
            {
                get { return "value"; }
            }

           
            #endregion



        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.Id));
            table.Columns.Add(new DataColumn(Columns.ProfileTypeFieldId));
            table.Columns.Add(new DataColumn(Columns.ProfileTypeId));
            table.Columns.Add(new DataColumn(Columns.UserId));
            table.Columns.Add(new DataColumn(Columns.Value));
            return table;
        }
        #endregion
    }
}
