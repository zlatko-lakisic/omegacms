using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions
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
            public static string PermissionId
            {
                get { return "permissionId"; }
            }


            public static string Method
            {
                get { return "method"; }
            }

            public static string Controller
            {
                get { return "controller"; }
            }
            public static string Function
            {
                get { return "function"; }
            }

            public static string IsApproved
            {
                get { return "isApproved"; }
            }

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
            table.Columns.Add(new DataColumn(Columns.Id));
            table.Columns.Add(new DataColumn(Columns.PermissionId));
            table.Columns.Add(new DataColumn(Columns.IsApproved));
            table.Columns.Add(new DataColumn(Columns.Function));
            table.Columns.Add(new DataColumn(Columns.Controller));
            table.Columns.Add(new DataColumn(Columns.Method));



            return table;
        }
        #endregion
    }
}
