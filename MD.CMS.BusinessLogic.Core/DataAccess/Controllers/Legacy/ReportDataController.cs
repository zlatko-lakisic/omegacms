using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportDataController : BaseController<ReportDataController>
    {
        /// <summary>
        ///     This method return us all ReportData data from database
        /// </summary>
        /// <returns>
        /// List of ReportData objects
        /// </returns>
        public List<ReportData> GetAll()
        {
            return GetAllAsync().Result;
        }

        /// <summary>
        ///  This method accept ReportScheduler object and return list of ReportData object by provided ReportSchedulerID   
        /// </summary>
        /// <param name="reportScheduler"></param>
        /// <returns>
        /// List<ReportData> 
        /// </returns>
        public List<ReportData> GetByReportScheduler(ReportScheduler reportScheduler)
        {
            return GetByReportSchedulerAsync(reportScheduler).Result;
        }

        /// <summary>
        /// This method accept ReportData object which we want to save in database
        /// </summary>
        /// <param name="report"></param>
        /// <returns>
        /// Returns ReportData object 
        /// </returns>
        public ReportData Save(ReportData report, long schedulerId)
        {
            return SaveAsync(report, schedulerId).Result;
        }
    }
}
