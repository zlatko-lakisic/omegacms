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
        public IEnumerable<UserPermissions> GetUserPermissionssByObject(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object, string _objectId)
        {
            return GetUserPermissionssByObjectAsync(_object, _objectId).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<UserPermissions> GetUserPermissionsByEntityId(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, string _entityId)
        {
            return GetUserPermissionsByEntityIdAsync(_entity, _entityId).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<UserPermissions> GetUserPermissionsByEntity(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object)
        {
            return GetUserPermissionsByEntityAsync(_entity, _object).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Save(UserPermissions obj, PermissionTypeEnum type = PermissionTypeEnum.Object)
        {
            return SaveAsync(obj, type).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool UserHasObjectPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object, string _objectId, string _userId, PermissionAccessTypeEnum _permission)
        {
            return UserHasObjectPermissionAsync(_object, _objectId, _userId, _permission).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool UserHasEntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity, string _userId, PermissionAccessTypeEnum _permission)
        {
            return UserHasEntityPermissionAsync(_entity, _userId, _permission).Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<UserPermissions> GetAllPermissionsByUser(User user)
        {
            return GetAllPermissionsByUserAsync(user).Result;
        }
    }
}
