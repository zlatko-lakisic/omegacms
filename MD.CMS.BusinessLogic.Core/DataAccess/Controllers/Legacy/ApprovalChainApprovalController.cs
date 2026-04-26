using System;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain;
using MD.CMS.BusinessLogic.Core.Helpers.Attributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain
{
    public partial class ApprovalChainApprovalController : BaseController<ApprovalChainApprovalController>
    {
        /// <summary>
        /// Get ApprovalChainApproval by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval, PermissionAccessTypeEnum.Read)]
        public ApprovalChainApproval GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        /// <summary>
        /// Get ApprovalChainApproval by Content waiting to be approved
        /// </summary>
        /// <param name="contentId">Content ID</param>
        /// <param name="lcid">Content LCID</param>
        /// <param name="contentDateCreated">Content date created</param>
        /// <returns>List of ApprovalChainApprovals</returns>
        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval, PermissionAccessTypeEnum.Read)]
        public List<ApprovalChainApproval> GetByContent(string contentId, int lcid, DateTime contentDateCreated)
        {
            return GetByContentAsync(contentId, lcid, contentDateCreated).Result;
        }


        /// <summary>
        /// Add new or update existing ApprovalChainApproval
        /// </summary>
        /// <param name="approval">ApprovalChainApproval to add or update</param>
        /// <returns></returns>
        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval, PermissionAccessTypeEnum.Write)]
        public ApprovalChainApproval Save(ApprovalChainApproval approval)
        {
            return SaveAsync(approval).Result;
        }

        /// <summary>
        /// Delete existing ApprovalChainApproval
        /// </summary>
        /// <param name="approvalId">ApprovalChainApproval ID</param>
        /// <returns></returns>
        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChainApproval, PermissionAccessTypeEnum.Delete)]
        public bool Delete(long approvalId)
        {
            return DeleteAsync(approvalId).Result;
        }
    }
}
