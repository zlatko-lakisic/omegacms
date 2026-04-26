using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues
{
  public  class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string Id
            {
                get { return "id"; }
            }
            public static string MediacontentId
            {
                get { return "mediaContentId"; }
            }
            public static string DateCreated
            {
                get { return "dateCreated"; }
            }
            public static string Value
            {
                get { return "value"; }
            }
            public static string MetaDataFieldId
            {
                get { return "metaDataFieldId"; }
            }
             
            public static string AttributeTypeDefinitionId
            {
                get { return "attributeTypeDefinitionId"; }
            } 
            public static string Name
            {
                get { return "name"; }
            }
            public static string ListValue 
            {
                get { return "listValue"; }
            } 
            public static string Delimiter 
            {
                get { return "delimiter"; }
            } 

             
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.Id));
            table.Columns.Add(new DataColumn(Columns.MediacontentId));
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.Value));
            table.Columns.Add(new DataColumn(Columns.MetaDataFieldId));
            table.Columns.Add(new DataColumn(Columns.AttributeTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.ListValue));
            table.Columns.Add(new DataColumn(Columns.Delimiter));
            
            return table;
        }
        #endregion
    }
}
