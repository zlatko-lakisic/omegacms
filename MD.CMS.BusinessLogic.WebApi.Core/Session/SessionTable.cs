using MD.CMS.BusinessLogic.Core.DataAccess.Controllers;
using System;
using System.Text;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.Properties;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Session
{
    public partial class SessionTable
    {
        #region Attributes
        private static string _rootSessionId;
        private static string _rootAuthData;
        #endregion

        #region Methods
        private static string GetSessionId(string username, string userId, string token, string providerName)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or empty.", nameof(username));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException($"'{nameof(userId)}' cannot be null or empty.", nameof(userId));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException($"'{nameof(token)}' cannot be null or empty.", nameof(token));
            }

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ArgumentException($"'{nameof(providerName)}' cannot be null or empty.", nameof(providerName));
            }

            return string.Concat(MD.Tools.Helpers.Core.Crypto.AESCrypt.Encrypt(
                    string.Format("{0}:{1}:{2}:{3}",
                    username,
                    userId,
                    SessionController.SessionDomain,
                    providerName
                ), token).Take(200));
        }

        private static string GetAuthData(string username, string sessionId)
        {
            return string.Concat(Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, sessionId))).Take(250));
        }

        public static async Task<MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session> AddUserAsync(string username, string userId, string token, string providerName)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or empty.", nameof(username));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException($"'{nameof(userId)}' cannot be null or empty.", nameof(userId));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException($"'{nameof(token)}' cannot be null or empty.", nameof(token));
            }

            if (string.IsNullOrEmpty(providerName))
            {
                throw new ArgumentException($"'{nameof(providerName)}' cannot be null or empty.", nameof(providerName));
            }

            string sessionId = GetSessionId(username, userId, token, providerName);
            string authdata = GetAuthData(username, sessionId);

            MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session obj = null;
            if (!string.IsNullOrEmpty(Settings.Default.RootId()) && userId.Equals(Settings.Default.RootId()))
            {
                obj = new BusinessLogic.Core.DataAccess.Entities.Session();
                obj.Authdata = authdata;
                obj.DateAdded = DateTime.Now;
                obj.SessionDomain = SessionController.SessionDomain;
                obj.SessionId = sessionId;
                obj.UserId = userId;
                obj.Username = username;

                _rootSessionId = sessionId;
                _rootAuthData = authdata;
            }
            else
            {
                obj = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.SessionController.GetNewInstance().DefaultPlugin(true).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).AddUserAsync(userId, username, authdata, sessionId);
            }
            
            return obj;
        }

        public static async Task<bool> UserAuthenticatedAsync(string authdata)
        {
            if (string.IsNullOrEmpty(authdata))
            {
                throw new ArgumentException($"'{nameof(authdata)}' cannot be null or empty.", nameof(authdata));
            }

            if (!string.IsNullOrEmpty(authdata) && !string.IsNullOrEmpty(_rootAuthData) && string.CompareOrdinal(authdata, _rootAuthData).Equals(0))
            {
                return true;
            }

            return await SessionController.GetNewInstance().DefaultPlugin(true).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).UserAuthenticatedAsync(authdata);
        }

        public static async Task<MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session> GetLoggedOnSessionAsync(string authdata)
        {
            if (string.IsNullOrEmpty(authdata))
            {
                throw new ArgumentException($"'{nameof(authdata)}' cannot be null or empty.", nameof(authdata));
            }

            MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session obj = null;

            if (!string.IsNullOrEmpty(_rootAuthData) && string.Compare(_rootAuthData, authdata).Equals(0) && !string.IsNullOrEmpty(Settings.Default.RootId()))
            {
                MD.CMS.BusinessLogic.Core.DataAccess.Entities.User user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().GetByIdAsync(Settings.Default.RootId());

                obj = new BusinessLogic.Core.DataAccess.Entities.Session();
                obj.Authdata = _rootAuthData;
                obj.DateAdded = DateTime.Now;
                obj.SessionDomain = SessionController.SessionDomain;
                obj.SessionId = _rootSessionId;
                obj.UserId = user.Id;
                obj.Username = user.Username;
            }
            else
            {
                obj = await SessionController.GetNewInstance().DefaultPlugin(true).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetLoggedOnUserAsync(authdata);
            }

            return obj;
        }

        public static async Task<string> GetLoggedOnUserIdAsync(string authdata)
        {
            if (string.IsNullOrEmpty(authdata))
            {
                throw new ArgumentException($"'{nameof(authdata)}' cannot be null or empty.", nameof(authdata));
            }

            if (!string.IsNullOrEmpty(Settings.Default.RootId()) && !string.IsNullOrEmpty(_rootAuthData) && string.Compare(_rootAuthData, authdata).Equals(0))
            {
                return Settings.Default.RootId();
            }

            MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session session = await GetLoggedOnSessionAsync(authdata);

            if(session != null)
            {
                return session.UserId;
            }

            return null;
        }

        public static async Task RemoveUserByAuthDataAsync(string authdata)
        {
            if (string.IsNullOrEmpty(authdata))
            {
                throw new ArgumentException($"'{nameof(authdata)}' cannot be null or empty.", nameof(authdata));
            }

            if (!string.IsNullOrEmpty(_rootAuthData) && string.Compare(_rootAuthData, authdata).Equals(0))
            {
                _rootAuthData = null;
                _rootSessionId = null;
            }
            else
            {
                await SessionController.GetNewInstance().DefaultPlugin(true).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).RemoveUserByAuthDataAsync(authdata);
            }
        }

        public static async Task RemoveUserByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException($"'{nameof(userId)}' cannot be null or empty.", nameof(userId));
            }

            if (!string.IsNullOrEmpty(Settings.Default.RootId()) && Settings.Default.RootId().Equals(userId))
            {
                _rootAuthData = null;
                _rootSessionId = null;
            }
            else
            {
                await SessionController.GetNewInstance().DefaultPlugin(true).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).RemoveUserByUserIdAsync(userId);
            }
        }
        #endregion
    }
}