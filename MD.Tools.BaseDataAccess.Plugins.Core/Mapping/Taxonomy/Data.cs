using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy
{
    public class Data
    {

        #region Columns
        public class Columns
        {
            #region Properties
            public static string Name
            {
                get { return "taxonomyName"; }
            }
            public static string LCID
            {
                get { return "lcid"; }
            }
            public static string Description
            {
                get { return "description"; }
            }
            public static string TaxonomyPath
            {
                get { return "TaxonomyPath"; }
            }
            public static string ParentId
            {
                get { return "parentId"; }
            }
            public static string TaxonomyId
            {
                get { return "taxonomyId"; }
            }
            public static string Order
            {
                get { return "order"; }
            }
            public static string TaxonomyCount { get { return "taxonomyCount"; } }
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
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.LCID));
            table.Columns.Add(new DataColumn(Columns.Description));
            table.Columns.Add(new DataColumn(Columns.TaxonomyPath));
            table.Columns.Add(new DataColumn(Columns.ParentId));
            table.Columns.Add(new DataColumn(Columns.TaxonomyId));
            table.Columns.Add(new DataColumn(Columns.TaxonomyCount));
            table.Columns.Add(new DataColumn(Columns.Order));
            table.Columns.Add(new DataColumn(Columns.TotalCount));
            return table;
        }
        #endregion

    }
}
