using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ReportSchedulerId
            {
                get { return "ReportSchedulerId"; }
            }
            public static string Name
            {
                get { return "Name"; }
            }
           
            public static string AuthorId
            {
                get { return "AuthorId"; }
            }
            public static string DateCreated
            {
                get { return "DateCreated"; }
            }
            public static string DateEdited
            {
                get { return "DateEdited"; }
            }
            public static string IsRecurring
            {
                get { return "IsRecurring"; }
            }
            public static string Interval
            {
                get { return "Interval"; }
            }
            public static string Start
            {
                get { return "Start"; }
            }
            public static string End
            {
                get { return "End"; }
            }
            public static string ReportDefinitionId
            {
                get { return "ReportDefinitionId"; }
            }
            public static string IsActive
            {
                get { return "IsActive"; }
            }
            public static string IsDeleted
            {
                get { return "IsDeleted"; }
            }
            public static string ReportSchedulersCount
            {
                get { return "ReportSchedulersCount"; }
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
            table.Columns.Add(new DataColumn(Columns.ReportSchedulerId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.AuthorId));
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.DateEdited));
            table.Columns.Add(new DataColumn(Columns.IsRecurring));
            table.Columns.Add(new DataColumn(Columns.Interval));
            table.Columns.Add(new DataColumn(Columns.Start));
            table.Columns.Add(new DataColumn(Columns.End));
            table.Columns.Add(new DataColumn(Columns.ReportDefinitionId));
            table.Columns.Add(new DataColumn(Columns.IsActive));
            table.Columns.Add(new DataColumn(Columns.IsDeleted));
            table.Columns.Add(new DataColumn(Columns.ReportSchedulersCount));
            table.Columns.Add(new DataColumn(Columns.TotalCount));

            return table;
        }
        #endregion
    }
}
