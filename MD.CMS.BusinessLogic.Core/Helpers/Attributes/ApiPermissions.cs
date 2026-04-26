using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.Helpers.Attributes
{
    public class EntityPermissionAttribute : System.Attribute
    {
        private Entities _entity;
        private HashSet<PermissionAccessTypeEnum> _accessTypes;

        public EntityPermissionAttribute(Entities entity, params PermissionAccessTypeEnum[] accessTypes)
        {
            _entity = entity;
            _accessTypes = new HashSet<PermissionAccessTypeEnum>();
            accessTypes.ToList().ForEach(accessType => _accessTypes.Add(accessType));
        }

        public Entities Entity { get => _entity; }
        public HashSet<PermissionAccessTypeEnum> AccessTypes { get => _accessTypes; }
    }
}
