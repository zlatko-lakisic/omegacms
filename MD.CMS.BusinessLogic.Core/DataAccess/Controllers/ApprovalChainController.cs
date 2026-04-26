using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.Helpers.Attributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain
{
    /// <summary>
    /// Controller for manipulating approval chain
    /// </summary>
    public partial class ApprovalChainController : BaseController<ApprovalChainController>
    {
        /// <summary>
        ///     This function accept DataRow with ApprovalChain columns,and make ApprovalChain object
        /// </summary>
        /// <param name="row"></param>
        /// <returns>
        ///  ApprovalChain object
        /// </returns>
        private async Task<Entities.ApprovalChain.ApprovalChain> CreateAsync(DataRow row)
        {
            Entities.ApprovalChain.ApprovalChain obj = Create<Entities.ApprovalChain.ApprovalChain, long>(row, Data.Columns.ChainId);

            if (obj != null)
            {
                obj.FolderId = row.GetValue<long>(Data.Columns.FolderId);
                obj.IsActive = Convert.ToBoolean(row.GetValue<int>(Data.Columns.IsActive));
                obj.Steps = await StepController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByApprovalChainIdAsync(obj.Id);
            }
            return obj;
        }

        /// <summary>
        ///     This method return us all ApprovalChain data from database
        /// </summary>
        /// <returns>
        /// List of ApprovalChain objects
        /// </returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain, PermissionAccessTypeEnum.Read)]
        public async Task<List<Entities.ApprovalChain.ApprovalChain>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Methods.GetAll.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;

                ConcurrentQueue<Entities.ApprovalChain.ApprovalChain> list = new ConcurrentQueue<Entities.ApprovalChain.ApprovalChain>();
                await Task.WhenAll((await ExecuteMethodTableAsync(method, UseDefaultPlugin)).AsEnumerable().Select(async row => {
                    list.Enqueue(await ApprovalChainController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row));
                }));

                return list.ToList();
            }
        }
        /// <summary>
        ///     Get ApprovalChain Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return ApprovalChain object
        /// </returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain, PermissionAccessTypeEnum.Read)]
        public async Task<Entities.ApprovalChain.ApprovalChain> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Methods.GetById.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Parameters.ChainId.GetIntValue()) { Value = id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
        }

        /// <summary>
        ///     This method accept FolderId id and return ApprovalChain object by provided folder id.
        ///     There can be only one approval chain per folder.
        /// </summary>
        /// <param name="id">Folder id for which approval chain is needed</param>
        /// <returns>
        /// Approval chain for given folder id
        /// </returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain, PermissionAccessTypeEnum.Read)]
        public async Task<Entities.ApprovalChain.ApprovalChain> GetByFolderIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Methods.SelectByFolderId.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Parameters.FolderId.GetIntValue()) { Value = id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "FolderId_i" });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "ChainId_i desc" });
                return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
        }


        /// <summary>
        /// This method accept ApprovalChain object which we want to save in database
        /// </summary>
        /// <param name="approvalChain">ApprovalChain to save</param>
        /// <returns>
        /// Returns ApprovalChain object 
        /// </returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain, PermissionAccessTypeEnum.Write)]
        public async Task<Entities.ApprovalChain.ApprovalChain> SaveAsync(Entities.ApprovalChain.ApprovalChain approvalChain)
        {
            await AuthenticateAndAuthorizeAsync();
            Entities.ApprovalChain.ApprovalChain savedApprovedChain = new Entities.ApprovalChain.ApprovalChain();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain;
                if (approvalChain.Id.Equals(default(long)))
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Methods.Insert.GetIntValue();
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Parameters.ChainId.GetIntValue()) { Value = approvalChain.Id });
                }
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Parameters.FolderId.GetIntValue()) { Value = approvalChain.FolderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Parameters.IsActive.GetIntValue()) { Value = approvalChain.IsActive });

                method.ClearCache = true;

                savedApprovedChain = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
            return savedApprovedChain;
        }

        /// <summary>
        ///      Delete ApprovalChain Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain, PermissionAccessTypeEnum.Delete)]
        public async Task<bool> DeleteAsync(Entities.ApprovalChain.ApprovalChain obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChain.Parameters.ChainId.GetIntValue()) { Value = obj.Id });
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
