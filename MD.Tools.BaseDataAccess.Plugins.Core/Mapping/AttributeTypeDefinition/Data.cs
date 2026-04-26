using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition
{
    public class Data
    {
        /*
         [StringValue("AttributeTypeDefinitionId")]
        AttributeTypeDefinitionId,
        [StringValue("Name")]
        Name,
        [StringValue("DefaultValue")]
        DefaultValue,
        [StringValue("Type")]
        Type,
        [StringValue("InputType")]
        InputType
         */
        #region Columns
        public class Columns
        {
            #region Properties
            public static string AttributeTypeDefinitionId
            {
                get { return "AttributeTypeDefinitionId"; }
            }
            public static string Name
            {
                get { return "Name"; }
            }
            public static string DefaultValue
            {
                get { return "DefaultValue"; }
            }
            public static string Type
            {
                get { return "Type"; }
            }

            public static string InputType
            {
                get { return "InputType"; }
            }
          
            #endregion



        }
        #endregion
        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.AttributeTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.DefaultValue));
            table.Columns.Add(new DataColumn(Columns.Type));
            table.Columns.Add(new DataColumn(Columns.InputType));

            return table;
        }
        #endregion
    }
}
