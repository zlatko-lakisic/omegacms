using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ProfileTypeFieldController : BaseController<ProfileTypeFieldController>
    {
        [Obsolete("Deprecated", true)]
        public ProfileTypeField GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ProfileTypeField> GetByProfileType(ProfileType obj, bool transformExpression = true)
        {
            return GetByProfileTypeAsync(obj, transformExpression).Result;
        }

        [Obsolete("Deprecated", true)]
        public ProfileTypeField Save(ProfileTypeField obj)
        {
            return SaveAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(ProfileTypeField obj)
        {
            return DeleteAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(long id)
        {
            return DeleteAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteAllByProfileTypeId(long profileTypeId)
        {
            return DeleteAllByProfileTypeIdAsync(profileTypeId).Result;
        }
    }
}
