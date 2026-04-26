using System;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ProfileTypeFieldValueController : BaseController<ProfileTypeFieldValueController>
    {
        [Obsolete("Deprecated", true)]
        public ProfileTypeFieldValue GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public ProfileTypeFieldValue GetByPrimaryKeys(long profileTypeFieldId, string userId, long profileTypeId)
        {
            return GetByPrimaryKeysAsync(profileTypeFieldId, userId, profileTypeId).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ProfileTypeFieldValue> GetByUser(User obj)
        {
            return GetByUserAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ProfileTypeFieldValue> GetByUserAndProfileType(User user, ProfileType profileType)
        {
            return GetByUserAndProfileTypeAsync(user, profileType).Result;
        }

        [Obsolete("Deprecated", true)]
        public ProfileTypeFieldValue Save(ProfileTypeFieldValue obj)
        {
            return SaveAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public ProfileTypeFieldValue Update(ProfileTypeFieldValue obj)
        {
            return UpdateAsync(obj).Result;
        }
    }
}
