using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportSchedulerActionController : BaseController<ReportSchedulerActionController>
    {
        /// <summary>
        ///     This method return us all ReportSchedulerAction data from database
        /// </summary>
        /// <returns>
        /// List of ReportSchedulerAction objects
        /// </returns>
        [Obsolete("Deprecated", true)]
        public List<ReportSchedulerAction> GetAll()
        {
            return GetAllAsync().Result;
        }

        /// <summary>
        ///     Get ReportSchedulerAction Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return ReportSchedulerAction object
        /// </returns>
        [Obsolete("Deprecated", true)]
        public ReportSchedulerAction GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        /// <summary>
        ///     This method accept author id and return list of ReportSchedulerAction object by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// List<ReportSchedulerAction> 
        /// </returns>
        [Obsolete("Deprecated", true)]
        public List<ReportSchedulerAction> GetByAuthorId(long id)
        {
            return GetByAuthorIdAsync(id).Result;
        }
        /// <summary>
        ///  This method accept ReportScheduler object and return list of ReportSchedulerAction object by provided ReportSchedulerID   
        /// </summary>
        /// <param name="reportScheduler"></param>
        /// <returns>
        /// List<ReportSchedulerAction> 
        /// </returns>
        [Obsolete("Deprecated", true)]
        public List<ReportSchedulerAction> GetByReportScheduler(ReportScheduler reportScheduler)
        {
            return GetByReportSchedulerAsync(reportScheduler).Result;
        }
        /// <summary>
        /// This method accept ReportSchedulerAction object which we want to save in database
        /// </summary>
        /// <param name="report"></param>
        /// <returns>
        /// Returns ReportSchedulerAction object 
        /// </returns>
        [Obsolete("Deprecated", true)]
        public ReportSchedulerAction Save(ReportSchedulerAction report)
        {
            return SaveAsync(report).Result;
        }

        /// <summary>
        ///      Delete ReportSchedulerAction Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        [Obsolete("Deprecated", true)]
        public bool Delete(ReportSchedulerAction obj)
        {
            return DeleteAsync(obj).Result;
        }

        /// <summary>
        ///      Delete ReportSchedulerAction Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        [Obsolete("Deprecated", true)]
        public bool DeleteByReportScheduler(ReportScheduler obj)
        {
            return DeleteByReportSchedulerAsync(obj).Result;
        }
    }
}
