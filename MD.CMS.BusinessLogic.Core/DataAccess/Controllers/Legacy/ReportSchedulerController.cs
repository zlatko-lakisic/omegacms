using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;


namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportSchedulerController : BaseController<ReportSchedulerController>
    {
        /// <summary>
        ///     This method return us all ReportScheduler data from database
        /// </summary>
        /// <returns>
        /// List of ReportScheduler objects
        /// </returns>
        public List<ReportScheduler> GetAll(string sort = "Name ASC")
        {
            return GetAllAsync(sort).Result;
        }
        /// <summary>
        /// This method return all ReportScheduler data with pagination from database
        /// </summary>
        /// <returns>
        /// List of ReportScheduler objects
        /// </returns>
        public Entities.Base.BasePaginationEntity<ReportScheduler> GetAllWithPagination(long pageIndex, long pageSize, string searchTerm, string searchColumn, string sort = "Name ASC")
        {
            return GetAllWithPaginationAsync(pageIndex, pageSize, searchTerm, searchColumn, sort).Result;
        }
        /// <summary>
        /// This method return count of all ReportScheduler data from database
        /// </summary>
        /// <returns>
        /// Number of objects
        /// </returns>
        public long GetAllCount(string searchTerm, string searchColumn)
        {
            return GetAllCountAsync(searchTerm, searchColumn).Result;
        }
        /// <summary>
        ///     Get ReportScheduler Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return ReportScheduler object
        /// </returns>
        public ReportScheduler GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }
        /// <summary>
        ///     This method accept author id and return list of ReportScheduler object by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// List<ReportScheduler> 
        /// </returns>
        public List<ReportScheduler> GetByAuthorId(long id)
        {
            return GetByAuthorIdAsync(id).Result;
        }
        /// <summary>
        ///  This method accept ReportDefinition object and return list of ReportScheduler object by provided ReportDefinitionID   
        /// </summary>
        /// <param name="reportDefinition"></param>
        /// <returns>
        /// List<ReportScheduler> 
        /// </returns>
        public List<ReportScheduler> GetByReportDefinition(ReportDefinition reportDefinition)
        {
            return GetByReportDefinitionAsync(reportDefinition).Result;
        }
        /// <summary>
        /// This method accept ReportScheduler object which we want to save in database
        /// </summary>
        /// <param name="report"></param>
        /// <returns>
        /// Returns ReportScheduler object 
        /// </returns>
        public ReportScheduler Save(ReportScheduler reportScheduler)
        {
            return SaveAsync(reportScheduler).Result;
        }

        /// <summary>
        ///      Delete ReportScheduler Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        public bool Delete(ReportScheduler obj)
        {
            return DeleteAsync(obj).Result;
        }

        public IEnumerable<ReportScheduler> GetSchedulersForProcessing()
        {
            return GetSchedulersForProcessingAsync().Result;
        }
    }
}