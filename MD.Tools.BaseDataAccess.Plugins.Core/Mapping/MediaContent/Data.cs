using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent
{
   public class Data
    { 
        #region Columns
        public class Columns
        {
            #region Properties
            public static string MediaContentId
            {
                get { return "mediaContentId"; }
            }
            public static string LCID
            {
                get { return "lcid"; }
            }
            public static string FolderId
            {
                get { return "folderId"; }
            }
            public static string FileType 
            {
                get { return "fileType"; }
            }
            public static string Size 
            {
                get { return "size"; }
            }
            public static string Path 
            {
                get { return "path"; }
            }
            public static string Name 
            {
                get { return "name"; }
            }
            public static string Description 
            {
                get { return "description"; }
            }
            public static string PreviewUrl 
            {
                get { return "previewUrl"; }
            }
            public static string FullNameFile 
            {
                get { return "fullNameFile"; }
            }
            public static string DateCreated
            {
                get { return "dateCreated"; }
            }
            public static string MediaContentCount
            {
                get { return "MediaContentCount"; }
            }
            public static string MediaContentByFolderCount { get { return "mediaContentByFolderCount"; } }
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
            table.Columns.Add(new DataColumn(Columns.MediaContentId));
            table.Columns.Add(new DataColumn(Columns.LCID));
            table.Columns.Add(new DataColumn(Columns.FolderId));
            table.Columns.Add(new DataColumn(Columns.FileType));
            table.Columns.Add(new DataColumn(Columns.Size));
            table.Columns.Add(new DataColumn(Columns.Path));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.Description));
            table.Columns.Add(new DataColumn(Columns.PreviewUrl));
            table.Columns.Add(new DataColumn(Columns.FullNameFile));
            table.Columns.Add(new DataColumn(Columns.DateCreated));
            table.Columns.Add(new DataColumn(Columns.MediaContentByFolderCount));
            table.Columns.Add(new DataColumn(Columns.MediaContentCount));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        #endregion

    }
}
