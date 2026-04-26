using MD.CMS.BusinessLogic.Core.DataAccess.Controllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication.BuiltIn
{
    public class BuiltInAuthenticationProvider : BaseController<BuiltInAuthenticationProvider>, IAuthenticationProvider
    {
        #region Attributes
        private bool _enabled;
        #endregion

        #region Properties
        public static string GetProviderName() => "BuiltInAuthenticationProvider";

        public string ProviderName => GetProviderName();

        public bool CanCreateUser => true;

        public bool CanUpdateUser => true;

        public bool CanDeleteUser => true;

        public bool CanResetAuthData => true;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        #endregion

        #region Methods
        public async Task<bool> DeleteAsync(IAuthUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(AuthData authData)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IUser>> GetAsync(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetCountAsync(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsValidAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsValidAsync(IAuthUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IAuthUser> LoginAsync(AuthData authData)
        {
            if (authData is null)
            {
                throw new ArgumentNullException(nameof(authData));
            }

            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.GetByUsernameAndPassword.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.ReadSingle;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Username.GetIntValue()) { Value = authData.GetData<string>(BuiltInFieldNames.Username) });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Password.GetIntValue()) { Value = authData.GetData<string>(BuiltInFieldNames.Password) });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.AuthenticationProvider.GetIntValue()) { Value = ProviderName });

            BuiltInAuthenticationUser user =  await CreateAsync(ExecuteMethodRow(method));

            if (user != null)
            {
                user.AuthDataString = authData.GetData<string>(BuiltInFieldNames.Token);
            }

            return user;
        }

        public Task<IAuthUser> SaveAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        private async Task<BuiltInAuthenticationUser> CreateAsync(DataRow row)
        {
            BuiltInAuthenticationUser obj = base.Create<BuiltInAuthenticationUser, string>(row, UserEnum.UserId.GetStringValue());

            if (obj != null)
            {
                IEnumerable<ProfileType> profiles = await ProfileTypeController.GetNewInstance().Caller(UserMakingTheCall).GetByUserIdAsync(obj.Id);
                obj.MemberOf = profiles.Select(p => new MemberOf()
                {
                    CmsProfileId = p.Id.ToString(CultureInfo.InvariantCulture),
                    ProviderGroupId = p.Id.ToString(CultureInfo.InvariantCulture)
                });
            }

            return obj;
        }
        #endregion
    }
}
