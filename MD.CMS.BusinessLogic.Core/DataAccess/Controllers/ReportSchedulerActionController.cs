using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Data;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportSchedulerActionController : BaseController<ReportSchedulerActionController>
    {
        /// <summary>
        ///     This function accept DataRow with ReportSchedulerAction columns,and make ReportSchedulerAction object
        /// </summary>
        /// <param name="row"></param>
        /// <returns>
        /// ReportSchedulerAction object
        /// </returns>
        private ReportSchedulerAction Create(DataRow row)
        {
            ReportSchedulerAction obj = Create<ReportSchedulerAction, long>(row, Data.Columns.ReportSchedulerActionId);

            if (obj != null)
            {
                obj.SchedulerId = row.GetValue<long>(Data.Columns.SchedulerId);
                obj.Name = row.GetValue<string>(Data.Columns.Name);
                obj.AuthorId = row.GetValue<string>(Data.Columns.AuthorId);
                obj.DateCreated = row.GetValue<DateTime>(Data.Columns.DateCreated);
                obj.DateEdited = row.GetValue<DateTime>(Data.Columns.DateEdited);
                obj.ActionType = (ReportSchedulerAction.EnumAction)row.GetValue<int>(Data.Columns.ActionType);
                obj.Options = row.GetValue<string>(Data.Columns.Options);
                obj.IsActive = row.GetValue<bool>(Data.Columns.IsActive);

            }

            return obj;
        }


        /// <summary>
        ///     This method return us all ReportSchedulerAction data from database
        /// </summary>
        /// <returns>
        /// List of ReportSchedulerAction objects
        /// </returns>
        public async Task<List<ReportSchedulerAction>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportSchedulerAction> schedulerActions = new List<ReportSchedulerAction>();
            using (Method method = new Method())
            {

                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportSchedulerAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Methods.GetAll.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                schedulerActions = (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(row => Create(row)).ToList();
            }
            return schedulerActions;
        }

        /// <summary>
        ///     Get ReportSchedulerAction Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return ReportSchedulerAction object
        /// </returns>
        public async Task<ReportSchedulerAction> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportSchedulerAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Methods.GetById.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.ReportSchedulerActionId.GetIntValue()) { Value = id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
        }

        /// <summary>
        ///     This method accept author id and return list of ReportSchedulerAction object by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// List<ReportSchedulerAction> 
        /// </returns>
        public async Task<List<ReportSchedulerAction>> GetByAuthorIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportSchedulerAction> schedulerAction = new List<ReportSchedulerAction>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportSchedulerAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Methods.SelectByAuthorId.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.AuthorId.GetIntValue()) { Value = id });

                schedulerAction = (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(row => Create(row)).ToList();

            }
            return schedulerAction;
        }
        /// <summary>
        ///  This method accept ReportScheduler object and return list of ReportSchedulerAction object by provided ReportSchedulerID   
        /// </summary>
        /// <param name="reportScheduler"></param>
        /// <returns>
        /// List<ReportSchedulerAction> 
        /// </returns>
        public async Task<List<ReportSchedulerAction>> GetByReportSchedulerAsync(ReportScheduler reportScheduler)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportSchedulerAction> schedulerAction = new List<ReportSchedulerAction>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportSchedulerAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Methods.SelectByReportSchedulerId.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.SchedulerId.GetIntValue()) { Value = reportScheduler.Id });

                schedulerAction = (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(row => Create(row)).ToList();

            }
            return schedulerAction;
        }
        /// <summary>
        /// This method accept ReportSchedulerAction object which we want to save in database
        /// </summary>
        /// <param name="report"></param>
        /// <returns>
        /// Returns ReportSchedulerAction object 
        /// </returns>
        public async Task<ReportSchedulerAction> SaveAsync(ReportSchedulerAction report)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportSchedulerAction;
                
                //if (report.Id.Equals(default(long)))
                //{
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Methods.Insert.GetIntValue();
                //}
                //else
                //{
                //    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                //    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Methods.Update.GetIntValue();
                //    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.ReportSchedulerActionId.GetIntValue()) { Value = report.Id });
                //}
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.SchedulerId.GetIntValue()) { Value = report.SchedulerId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.AuthorId.GetIntValue()) { Value = report.AuthorId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.Name.GetIntValue()) { Value = report.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.ActionType.GetIntValue()) { Value = report.ActionType });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.Options.GetIntValue()) { Value = report.Options });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.IsActive.GetIntValue()) { Value = report.IsActive });

                method.ClearCache = true;

                report = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

            }
            return report;
        }

        /// <summary>
        ///      Delete ReportSchedulerAction Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        public async Task<bool> DeleteAsync(ReportSchedulerAction obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportSchedulerAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.ReportSchedulerActionId.GetIntValue()) { Value = obj.Id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);

                if (success)
                    obj = null;
                method.End();
                method.WaitForOnAfterCompleted();
            }
            return success;
        }

        /// <summary>
        ///      Delete ReportSchedulerAction Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        public async Task<bool> DeleteByReportSchedulerAsync(ReportScheduler obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportSchedulerAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Methods.DeleteAllBySchedulerId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction.Parameters.SchedulerId.GetIntValue()) { Value = obj.Id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);

                if (success)
                    obj = null;
                method.End();
                method.WaitForOnAfterCompleted();
            }
            return success;
        }
    }
}
