using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ProfileController : BaseController<ProfileController>
    {
        public bool Save(User user, ProfileType profileType)
        {
            return SaveAsync(user, profileType).Result;
        }

        public bool Delete(User user, ProfileType profileType)
        {
            return DeleteAsync(user, profileType).Result;
        }
        public bool DeleteAll(string id)
        {
            return DeleteAllAsync(id).Result;
        }
    }
}
