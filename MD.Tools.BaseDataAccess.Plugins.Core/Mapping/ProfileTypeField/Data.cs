using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField
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
            public static string ProfileTypeFieldId
            {
                get { return "profileTypeFieldId"; }
            }
            public static string AttributeTypeDefinitionId
            {
                get { return "attributeTypeDefinitionId"; }
            }
            public static string Name
            {
                get { return "name"; }
            }
            public static string Description
            {
                get { return "description"; }
            }
            public static string DefaultValue
            {
                get { return "defaultValue"; }
            }
            public static string ListValue
            {
                get { return "listValue"; }
            }
            public static string Delimiter
            {
                get { return "delimiter"; }
            }
            public static string Deleted
            {
                get { return "deleted"; }
            }
            public static string Order
            {
                get { return "order"; }
            }
            public static string Options
            {
                get { return "Options"; }
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
            table.Columns.Add(new DataColumn(Columns.ProfileTypeFieldId));
            table.Columns.Add(new DataColumn(Columns.AttributeTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.Description));
            table.Columns.Add(new DataColumn(Columns.DefaultValue));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.ListValue));
            table.Columns.Add(new DataColumn(Columns.Delimiter));
            table.Columns.Add(new DataColumn(Columns.Deleted));
            table.Columns.Add(new DataColumn(Columns.Order));
            table.Columns.Add(new DataColumn(Columns.Options));
           
            return table;
        }
        #endregion
    }
}
