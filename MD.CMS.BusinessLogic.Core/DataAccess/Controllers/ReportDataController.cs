using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportDataController : BaseController<ReportDataController>
    {
        /// <summary>
        ///     This function accept DataRow with ReportData columns,and make ReportData object
        /// </summary>
        /// <param name="row"></param>
        /// <returns>
        ///  ReportData object
        /// </returns>
        private ReportData Create(DataRow row)
        {
            ReportData obj = Create<ReportData, long>(row, Data.Columns.ReportSchedulerId);

            if (obj != null)
            {
                obj.DateCreated = row.GetValue<DateTime>(Data.Columns.DateCreated);
                obj.Data = row.GetValue<DataSet>(Data.Columns.Data);
            }
            return obj;
        }

        /// <summary>
        ///     This method return us all ReportData data from database
        /// </summary>
        /// <returns>
        /// List of ReportData objects
        /// </returns>
        public async Task<List<ReportData>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportData> reportData = new List<ReportData>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportData;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportData.Methods.GetAll.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                reportData = (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(row => Create(row)).ToList();
            }
            return reportData;
        }

        /// <summary>
        ///  This method accept ReportScheduler object and return list of ReportData object by provided ReportSchedulerID   
        /// </summary>
        /// <param name="reportScheduler"></param>
        /// <returns>
        /// List<ReportData> 
        /// </returns>
        public async Task<List<ReportData>> GetByReportSchedulerAsync(ReportScheduler reportScheduler)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportData> reportData = new List<ReportData>();
            using (Method method = new Method())
            {

                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportData;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportData.Methods.GetByReportSchedulerId.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportData.Parameters.ReportSchedulerId.GetIntValue()) { Value = reportScheduler.Id });

                reportData = (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(row => Create(row)).ToList();

            }
            return reportData;
        }

        /// <summary>
        /// This method accept ReportData object which we want to save in database
        /// </summary>
        /// <param name="report"></param>
        /// <returns>
        /// Returns ReportData object 
        /// </returns>
        public async Task<ReportData> SaveAsync(ReportData report, long schedulerId)
        {
            await AuthenticateAndAuthorizeAsync();
            ReportData savedReport = new ReportData();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportData;
              
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportData.Methods.Insert.GetIntValue();

                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportData.Parameters.ReportId.GetIntValue()) {Value = report.ReportId});
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportData.Parameters.ReportSchedulerId.GetIntValue()) { Value = schedulerId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportData.Parameters.Data.GetIntValue()) { Value = ConvertDataSetToBlob(report.Data) });

                method.ClearCache = true;

                savedReport = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
               
            }
            return savedReport;
        }


        private string ConvertDataTableToString(DataTable dataTable)
        {
            StringBuilder output = new StringBuilder();
            int[] columnsWidths = new int[dataTable.Columns.Count];

            //Get column widths
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    int length = row[i].ToString().Length;
                    if (columnsWidths[i] < length)
                    {
                        columnsWidths[i] = length;
                    }
                }
            }

            //Get column titles
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                int length = dataTable.Columns[i].ColumnName.Length;
                if (columnsWidths[i] < length)
                {
                    columnsWidths[i] = length;
                }
            }

            //Write column titles
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                string text = dataTable.Columns[i].ColumnName;
                output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
            }
            output.Append("|\n" + new string('=', output.Length) + "\n");

            // Write Rows
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var text = row[i].ToString();
                    output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
                }
                output.Append("|\n");
            }

            return output.ToString();
        }

        private string PadCenter(string text, int maxLength)
        {
            int diff = maxLength - text.Length;
            return new string(' ', diff / 2) + text + new string(' ', (int)(diff / 2.0 + 0.5));
        }

        private byte[] ConvertDataSetToBlob(DataSet set)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DataTable table in set.Tables)
            {
                builder.Append(ConvertDataTableToString(table));
                builder.Append("_");
            }
            string dataSetString = builder.ToString();
            byte[] blob = Encoding.ASCII.GetBytes(dataSetString);
            return blob;
        }
    }
}
