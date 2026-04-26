using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain
{
    /// <summary>
    /// Actions representing user reject or approve of a new content added to certain folder.
    /// </summary>
    public class ApprovalChainApproval : BaseEntity<long>
    {
        #region Attributes
        private User _user;
        private Step _step;
        private Content _content;
        private DateTime _reviewDate;
        private String _comment;
        private StepActionType _approvalType;
        #endregion

        #region Properties

        /// <summary>
        /// User that approved or rejected content
        /// </summary>
        public User User 
        {
            get { return _user; }
            set { _user = value; } 
        }

        /// <summary>
        /// On which step did user take action
        /// </summary>
        public Step Step
        {
            get { return _step; }
            set { _step = value;  }
        }

        /// <summary>
        /// Content waiting for approval
        /// </summary>
        public Content Content
        {
            get { return _content;  }
            set { _content = value; }
        }

        /// <summary>
        /// Date on which user rejected or approved content
        /// </summary>
        public DateTime ReviewDate
        {
            get { return _reviewDate; }
            set { _reviewDate = value; }
        }

        /// <summary>
        /// Optional description why the content was approved or rejected
        /// </summary>
        public String Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        /// <summary>
        /// Type describing did user approve or reject content
        /// </summary>
        public StepActionType ApprovalType
        {
            get { return _approvalType; }
            set { _approvalType = value; }
        }

        #endregion
    }
}
