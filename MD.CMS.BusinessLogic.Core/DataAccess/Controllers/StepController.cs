using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Step;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain
{
    /// <summary>
    /// Controller for managing step objects with step DB table
    /// </summary>
   public partial class StepController:BaseController<StepController>
    {
        /// <summary>
        ///     Create step object from the data returned by the databse
        /// </summary>
        /// <param name="row"></param>
        /// <returns>
        ///  Step object
        /// </returns>
        private async Task<Step> CreateAsync(DataRow row)
        {

            Step obj = Create<Step, long>(row, Data.Columns.StepId);

            if (obj != null)
            {
                obj.Order = row.GetValue<int>(Data.Columns.Order);
                obj.ComboOperator = (StepComboOperator)row.GetValue<int>(Data.Columns.ComboOperator);
                obj.ApprovalChainId = row.GetValue<long>(Data.Columns.ChainId);
                obj.Actions = await StepActionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByStepIdAsync(obj.Id);
                obj.UserIds = await GetUserIdsByStepIdAsync(obj.Id);
            }

            return obj;
        }

        /// <summary>
        ///     This method return us all Step data from database
        /// </summary>
        /// <returns>
        /// List of Step objects
        /// </returns>
        public async Task<List<Step>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<Step> steps = new List<Step>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Step;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Methods.GetAll.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                steps = (await Task.WhenAll((await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(async row => await CreateAsync(row)))).ToList();
            }
            return steps;
        }

        /// <summary>
        ///     Get Step Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return Step object
        /// </returns>
        public async Task<Step> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Step;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Methods.GetById.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Parameters.StepId.GetIntValue()) { Value = id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
        }

        /// <summary>
        ///     This method accept ApprovalChain id and return list of Steps object by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// List<Step> 
        /// </returns>
        public async Task<List<Step>> GetByApprovalChainIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Step> steps = new List<Step>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Step;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Methods.SelectByChainId.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Parameters.ChainId.GetIntValue()) { Value = id });

                steps = (await Task.WhenAll((await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(async row => await CreateAsync(row)))).ToList();

            }
            return steps;
        }


        /// <summary>
        ///     This method accept ApprovalChain Step id and return list of User object by provided id
        /// </summary>
        /// <param name="id">Steo id</param>
        /// <returns>
        /// List<Step> 
        /// </returns>
        //Call Method from StepUser Methods
        public async Task<List<string>> GetUserIdsByStepIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<string> userIds = new List<string>();

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.StepUser;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.StepUser.Methods.SelectByStepId.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepUser.Parameters.StepId.GetIntValue()) { Value = id });


                DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

                foreach (DataRow row in results.Rows)
                {
                    userIds.Add(row.GetValue<string>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.StepUser.Data.Columns.UserId));
                }
              
            }
            return userIds;
        }

        /// <summary>
        /// This method accept Step object which we want to save in database
        /// </summary>
        /// <param name="step"></param>
        /// <returns>
        /// Returns Step object 
        /// </returns>
        public async Task<Step> SaveAsync(Step step)
        {
            await AuthenticateAndAuthorizeAsync();
            Step savedStep = new Step();
            List<string> userIds = new List<string>();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Step;
                if (step.Id.Equals(default(long)))
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Methods.Insert.GetIntValue();
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Parameters.StepId.GetIntValue()) { Value = step.Id });
                    userIds = await GetUserIdsByStepIdAsync(step.Id);
                }
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Parameters.ChainId.GetIntValue()) { Value = step.ApprovalChainId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Parameters.Order.GetIntValue()) { Value = step.Order });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Parameters.ComboOperator.GetIntValue()) { Value = step.ComboOperator });

                method.ClearCache = true;

                savedStep = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

                //Add users to step

                foreach (var userId in step.UserIds)
                {
                    Method userMethod = new Method();

                    if (userIds.Contains(userId))
                    {
                        continue; // skip StepUser creation if already exists
                    }

                    userMethod.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    userMethod.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Methods.Insert.GetIntValue();
                    userMethod.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.StepUser;
                    userMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepUser.Parameters.StepId.GetIntValue()) { Value = savedStep.Id });
                    userMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepUser.Parameters.UserId.GetIntValue()) { Value = userId });

                    await ExecuteMethodRowAsync(userMethod, this.UseDefaultPlugin);
                }

                //Check for removed StepUser and remove them from DB if not existing in new list
                foreach (string userId in userIds)
                {
                    Method userMethod = new Method();

                    if (step.UserIds.Contains(userId))
                    {
                        continue; // stepuser is not removed from the list
                    }
                        
                    userMethod.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                    userMethod.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.StepAction.Methods.Delete.GetIntValue();
                    userMethod.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.StepUser;
                    userMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepUser.Parameters.StepId.GetIntValue()) { Value = savedStep.Id });
                    userMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.StepUser.Parameters.UserId.GetIntValue()) { Value = userId });

                    await ExecuteMethodBooleanAsync(userMethod, this.UseDefaultPlugin);
                }

            }
            savedStep.Actions = await StepActionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByStepIdAsync(savedStep.Id);
            savedStep.UserIds = await GetUserIdsByStepIdAsync(savedStep.Id);
            return savedStep;
        }

        /// <summary>
        ///      Delete Step Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        public async Task<bool> DeleteAsync(Step obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Step;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Step.Parameters.StepId.GetIntValue()) { Value = obj.Id });
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
