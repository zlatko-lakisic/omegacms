using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Session;
using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using MD.Tools.Helpers.Core.Logging;
using System.Globalization;

namespace MD.CMS.BusinessLogic.WebApi.Core.BaseControllers
{
    public abstract class BaseLoggedOnWebApiController : BaseController
    {
        private class UserModelStore
        {
            public User User { get; set; }
            public DateTime RefreshTime { get; set; }
        }

        private User _loggedOnUser;
        private static ConcurrentDictionary<string, UserModelStore> _currentUsers = new ConcurrentDictionary<string, UserModelStore>();
        private static TimeSpan _checkInterval = TimeSpan.Parse("00:01:00", CultureInfo.InvariantCulture);

        #region Properties
        /// <summary>
        /// Get the logged on user
        /// </summary>
        [Obsolete("Obsolete Property", true)]
        public User LoggedOnUser
        {
            get
            {
                if (_loggedOnUser == null && HttpContext != null && (HttpContext.Request.Headers.ContainsKeyName(Settings.Default.AuthenticateHeaderName) || HttpContext.Request.Query.ContainsKeyName(Settings.Default.AuthenticateHeaderName)))
                {
                    string authenticationHeaderString = HttpContext.Request.Headers.GetValue(Settings.Default.AuthenticateHeaderName);
                    if (string.IsNullOrEmpty(authenticationHeaderString))
                    {
                        authenticationHeaderString = HttpContext.Request.Query.GetValue(Settings.Default.AuthenticateHeaderName);
                    }
                    string rootId = BusinessLogic.Core.Properties.Settings.Default.RootId();
                    _loggedOnUser = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(rootId).GetByIdAsync(SessionTable.GetLoggedOnUserIdAsync(authenticationHeaderString).Result).Result;
                    if (_loggedOnUser != null && !_loggedOnUser.Id.Equals(default))
                    {
                        Task.Run(async () =>
                        {
                            await SessionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(rootId).ExtendSessionAsync(_loggedOnUser.Id, MD.CMS.BusinessLogic.Core.DataAccess.Controllers.SessionController.SessionDomain);
                        }).Wait();
                    }
                }
                return _loggedOnUser;
            }
        }
        /// <summary>
        /// Is this an administration api call?
        /// </summary>
        public bool IsAdministration
        {
            get
            {
                if (HttpContext != null && (HttpContext.Request.Headers.ContainsKeyName(Settings.Default.IsAdministrationHeaderName) || HttpContext.Request.Query.ContainsKeyName(Settings.Default.IsAdministrationHeaderName)))
                {
                    string isAdministrationValue = HttpContext.Request.Headers.GetValue(Settings.Default.IsAdministrationHeaderName);
                    if (string.IsNullOrEmpty(isAdministrationValue))
                    {
                        isAdministrationValue = HttpContext.Request.Query.GetValue(Settings.Default.IsAdministrationHeaderName);
                    }


                    if (string.IsNullOrEmpty(isAdministrationValue))
                    {
                        return isAdministrationValue.ToBoolean(false);
                    }
                }
                return false;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the logged on user
        /// </summary>
        protected async Task<User> GetLoggedOnUser()
        {
            string authenticationHeaderString = HttpContext.Request.Headers.GetValue(Settings.Default.AuthenticateHeaderName);
            UserModelStore loggedOnUser = null;
            if (string.IsNullOrEmpty(authenticationHeaderString))
            {
                authenticationHeaderString = HttpContext.Request.Query.GetValue(Settings.Default.AuthenticateHeaderName);
            }

            if (!string.IsNullOrEmpty(authenticationHeaderString))
            {
                string rootId = BusinessLogic.Core.Properties.Settings.Default.RootId();
                if (_currentUsers.ContainsKey(authenticationHeaderString))
                {
                    loggedOnUser = _currentUsers[authenticationHeaderString];
                    if (loggedOnUser == null || loggedOnUser.User == null)
                    {
                        try
                        {
                            _currentUsers.TryRemove(authenticationHeaderString, out loggedOnUser);
                        }
                        catch (Exception error)
                        {
                            typeof(BaseLoggedOnWebApiController).Log(error);
                        }
                    }
                    else
                    {
                        if(loggedOnUser.RefreshTime.Subtract(DateTime.Now).TotalSeconds < 0)
                        {
                            BusinessLogic.Core.DataAccess.Entities.Session session = await SessionTable.GetLoggedOnSessionAsync(authenticationHeaderString);
                            if (session != null)
                            {
                                await SessionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(rootId).ExtendSessionAsync(loggedOnUser.User.Id, MD.CMS.BusinessLogic.Core.DataAccess.Controllers.SessionController.SessionDomain);
                                loggedOnUser.RefreshTime = loggedOnUser.RefreshTime.Add(_checkInterval);
                            } 
                            else
                            {
                                try
                                {
                                    _currentUsers.TryRemove(authenticationHeaderString, out loggedOnUser);
                                    loggedOnUser = null;
                                }
                                catch (Exception error)
                                {
                                    typeof(BaseLoggedOnWebApiController).Log(error);
                                }
                            }
                        }
                    }
                }
                else
                {
                    BusinessLogic.Core.DataAccess.Entities.Session session = await SessionTable.GetLoggedOnSessionAsync(authenticationHeaderString);
                    if (session != null)
                    {
                        loggedOnUser = new UserModelStore()
                        {
                            User = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(rootId).GetByIdAsync(session.UserId),
                            RefreshTime = DateTime.Now.Add(_checkInterval)
                        };
                        if (loggedOnUser != null)
                        {
                            try
                            {
                                _currentUsers.TryAdd(authenticationHeaderString, loggedOnUser);
                            }
                            catch (Exception error)
                            {
                                typeof(BaseLoggedOnWebApiController).Log(error);
                            }
                        }
                    }
                }

                if (loggedOnUser != null && loggedOnUser.User != null)
                {
                    return loggedOnUser.User;
                }
            }

            return null;
        }
        #endregion
    }
}
