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
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class PermissionsController : BaseController<PermissionsController>
    {
        private IEnumerable<ProfileTypePermissions> ConstructProfileTypePermissions(PermissionTypeEnum type, DataTable table, ProfileType profileType = null)
        {
            List<ProfileTypePermissions> permissions = new List<ProfileTypePermissions>();
            if (table.Rows.Count > 0)
            {
                IEnumerable<long> profileTypeIds = table.AsEnumerable().Select(row => row.GetValue<long>(PermissionsEnum.EntityId.GetStringValue())).ToList().Distinct();

                foreach (long profileTypeId in profileTypeIds)
                {
                    ProfileTypePermissions obj = new ProfileTypePermissions();
                    obj.ProfileId = profileTypeId;
                    IEnumerable<DataRow> foundPermissions = table.AsEnumerable().Where(row => row.GetValue<long>(PermissionsEnum.EntityId.GetStringValue()).Equals(profileTypeId));

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
                                obj.EntityPermissions = Entity.GetNewInstance().Create(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, apiPermissionsTable);
                                break;
                        }
                    }

                    permissions.Add(obj);
                }
            }

            if (profileType != null && !permissions.Any(p => p.ProfileId.Equals(profileType.Id)))
            {
                permissions.Add(new ProfileTypePermissions()
                {
                    Id = 0,
                    ProfileId = profileType.Id
                });
            }

            return permissions;
        }

        public async Task<IEnumerable<ProfileTypePermissions>> GetProfileTypePermissionsByObjectAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object, string _objectId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.GetByObjectId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Object.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = _object.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.ObjectId.GetIntValue()) { Value = _objectId });
            return ConstructProfileTypePermissions(PermissionTypeEnum.Object, await ExecuteMethodTableAsync(method, UseDefaultPlugin));
        }

        public async Task<IEnumerable<ProfileTypePermissions>> GetApiProfileTypePermissionsByEntityObjectAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.GetObjectEntity.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Api.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = _entity.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = _object.GetIntValue() });
            return ConstructProfileTypePermissions(PermissionTypeEnum.Api, await ExecuteMethodTableAsync(method, UseDefaultPlugin));
        }

        public async Task<IEnumerable<ProfileTypePermissions>> GetProfileTypePermissionsByEntityIdAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, string _entityId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.GetByEntityId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Api.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = _entity.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = _entityId });
            return ConstructProfileTypePermissions(PermissionTypeEnum.Api, await ExecuteMethodTableAsync(method, UseDefaultPlugin));
        }

        public async Task<IEnumerable<ProfileTypePermissions>> GetProfileTypePermissionsByEntityAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.GetByEntity.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Type.GetIntValue()) { Value = PermissionTypeEnum.Api.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = _object.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = _entity.GetIntValue() });
            return ConstructProfileTypePermissions(PermissionTypeEnum.Api, await ExecuteMethodTableAsync(method, UseDefaultPlugin));
        }

        public async Task<bool> SaveAsync(ProfileTypePermissions obj, PermissionTypeEnum type = PermissionTypeEnum.Object)
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
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType.GetIntValue() });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = obj.ProfileId });
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
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Object.GetIntValue()) { Value = permission.Entity.GetIntValue() });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType.GetIntValue() });
                deleteMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = obj.ProfileId });
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
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType.GetIntValue() });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = obj.ProfileId });
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
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType.GetIntValue() });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = obj.ProfileId });
                        saveMethod.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.AccessType.GetIntValue()) { Value = accessType.GetIntValue() });
                        saveMethod.ClearCache = true;

                        await ExecuteMethodBooleanAsync(saveMethod);
                    }
                }
            }

            return true;
        }

        public async Task<bool> ProfileTypeHasObjectPermissionAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object, string _objectId, long _profileTypeId, PermissionAccessTypeEnum _permission)
        {
            IEnumerable<ProfileTypePermissions> permissions = await GetProfileTypePermissionsByObjectAsync(_object, _objectId);
            if (!permissions.Any())
            {
                return true;
            }
            return permissions.Any(p => p.ProfileId.Equals(_profileTypeId) && p.ObjectPermissions.Any(op => op.AccessTypes.Contains(_permission)));
        }

        public async Task<bool> ProfileTypeHasEntityPermissionAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, long _profileTypeId, PermissionAccessTypeEnum _permission)
        {
            IEnumerable<ProfileTypePermissions> permissions = await GetProfileTypePermissionsByEntityIdAsync(_entity, _profileTypeId.ToString());
            if (!permissions.Any())
            {
                return true;
            }
            return permissions.Any(p => p.ProfileId.Equals(_profileTypeId) && p.EntityPermissions.Any(op => op.AccessTypes.Contains(_permission)));
        }

        public async Task<IEnumerable<ProfileTypePermissions>> GetAllPermissionsByProfileTypeAsync(ProfileType profileType)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Permissions;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods.GetAllByEntityId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.Entity.GetIntValue()) { Value = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Parameters.EntityId.GetIntValue()) { Value = profileType.Id });
            return ConstructProfileTypePermissions(PermissionTypeEnum.Api, await ExecuteMethodTableAsync(method, UseDefaultPlugin), profileType);
        }
    }
}
