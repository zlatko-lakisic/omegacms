using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Content
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string LCID { get { return "LCID"; } }
            public static string ContentId { get { return "ContentId"; } }
            public static string Title { get { return "Title"; } }
            public static string FolderId { get { return "FolderId"; } }
            public static string AuthorId { get { return "AuthorId"; } }
            public static string Html { get { return "Html"; } }
            public static string ContentTypeDefinitionId { get { return "ContentTypeDefinitionId"; } }
            public static string ContentName { get { return "ContentName"; } }
            public static string DateCreated { get { return "DateCreated"; } }
            public static string Alias { get { return "Alias"; } }
            public static string ContentByFolderCount { get { return "ContentByFolderCount"; } }
            public static string IsPublished { get { return "IsPublished"; } }
            public static string FolderPath { get { return "FolderPath"; } }
            public static string ApprovalPending { get { return "ApprovalPending";  } }
            public static string TotalCount { get { return "TotalCount"; } }
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.LCID));
            table.Columns.Add(new DataColumn(Columns.ContentId));
            table.Columns.Add(new DataColumn(Columns.Title));
            table.Columns.Add(new DataColumn(Columns.FolderId));
            table.Columns.Add(new DataColumn(Columns.AuthorId));
            table.Columns.Add(new DataColumn(Columns.Html));
            table.Columns.Add(new DataColumn(Columns.ContentTypeDefinitionId));
            table.Columns.Add(new DataColumn(Columns.ContentName));
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.Alias));
            table.Columns.Add(new DataColumn(Columns.ContentByFolderCount));
            table.Columns.Add(new DataColumn(Columns.IsPublished));
            table.Columns.Add(new DataColumn(Columns.ApprovalPending));
            table.Columns.Add(new DataColumn(Columns.FolderPath));
            table.Columns.Add("Childs", typeof(DataTable));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        public static DataTable GetChildTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("name"));
            table.Columns.Add(new DataColumn("value"));

            return table;
        }
        #endregion
    }
}
