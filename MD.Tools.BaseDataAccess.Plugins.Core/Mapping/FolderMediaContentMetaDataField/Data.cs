using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMediaContentMetaDataField
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            
            public static string MetaDataFieldId
            {
                get { return "metaDataFieldId"; }
            }
            public static string FolderId
            {
                get { return "folderId"; }
            }
            public static string IsRequired
            {
                get { return "isRequired"; }
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
            public static string DefaultValue
            {
                get { return "DefaultValue"; }
            }

            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.MetaDataFieldId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.AttributeTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.ListValue));
            table.Columns.Add(new DataColumn(Columns.Delimiter));
            table.Columns.Add(new DataColumn(Columns.FolderId));
            table.Columns.Add(new DataColumn(Columns.IsRequired));
            table.Columns.Add(new DataColumn(Columns.DefaultValue));
       

            return table;
        }
        #endregion
    }
}
