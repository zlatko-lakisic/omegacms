using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ProfileTypeController : BaseController<ProfileTypeController>
    {
        [Obsolete("Deprecated", true)]
        public ProfileType GetById(long id, bool transformExpression = true)
        {
            return GetByIdAsync(id, transformExpression).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ProfileType> GetAllWithPagination(long pageIndex, long pageSize, string searchTerm, string sort = "Name ASC")
        {
            return GetAllWithPaginationAsync(pageIndex, pageSize, searchTerm, sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public long GetAllCount(string searchTerm)
        {
            return GetAllCountAsync(searchTerm).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ProfileType> GetAll(string sort = "Name ASC")
        {
            return GetAllAsync(sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ProfileType> GetByUser(User user)
        {
            return GetByUserIdAsync(user.Id).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ProfileType> GetByUserId(string id)
        {
            return GetByUserIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public int GetByUserCount(string userId)
        {
            return GetByUserCountAsync(userId).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ProfileType> GetNotBelongingProfileTypesByUser(User user)
        {
            return GetNotBelongingProfileTypesByUserAsync(user).Result;
        }

        [Obsolete("Deprecated", true)]
        public ProfileType Save(ProfileType profileType)
        {
            return SaveAsync(profileType).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(ProfileType obj)
        {
            return DeleteAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ProfileType> Search(string searchTerm)
        {
            return SearchAsync(searchTerm).Result;
        }
    }
}
