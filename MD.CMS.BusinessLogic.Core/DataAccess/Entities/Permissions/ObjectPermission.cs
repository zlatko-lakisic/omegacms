using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions
{
    public class ObjectPermission
    {
        #region Attributes
        private Tools.BaseDataAccess.Plugins.Core.Mapping.Entities _object;
        private List<PermissionAccessTypeEnum> _accessTypes;
        private string _objectId;
        #endregion

        #region Properties
        public Tools.BaseDataAccess.Plugins.Core.Mapping.Entities Object { get => _object; set => _object = value; }
        public string ObjectId
        {
            get => _objectId ?? string.Empty;
            set => _objectId = value;
        }
        public List<PermissionAccessTypeEnum> AccessTypes { get => _accessTypes; set => _accessTypes = value; }
        #endregion
    }
}
