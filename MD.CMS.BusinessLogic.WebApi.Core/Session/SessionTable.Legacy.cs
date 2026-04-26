using System;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Session
{
    public partial class SessionTable
    {
        #region Methods
        [Obsolete]
        public static MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session AddUser(string username, string userId, string token, string providerName)
        {
            return AddUserAsync(username, userId, token, providerName).Result;
        }

        [Obsolete]
        public static bool UserAuthenticated(string authdata)
        {
            return UserAuthenticatedAsync(authdata).Result;
        }

        [Obsolete]
        public static MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session GetLoggedOnSession(string authdata)
        {
            return GetLoggedOnSessionAsync(authdata).Result;
        }

        [Obsolete]
        public static string GetLoggedOnUserId(string authdata)
        {
            return GetLoggedOnUserIdAsync(authdata).Result;
        }

        [Obsolete]
        public static void RemoveUserByAuthData(string authdata)
        {
            Task.Run(async () => {
                await RemoveUserByAuthDataAsync(authdata); }).Wait();
        }

        [Obsolete]
        public static void RemoveUserByUserId(string userId)
        {
                Task.Run(async () => {
                    await RemoveUserByUserIdAsync(userId); }).Wait();
        }
        #endregion
    }
}