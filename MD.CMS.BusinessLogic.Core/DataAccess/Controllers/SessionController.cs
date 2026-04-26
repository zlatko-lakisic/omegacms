using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class SessionController : BaseController<SessionController>
    {
        #region Attributes
        private static DataTable _loggedInUsersTable;
        private static string _sessionDomain;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(0, 1);
        #endregion

        #region Properties
        public static string SessionDomain
        {
            get 
            {
                if (!string.IsNullOrEmpty(_sessionDomain))
                {
                    return _sessionDomain;
                }
                return Settings.Default.SessionDomain; 
            }
            set { _sessionDomain = value; }
        }
        #endregion

        #region Methods
        private Session Create(DataRow row)
        {
            Session obj = null;
            if (row != null)
            {
                obj = new Session();
                obj.UserId = row.GetValue<string>("UserId");
                obj.Username = row.GetValue<string>("Username");
                obj.Authdata = row.GetValue<string>("Authdata");
                obj.SessionId = row.GetValue<string>("SessionId");
                obj.DateAdded = row.GetValue<DateTime>("DateAdded", DateTime.Now);
                obj.SessionDomain = row.GetValue<string>("SessionDomain");
            }
            return obj;
        }

        /// <summary>
        /// Initialize the in process session table
        /// </summary>
        private static void InitializeTable()
        {
            if (_loggedInUsersTable == null)
            {
                _loggedInUsersTable = new DataTable();
                _loggedInUsersTable.Columns.Clear();
                _loggedInUsersTable.Columns.Add("UserId");
                _loggedInUsersTable.Columns.Add("Username");
                _loggedInUsersTable.Columns.Add("Authdata");
                _loggedInUsersTable.Columns.Add("SessionId");
                _loggedInUsersTable.Columns.Add("SessionDomain");
                _loggedInUsersTable.Columns.Add("DateAdded");
            }
        }

        private async Task ClearOldLoginsAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            if (Settings.Default.InProcSessions)
            {
                _loggedInUsersTable = _loggedInUsersTable.AsEnumerable().Where(row =>
                {
                    TimeSpan sessionTime = DateTime.Now - row.GetValue("DateAdded", DateTime.MinValue);
                    return Settings.Default.SessionTimeout <= sessionTime;
                }).CopyToDataTable();
            }
            else
            {
                Method method = new Method();
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Session;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Methods.ClearOldSessions.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.SessionTimeout.GetIntValue()) { Value = Settings.Default.SessionTimeout.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture) });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.SessionDomain.GetIntValue()) { Value = SessionDomain });
                method.ClearCache = true;

                //ExecuteMethodBoolean(method, this.UseDefaultPlugin);
            }
        }

        /// <summary>
        /// Add a new session for a user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="username">Username</param>
        /// <param name="authdata">Authentication data token</param>
        /// <param name="sessionId">Session id</param>
        /// <returns></returns>
        public async Task<Session> AddUserAsync(string userId, string username, string authdata, string sessionId)
        {
            await AuthenticateAndAuthorizeAsync();
            DataRow result = null;

            if (Settings.Default.InProcSessions)
            {
                InitializeTable();
                await _semaphore.WaitAsync();
                try
                {
                    await ClearOldLoginsAsync();
                    if (!SessionController._loggedInUsersTable.AsEnumerable().Any(row => string.Equals(row.GetValue<string>("Authdata", string.Empty), authdata, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        DataRow rowToAdd = SessionController._loggedInUsersTable.NewRow();
                        rowToAdd["UserId"] = userId;
                        rowToAdd["Username"] = username;
                        rowToAdd["Authdata"] = authdata;
                        rowToAdd["SessionId"] = sessionId;
                        rowToAdd["DateAdded"] = DateTime.Now;
                        rowToAdd["SessionDomain"] = SessionDomain;
                        SessionController._loggedInUsersTable.Rows.Add(rowToAdd);
                        result = rowToAdd;
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            else
            {
                await ClearOldLoginsAsync();
                Method method = new Method();
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Session;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Methods.AddUser.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.UserId.GetIntValue()) { Value = userId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.Username.GetIntValue()) { Value = username });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.Authdata.GetIntValue()) { Value = authdata });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.SessionDomain.GetIntValue()) { Value = SessionDomain });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.SessionId.GetIntValue()) { Value = sessionId });
                result = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            }

            if (result != null)
                return Create(result);

            return new Session();
        }


        public async Task<Session> ExtendSessionAsync(string userId, string authdata)
        {
            await AuthenticateAndAuthorizeAsync();
            DataRow result = null;

            if (Settings.Default.InProcSessions)
            {
                InitializeTable();
                await _semaphore.WaitAsync();
                try
                {
                    await ClearOldLoginsAsync();
                    if (SessionController._loggedInUsersTable.AsEnumerable().Any(row => string.Equals(row.GetValue<string>("Authdata", string.Empty), authdata, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        result = SessionController._loggedInUsersTable.AsEnumerable().FirstOrDefault(row => string.Equals(row.GetValue<string>("Authdata", string.Empty), authdata, StringComparison.InvariantCultureIgnoreCase));
                        result["DateAdded"] = DateTime.Now.Add(BusinessLogic.Core.Properties.Settings.Default.SessionTimeout);
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            else
            {
                await ClearOldLoginsAsync();
                Method method = new Method();
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Session;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Methods.ExtendSession.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.UserId.GetIntValue()) { Value = userId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.SessionDomain.GetIntValue()) { Value = SessionDomain });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.DateAdded.GetIntValue()) { Value = DateTime.Now.Add(BusinessLogic.Core.Properties.Settings.Default.SessionTimeout).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) });
                method.ClearCache = true;

                result = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            }

            if (result != null)
                return Create(result);

            return new Session();
        }

        /// <summary>
        /// Dewtermine wether a user is authenticated by the provided authentication data
        /// </summary>
        /// <param name="authdata">Authentication data token</param>
        /// <returns>Boolean value, true if user is authenticated otherwise false</returns>
        public async Task<bool> UserAuthenticatedAsync(string authdata)
        {
            return (await GetLoggedOnUserAsync(authdata)) != null;
        }

        /// <summary>
        /// Get a logged on user by the provided authentication data
        /// </summary>
        /// <param name="authdata">Authentication data token</param>
        /// <returns>Authenticated session object</returns>
        public async Task<Session> GetLoggedOnUserAsync(string authdata)
        {
            await AuthenticateAndAuthorizeAsync();
            DataRow result = null;
            if (Settings.Default.InProcSessions)
            {
                InitializeTable();
                await _semaphore.WaitAsync();
                try
                {
                    await ClearOldLoginsAsync();
                    result = SessionController._loggedInUsersTable.AsEnumerable().FirstOrDefault(row => string.Equals(row.GetValue<string>("Authdata", string.Empty), authdata, StringComparison.InvariantCultureIgnoreCase));
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            else
            {
                await ClearOldLoginsAsync();
                Method method = new Method();
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Session;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Methods.GetLoggedOnUser.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.Authdata.GetIntValue()) { Value = authdata });
                result = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            }

            if (result != null)
            {
                return Create(result);
            }

            return null;
        }

        /// <summary>
        /// Delete a session by the provided authentication data
        /// </summary>
        /// <param name="authdata">Authentication data token</param>
        public async Task RemoveUserByAuthDataAsync(string authdata)
        {
            await AuthenticateAndAuthorizeAsync();
            if (Settings.Default.InProcSessions)
            {
                InitializeTable();
                await _semaphore.WaitAsync();
                try
                {
                    await ClearOldLoginsAsync();
                    DataRow result = SessionController._loggedInUsersTable.AsEnumerable().FirstOrDefault(row => string.Equals(row.GetValue<string>("Authdata", string.Empty), authdata, StringComparison.InvariantCultureIgnoreCase));
                    if (result != null)
                    {
                        result.Delete();
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            else
            {
                await ClearOldLoginsAsync();
                Method method = new Method();
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Session;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Methods.RemoveUserByAuthData.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.Authdata.GetIntValue()) { Value = authdata });
                DataRow result = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            }
        }

        /// <summary>
        /// Delete a session by the user id
        /// </summary>
        /// <param name="userId">User Id</param>
        public async Task RemoveUserByUserIdAsync(string userId)
        {
            await AuthenticateAndAuthorizeAsync();
            if (Settings.Default.InProcSessions)
            {
                InitializeTable();
                await _semaphore.WaitAsync();
                try
                {
                    await ClearOldLoginsAsync();
                    DataRow result = SessionController._loggedInUsersTable.AsEnumerable().FirstOrDefault(row => row.GetValue<string>("UserId").Equals(userId));
                    if (result != null)
                    {
                        result.Delete();
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            else
            {
                await ClearOldLoginsAsync();
                Method method = new Method();
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Session;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Methods.RemoveUserById.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Parameters.UserId.GetIntValue()) { Value = userId });
                DataRow result = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            }
        }
        #endregion
    }
}
