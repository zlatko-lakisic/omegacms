using MD.Tools.BaseDataAccess.Core.Entities;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain
{
    /// <summary>
    /// Represents one step in approval chain
    /// </summary>
    public class Step : BaseEntity<long>
    {
        #region Attributes
        private int _order;
        private StepComboOperator _comboOperator;
        private List<string> _userIds;
        private List<StepAction> _actions;
        private long _approvalChainId;
        #endregion

        #region Properties

        /// <summary>
        /// Order number of the step
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        /// <summary>
        /// Can be 'and' (all users must allow it) and 'or' (only one user can allow it)
        /// </summary>
        public StepComboOperator ComboOperator
        {
            get { return _comboOperator; }
            set { _comboOperator = value; }
        }

        /// <summary>
        /// Can contain one or more folder administrators. 
        /// </summary>
        public List<string> UserIds
        {
            get
            {
                if (_userIds == null)
                {
                    _userIds = new List<string>();
                }
                return _userIds;
            }
            set
            {
                _userIds = value;
            }
        }

        /// <summary>
        /// Steps have actions which connect administrator and steps and keep track of administrators decision on that step
        /// </summary>
        public List<StepAction> Actions
        {
            get
            {
                if (_actions == null)
                {
                    _actions = new List<StepAction>();
                }
                return _actions;
            }
            set
            {
                _actions = value;
            }
        }

        /// <summary>
        /// Id of approval chain to which is this step connected. 
        /// </summary>
        public long ApprovalChainId
        {
            get { return _approvalChainId; }
            set { _approvalChainId = value; }
        }

        #endregion
    }
}
