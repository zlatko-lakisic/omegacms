using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Report
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
            public static string ContentId
            {
                get { return "contentId"; }
            }
            public static string Title
            {
                get { return "title"; }
            }
            public static string FolderId
            {
                get { return "folderId"; }
            }
            public static string AuthorId
            {
                get { return "authorId"; }
            }
            public static string Html
            {
                get { return "html"; }
            }
            public static string ContentTypeDefinitionId
            {
                get { return "contentTypeDefinitionId"; }
            }
            public static string ContentName
            {
                get { return "contentName"; }
            }
            public static string DateCreated
            {
                get { return "dateCreated"; }
            }
            public static string Alias
            {
                get { return "alias"; }
            }
            public static string ContentByFolderCount 
            {
                get { return "ContentByFolderCount"; }
            }
            
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
            table.Columns.Add("Childs",typeof(DataTable));
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
