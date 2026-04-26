using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Properties;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class PermissionsController : BaseController<PermissionsController>
    {
        private IEnumerable<UserPermissions> ConstructUserPermissions(PermissionTypeEnum type, DataTable table)
        {
            List<UserPermissions> permissions = new List<UserPermissions>();
            if (table.Rows.Count > 0)
            {
                IEnumerable<string> userIds = table.AsEnumerable().Select(row => row.GetValue<string>(PermissionsEnum.EntityId.GetStringValue())).ToList().Distinct();


                foreach (string userId in userIds)
                {
                    UserPermissions obj = new UserPermissions();
                    obj.UserId = userId;
                    IEnumerable<DataRow> foundPermissions = table.AsEnumerable().Where(row => row.GetValue<long>(PermissionsEnum.EntityId.GetStringValue()).Equals(userId));

                    if (foundPermissions != null && foundPermissions.Count() > 0)
                    {
                        switch (type)
                        {
                            case PermissionTypeEnum.Object:
                                DataTable objectPermissionsTable = table.Copy();
                                objectPermissionsTable.Rows.Clear();
                                foreach (DataRow rowToCopy in foundPermissions)
                                {
                                    objectPermissionsTable.ImportRow(rowToCopy);
                                }
                                obj.ObjectPermissions = Object.GetNewInstance().Create(objectPermissionsTable);
                                break;
                            case PermissionTypeEnum.Api:
                                DataTable apiPermissionsTable = table.Copy();
                                apiPermissionsTable.Rows.Clear();
                                foreach (DataRow rowToCopy in foundPermissions)
                                {
                                    apiPermissionsTable.ImportRow(rowToCopy);
                                }
                                obj.EntityPermissions = Entity.GetNewInstance().Create(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, apiPermissionsTable);
                                break;
                        }
                    }

                    permissions.Add(obj);
                }
            }
            return permissions;
        }

        public async Task<IEnumerable<UserPermissions>> GetUserPermissionssByObjectAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object, string _objectId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.GetByObjectId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Object.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = _object.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.ObjectId.GetIntValue()) { Value = _objectId });
            return ConstructUserPermissions(PermissionTypeEnum.Object, await ExecuteMethodTableAsync(method, UseDefaultPlugin));
        }

        public async Task<IEnumerable<UserPermissions>> GetUserPermissionsByEntityIdAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, string _entityId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.GetByEntityId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Api.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = _entity.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = _entityId });
            return ConstructUserPermissions(PermissionTypeEnum.Api, await ExecuteMethodTableAsync(method, UseDefaultPlugin));
        }

        public async Task<IEnumerable<UserPermissions>> GetUserPermissionsByEntityAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.GetByEntity.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Api.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = _object.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = _entity.GetIntValue() });
            return ConstructUserPermissions(PermissionTypeEnum.Api, await ExecuteMethodTableAsync(method, UseDefaultPlugin));
        }

        public async Task<bool> SaveAsync(UserPermissions obj, PermissionTypeEnum type = PermissionTypeEnum.Object)
        {
            await AuthenticateAndAuthorizeAsync();
            foreach (ObjectPermission permission in obj.ObjectPermissions)
            {
                Method deleteMethod = new Method();
                deleteMethod.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                deleteMethod.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
                deleteMethod.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.DeleteByEntity.GetIntValue();
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = type.GetIntValue() });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = permission.Object.GetIntValue() });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.ObjectId.GetIntValue()) { Value = permission.ObjectId });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User.GetIntValue() });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = obj.UserId });
                deleteMethod.ClearCache = true;

                await ExecuteMethodBooleanAsync(deleteMethod);
            }

            foreach (EntityPermission permission in obj.EntityPermissions)
            {
                Method deleteMethod = new Method();
                deleteMethod.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                deleteMethod.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
                deleteMethod.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.DeleteByEntity.GetIntValue();
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Api.GetIntValue() });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = permission.Object.GetIntValue() });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = permission.Entity.GetIntValue() });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = obj.UserId });
                deleteMethod.ClearCache = true;

                await ExecuteMethodBooleanAsync(deleteMethod);
            }

            if (!obj.IsDeleted)
            {
                foreach (ObjectPermission permission in obj.ObjectPermissions)
                {
                    foreach (PermissionAccessTypeEnum accessType in permission.AccessTypes)
                    {
                        Method saveMethod = new Method();
                        saveMethod.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                        saveMethod.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
                        saveMethod.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.Save.GetIntValue();
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Object.GetIntValue() });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = permission.Object.GetIntValue() });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.ObjectId.GetIntValue()) { Value = permission.ObjectId });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User.GetIntValue() });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = obj.UserId });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.AccessType.GetIntValue()) { Value = accessType.GetIntValue() });
                        saveMethod.ClearCache = true;

                        await ExecuteMethodBooleanAsync(saveMethod);
                    }
                }

                foreach (EntityPermission permission in obj.EntityPermissions)
                {
                    foreach (PermissionAccessTypeEnum accessType in permission.AccessTypes)
                    {
                        Method saveMethod = new Method();
                        saveMethod.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                        saveMethod.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
                        saveMethod.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.Save.GetIntValue();
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Api.GetIntValue() });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = permission.Entity.GetIntValue() });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User.GetIntValue() });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = obj.UserId });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.AccessType.GetIntValue()) { Value = accessType.GetIntValue() });
                        saveMethod.ClearCache = true;

                        await ExecuteMethodBooleanAsync(saveMethod);
                    }
                }
            }

            return true;
        }

        public async Task<bool> UserHasObjectPermissionAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object, string _objectId, string _userId, PermissionAccessTypeEnum _permission)
        {
            if (_userId.Equals(Settings.Default.RootId()))
            {
                return true;
            }

            IEnumerable<UserPermissions> permissions = await GetUserPermissionssByObjectAsync(_object, _objectId);
            if (!permissions.Any())
            {
                return true;
            }
            return permissions.Any(p => p.UserId.Equals(_userId) && p.ObjectPermissions.Any(op => op.AccessTypes.Contains(_permission)));
        }

        public async Task<bool> UserHasEntityPermissionAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, string _userId, PermissionAccessTypeEnum _permission)
        {
            IEnumerable<UserPermissions> permissions = null;

            if (_userId.Equals(Settings.Default.RootId()))
            {
                permissions = new List<UserPermissions>() {
                    new UserPermissions()
                    {
                        EntityPermissions = Settings.Default.RootEntityPermissions().ToList(),
                        ObjectPermissions = new List<ObjectPermission>(),
                        UserId = _userId
                    }
                };
            }
            else
            {
                permissions = await GetUserPermissionsByEntityAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, _entity);
            }

            return permissions.Any(p => p.UserId.Equals(_userId) && p.EntityPermissions.Any(op => op.AccessTypes.Contains(_permission)));
        }

        public async Task<IEnumerable<UserPermissions>> GetAllPermissionsByUserAsync(User user)
        {
            await AuthenticateAndAuthorizeAsync();
            if (user.Id.Equals(Settings.Default.RootId()))
            {
                return new List<UserPermissions>()
                {
                    new UserPermissions()
                    {
                        UserId = user.Id,
                        EntityPermissions = Settings.Default.RootEntityPermissions().ToList()
                    }
                };
            }

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.GetAllByEntityId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = user.Id });
            return ConstructUserPermissions(PermissionTypeEnum.Api, await ExecuteMethodTableAsync(method, UseDefaultPlugin));
        }
    }
}
