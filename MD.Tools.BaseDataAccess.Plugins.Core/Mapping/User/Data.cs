using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.User
{
    public class Data
    {
         #region Columns
        public class Columns
        {
            #region Properties
            public static string UserId
            {
                get { return "userId"; }
            }
            public static string UserName
            {
                get { return "username"; }
            }
            public static string Password
            {
                get { return "password"; }
            }
            public static string IsDeleted
            {
                get { return "isDeleted"; }
            }
            public static string AdministrationAllowed => "AdministrationAllowed";
            public static string Token
            {
                get { return "token"; }
            }
            public static string DateRefreshToken
            {
                get { return "dateRefreshToken"; }
            }
            public static string PermissionsId
            {
                get { return "permissionsId"; }
            }
            public static string ProfileTypeId
            {
                get { return "profileTypeId"; }
            }
            public static string UserCount
            {
                get { return "userCount"; }
            }
            public static string CurrentPageIndex
            {
                get { return "currentPageIndex"; }
            }
            public static string MaxNumberOfRows
            {
                get { return "maxNumberOfRows"; }
            }
            public static string TotalCount
            {
                get{ return "TotalCount"; }
            }

            #endregion
        }
         #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.UserId));
            table.Columns.Add(new DataColumn(Columns.UserName));
            table.Columns.Add(new DataColumn(Columns.Password));
            table.Columns.Add(new DataColumn(Columns.IsDeleted));
            table.Columns.Add(new DataColumn(Columns.AdministrationAllowed));
            table.Columns.Add(new DataColumn(Columns.Token));
            table.Columns.Add(new DataColumn(Columns.PermissionsId));
            table.Columns.Add(new DataColumn(Columns.ProfileTypeId));
            table.Columns.Add(new DataColumn(Columns.UserCount));
            table.Columns.Add(new DataColumn(Columns.CurrentPageIndex));
            table.Columns.Add(new DataColumn(Columns.MaxNumberOfRows));
            table.Columns.Add(new DataColumn(Columns.DateRefreshToken));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            table.Columns.Add("Childs", typeof(DataTable));
            return table;
        }
        public static DataTable GetChildTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("name"));
            table.Columns.Add(new DataColumn("value"));

            return table;
        }
        #endregion
    }
}
