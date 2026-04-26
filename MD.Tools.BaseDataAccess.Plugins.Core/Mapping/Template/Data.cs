using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Template
{
    public class Data
    {
        #region Columns
        public class Columns
        {
            #region Properties
            public static string TemplateId
            {
                get { return "templateId"; }
            }
            public static string Name
            {
                get { return "name"; }
            }
            public static string Description
            {
                get { return "description"; }
            }
            public static string TemplateUrl
            {
                get { return "templateUrl"; }
            }
            public static string IsDeleted
            {
                get { return "isDeleted"; }
            }
            public static string TemplatesCount
            {
                get { return "templatesCount"; }
            } 
            public static string TotalCount 
            {
                get{ return "TotalCount"; }
            }
            #endregion
        }
        #endregion

        #region Methods
        public static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(Columns.TemplateId));
            table.Columns.Add(new DataColumn(Columns.Name));
            table.Columns.Add(new DataColumn(Columns.Description));
            table.Columns.Add(new DataColumn(Columns.TemplateUrl));          
            //table.Columns.Add(new DataColumn(Columns.IsDeleted));
            table.Columns.Add(new DataColumn(Columns.TemplatesCount));          
            table.Columns.Add(new DataColumn(Columns.TotalCount));          
           
            table.Columns.Add("Childs", typeof(DataTable));
            return table;
        }

        //public static DataTable GetChildTable()
        //{
        //    DataTable table = new DataTable();
        //    table.Columns.Add(new DataColumn("name"));
        //    table.Columns.Add(new DataColumn("value"));

        //    return table;
        //}
        #endregion
    }
}
