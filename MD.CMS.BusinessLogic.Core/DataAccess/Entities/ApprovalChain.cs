using MD.Tools.BaseDataAccess.Core.Entities;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain
{
    /// <summary>
    /// Approval chain gives administrators control over which user with write permission can write content into certain folder. 
    /// Administrators can choose how many users are required to give the approval before content can be written and published
    /// </summary>
    public class ApprovalChain : BaseEntity<long>
    {
        #region Attributes
        private long _folderId;
        private bool _isActive;
        private List<Step> _steps;
        #endregion

        #region Properties

        /// <summary>
        /// Each folder have it’s own approval chain
        /// </summary>
        public long FolderId
        {
            get { return _folderId; }
            set { _folderId = value; }
        }

        /// <summary>
        /// Approval Chain can be active or not
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        /// <summary>
        /// Chain consists of approval steps which enables flow control in giving approval for writing content to folder.
        /// </summary>
        public List<Step> Steps
        {
            get
            {
                if (_steps == null)
                {
                    _steps = new List<Step>();
                }
                return _steps;
            }
            set { _steps = value; }
        }
        #endregion
    }
}
