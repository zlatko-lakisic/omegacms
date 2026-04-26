using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions
{
    public abstract class PermissionsBase : BaseEntity<long>
    {
        #region Attributes
        private List<EntityPermission> _entityPermissions;
        private List<ObjectPermission> _objectPermissions;
        #endregion

        #region Properties
        public List<EntityPermission> EntityPermissions
        {
            get
            {
                if(_entityPermissions == null)
                {
                    _entityPermissions = new List<EntityPermission>();
                }
                return _entityPermissions;
            }
            set => _entityPermissions = value;
        }
        public List<ObjectPermission> ObjectPermissions
        {
            get
            {
                if (_objectPermissions == null)
                {
                    _objectPermissions = new List<ObjectPermission>();
                }
                return _objectPermissions;
            }
            set => _objectPermissions = value;
        }
        #endregion
    }
}
