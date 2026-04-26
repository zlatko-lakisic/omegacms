using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    /// <summary>
    /// Controlling methods for MessageFolder object
    /// </summary>
    public partial class MessageFolderController : BaseController<MessageFolderController>
    {
        /// <summary>
        /// Creating MessageFolder object from DataRow
        /// </summary>
        /// <param name="row">DataRow returned by stored procedure</param>
        /// <returns>MessageFolder object created from DataRow</returns>
        public async Task<MessageFolder> CreateAsync(DataRow row, User loggedUser)
        {
            MessageFolder messageFolder = base.Create<MessageFolder, int>(row, MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Data.Columns.MessageFolderId);
            if (messageFolder != null)
            {
                messageFolder.Id = row.GetValue<int>("MessageFolderId");
                messageFolder.Name = row.GetValue<string>("Name");
                messageFolder.Icon = row.GetValue<string>("Icon");
                messageFolder.AuthorId = row.GetValue<long>("AuthorId");
                messageFolder.Author = null;
                if (messageFolder.AuthorId != null)
                {
                    messageFolder.Author = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(row.GetValue<string>("AuthorId"));
                }
                int isGlobal = row.GetValue<int>("IsGlobal");
                if (isGlobal == 0){
                    messageFolder.IsGlobal = false;
                }
                else
                {
                    messageFolder.IsGlobal = true;
                }
                
                if (loggedUser != null)
                {
                    messageFolder.MessagesCount = await MessageController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByMessageFolderAndUserCountAsync(messageFolder, loggedUser);
                }               
            }
            return messageFolder;
        }

        /// <summary>
        /// Gets MessageFolder object by id
        /// </summary>
        /// <param name="id">Id of wanted MessageFolder object</param>
        /// <returns>MessageFolder object with given id</returns>
        public async Task<MessageFolder> GetByIdAsync(int id, User loggedUser)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MessageFolder;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.MessageFolderId.GetIntValue()) { Value = id });
            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), loggedUser);
        }

        /// <summary>
        /// Gets MessageFolder object by its own id and its creator id
        /// </summary>
        /// <param name="id">Id of wanted MessageFolder object</param>
        /// <param name="authorId">Id of user who created folder (which at the same time should be LoggedOnUser id). If null, it's about system folder.</param>
        /// <returns>MessageFolder object with given id and authorId</returns>
        public async Task<MessageFolder> GetByIdAndAuthorIdAsync(int id, User loggedUser)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MessageFolder;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Methods.GetByIdAndAuthorId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.MessageFolderId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.AuthorId.GetIntValue()) { Value = loggedUser.Id });
            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), loggedUser);
        }

        /// <summary>
        /// Gets all MessageFolder objects from the database
        /// </summary>
        /// <returns>All MessageFolder objects from the database</returns>
        public async Task<List<MessageFolder>> GetAllAsync(User loggedUser)
        {
            await AuthenticateAndAuthorizeAsync();
            List<MessageFolder> allMessageFolders = new List<MessageFolder>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MessageFolder;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Methods.GetAll.GetIntValue();
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                allMessageFolders.Add(await CreateAsync(row, loggedUser));
            }
            return allMessageFolders;
        }

        /// <summary>
        /// Gets all system message folders
        /// </summary>
        /// <returns>All system message folders</returns>
        public async Task<List<MessageFolder>> GetAllSystemFoldersAsync(User loggedUser)
        {
            await AuthenticateAndAuthorizeAsync();
            List<MessageFolder> allMessageFolders = new List<MessageFolder>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MessageFolder;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Methods.GetAllSystemFolders.GetIntValue();
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                allMessageFolders.Add(await CreateAsync(row, loggedUser));
            }
            return allMessageFolders;
        }

        /// <summary>
        /// Gets all message folders created by given user (usually LoggedOnUser)
        /// </summary>
        /// <param name="authorId">Message folders creator</param>
        /// <returns>List of MessageFolder objects created by user with authorId</returns>
        public async Task<List<MessageFolder>> GetByAuthorIdAsync(User loggedUser)
        {
            await AuthenticateAndAuthorizeAsync();
            List<MessageFolder> allMessageFolders = new List<MessageFolder>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MessageFolder;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Methods.GetByAuthorId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.AuthorId.GetIntValue()) { Value = loggedUser.Id });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                allMessageFolders.Add(await CreateAsync(row, loggedUser));
            }
            return allMessageFolders;
        }

        /// <summary>
        /// Saves or updates MessageFolder. Throws exception if revieved MessageFolder is not editable.
        /// </summary>
        /// <param name="messageFolder">MessageFolder to save or update</param>
        /// <returns>Saved/updated MessageFolder returned from the database after saving/updateing</returns>
        public async Task<MessageFolder> SaveAsync(MessageFolder messageFolder, User loggedUser)
        {
            await AuthenticateAndAuthorizeAsync();
            MessageFolder savedMessageFolder = new MessageFolder();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MessageFolder;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.Name.GetIntValue()) { Value = messageFolder.Name });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.Icon.GetIntValue()) { Value = messageFolder.Icon });

            if (messageFolder.IsNew)
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.AuthorId.GetIntValue()) { Value = messageFolder.AuthorId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.IsGlobal.GetIntValue()) { Value = messageFolder.IsGlobal });
            }
            else
            {
                if (!await IsUpdateAllowedAsync(messageFolder))
                {
                    throw new UnauthorizedAccessException("Cannot update system message folder");
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.MessageFolderId.GetIntValue()) { Value = messageFolder.Id });
                }
            }
            method.ClearCache = true;

            savedMessageFolder = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), loggedUser);
            return savedMessageFolder;
        }

        /// <summary>
        /// Deletes MessageFolder from the database. Throws exception if recieved MessageFolder is not editable
        /// </summary>
        /// <param name="messageFolder">MessageFolder to delete</param>
        /// <returns>True if deletion is successful, false otherwise</returns>
        public async Task<bool> DeleteAsync(MessageFolder messageFolder)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            if (!await IsUpdateAllowedAsync(messageFolder))
            {

                throw new UnauthorizedAccessException("Cannot delete system message folder");
            }
            else
            {
                Method method = new Method();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MessageFolder;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MessageFolder.Parameters.MessageFolderId.GetIntValue()) { Value = messageFolder.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
            }
            return success;
        }

        private async Task<bool> IsUpdateAllowedAsync(MessageFolder messageFolder)
        {
            if (messageFolder.Id <= Settings.Default.NumberOfSystemMessageFolders)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}