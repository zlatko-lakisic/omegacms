using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.Helpers.Attributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    //this whole class uses default plugin
    public partial class UserController : BaseController<UserController>
    {
        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public List<User> GetAll()
        {
            return GetAllAsync().Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public Entities.Base.BasePaginationEntity<User> GetAllWithPagination(int currentPageIndex, int maxNumberOfRows, string searchTerm, string sort = "Username ASC")
        {
            return GetAllWithPaginationAsync(currentPageIndex, maxNumberOfRows, searchTerm, sort).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public int SelectAllCount(string searchTerm)
        {
            return SelectAllCountAsync(searchTerm).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public User GetById(string id, bool useDefaultPlugin = false, bool isFull = true)
        {
            return GetByIdAsync(id, useDefaultPlugin, isFull).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public User GetByReferenceIdAndProvider(string id, string authenticationProvider, bool isFull = true)
        {
            return GetByReferenceIdAndProviderAsync(id, authenticationProvider, isFull).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public User GetByAuthData(AuthData data)
        {
            return GetByAuthDataAsync(data).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public User Save(User user)
        {
            return SaveAsync(user).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public User Update(User user)
        {
            return UpdateAsync(user).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public User UpdateToken(User user)
        {
            return UpdateTokenAsync(user).Result;
        }

        [Obsolete("Deprecated", true)]
        private void SaveOrUpdateFieldValue(ProfileType profileType)
        {
            Task.Run(async () => {
                await SaveOrUpdateFieldValueAsync(profileType); }).Wait();
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Delete)]
        public bool Delete(User obj)
        {
            return DeleteAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public int GetUsersByProfileTypeCount(ProfileType profileType)
        {
            return GetUsersByProfileTypeCountAsync(profileType).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public User GetIdByUserName(string username)
        {
            return GetIdByUserNameAsync(username).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public User UpdateUser(string idUser, string token1, string tokenDate)
        {
            return UpdateUserAsync(idUser, token1, tokenDate).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public User GetIdByToken(string token)
        {
            return GetIdByTokenAsync(token).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public User UpdateUserByToken(User user)
        {
            return UpdateUserByTokenAsync(user).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public List<User> Search(string searchTerm)
        {
            return SearchAsync(searchTerm).Result;
        }

        [Obsolete("Deprecated", true)]
        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public User ResetPassword(User user)
        {
            return ResetPasswordAsync(user).Result;
        }

        [Obsolete("Deprecated", true)]
        public int GetCount()
        {
            return GetCountAsync().Result;
        }
    }
}
