using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ContentTypeDefinitionId { get { return "contentTypeDefinitionId"; } }
            public static string ContentTypeDefinitionFieldId { get { return "contentTypeDefinitionFieldId"; } }
            public static string LCID { get { return "lcid"; } }
            public static string Value { get { return "value"; } }
            public static string Name { get { return "name"; } }
            public static string AttributeTypeDefinitionId { get { return "attributeTypeDefinitionId"; } }
            public static string Order { get { return "order"; } }
            public static string DateCreated { get { return "DateCreated"; } }
            public static string ContentId { get { return "ContentId"; } }
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionFieldId));
            table.Columns.Add(new DataColumn(Columns.LCID));
            table.Columns.Add(new DataColumn(Columns.Value));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.AttributeTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.Order));
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.ContentId));
            return table;
        }

        #endregion
    }
}
