using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Message
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string MessageId
            {
                get { return "MessageId"; }
            }
            public static string Subject
            {
                get { return "Subject"; }
            }
            public static string MessageContent
            {
                get { return "MessageContent"; }
            }
            public static string ParentId
            {
                get { return "ParentId"; }
            }
            public static string IsRead
            {
                get { return "IsRead"; }
            }
            public static string MessageFolderId
            {
                get { return "MessageFolderId"; }
            }
            public static string DateAdded
            {
                get { return "DateAdded"; }
            }
            public static string Type
            {
                get { return "Type"; }
            }
            public static string UserId
            {
                get { return "UserId"; }
            }
            public static string MainThread
            {
                get { return "MainThread"; }
            }  
            public static string User2Id
            {
                get { return "User2Id"; }
            }
            public static string MessagesCount
            {
                get { return "MessagesCount"; }
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
            table.Columns.Add(new DataColumn(Columns.MessageId));
            table.Columns.Add(new DataColumn(Columns.Subject));
            table.Columns.Add(new DataColumn(Columns.MessageContent));
            table.Columns.Add(new DataColumn(Columns.ParentId));
            table.Columns.Add(new DataColumn(Columns.IsRead));
            table.Columns.Add(new DataColumn(Columns.MessageFolderId));
            table.Columns.Add(new DataColumn(Columns.DateAdded));
            table.Columns.Add(new DataColumn(Columns.Type));
            table.Columns.Add(new DataColumn(Columns.MainThread));
            table.Columns.Add(new DataColumn(Columns.User2Id));
            table.Columns.Add(new DataColumn(Columns.MessagesCount));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        #endregion
    }
}
