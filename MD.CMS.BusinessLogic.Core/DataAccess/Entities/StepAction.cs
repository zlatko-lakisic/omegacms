using MD.Tools.BaseDataAccess.Core.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain
{
    /// <summary>
    /// Template for actions representing user response for adding new content into folder
    /// where they are required to review that content. It holds informations about what to do
    /// when user reject or approve content on that step.
    /// </summary>
    public class StepAction : BaseEntity<long>
    {
        #region Attributes
        private long _stepId;
        private StepActionType _type;
        private StepActionAction _action;
        private long _redirectTo;       
        #endregion

        #region Properties

        /// <summary>
        /// Id of the step to which this action is linked to
        /// </summary>
        public long StepId
        {
            get { return _stepId; }
            set { _stepId = value; }
        }

        /// <summary>
        /// Rejected or approved
        /// </summary>
        public StepActionType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Can be redirect, publish or end respectively
        /// </summary>
        public StepActionAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        /// <summary>
        /// If action is 'redirect', this represents the id of the step we want to go next to.
        /// </summary>
        public long RedirectTo
        {
            get { return _redirectTo; }
            set { _redirectTo = value; }
        }
        #endregion
    }
     
}
