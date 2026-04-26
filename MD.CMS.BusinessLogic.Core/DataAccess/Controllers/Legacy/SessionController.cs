using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class SessionController : BaseController<SessionController>
    {
        #region Methods
        [Obsolete("Deprecated", true)]
        private void ClearOldLogins()
        {
            Task.Run(async () => {
                await ClearOldLoginsAsync(); }).Wait();
        }

        /// <summary>
        /// Add a new session for a user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="username">Username</param>
        /// <param name="authdata">Authentication data token</param>
        /// <param name="sessionId">Session id</param>
        /// <returns></returns>
        [Obsolete("Deprecated", true)]
        public Session AddUser(string userId, string username, string authdata, string sessionId)
        {
            return AddUserAsync(userId, username, authdata, sessionId).Result;
        }


        [Obsolete("Deprecated", true)]
        public Session ExtendSession(string userId, string authdata)
        {
            return ExtendSessionAsync(userId, authdata).Result;
        }

        /// <summary>
        /// Dewtermine wether a user is authenticated by the provided authentication data
        /// </summary>
        /// <param name="authdata">Authentication data token</param>
        /// <returns>Boolean value, true if user is authenticated otherwise false</returns>
        [Obsolete("Deprecated", true)]
        public bool UserAuthenticated(string authdata)
        {
            return GetLoggedOnUser(authdata) != null;
        }

        /// <summary>
        /// Get a logged on user by the provided authentication data
        /// </summary>
        /// <param name="authdata">Authentication data token</param>
        /// <returns>Authenticated session object</returns>
        [Obsolete("Deprecated", true)]
        public Session GetLoggedOnUser(string authdata)
        {
            return GetLoggedOnUserAsync(authdata).Result;
        }

        /// <summary>
        /// Delete a session by the provided authentication data
        /// </summary>
        /// <param name="authdata">Authentication data token</param>
        [Obsolete("Deprecated", true)]
        public void RemoveUserByAuthData(string authdata)
        {
            Task.Run(async () =>
            {
                await RemoveUserByAuthDataAsync(authdata);
            }).Wait();
        }

        /// <summary>
        /// Delete a session by the user id
        /// </summary>
        /// <param name="userId">User Id</param>
        [Obsolete("Deprecated", true)]
        public void RemoveUserByUserId(string userId)
        {
                    Task.Run(async () => {
                        await RemoveUserByUserIdAsync(userId); }).Wait();
        }
        #endregion
    }
}
