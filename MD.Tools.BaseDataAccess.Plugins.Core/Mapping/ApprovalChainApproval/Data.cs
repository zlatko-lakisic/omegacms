using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ApprovalId
            {
                get { return "ApprovalId"; }
            }
            public static string ApprovalType
            {
                get { return "ApprovalType"; }
            }
            public static string UserId
            {
                get { return "UserId"; }
            }
            public static string ContentId
            {
                get { return "ContentId"; }
            }
            public static string ContentLCID
            {
                get { return "ContentLCID"; }
            }
            public static string ContentDateCreated
            {
                get { return "ContentDateCreated"; }
            }
            public static string StepId
            {
                get { return "StepId"; }
            }
            public static string ReviewDate
            {
                get { return "ReviewDate"; }
            }
            public static string Comment
            {
                get { return "Comment"; }
            }
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.ApprovalId));
            table.Columns.Add(new DataColumn(Columns.ApprovalType));
            table.Columns.Add(new DataColumn(Columns.UserId));
            table.Columns.Add(new DataColumn(Columns.ContentId));
            table.Columns.Add(new DataColumn(Columns.ContentLCID));
            table.Columns.Add(new DataColumn(Columns.ContentDateCreated));
            table.Columns.Add(new DataColumn(Columns.StepId));
            table.Columns.Add(new DataColumn(Columns.ReviewDate));
            table.Columns.Add(new DataColumn(Columns.Comment));
            return table;
        }
        #endregion
    }
}
