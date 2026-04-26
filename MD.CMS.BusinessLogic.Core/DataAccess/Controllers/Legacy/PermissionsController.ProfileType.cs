using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class PermissionsController : BaseController<PermissionsController>
    {
        [Obsolete("Deprecated", true)]
        public IEnumerable<ProfileTypePermissions> GetProfileTypePermissionsByObject(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object, string _objectId)
        {
            return GetProfileTypePermissionsByObjectAsync(_object, _objectId).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<ProfileTypePermissions> GetApiProfileTypePermissionsByEntityObject(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object)
        {
            return GetApiProfileTypePermissionsByEntityObjectAsync(_entity, _object).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<ProfileTypePermissions> GetProfileTypePermissionsByEntityId(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, string _entityId)
        {
            return GetProfileTypePermissionsByEntityIdAsync(_entity, _entityId).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<ProfileTypePermissions> GetProfileTypePermissionsByEntity(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object)
        {
            return GetProfileTypePermissionsByEntityAsync(_entity, _object).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Save(ProfileTypePermissions obj, PermissionTypeEnum type = PermissionTypeEnum.Object)
        {
            return SaveAsync(obj, type).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool ProfileTypeHasObjectPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object, string _objectId, long _profileTypeId, PermissionAccessTypeEnum _permission)
        {
            return ProfileTypeHasObjectPermissionAsync(_object, _objectId, _profileTypeId, _permission).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool ProfileTypeHasEntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, long _profileTypeId, PermissionAccessTypeEnum _permission)
        {
            return ProfileTypeHasEntityPermissionAsync(_entity, _profileTypeId, _permission).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<ProfileTypePermissions> GetAllPermissionsByProfileType(ProfileType profileType)
        {
            return GetAllPermissionsByProfileTypeAsync(profileType).Result;
        }
    }
}
