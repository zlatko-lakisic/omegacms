using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ContentTypeDefinitionId
            {
                get { return "contentTypeDefinitionId"; }
            }
            public static string Description
            {
                get { return "description"; }
            }
            public static string FolderId
            {
                get { return "folderId"; }
            }
            public static string Name
            {
                get { return "name"; }
            }           
            public static string Options
            {
                get { return "options"; }
            }
            public static string Icon
            {
                get { return "icon"; }
            }    
            public static string ContentTypeDefinitionsCount { 
                get { return "contentTypeDefinitionsCount"; } 
            }
            public static string IsDeleted
            {
                get { return "isDeleted"; }
            }
            public static string IsEditable
            {
                get { return "isEditable"; }
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
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.Description));
            table.Columns.Add(new DataColumn(Columns.FolderId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.Options));
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionsCount));
            table.Columns.Add(new DataColumn(Columns.Icon));
            table.Columns.Add(new DataColumn(Columns.IsDeleted));
            table.Columns.Add(new DataColumn(Columns.IsEditable));
            table.Columns.Add(new DataColumn(Columns.TotalCount));

            return table;
        }
        #endregion
    }
}
