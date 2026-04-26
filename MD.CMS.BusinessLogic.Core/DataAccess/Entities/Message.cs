using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class Message : BaseEntity<long>
    {
        #region Attributes
        private string _subject;
        private string _messageContent;
        private long _parentId;
        private bool _isRead;
        private int _messageFolderId;
        private DateTime _dateAdded;
        private MessageType _type;
        private string _fromUserId;
        private User _fromUser;
        private string _toUserId;
        private User _toUser;
        private long _mainThread;     
        #endregion

        #region Properties
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string MessageContent
        {
            get { return _messageContent; }
            set { _messageContent = value; }
        }

        public long ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        public bool IsRead
        {
            get { return _isRead; }
            set { _isRead = value; }
        }

        public int MessageFolderId
        {
            get { return _messageFolderId; }
            set { _messageFolderId = value; }
        }

        public string DateAdded
        {
            get
            {
                return _dateAdded.ToString("yyyy-MM-dd HH:mm:ss ", CultureInfo.InvariantCulture);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _dateAdded = DateTime.Parse(value, CultureInfo.InvariantCulture);
                }
                else
                {
                    _dateAdded = DateTime.UtcNow;
                }

            }
        }

        public MessageType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string FromUserId
        {
            get { return _fromUserId; }
            set { _fromUserId = value; }
        }

        public string ToUserId
        {
            get { return _toUserId; }
            set { _toUserId = value; }
        }

        public User FromUser
        {
            get { return _fromUser; }
            set { _fromUser = value; }
        }

        public User ToUser
        {
            get { return _toUser; }
            set { _toUser = value; }
        }

        public long MainThread
        {
            get { return _mainThread; }
            set { _mainThread = value; }
        }
     
        #endregion
    }
}
