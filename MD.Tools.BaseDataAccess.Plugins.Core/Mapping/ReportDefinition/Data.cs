using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ReportDefinitionId
            {
                get { return "ReportDefinitionId"; }
            }
            public static string Name
            {
                get { return "Name"; }
            }
            public static string AuthorId
            {
                get { return "AuthorId"; }
            }
            public static string ReportDefinitionSql
            {
                get { return "ReportDefinitionSql"; }
            }
            public static string ReportDefinitionJson
            {
                get { return "ReportDefinitionJson"; }
            }
            public static string IsDeleted
            {
                get { return "IsDeleted"; }
            }
            public static string DateCreated
            {
                get { return "DateCreated"; }
            }
            public static string DateUpdated
            {
                get { return "DateUpdated"; }
            }

            public static string ReportDefinitionsCount{
                get { return "ReportDefinitionsCount"; }
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
            table.Columns.Add(new DataColumn(Columns.ReportDefinitionId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.AuthorId));
            table.Columns.Add(new DataColumn(Columns.ReportDefinitionSql));
            table.Columns.Add(new DataColumn(Columns.ReportDefinitionJson));
            table.Columns.Add(new DataColumn(Columns.IsDeleted));
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.DateUpdated));
            table.Columns.Add(new DataColumn(Columns.ReportDefinitionsCount));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        #endregion
    }
}
