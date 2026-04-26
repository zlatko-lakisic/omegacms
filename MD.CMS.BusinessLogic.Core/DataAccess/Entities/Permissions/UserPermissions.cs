using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions
{
    public class UserPermissions : PermissionsBase
    {
        #region Attributes
        private string _userId;
        #endregion

        #region Properties
        public string UserId { get => _userId; set => _userId = value; }
        #endregion
    }
}
