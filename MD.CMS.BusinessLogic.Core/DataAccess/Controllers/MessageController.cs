using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    /// <summary>
    /// Controlling methods for Message object
    /// </summary>
    public partial class MessageController : BaseController<MessageController>
    {
        /// <summary>
        /// Creating message object from data row
        /// </summary>
        /// <param name="row">DataRow returned by stored procedure</param>
        /// <returns>Message object created from DataRow</returns>
        public async Task<Message> CreateAsync(DataRow row)
        {
            Message message = base.Create<Message, long>(row, MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Data.Columns.MessageId);
            if (message != null)
            {
                message.Id = row.GetValue<long>("MessageId");
                message.Subject = row.GetValue<string>("Subject");
                message.MessageContent = row.GetValue<string>("MessageContent");
                int isRead = row.GetValue<int>("IsRead");
                if (isRead == 1)
                {
                    message.IsRead = true;
                }
                else
                {
                    message.IsRead = false;
                }
                message.MessageFolderId = row.GetValue<int>("MessageFolderId");
                message.ParentId = row.GetValue<long>("ParentId");
                message.DateAdded = row.GetValue<string>("DateAdded");
                message.Type = (MessageType)row.GetValue<int>("Type");
                message.MainThread = row.GetValue<long>("MainThread");

                await SetMessageUserAsync(message, row);
            }
            return message;
        }

        /// <summary>
        /// Discovering the role of User2Id - is it sender or reciever (determinated with type) 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="row"></param>
        private async Task SetMessageUserAsync(Message message, DataRow row)
        {
            if (message.Type == MessageType.Sent)
            {
                message.FromUserId = row.GetValue<string>("UserId");
                message.FromUser = await UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(message.FromUserId, isFull: false);

                message.ToUserId = row.GetValue<string>("User2Id");
                message.ToUser = await UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(message.ToUserId, isFull: false);
            }
            else if (message.Type == MessageType.Received)
            {
                message.ToUserId = row.GetValue<string>("UserId");
                message.ToUser = await UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(message.ToUserId, isFull: false);

                message.FromUserId = row.GetValue<string>("User2Id");
                message.FromUser = await UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(message.FromUserId, isFull: false);
            }
        }


        public async Task<Message> GetByIdAndUserIdAsync(long id, User user)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetByIdAndUserId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = user.Id });
            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<List<Message>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> allMessages = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetAll.GetIntValue();
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                allMessages.Add(await CreateAsync(row));
            }
            return allMessages;
        }

        public async Task<List<Message>> GetByMessageFolderAsync(MessageFolder messageFolder)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> messagesByFolder = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetByMessageFolder.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageFolderId.GetIntValue()) { Value = messageFolder.Id });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                messagesByFolder.Add(await CreateAsync(row));
            }
            return messagesByFolder;
        }

        public async Task<Entities.Base.BasePaginationEntity<Message>> GetByMessageFolderAndUserAsync(MessageFolder messageFolder, User user, int currentPageIndex = 0, int maxNumberOfRows = 10, string searchTerm = "")
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> messagesByFolderAndUser = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetByMessageFolderAndUser.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageFolderId.GetIntValue()) { Value = messageFolder.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = user.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MainThread_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateAdded_s desc" });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                messagesByFolderAndUser.Add(await CreateAsync(row));
            }
            Entities.Base.BasePaginationEntity<Message> basePaginationEntitiy = new Entities.Base.BasePaginationEntity<Message>();
            basePaginationEntitiy.Items = messagesByFolderAndUser;
            if (results.Rows.Count > 0) {
                basePaginationEntitiy.TotalCount = results.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntitiy;
        }


        public async Task<List<Message>> GetByMessageFolderIdAndUserIdAsync(int messageFolderId, string userId)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> messagesByFolderAndUser = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetByMessageFolderAndUser.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageFolderId.GetIntValue()) { Value = messageFolderId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = userId });


            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                messagesByFolderAndUser.Add(await CreateAsync(row));
            }
            return messagesByFolderAndUser;
        }

        public async Task<int> GetByMessageFolderAndUserCountAsync(MessageFolder messageFolder, User user)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetByMessageFolderAndUserCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageFolderId.GetIntValue()) { Value = messageFolder.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = user.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MainThread_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateAdded_s desc" });
            int totalNumberOfMessagesInFolder = (await ExecuteMethodRowAsync(method, this.UseDefaultPlugin)).GetValue<int>("MessagesCount");
            return totalNumberOfMessagesInFolder;
        }

        public async Task<List<Message>> GetUnreadByUserAsync(User user)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> unreadMessages = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetUnreadByUser.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = user.Id });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                unreadMessages.Add(await CreateAsync(row));
            }
            return unreadMessages;
        }

        public async Task<List<Message>> SearchAsync(User user, string searchTerm, int currentPageIndex = 0, int maxNumberOfRows = 100)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> searchResults = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.Search.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = user.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                searchResults.Add(await CreateAsync(row));
            }
            return searchResults;
        }

        public async Task<int> SearchCountAsync(User user, string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.SearchCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = user.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            int totalNumberOfSearchResults = (await ExecuteMethodRowAsync(method, this.UseDefaultPlugin)).GetValue<int>("MessagesCount");
            return totalNumberOfSearchResults;
        }

        public async Task<List<Message>> GetByParentAsync(Message parent)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> messagesByParent = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetByMessageFolderAndUser.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.ParentId.GetIntValue()) { Value = parent.Id });


            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                messagesByParent.Add(await CreateAsync(row));
            }
            return messagesByParent;
        }

        public async Task<List<Message>> GetByUserIdAsync(User user)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> messagesByUser = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetByUserId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = user.Id });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                messagesByUser.Add(await CreateAsync(row));
            }
            return messagesByUser;
        }

        public async Task<List<Message>> GetByMainThreadAsync(long mainThread)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> messagesByThread = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetByMainThread.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MainThread.GetIntValue()) { Value = mainThread });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                messagesByThread.Add(await CreateAsync(row));
            }
            return messagesByThread;
        }

        public async Task<List<Message>> GetByMainThreadAndUserAsync(long mainThread, string userId)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Message> messages = new List<Message>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.GetByMainThreadAndUser.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MainThread.GetIntValue()) { Value = mainThread });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = userId });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                messages.Add(await CreateAsync(row));
            }
            return messages;
        }

        public async Task<Message> SaveAsync(Message message)
        {
            await AuthenticateAndAuthorizeAsync();
            Message sentMessage = await SendMessageAsync(message);
            message.Id = sentMessage.Id;
            message.MainThread = sentMessage.MainThread;
            Message receivedMessage = await ReceiveMessageAsync(message);
            message.MainThread = receivedMessage.MainThread;
            return message;
        }

        public async Task<Message> SaveAsync(Message message, SystemMessageFolder systemMessageFolder)
        {
            await AuthenticateAndAuthorizeAsync();
            message.Id = 0;
            Message receivedMessage = await ReceiveMessageAsync(message, systemMessageFolder);
            return message;
        }

        private async Task<Message> SendMessageAsync(Message messageToSend)
        {
            await AuthenticateAndAuthorizeAsync();
            if (messageToSend.MessageFolderId == 0 || messageToSend.MessageFolderId == SystemMessageFolder.Inbox.GetIntValue())
            {
                messageToSend.MessageFolderId = SystemMessageFolder.Sent.GetIntValue();
            }

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.Insert.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageId.GetIntValue()) { Value = 0 });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.Subject.GetIntValue()) { Value = messageToSend.Subject });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageContent.GetIntValue()) { Value = messageToSend.MessageContent });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.ParentId.GetIntValue()) { Value = messageToSend.ParentId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.IsRead.GetIntValue()) { Value = 0 });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageFolderId.GetIntValue()) { Value = messageToSend.MessageFolderId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.DateAdded.GetIntValue()) { Value = DateTime.Now });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.Type.GetIntValue()) { Value = MessageType.Sent.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = messageToSend.FromUserId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MainThread.GetIntValue()) { Value = messageToSend.MainThread });
            method.ClearCache = true;

            Message sentMessage = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            return sentMessage;
        }


        private async Task<Message> ReceiveMessageAsync(Message messageToReceive, SystemMessageFolder smf = SystemMessageFolder.Inbox)
        {
            if (smf == SystemMessageFolder.Inbox)
            {
                messageToReceive.MessageFolderId = await GetMessageFolderIdAsync(messageToReceive.MainThread, messageToReceive.ToUserId);
                if (messageToReceive.MessageFolderId == 0 || messageToReceive.MessageFolderId == SystemMessageFolder.Sent.GetIntValue())
                {
                    messageToReceive.MessageFolderId = SystemMessageFolder.Inbox.GetIntValue();
                }
            }
            else
            {
                messageToReceive.MessageFolderId = smf.GetIntValue();
            }

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.Insert.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageId.GetIntValue()) { Value = messageToReceive.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.Subject.GetIntValue()) { Value = messageToReceive.Subject });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageContent.GetIntValue()) { Value = messageToReceive.MessageContent });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.ParentId.GetIntValue()) { Value = messageToReceive.ParentId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.IsRead.GetIntValue()) { Value = 0 });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageFolderId.GetIntValue()) { Value = messageToReceive.MessageFolderId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.DateAdded.GetIntValue()) { Value = DateTime.Now });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.Type.GetIntValue()) { Value = MessageType.Received.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = messageToReceive.ToUserId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MainThread.GetIntValue()) { Value = messageToReceive.MainThread });

            method.ClearCache = true;

            Message sentMessage = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            return sentMessage;
        }


        private async Task<int> GetMessageFolderIdAsync(long mainThread, string userId)
        {
            List<Message> messages = await GetByMainThreadAndUserAsync(mainThread, userId);
            if (messages != null && messages.Any())
            {
                return messages.First().MessageFolderId;
            }
            else
            {
                return SystemMessageFolder.Inbox.GetIntValue();
            }
        }


        public async Task<Message> SetIsReadAsync(Message message, User loggedUser)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.Update.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageId.GetIntValue()) { Value = message.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.IsRead.GetIntValue()) { Value = 1 });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageFolderId.GetIntValue()) { Value = message.MessageFolderId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = loggedUser.Id });
            Message updatedMessage = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            return updatedMessage;
        }


        public async Task<Message> ReplaceAsync(Message messageToReplace, User loggedUser)
        {
            await AuthenticateAndAuthorizeAsync();
            if (await CheckIfReplacementIsAllowedAsync(messageToReplace))
            {
                Method method = new Method();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.Update.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageId.GetIntValue()) { Value = messageToReplace.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.IsRead.GetIntValue()) { Value = messageToReplace.IsRead });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageFolderId.GetIntValue()) { Value = messageToReplace.MessageFolderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = loggedUser.Id });
                method.ClearCache = true;

                Message updatedMessage = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                return updatedMessage;
            }
            else
            {
                if (messageToReplace.Type == MessageType.Sent)
                {
                    throw new UnauthorizedAccessException("Cannot replace sent message to inbox");
                }
                else
                {
                    throw new UnauthorizedAccessException("Cannot replace recieved message to sent");
                }
            }
        }

        private async Task<bool> CheckIfReplacementIsAllowedAsync(Message messageToReplace)
        {
            if (messageToReplace.Type == MessageType.Received && messageToReplace.MessageFolderId == SystemMessageFolder.Sent.GetIntValue())
            {
                return false;
            }
            if (messageToReplace.Type == MessageType.Sent && messageToReplace.MessageFolderId == SystemMessageFolder.Inbox.GetIntValue())
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(Message messageToDelete, User loggedUser)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.Delete.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageId.GetIntValue()) { Value = messageToDelete.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = loggedUser.Id });
            method.ClearCache = true;

            success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
            return success;
        }

        public async Task<bool> DeleteByMessageAndUserIdAsync(Message messageToDelete, string loggedUserId)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Methods.Delete.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.MessageId.GetIntValue()) { Value = messageToDelete.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Message.Parameters.UserId.GetIntValue()) { Value = loggedUserId });
            method.ClearCache = true;

            success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
            return success;
        }
    }
}