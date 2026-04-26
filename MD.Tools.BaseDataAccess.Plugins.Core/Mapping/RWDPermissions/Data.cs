using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions
{
    public class Data
    {

        #region Columns
        public class Columns
        {
            #region Properties
            public static string Read
            {
                get { return "read"; }
            }
            public static string Write
            {
                get { return "write"; }
            }
            public static string Delete
            {
                get { return "delete"; }
            }
            public static string ContentId
            {
                get { return "contentId"; }
            }
            public static string ContentLCID
            {
                get { return "contentLCID"; }
            }
            public static string ContentDateCreated
            {
                get { return "contentDateCreated"; }
            }
            public static string MediaContentId
            {
                get { return "mediaContentId"; }
            }
            public static string MediaContentLCID
            {
                get { return "mediaContentLCID"; }
            }
            public static string MediaContentDateCreated
            {
                get { return "mediaContentDateCreated"; }
            }
            public static string FolderId
            {
                get { return "folderId"; }
            }
            public static string UserId
            {
                get { return "userId"; }
            }
            public static string ProfileTypeId
            {
                get { return "profileTypeId"; }
            }
            public static string Username
            {
                get { return "username"; }
            }

            #endregion



        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.Read));
            table.Columns.Add(new DataColumn(Columns.Write));
            table.Columns.Add(new DataColumn(Columns.Delete));
            table.Columns.Add(new DataColumn(Columns.ContentId));
            table.Columns.Add(new DataColumn(Columns.ContentLCID));
            table.Columns.Add(new DataColumn(Columns.ContentDateCreated));
            table.Columns.Add(new DataColumn(Columns.MediaContentId));
            table.Columns.Add(new DataColumn(Columns.MediaContentLCID));
            table.Columns.Add(new DataColumn(Columns.MediaContentDateCreated));
            table.Columns.Add(new DataColumn(Columns.FolderId));
            table.Columns.Add(new DataColumn(Columns.UserId));
            table.Columns.Add(new DataColumn(Columns.ProfileTypeId));
            table.Columns.Add(new DataColumn(Columns.Username));
            return table;
        }
        #endregion
    }
}
