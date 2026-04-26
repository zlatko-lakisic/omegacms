using System;
using System.Collections.Generic;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.CMS.BusinessLogic.Core.Helpers.Attributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain
{
    public partial class ApprovalChainApprovalController : BaseController<ApprovalChainApprovalController>
    {
        /// <summary>
        /// Create ApprovalChainApproval from DataRow and fill all the necessary fields
        /// </summary>
        /// <param name="row">Row returned by sql procedure</param>
        /// <returns>ApprovalChainApproval object</returns>
        private async Task<ApprovalChainApproval> CreateAsync(DataRow row)
        {
            ApprovalChainApproval obj = Create<ApprovalChainApproval, long>(row, Data.Columns.ApprovalId);

            if (obj != null)
            {
                obj.User = await UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(row.GetValue<string>(Data.Columns.UserId));
                obj.Step = await StepController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(row.GetValue<long>(Data.Columns.StepId));
                obj.Content = (await ContentController<Entities.Content>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(new Options.ContentOptions()
                {
                    ContentIds = new string[] { row.GetValue<string>(Data.Columns.ContentId) }.ToList(),
                    FillFields = false,
                    LoadAuthor = false,
                    FillMetaData = false,
                    Lcid = row.GetValue<int>(Data.Columns.ContentLCID)
                })).FirstOrDefault();
                obj.ApprovalType = (StepActionType)row.GetValue<int>(Data.Columns.ApprovalType);
                obj.Comment = row.GetValue<String>(Data.Columns.Comment);
                obj.ReviewDate = row.GetValue<DateTime>(Data.Columns.ReviewDate);
            }

            return obj;
        }

        /// <summary>
        /// Get ApprovalChainApproval by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval, PermissionAccessTypeEnum.Read)]
        public async Task<ApprovalChainApproval> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Methods.GetById.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ApprovalId.GetIntValue()) { Value = id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
        }

        /// <summary>
        /// Get ApprovalChainApproval by Content waiting to be approved
        /// </summary>
        /// <param name="contentId">Content ID</param>
        /// <param name="lcid">Content LCID</param>
        /// <param name="contentDateCreated">Content date created</param>
        /// <returns>List of ApprovalChainApprovals</returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval, PermissionAccessTypeEnum.Read)]
        public async Task<List<ApprovalChainApproval>> GetByContentAsync(string contentId, int lcid, DateTime contentDateCreated)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Methods.GetByContent.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ContentId.GetIntValue()) { Value = contentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ContentDateCreated.GetIntValue()) { Value = contentDateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ContentLCID.GetIntValue()) { Value = lcid });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ApprovalId_i" });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "ReviewDate_s desc" });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;

                ConcurrentQueue<ApprovalChainApproval> list = new ConcurrentQueue<ApprovalChainApproval>();
                await Task.WhenAll((await ExecuteMethodTableAsync(method, UseDefaultPlugin)).AsEnumerable().Select(async row => {
                    list.Enqueue(await ApprovalChainApprovalController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row));
                }));

                return list.ToList();
            }
        }


        /// <summary>
        /// Add new or update existing ApprovalChainApproval
        /// </summary>
        /// <param name="approval">ApprovalChainApproval to add or update</param>
        /// <returns></returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval, PermissionAccessTypeEnum.Write)]
        public async Task<ApprovalChainApproval> SaveAsync(ApprovalChainApproval approval)
        {
            await AuthenticateAndAuthorizeAsync();
            ApprovalChainApproval savedApproval = new ApprovalChainApproval();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval;
                if (approval.Id.Equals(default(long)))
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Methods.Insert.GetIntValue();
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ApprovalId.GetIntValue()) { Value = approval.Id });
                }

                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.UserId.GetIntValue()) { Value = approval.User.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.StepId.GetIntValue()) { Value = approval.Step.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ContentId.GetIntValue()) { Value = approval.Content.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ContentDateCreated.GetIntValue()) { Value = approval.Content.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ContentLCID.GetIntValue()) { Value = approval.Content.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ReviewDate.GetIntValue()) { Value = DateTime.Now });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.Comment.GetIntValue()) { Value = approval.Comment});
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ApprovalType.GetIntValue()) { Value = approval.ApprovalType });

                method.ClearCache = true;

                savedApproval = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
            return savedApproval;
        }

        /// <summary>
        /// Delete existing ApprovalChainApproval
        /// </summary>
        /// <param name="approvalId">ApprovalChainApproval ID</param>
        /// <returns></returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval, PermissionAccessTypeEnum.Delete)]
        public async Task<bool> DeleteAsync(long approvalId)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ApprovalChainApproval.Parameters.ApprovalId.GetIntValue()) { Value = approvalId });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.ClearCache = true;
                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                method.WaitForOnAfterCompleted();
            }
            return success;
        }
    }
}
