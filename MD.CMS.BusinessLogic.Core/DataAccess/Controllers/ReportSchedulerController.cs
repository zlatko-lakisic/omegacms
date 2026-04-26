using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportSchedulerController : BaseController<ReportSchedulerController>
    {
        /// <summary>
        ///     This function accept DataRow with ReportScheduler columns,and make ReportScheduler object
        /// </summary>
        /// <param name="row"></param>
        /// <returns>
        ///  ReportScheduler object
        /// </returns>
        private async Task<ReportScheduler> CreateAsync(DataRow row)
        {
            ReportScheduler obj = Create<ReportScheduler, long>(row, Data.Columns.ReportSchedulerId);

            if (obj != null)
            {
                obj.Name = row.GetValue<string>(Data.Columns.Name);
                obj.AuthorId = row.GetValue<string>(Data.Columns.AuthorId);
                obj.DateCreated = row.GetValue<DateTime>(Data.Columns.DateCreated);
                obj.DateEdited = row.GetValue<DateTime>(Data.Columns.DateEdited);
                obj.IsRecurring = row.GetValue<bool>(Data.Columns.IsRecurring);
                obj.Interval = new TimeSpan(0, 0, 0,row.GetValue<int>(Data.Columns.Interval));
                obj.Start = row.GetValue<DateTime>(Data.Columns.Start);
                obj.End = row.GetValue<DateTime>(Data.Columns.End);
                obj.ReportId = row.GetValue<int>(Data.Columns.ReportDefinitionId);
                obj.IsActive = row.GetValue<bool>(Data.Columns.IsActive);
                obj.IsDeleted = row.GetValue<bool>(Data.Columns.IsDeleted);
                obj.Actions = await ReportSchedulerActionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByReportSchedulerAsync(obj);
                if (!obj.AuthorId.Equals(default(long)))
                {
                    obj.Author = await UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(obj.AuthorId, true, isFull: false);
                }
            }

            return obj;
        }


        /// <summary>
        ///     This method return us all ReportScheduler data from database
        /// </summary>
        /// <returns>
        /// List of ReportScheduler objects
        /// </returns>
        public async Task<List<ReportScheduler>> GetAllAsync(string sort = "Name ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportScheduler> schedulers = new List<ReportScheduler>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportScheduler;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.GetAll.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.Sort.GetIntValue()) {Value = sort});
                schedulers = (await Task.WhenAll((await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(async row => await CreateAsync(row)))).ToList();
            }
            return schedulers;
        }
        /// <summary>
        /// This method return all ReportScheduler data with pagination from database
        /// </summary>
        /// <returns>
        /// List of ReportScheduler objects
        /// </returns>
        public async Task<Entities.Base.BasePaginationEntity<ReportScheduler>> GetAllWithPaginationAsync(long pageIndex, long pageSize, string searchTerm, string searchColumn, string sort = "Name ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportScheduler> schedulers = new List<ReportScheduler>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportScheduler;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.GetAllWithPagination.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = pageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = pageSize });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows) {
                schedulers.Add(await CreateAsync(row));
            }
            Entities.Base.BasePaginationEntity<ReportScheduler> basePaginationEntity = new Entities.Base.BasePaginationEntity<ReportScheduler>();
            basePaginationEntity.Items = schedulers;
            if (table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntity;
        }
        /// <summary>
        /// This method return count of all ReportScheduler data from database
        /// </summary>
        /// <returns>
        /// Number of objects
        /// </returns>
        public async Task<long> GetAllCountAsync(string searchTerm, string searchColumn)
        {
            await AuthenticateAndAuthorizeAsync();
            long count = 0;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportScheduler;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.GetAllCount.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
                DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
                count = row.GetValue<long>("ReportSchedulersCount");
            }
            return count;
        }
        /// <summary>
        ///     Get ReportScheduler Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return ReportScheduler object
        /// </returns>
        public async Task<ReportScheduler> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportScheduler;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.GetById.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.ReportSchedulerId.GetIntValue()) { Value = id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
        }
        /// <summary>
        ///     This method accept author id and return list of ReportScheduler object by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// List<ReportScheduler> 
        /// </returns>
        public async Task<List<ReportScheduler>> GetByAuthorIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportScheduler> schedulers = new List<ReportScheduler>();
            using (Method method = new Method())
            {

                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportScheduler;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.SelectByAuthorId.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.AuthorId.GetIntValue()) { Value = id });

                schedulers = (await Task.WhenAll((await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(async row => await CreateAsync(row)))).ToList();

            }
            return schedulers;
        }
        /// <summary>
        ///  This method accept ReportDefinition object and return list of ReportScheduler object by provided ReportDefinitionID   
        /// </summary>
        /// <param name="reportDefinition"></param>
        /// <returns>
        /// List<ReportScheduler> 
        /// </returns>
        public async Task<List<ReportScheduler>> GetByReportDefinitionAsync(ReportDefinition reportDefinition)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportScheduler> schedulers = new List<ReportScheduler>();
            using (Method method = new Method())
            {

                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportScheduler;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.SelectByReportDefinitionId.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.ReportDefinitionId.GetIntValue()) { Value = reportDefinition.Id });

                schedulers = (await Task.WhenAll((await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(async row => await CreateAsync(row)))).ToList();

            }
            return schedulers;
        }
        /// <summary>
        /// This method accept ReportScheduler object which we want to save in database
        /// </summary>
        /// <param name="report"></param>
        /// <returns>
        /// Returns ReportScheduler object 
        /// </returns>
        public async Task<ReportScheduler> SaveAsync(ReportScheduler reportScheduler)
        {
            await AuthenticateAndAuthorizeAsync();
            ReportScheduler savedReportScheduler = new ReportScheduler();

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportScheduler;
                if (reportScheduler.Id.Equals(default(long)))
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.Insert.GetIntValue();
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.ReportSchedulerId.GetIntValue()) { Value = reportScheduler.Id });
                }
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.AuthorId.GetIntValue()) { Value = reportScheduler.AuthorId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.Name.GetIntValue()) { Value = reportScheduler.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.IsRecurring.GetIntValue()) { Value = reportScheduler.IsRecurring });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.Interval.GetIntValue()) { Value = (long)reportScheduler.Interval.TotalSeconds });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.Start.GetIntValue()) { Value = reportScheduler.Start });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.End.GetIntValue()) { Value = reportScheduler.End });
                //We will need to remove this hardcoded reportDefinitionID later
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.ReportDefinitionId.GetIntValue()) { Value = reportScheduler.ReportId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.IsActive.GetIntValue()) { Value = reportScheduler.IsActive });

                method.ClearCache = true;

                savedReportScheduler = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                //method.WaitForOnBeforeCompleted();
                List<ReportSchedulerAction> newActions = new List<ReportSchedulerAction>();
                List<ReportSchedulerAction> exActions = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerActionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByReportSchedulerAsync(reportScheduler);



                if (exActions != null)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerActionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByReportSchedulerAsync(reportScheduler);
                }
                if (savedReportScheduler != null && reportScheduler.Actions != null && reportScheduler.Actions.Any())
                {
                    foreach (ReportSchedulerAction action in reportScheduler.Actions)
                    {
                        action.SchedulerId = savedReportScheduler.Id;
                        action.AuthorId = savedReportScheduler.AuthorId;
                        ReportSchedulerAction savedAction = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerActionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(action);
                        newActions.Add(action);

                    }
                }
                savedReportScheduler.Actions = newActions;
              
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return savedReportScheduler;
        }

        /// <summary>
        ///      Delete ReportScheduler Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        public async Task<bool> DeleteAsync(ReportScheduler obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportScheduler;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Parameters.ReportSchedulerId.GetIntValue()) { Value = obj.Id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);

                if (success)
                    obj = null;
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<IEnumerable<ReportScheduler>> GetSchedulersForProcessingAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportScheduler> schedulersToProcess = new List<ReportScheduler>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportScheduler;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler.Methods.GetSchedulersForProcessing.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                schedulersToProcess = (await Task.WhenAll((await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(async row => await CreateAsync(row)))).ToList();
            }
            return schedulersToProcess;
        }
    }
}