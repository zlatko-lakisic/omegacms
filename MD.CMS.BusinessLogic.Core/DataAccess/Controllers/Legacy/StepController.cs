using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain;
using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain
{
    /// <summary>
    /// Controller for managing step objects with step DB table
    /// </summary>
    public partial class StepController:BaseController<StepController>
    {
        /// <summary>
        ///     This method return us all Step data from database
        /// </summary>
        /// <returns>
        /// List of Step objects
        /// </returns>
        [Obsolete("Deprecated", true)]
        public List<Step> GetAll()
        {
            return GetAllAsync().Result;
        }

        /// <summary>
        ///     Get Step Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return Step object
        /// </returns>
        [Obsolete("Deprecated", true)]
        public Step GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        /// <summary>
        ///     This method accept ApprovalChain id and return list of Steps object by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// List<Step> 
        /// </returns>
        [Obsolete("Deprecated", true)]
        public List<Step> GetByApprovalChainId(long id)
        {
            return GetByApprovalChainIdAsync(id).Result;
        }


        /// <summary>
        ///     This method accept ApprovalChain Step id and return list of User object by provided id
        /// </summary>
        /// <param name="id">Steo id</param>
        /// <returns>
        /// List<Step> 
        /// </returns>
        [Obsolete("Deprecated", true)]
        public List<string> GetUserIdsByStepId(long id)
        {
            return GetUserIdsByStepIdAsync(id).Result;
        }

        /// <summary>
        /// This method accept Step object which we want to save in database
        /// </summary>
        /// <param name="step"></param>
        /// <returns>
        /// Returns Step object 
        /// </returns>
        [Obsolete("Deprecated", true)]
        public Step Save(Step step)
        {
            return SaveAsync(step).Result;
        }

        /// <summary>
        ///      Delete Step Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        [Obsolete("Deprecated", true)]
        public bool Delete(Step obj)
        {
            return DeleteAsync(obj).Result;
        }
    }
}
