using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string ChainId
            {
                get { return "ChainId"; }
            }

            public static string FolderId
            {
                get { return "FolderId"; }
            }
            public static string IsActive
            {
                get { return "IsActive"; }
            }

            #endregion
        }
        #endregion
        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.ChainId));
            table.Columns.Add(new DataColumn(Columns.FolderId));
            table.Columns.Add(new DataColumn(Columns.IsActive));
            return table;
        }
        #endregion
    }
}
