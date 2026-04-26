using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ContentId
            {
                get { return "contentId"; }
            }
            public static string LCID
            {
                get { return "lcid"; }
            }
            public static string DateCreated
            {
                get { return "dateCreated"; }
            }
            public static string MetaDataFieldId
            {
                get { return "metaDataFieldId"; }
            }
            public static string Value
            {
                get { return "value"; }
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
            table.Columns.Add(new DataColumn(Columns.ContentId));
            table.Columns.Add(new DataColumn(Columns.LCID));
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.MetaDataFieldId));
            table.Columns.Add(new DataColumn(Columns.Value));
            table.Columns.Add(new DataColumn(Columns.AttributeTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.ListValue));
            table.Columns.Add(new DataColumn(Columns.Delimiter));
            return table;
        }
        #endregion
    }
}
