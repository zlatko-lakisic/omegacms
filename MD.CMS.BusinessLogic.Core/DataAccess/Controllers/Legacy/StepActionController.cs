using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain;
using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain
{
    public partial class StepActionController : BaseController<StepActionController>
    {
        /// <summary>
        ///     This method return us all StepAction data from database
        /// </summary>
        /// <returns>
        /// List of StepAction objects
        /// </returns>
        [Obsolete("Deprecated", true)]
        public List<StepAction> GetAll()
        {
            return GetAllAsync().Result;
        }

        /// <summary>
        ///     Get StepAction Data  by the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Return StepAction object
        /// </returns>
        [Obsolete("Deprecated", true)]
        public StepAction GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        /// <summary>
        ///     This method accept StepId id and return list of StepAction object by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// List<StepAction> 
        /// </returns>
        [Obsolete("Deprecated", true)]
        public List<StepAction> GetByStepId(long id)
        {
            return GetByStepIdAsync(id).Result;
        }

        /// <summary>
        /// This method accept StepAction object which we want to save in database
        /// </summary>
        /// <param name="stepAction"></param>
        /// <returns>
        /// Returns StepAction object 
        /// </returns>
        [Obsolete("Deprecated", true)]
        public StepAction Save(StepAction stepAction)
        {
            return SaveAsync(stepAction).Result;
        }

        /// <summary>
        ///      Delete StepAction Data  by the provided id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean value, true if delete is successful, otherwise false</returns>
        [Obsolete("Deprecated", true)]
        public bool Delete(StepAction obj)
        {
            return DeleteAsync(obj).Result;
        }

    }
}
