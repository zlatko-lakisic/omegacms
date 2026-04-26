using System.Collections.Generic;
namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions
{
    public class EntityPermission
    {
        #region Attributes
        private Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object;
        private Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _entity;
        private HashSet<PermissionAccessTypeEnum> _accessTypes;
        #endregion

        #region Properties
        public Tools.BaseDataAccess.Plugins.Core.Mapping.Entities Entity { get => _entity; set => _entity = value; }
        public HashSet<PermissionAccessTypeEnum> AccessTypes 
        { 
            get
            {
                if(_accessTypes == null)
                {
                    _accessTypes = new HashSet<PermissionAccessTypeEnum>();
                }
                return _accessTypes;
            }
            set => _accessTypes = value; 
        }
        public Tools.BaseDataAccess.Plugins.Core.Mapping.Entities Object { get => _object; set => _object = value; }
        #endregion
    }
}
