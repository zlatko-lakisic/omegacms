using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    /// <summary>
    /// Messages container
    /// Can be common for all users (previously added to database, without options to update or delete - with names Inbox, Sent and Trash and ids 1, 2 and 3
    /// Or it can be created by user and displayed just for that user
    /// If there is no authorId, it means that it is global (_isGlobal = true) and not editable
    /// </summary>
    public class MessageFolder : BaseEntity<int>
    {
        #region Attributes
        private string _name;
        private string _icon;
        private User _author;
        private long? _authorId;
        private bool _isGlobal;
        private List<Message> _messages;
        private int _messagesCount;
        #endregion

        #region Properties
        /// <summary>
        /// Message folders name
        /// Inbox, Sent and Trash for global non-editable folders
        /// or anything else for individual ones
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Represents material icon symbol
        /// </summary>
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        /// <summary>
        /// Potential message folders creator
        /// Null if it's about predefined message folders with ids 1, 2, or 3
        /// </summary>
        public User Author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// Potential message folders creators id
        /// Null if it's about predefined message folders with ids 1, 2, or 3
        /// </summary>
        public long? AuthorId
        {
            get { return _authorId; }
            set { _authorId = value; }
        }

        /// <summary>
        /// True for common, predefined, non-editable and non-deletable message folders (Inbox, Sent and Trash specifically)
        /// </summary>
        public bool IsGlobal
        {
            get { return _isGlobal; }
            set { _isGlobal = value; }
        }

        /// <summary>
        /// List of messages stored in given message folder for logged user
        /// </summary>
        public List<Message> Messages
        {
            get
            {
                if (_messages == null)
                {
                    _messages = new List<Message>();
                }
                return _messages;
            }
            set
            {
                _messages = value;
            }
        }

        public int MessagesCount
        {
            get { return _messagesCount; }
            set { _messagesCount = value; }
        }

        public bool IsNew
        {
            get
            {
                return Id.Equals(default(int));
            }
        }
        #endregion
    }
}
