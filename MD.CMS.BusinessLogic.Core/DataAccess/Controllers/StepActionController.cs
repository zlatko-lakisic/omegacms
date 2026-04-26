using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain
{
    public partial class StepActionController : BaseController<StepActionController>
    {
        /// <summary>
        ///     This function accept DataRow with StepAction columns,and make StepAction object
        /// </summary>
        /// <param name="row"></param>
        /// <returns>
        ///  StepAction object
        /// </returns>
        private StepAction Create(DataRow row)
        {

            StepAction obj = Create<StepAction, long>(row, Data.Columns.ActionId);

            if (obj != null)
            {
                obj.StepId = row.GetValue<long>(Data.Columns.StepId);
                obj.Type = (StepActionType)row.GetValue<int>(Data.Columns.Type);
                obj.Action = (StepActionAction)row.GetValue<int>(Data.Columns.Action);
                obj.RedirectTo = row.GetValue<int>(Data.Columns.RedirectTo);
                
            }

            return obj;
        }

        /// <summary>
        ///     This method return us all StepAction data from database
        /// </summary>
        /// <returns>
        /// List of StepAction objects
        /// </returns>
        public async Task<List<StepAction>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<StepAction> stepActions = new List<StepAction>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.StepAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Methods.GetAll.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                stepActions = (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(row => Create(row)).ToList();
            }
            return stepActions;
        }

        /// <summary>
        ///     Get StepAction Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return StepAction object
        /// </returns>
        public async Task<StepAction> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.StepAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Methods.GetById.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Parameters.ActionId.GetIntValue()) { Value = id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
        }

        /// <summary>
        ///     This method accept StepId id and return list of StepAction object by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// List<StepAction> 
        /// </returns>
        public async Task<List<StepAction>> GetByStepIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<StepAction> stepActions = new List<StepAction>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.StepAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Methods.SelectByStepId.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Parameters.StepId.GetIntValue()) { Value = id });

                stepActions = (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(row => Create(row)).ToList();

            }
            return stepActions;
        }

        /// <summary>
        /// This method accept StepAction object which we want to save in database
        /// </summary>
        /// <param name="stepAction"></param>
        /// <returns>
        /// Returns StepAction object 
        /// </returns>
        public async Task<StepAction> SaveAsync(StepAction stepAction)
        {
            await AuthenticateAndAuthorizeAsync();
            StepAction savedStepAction = new StepAction();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.StepAction;
                if (stepAction.Id.Equals(default(long)))
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Methods.Insert.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Parameters.StepId.GetIntValue()) { Value = stepAction.StepId });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Parameters.Type.GetIntValue()) { Value = stepAction.Type });
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Parameters.ActionId.GetIntValue()) { Value = stepAction.Id });
                }
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Parameters.Action.GetIntValue()) { Value = stepAction.Action });
                if(stepAction.Action == StepActionAction.Publish)
                {
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Parameters.RedirectTo.GetIntValue()) { Value = null });
                }
                else
                {
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Parameters.RedirectTo.GetIntValue()) { Value = stepAction.RedirectTo });
                }

                method.ClearCache = true;


                savedStepAction = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

            }
            return savedStepAction;
        }

        /// <summary>
        ///      Delete StepAction Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        public async Task<bool> DeleteAsync(StepAction obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.StepAction;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Parameters.ActionId.GetIntValue()) { Value = obj.Id });
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

    }
}
