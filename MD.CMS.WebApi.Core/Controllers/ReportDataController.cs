using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Net;
using System.Threading.Tasks;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace MD.CMS.WebApi.Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Reporting")]
    public class ReportDataController : BaseLoggedOnWebApiController
    {
        /// <summary>
        /// Gets all ReportData objects
        /// </summary>
        /// <returns>If everything went fine, "Ok" status code with list of ReportData objects, InternalServerError otherwise</returns>
        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<ReportData> reportData = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDataController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync();
            if (reportData != null)
            {
                return Ok(reportData);
            }
            else
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets all ReportData objects which belong to ReportScheduler object with given id
        /// </summary>
        /// <param name="id">Report Scheduler Id</param>
        /// <returns>If found, "Ok" status code with list of found ReportData objects, else "NotFound" or "BadRequest" status code</returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetByReportSchedulerId")]
        public async Task<IActionResult> GetByReportSchedulerId(int id)
        {
            if (id != default(long))
            {
                ReportScheduler reportScheduler = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
                if (reportScheduler != null)
                {
                    List<ReportData> reportData = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDataController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByReportSchedulerAsync(reportScheduler);
                    return Ok(reportData);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Saves ReportData object with all its properties
        /// </summary>
        /// <param name="reportData">ReportData object to save</param>
        /// <returns>"Ok" status code with saved ReportData object or "BadRequest" with ModelState in case that ModelState is not valid</returns>
        [HttpPost]
        [Route("[action]")]
        [ActionName("Save")]
        public async Task<IActionResult> Save([FromBody] ReportData reportData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //todo - get real reportScheduler id value
            ReportData newReportData = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDataController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(reportData, 1);
            return Ok(newReportData);
        }


        /// <summary>
        /// Saves ReportData object with all its properties
        /// </summary>
        /// <param name="obj">Report definition object to process</param>
        /// <returns>"Ok" status code with saved ReportData object or "BadRequest" with ModelState in case that ModelState is not valid</returns>
        [HttpPost]
        [Route("[action]")]
        [ActionName("GenerateReportdata")]
        public async Task<IActionResult> GenerateReportdata([FromBody] ReportDefinition obj)
        {
            DataTable results = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetDataAsync(obj);
            dynamic resultsTable = new ExpandoObject();
            resultsTable.columns = results.Columns.Cast<DataColumn>().Select(column =>
            {
                dynamic c = new ExpandoObject();
                c.title = column.ColumnName;
                return c;
            });
            resultsTable.rows = results.Rows.Cast<DataRow>().Select(row =>
            {
                List<dynamic> rowValues = new List<dynamic>();
                foreach (DataColumn column in results.Columns)
                {
                    dynamic rowValue = new ExpandoObject();
                    rowValue.value = row[column];
                    rowValues.Add(rowValue);
                }
                return rowValues;
            });
            return Ok(resultsTable);
        }

    }
}