using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions
{
    public class ProfileTypePermissions : PermissionsBase
    {
        #region Attributes
        private long _profileId;
        #endregion

        #region Properties
        public long ProfileId { get => _profileId; set => _profileId = value; }
        #endregion
    }
}
