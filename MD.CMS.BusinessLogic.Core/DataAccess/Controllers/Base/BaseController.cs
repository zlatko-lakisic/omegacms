using System.Data;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Diagnostics;
using System.Reflection;
using MD.CMS.BusinessLogic.Core.Helpers.Attributes;
using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.Helpers.Core.Exceptions;
using System;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MD.Tools.Helpers.Core.Logging;
using System.Globalization;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base
{
    public abstract class BaseController<T> : MD.Tools.BaseDataAccess.PluginMethods.Core.Controllers.BaseController<T>
       where T : class, new()
    {
        private class PermissionsModelStore<T>
            where T : PermissionsBase
        {
            public IEnumerable<T> Permissions { get; set; }
            public DateTime RefreshTime { get; set; }
        }

        #region Attributes
        private bool _useDefaultPlugin;
        private User _userMakingTheCall;
        private static ConcurrentDictionary<string, PermissionsModelStore<UserPermissions>> _userPermissions = new ConcurrentDictionary<string, PermissionsModelStore<UserPermissions>>();
        private static ConcurrentDictionary<string, PermissionsModelStore<ProfileTypePermissions>> _profileTypePermissions = new ConcurrentDictionary<string, PermissionsModelStore<ProfileTypePermissions>>();
        private static TimeSpan _checkInterval = TimeSpan.Parse("00:01:00", CultureInfo.InvariantCulture);
        #endregion

        #region Properties
        /// <summary>
        /// Wether to use the default plugin
        /// </summary>
        public bool UseDefaultPlugin
        {
            get { return _useDefaultPlugin; }
            set { _useDefaultPlugin = value; }
        }
        /// <summary>
        /// What user is making this call - This is not needed for public calls
        /// </summary>
        public User UserMakingTheCall
        {
            set
            {
                _userMakingTheCall = value;
            }
            get
            {
                return _userMakingTheCall;
            }
        }

        private async Task<IEnumerable<UserPermissions>> UserMakingTheCallUserPermissionsAsync()
        {
            if(_userMakingTheCall == null)
            {
                return new List<UserPermissions>();
            }

            IEnumerable<UserPermissions> permissions = new List<UserPermissions>();

            if (_userMakingTheCall.Id == User.SystemUser().Id)
            {
                permissions = new List<UserPermissions>
                { 
                    new UserPermissions() {
                        EntityPermissions = Settings.Default.SystemUserPermissions,
                        UserId = _userMakingTheCall.Id
                    }
                };
            }
            else
            {
                if (_userPermissions.ContainsKey(_userMakingTheCall.Id))
                {
                    if (_userPermissions[_userMakingTheCall.Id].RefreshTime.Subtract(DateTime.Now).TotalSeconds < 0)
                    {
                        _userPermissions[_userMakingTheCall.Id].Permissions = await Controllers.PermissionsController.GetNewInstance().Caller(User.SystemUser()).GetAllPermissionsByUserAsync(_userMakingTheCall);
                        _userPermissions[_userMakingTheCall.Id].RefreshTime = _userPermissions[_userMakingTheCall.Id].RefreshTime.Add(_checkInterval);
                    }
                    permissions = _userPermissions[_userMakingTheCall.Id].Permissions;
                }
                else
                {
                    try
                    {
                        permissions = await Controllers.PermissionsController.GetNewInstance().Caller(User.SystemUser()).GetAllPermissionsByUserAsync(_userMakingTheCall);
                        _userPermissions.TryAdd(_userMakingTheCall.Id, new PermissionsModelStore<UserPermissions> {
                            Permissions = permissions,
                            RefreshTime = DateTime.Now.Add(_checkInterval)
                        });
                    }
                    catch (Exception error)
                    {
                        typeof(BaseController<T>).Log(error);
                    }
                }
            }

            return permissions;
        }

        private async Task<IEnumerable<ProfileTypePermissions>> UserMakingTheCallProfileTypePermissionsAsync()
        {
            if (_userMakingTheCall == null)
            {
                return new List<ProfileTypePermissions>();
            }

            List<ProfileTypePermissions> permissions = new List<ProfileTypePermissions>();

            if (_profileTypePermissions.ContainsKey(_userMakingTheCall.Id))
            {
                if (_profileTypePermissions[_userMakingTheCall.Id].RefreshTime.Subtract(DateTime.Now).TotalSeconds < 0)
                {
                    permissions = new List<ProfileTypePermissions>();
                    foreach (ProfileType profile in _userMakingTheCall.ProfileTypes)
                    {
                        permissions.AddRange(await Controllers.PermissionsController.GetNewInstance().Caller(User.SystemUser()).GetAllPermissionsByProfileTypeAsync(profile));
                    }

                    _profileTypePermissions[_userMakingTheCall.Id].Permissions = permissions;
                    _profileTypePermissions[_userMakingTheCall.Id].RefreshTime = _profileTypePermissions[_userMakingTheCall.Id].RefreshTime.Add(_checkInterval);
                }
                permissions = _profileTypePermissions[_userMakingTheCall.Id].Permissions.ToList();
            }
            else
            {
                try
                {
                    permissions = new List<ProfileTypePermissions>();
                    foreach (ProfileType profile in _userMakingTheCall.ProfileTypes)
                    {
                        permissions.AddRange(await Controllers.PermissionsController.GetNewInstance().Caller(User.SystemUser()).GetAllPermissionsByProfileTypeAsync(profile));
                    }
                    _profileTypePermissions.TryAdd(_userMakingTheCall.Id, new PermissionsModelStore<ProfileTypePermissions>
                    {
                        Permissions = permissions,
                        RefreshTime = DateTime.Now.Add(_checkInterval)
                    });
                }
                catch (Exception error)
                {
                    typeof(BaseController<T>).Log(error);
                }
            }

            return permissions;
        }

        private bool CheckAuthorized(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Entities type, EntityPermissionAttribute[] attrs, EntityPermission[] entityPermissions, bool isAuthorized)
        {
            if (!isAuthorized)
            {
                int numberOfPermissionsRequired = attrs.Length;
                int numberOfPermissionsAllowed = 0;

                foreach (EntityPermissionAttribute apiPermissionAttribute in attrs)
                {
                    if (apiPermissionAttribute.Entity != null && apiPermissionAttribute.AccessTypes != null)
                    {
                        foreach (EntityPermission entityPermission in entityPermissions)
                        {
                            if((type == Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User && entityPermission.Object == apiPermissionAttribute.Entity) || 
                               (type == Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType && entityPermission.Entity == apiPermissionAttribute.Entity))
                            {
                                if (apiPermissionAttribute.AccessTypes.Any())
                                {
                                    if(apiPermissionAttribute.AccessTypes.Except(entityPermission.AccessTypes).Count() == 0)
                                    {
                                        numberOfPermissionsAllowed++;
                                    }
                                }
                                else
                                {
                                    numberOfPermissionsAllowed++;
                                }
                            }
                        }
                    }
                }
                isAuthorized = numberOfPermissionsRequired == numberOfPermissionsAllowed;
            }
            return isAuthorized;
        }

        public async Task AuthenticateAndAuthorizeAsync(Func<EntityPermissionAttribute[], bool> specialAuthentication = null)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrames()[1];
            MethodBase methodInfo = frame.GetMethod();
            EntityPermissionAttribute[] attrs = methodInfo.GetCustomAttributes(typeof(EntityPermissionAttribute), false) as EntityPermissionAttribute[];

            if (attrs == null || !attrs.Any())
            {
                return;
            }

            if (specialAuthentication != null)
            {
                IsAuthorized = specialAuthentication(attrs);
            }

            if (!IsAuthorized)
            {
                if (_userMakingTheCall != null)
                {
                    foreach (UserPermissions up in await UserMakingTheCallUserPermissionsAsync())
                    {
                        IsAuthorized = CheckAuthorized(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User ,attrs, up.EntityPermissions.ToArray(), IsAuthorized);
                    }

                    foreach (ProfileTypePermissions up in await UserMakingTheCallProfileTypePermissionsAsync())
                    {
                        IsAuthorized = CheckAuthorized(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, attrs, up.EntityPermissions.ToArray(), IsAuthorized);
                    }
                }
            }

            if (!IsAuthorized)
            {
                if(_userMakingTheCall == null)
                {
                    throw new MDEntityUnauthorizedException();
                }
                else
                {
                    throw new MDEntityUnauthorizedException(_userMakingTheCall.Id, _userMakingTheCall.Username, attrs.Select(attr => {
                        return new MDEntityUnauthorizedException.MDExceptionEntityMapping(attr.Entity.ToString(), attr.AccessTypes.Select(accessType => accessType.ToString()).ToArray());
                    }).ToArray());
                }
            }
        }

        /// <summary>
        /// Create an instance of the base entity class
        /// </summary>
        /// <typeparam name="E">Type that inherits the BaseEntity class</typeparam>
        /// <typeparam name="K">Id property type</typeparam>
        /// <param name="row">Data row for entity</param>
        /// <param name="idColumnName">Name of the column for the Id property</param>
        /// <param name="isDeleteColumName">Name of the column for the IsDeleted property</param>
        /// <returns>Instance of class</returns>
        public virtual E Create<E, K>(DataRow row, string idColumnName = "", string isDeleteColumName = "")
            where E : BaseEntity<K>, new()
        {
            E obj = null;
            if (row != null)
            {
                obj = new E();
                if (!string.IsNullOrEmpty(idColumnName))
                {
                    obj.Id = row.GetValue<K>(idColumnName);
                }

                if (!string.IsNullOrEmpty(isDeleteColumName))
                {
                    obj.IsDeleted = row.GetValue<bool>(isDeleteColumName);
                }

            }
            return obj;
        }
        #endregion
    }
}
