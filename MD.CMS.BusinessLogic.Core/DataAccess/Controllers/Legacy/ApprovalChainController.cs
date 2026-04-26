using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.Helpers.Attributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain
{
    /// <summary>
    /// Controller for manipulating approval chain
    /// </summary>
    public partial class ApprovalChainController : BaseController<ApprovalChainController>
    {
        /// <summary>
        ///     This method return us all ApprovalChain data from database
        /// </summary>
        /// <returns>
        /// List of ApprovalChain objects
        /// </returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain, PermissionAccessTypeEnum.Read)]
        public List<Entities.ApprovalChain.ApprovalChain> GetAll()
        {
            return GetAllAsync().Result;
        }
        /// <summary>
        ///     Get ApprovalChain Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return ApprovalChain object
        /// </returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain, PermissionAccessTypeEnum.Read)]
        public Entities.ApprovalChain.ApprovalChain GetById(long id)
        {
            return GetByIdAsync(id).Result;
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
        public Entities.ApprovalChain.ApprovalChain GetByFolderId(long id)
        {
            return GetByFolderIdAsync(id).Result;
        }


        /// <summary>
        /// This method accept ApprovalChain object which we want to save in database
        /// </summary>
        /// <param name="approvalChain">ApprovalChain to save</param>
        /// <returns>
        /// Returns ApprovalChain object 
        /// </returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain, PermissionAccessTypeEnum.Write)]
        public Entities.ApprovalChain.ApprovalChain Save(Entities.ApprovalChain.ApprovalChain approvalChain)
        {
            return SaveAsync(approvalChain).Result;
        }

        /// <summary>
        ///      Delete ApprovalChain Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ApprovalChain, PermissionAccessTypeEnum.Delete)]
        public bool Delete(Entities.ApprovalChain.ApprovalChain obj)
        {
            return DeleteAsync(obj).Result;
        }

    }
}
