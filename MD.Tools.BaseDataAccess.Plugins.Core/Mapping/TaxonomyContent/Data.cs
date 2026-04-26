using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties

            public static string LCID
            {
                get { return "lcid"; }
            }
            public static string DateCreated
            {
                get { return "dateCreated"; }
            }
            public static string TaxonomyId
            {
                get { return "taxonomyId"; }
            }
            public static string Title
            {
                get { return "title"; }
            }
            public static string TaxonomyContentId
            {
                get { return "taxonomyContentId"; }
            }

            public static string ContentId
            {
                get { return "contentId"; }
            }
            public static string Id
            {
                get { return "id"; }
            }
            public static string CurrentPageIndex
            {
                get { return "currentPageIndex"; }
            }
            public static string MaxNumberOfRows
            {
                get { return "maxNumberOfRows"; }
            }
            public static string TaxonomyContentCount
            {
                get { return "taxonomyContentCount"; }
            }
            public static string FolderPath
            {
                get { return "folderPath"; }
            }
            public static string Order
            {
                get { return "order"; }
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
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.LCID));
            table.Columns.Add(new DataColumn(Columns.Title));
            table.Columns.Add(new DataColumn(Columns.TaxonomyContentId));
            table.Columns.Add(new DataColumn(Columns.ContentId));
            table.Columns.Add(new DataColumn(Columns.TaxonomyId));
            table.Columns.Add(new DataColumn(Columns.Id));
            table.Columns.Add(new DataColumn(Columns.CurrentPageIndex));
            table.Columns.Add(new DataColumn(Columns.MaxNumberOfRows));
            table.Columns.Add(new DataColumn(Columns.TaxonomyContentCount));
            table.Columns.Add(new DataColumn(Columns.FolderPath));
            table.Columns.Add(new DataColumn(Columns.Order));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        #endregion
    }
}
