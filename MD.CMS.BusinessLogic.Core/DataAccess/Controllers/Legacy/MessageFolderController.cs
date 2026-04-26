using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    /// <summary>
    /// Controlling methods for MessageFolder object
    /// </summary>
    public partial class MessageFolderController : BaseController<MessageFolderController>
    {
        /// <summary>
        /// Gets MessageFolder object by id
        /// </summary>
        /// <param name="id">Id of wanted MessageFolder object</param>
        /// <returns>MessageFolder object with given id</returns>
        [Obsolete("Deprecated", true)]
        public MessageFolder GetById(int id, User loggedUser)
        {
            return GetByIdAsync(id, loggedUser).Result;
        }

        /// <summary>
        /// Gets MessageFolder object by its own id and its creator id
        /// </summary>
        /// <param name="id">Id of wanted MessageFolder object</param>
        /// <param name="authorId">Id of user who created folder (which at the same time should be LoggedOnUser id). If null, it's about system folder.</param>
        /// <returns>MessageFolder object with given id and authorId</returns>
        [Obsolete("Deprecated", true)]
        public MessageFolder GetByIdAndAuthorId(int id, User loggedUser)
        {
            return GetByIdAndAuthorIdAsync(id, loggedUser).Result;
        }

        /// <summary>
        /// Gets all MessageFolder objects from the database
        /// </summary>
        /// <returns>All MessageFolder objects from the database</returns>
        [Obsolete("Deprecated", true)]
        public List<MessageFolder> GetAll(User loggedUser)
        {
            return GetAllAsync(loggedUser).Result;
        }

        /// <summary>
        /// Gets all system message folders
        /// </summary>
        /// <returns>All system message folders</returns>
        [Obsolete("Deprecated", true)]
        public List<MessageFolder> GetAllSystemFolders(User loggedUser)
        {
            return GetAllSystemFoldersAsync(loggedUser).Result;
        }

        /// <summary>
        /// Gets all message folders created by given user (usually LoggedOnUser)
        /// </summary>
        /// <param name="authorId">Message folders creator</param>
        /// <returns>List of MessageFolder objects created by user with authorId</returns>
        [Obsolete("Deprecated", true)]
        public List<MessageFolder> GetByAuthorId(User loggedUser)
        {
            return GetByAuthorIdAsync(loggedUser).Result;
        }

        /// <summary>
        /// Saves or updates MessageFolder. Throws exception if revieved MessageFolder is not editable.
        /// </summary>
        /// <param name="messageFolder">MessageFolder to save or update</param>
        /// <returns>Saved/updated MessageFolder returned from the database after saving/updateing</returns>
        [Obsolete("Deprecated", true)]
        public MessageFolder Save(MessageFolder messageFolder, User loggedUser)
        {
            return SaveAsync(messageFolder, loggedUser).Result;
        }

        /// <summary>
        /// Deletes MessageFolder from the database. Throws exception if recieved MessageFolder is not editable
        /// </summary>
        /// <param name="messageFolder">MessageFolder to delete</param>
        /// <returns>True if deletion is successful, false otherwise</returns>
        [Obsolete("Deprecated", true)]
        public bool Delete(MessageFolder messageFolder)
        {
            return DeleteAsync(messageFolder).Result;
        }

        [Obsolete("Deprecated", true)]
        private bool IsUpdateAllowed(MessageFolder messageFolder)
        {
            return IsUpdateAllowedAsync(messageFolder).Result;
        }
    }
}