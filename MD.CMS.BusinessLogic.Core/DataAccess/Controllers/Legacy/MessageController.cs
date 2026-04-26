using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    /// <summary>
    /// Controlling methods for Message object
    /// </summary>
    public partial class MessageController : BaseController<MessageController>
    {
        [Obsolete("Deprecated", true)]
        public Message GetByIdAndUserId(long id, User user)
        {
            return GetByIdAndUserIdAsync(id, user).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Message> GetAll()
        {
            return GetAllAsync().Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Message> GetByMessageFolder(MessageFolder messageFolder)
        {
            return GetByMessageFolderAsync(messageFolder).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<Message> GetByMessageFolderAndUser(MessageFolder messageFolder, User user, int currentPageIndex = 0, int maxNumberOfRows = 10, string searchTerm = "")
        {
            return GetByMessageFolderAndUserAsync(messageFolder, user, currentPageIndex, maxNumberOfRows, searchTerm).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Message> GetByMessageFolderIdAndUserId(int messageFolderId, string userId)
        {
            return GetByMessageFolderIdAndUserIdAsync(messageFolderId, userId).Result;
        }

        [Obsolete("Deprecated", true)]
        public int GetByMessageFolderAndUserCount(MessageFolder messageFolder, User user)
        {
            return GetByMessageFolderAndUserCountAsync(messageFolder, user).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Message> GetUnreadByUser(User user)
        {
            return GetUnreadByUserAsync(user).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Message> Search(User user, string searchTerm, int currentPageIndex = 0, int maxNumberOfRows = 100)
        {
            return SearchAsync(user, searchTerm, currentPageIndex, maxNumberOfRows).Result;
        }

        [Obsolete("Deprecated", true)]
        public int SearchCount(User user, string searchTerm)
        {
            return SearchCountAsync(user, searchTerm).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Message> GetByParent(Message parent)
        {
            return GetByParentAsync(parent).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Message> GetByUserId(User user)
        {
            return GetByUserIdAsync(user).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Message> GetByMainThread(long mainThread)
        {
            return GetByMainThreadAsync(mainThread).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<Message> GetByMainThreadAndUser(long mainThread, string userId)
        {
            return GetByMainThreadAndUserAsync(mainThread, userId).Result;
        }

        [Obsolete("Deprecated", true)]
        public Message Save(Message message)
        {
            return SaveAsync(message).Result;
        }

        [Obsolete("Deprecated", true)]
        public Message Save(Message message, SystemMessageFolder systemMessageFolder)
        {
            return SaveAsync(message, systemMessageFolder).Result;
        }

        [Obsolete("Deprecated", true)]
        private Message SendMessage(Message messageToSend)
        {
            return SendMessageAsync(messageToSend).Result;
        }

        [Obsolete("Deprecated", true)]
        private Message ReceiveMessage(Message messageToReceive, SystemMessageFolder smf = SystemMessageFolder.Inbox)
        {
            return ReceiveMessageAsync(messageToReceive, smf).Result;
        }

        [Obsolete("Deprecated", true)]
        private int GetMessageFolderId(long mainThread, string userId)
        {
            return GetMessageFolderIdAsync(mainThread, userId).Result;
        }

        [Obsolete("Deprecated", true)]
        public Message SetIsRead(Message message, User loggedUser)
        {
            return SetIsReadAsync(message, loggedUser).Result;
        }

        [Obsolete("Deprecated", true)]
        public Message Replace(Message messageToReplace, User loggedUser)
        {
            return ReplaceAsync(messageToReplace, loggedUser).Result;
        }

        [Obsolete("Deprecated", true)]
        private bool CheckIfReplacementIsAllowed(Message messageToReplace)
        {
            return CheckIfReplacementIsAllowedAsync(messageToReplace).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(Message messageToDelete, User loggedUser)
        {
            return DeleteAsync(messageToDelete, loggedUser).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteByMessageAndUserId(Message messageToDelete, string loggedUserId)
        {
            return DeleteByMessageAndUserIdAsync(messageToDelete, loggedUserId).Result;
        }
    }
}