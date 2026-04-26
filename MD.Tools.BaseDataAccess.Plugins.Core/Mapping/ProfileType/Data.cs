using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType
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
            public static string Id
            {
                get { return "id"; }
            }
            public static string Name
            {
                get { return "name"; }
            }
            public static string PermissionXmlText
            {
                get { return "permissionXmlText"; }
            }
            public static string UserId
            {
                get { return "userId"; }
            }
       
            public static string ProfileTypesByUserCount
            {
                get { return "profileTypesByUserCount"; }
            }
       
            public static string Username
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
            public static string Token
            {
                get { return "token"; }
            }
            public static string DateRefreshToken
            {
                get { return "dateRefreshToken"; }
            }
            public static string ProfileTypesCount
            {
                get { return "profiletypescount"; }
            }
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
            table.Columns.Add(new DataColumn(Columns.ProfileTypeId));
            table.Columns.Add(new DataColumn(Columns.UserId));
            table.Columns.Add(new DataColumn(Columns.Id));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.PermissionXmlText));
            table.Columns.Add(new DataColumn(Columns.IsDeleted));
            table.Columns.Add(new DataColumn(Columns.ProfileTypesByUserCount));
            table.Columns.Add(new DataColumn(Columns.ProfileTypesCount));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        #endregion
    }
}
