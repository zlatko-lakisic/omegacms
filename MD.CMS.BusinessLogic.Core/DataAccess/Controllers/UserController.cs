using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Data;
using System.Linq;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.Helpers.Attributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Globalization;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.Helpers.Core.Crypto;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication;
using MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication.BuiltIn;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.Helpers.Calculations;
using System;
using System.Collections.Concurrent;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    //this whole class uses default plugin
    public partial class UserController : BaseController<UserController>
    {
        private async Task<User> CreateAsync(DataRow row, bool isFull = false)
        {
            User obj = base.Create<User, string>(row, UserEnum.UserId.GetStringValue());
            if (obj != null)
            {
                obj.Username = row.GetValue<string>(UserEnum.Username.GetStringValue());
                obj.ProfileTypeId = row.GetValue<long>("ProfileTypeId");
                obj.Token = row.GetValue<string>("Token");
                obj.DateRefreshToken = row.GetValue<string>("DateRefreshToken");
                obj.IsDeleted = row.GetValue<bool>("IsDeleted");
                obj.AdministrationAllowed = row.GetValue<bool>("AdministrationAllowed");
                obj.ReferenceId = row.GetValue<string>("ReferenceId");
                obj.AuthenticationProvider = row.GetValue<string>("AuthenticationProvider");

                if (isFull)
                {
                    obj.ProfileTypes = new ProfileTypeList(await ProfileTypeController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByUserAsync(obj));
                    await Task.WhenAll(obj.ProfileTypes.Select(async profileType => {
                        if (profileType != null)
                        {
                            List<Task> tasks = new List<Task>()
                            {
                                Task.Run(async () => {
                                    profileType.Fields = await ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByUserAndProfileTypeAsync(obj, profileType);
                                })
                            };

                            ConcurrentQueue<ProfileTypeFieldValue> profileTypeFieldValues = new ConcurrentQueue<ProfileTypeFieldValue>();

                            tasks.AddRange((await ProfileTypeFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByProfileTypeAsync(profileType)).Select(async field => {
                                if (field.AttributeTypeDefinitionId == 18)
                                {
                                    ProfileTypeFieldValue calculated = new ProfileTypeFieldValue(field);
                                    await PostfixEvaluator.EvaluateAsync(UserMakingTheCall, profileTypeFieldValues, calculated, calculated.DefaultValue);
                                    profileTypeFieldValues.Enqueue(calculated);
                                }
                                else
                                {
                                    ProfileTypeFieldValue value = new ProfileTypeFieldValue(field);
                                    profileTypeFieldValues.Enqueue(value);
                                }
                            }));

                            await Task.WhenAll(tasks);

                            foreach (ProfileTypeFieldValue field in profileTypeFieldValues)
                            {
                                foreach (ProfileTypeFieldValue fieldold in profileType.Fields)
                                {
                                    if (fieldold.AttributeTypeDefinitionId != 18)
                                    {
                                        if (field.ProfileTypeId == fieldold.ProfileTypeId && field.ProfileTypeFieldId == fieldold.ProfileTypeFieldId)
                                        {
                                            field.Value = fieldold.Value;
                                            field.UserId = fieldold.UserId;
                                        }
                                    }
                                    else if (fieldold.AttributeTypeDefinitionId == 18)
                                    {
                                        field.UserId = fieldold.UserId;
                                    }
                                }

                            }

                            profileType.Fields = profileTypeFieldValues.ToList();
                        }
                    }));
                }

            }
            return obj;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<User> GetByAuthDataAsync(AuthData data)
        {
            await AuthenticateAndAuthorizeAsync();

            if (Settings.Default.RootAdmin() != null &&
                string.Compare(Settings.Default.RootAdmin().Username, data.GetData<string>(BuiltInFieldNames.Username)).Equals(0) &&
                string.Compare(Settings.Default.RootAdminPassword(), data.GetData<string>(BuiltInFieldNames.Password)).Equals(0))
            {
                return Settings.Default.RootAdmin();
            }

            IAuthUser user = await AuthenticationProviders.Registered[data.AuthenticationProviderName].LoginAsync(data);

            if(user == null)
            {
                return null;
            }

            User cmsUser = await GetByReferenceIdAndProviderAsync(user.ReferenceId, data.AuthenticationProviderName);

            if(cmsUser == null && !string.Compare(data.AuthenticationProviderName, BuiltInAuthenticationProvider.GetProviderName(), true, CultureInfo.InvariantCulture).Equals(0))
            {
                cmsUser = new User()
                {
                    Username = user.Username,
                    Password = AESCrypt.Encrypt(user.ReferenceId, user.ReferenceId),
                    AdministrationAllowed = true,
                    ProfileTypes = new ProfileTypeList(),
                    ReferenceId = user.ReferenceId,
                    AuthenticationProvider = data.AuthenticationProviderName
                };

                cmsUser = await this.Caller(Settings.Default.RootAdmin()).SaveAsync(cmsUser);
            }

            if (cmsUser == null)
            {
                return null;
            }

            foreach (MemberOf memberOf in user.MemberOf.Where(m => !cmsUser.ProfileTypes.Any(p => string.CompareOrdinal(m.CmsProfileId, p.Id.ToString(CultureInfo.InvariantCulture)).Equals(0))))
            {
                ProfileType profileTypeToSave = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(memberOf.CmsProfileId.ToInt64(default));
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(cmsUser, profileTypeToSave);
            }

            cmsUser.Token = user.AuthDataString;

            return cmsUser;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<User> GetByReferenceIdAndProviderAsync(string id, string authenticationProvider, bool isFull = true)
        {
            if (Settings.Default.RootAdmin() != null && !string.IsNullOrEmpty(Settings.Default.RootId()) && id.Equals(Settings.Default.RootId()))
            {
                return Settings.Default.RootAdmin();
            }

            await AuthenticateAndAuthorizeAsync();

            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.GetByReferenceIdAndProvider.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.ReferenceId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.AuthenticationProvider.GetIntValue()) { Value = authenticationProvider });
            User result = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), isFull);
            result.AuthenticationProvider = authenticationProvider;
            return result;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<User> GetByIdAsync(string id, bool useDefaultPlugin = false, bool isFull = true)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            if (Settings.Default.RootAdmin() != null && !string.IsNullOrEmpty(Settings.Default.RootId()) && id.Equals(Settings.Default.RootId()))
            {
                return Settings.Default.RootAdmin();
            }

            await AuthenticateAndAuthorizeAsync();

            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.GetById.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.UserId.GetIntValue()) { Value = id });
            User result = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), isFull);
            return result;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<List<User>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<User> users = new List<User>();

            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.GetAll.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                User user = await CreateAsync(row);
                if (!user.Id.Equals(default(long)))
                {
                    users.Add(user);
                }
            }
            return users;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<Entities.Base.BasePaginationEntity<User>> GetAllWithPaginationAsync(int currentPageIndex, int maxNumberOfRows, string searchTerm, string sort = "Username ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            List<User> users = new List<User>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.SelectAllWithPagination.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                User user = await CreateAsync(row, isFull: true);
                if (!user.Id.Equals(default(long)))
                {
                    users.Add(user);
                }
            }
            Entities.Base.BasePaginationEntity<User> basePaginationEntity = new Entities.Base.BasePaginationEntity<User>();
            basePaginationEntity.Items = users;
            if (table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntity;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<int> SelectAllCountAsync(string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.SelectAllCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("UserCount");
            return count;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        [Obsolete("Obsolete action, please switch to GetByAuthData", true)]
        public User GetByUsernameAndPassword(string username, string password, string authenticationProvider = null)
        {
            if (string.IsNullOrEmpty(authenticationProvider))
            {
                authenticationProvider = BuiltInAuthenticationProvider.GetProviderName();
            }

            AuthData data = new AuthData();
            data.Values.Add(BuiltInFieldNames.Username, username);
            data.Values.Add(BuiltInFieldNames.Password, password);
            data.AuthenticationProviderName = authenticationProvider;

            return GetByAuthDataAsync(data).Result;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public async Task<User> SaveAsync(User user)
        {
            await AuthenticateAndAuthorizeAsync();
            User newUser = null;
            ProfileType initialProfileType = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Username.GetIntValue()) { Value = user.Username });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Password.GetIntValue()) { Value = user.Password });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.AdministrationAllowed.GetIntValue()) { Value = user.AdministrationAllowed });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.ReferenceId.GetIntValue()) { Value = user.ReferenceId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.AuthenticationProvider.GetIntValue()) { Value = user.AuthenticationProvider });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.ClearCache = true;

                newUser = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                newUser.AuthenticationProvider = BuiltInAuthenticationProvider.GetProviderName();
                newUser.Password = user.Password;
                newUser.ReferenceId = newUser.Id;
                newUser.AdministrationAllowed = user.AdministrationAllowed;
                await UpdateAsync(newUser);

                //method.WaitForOnBeforeCompleted();
                if (!user.ProfileTypeId.Equals(default))
                {
                    initialProfileType = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(user.ProfileTypeId);
                    if (initialProfileType != null)
                    {
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(newUser, initialProfileType);
                    }
                }
                method.End();
                //method.WaitForOnAfterCompleted();

            }
            return newUser;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public async Task<User> UpdateAuthData(string id, AuthData authData)
        {
            await AuthenticateAndAuthorizeAsync();
            User user = await GetByIdAsync(id);
            user.Password = authData.GetData<string>(BuiltInFieldNames.Password, null);
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Username.GetIntValue()) { Value = user.Username });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Password.GetIntValue()) { Value = user.Password });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.AdministrationAllowed.GetIntValue()) { Value = user.AdministrationAllowed });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.ReferenceId.GetIntValue()) { Value = user.ReferenceId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.AuthenticationProvider.GetIntValue()) { Value = user.AuthenticationProvider });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.ClearCache = true;

                user = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

            }
            return user;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public async Task<User> UpdateAsync(User user)
        {
            await AuthenticateAndAuthorizeAsync();
            User newUser = null;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.Update.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Username.GetIntValue()) { Value = user.Username });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Password.GetIntValue()) { Value = user.Password });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.UserId.GetIntValue()) { Value = user.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.AdministrationAllowed.GetIntValue()) { Value = user.AdministrationAllowed });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.ReferenceId.GetIntValue()) { Value = user.ReferenceId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.AuthenticationProvider.GetIntValue()) { Value = user.AuthenticationProvider });
                method.ClearCache = true;

                newUser = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                //method.WaitForOnBeforeCompleted();
                /*foreach (ProfileType profileType in user.ProfileTypes)
                {
                    foreach (ProfileTypeFieldValue field in profileType.Fields)
                    {
                        field.UserId = user.Id;
                    }
                    await SaveOrUpdateFieldValueAsync(profileType);
                }*/
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return newUser;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public async Task<User> UpdateTokenAsync(User user)
        {
            await AuthenticateAndAuthorizeAsync((attrs) => {
                return UserMakingTheCall != null && string.CompareOrdinal(UserMakingTheCall.Id, user.Id).Equals(0);
            });
            User newUser = null;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.UpdateToken.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Token.GetIntValue()) { Value = user.Token });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.DateRefreshToken.GetIntValue()) { Value = user.DateRefreshToken });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.UserId.GetIntValue()) { Value = user.Id });
                method.ClearCache = true;

                newUser = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
            }
            return newUser;
        }

        private async Task SaveOrUpdateFieldValueAsync(ProfileType profileType)
        {
            if (profileType.Fields != null && profileType.Fields.Any())
            {
                foreach (ProfileTypeFieldValue fieldValue in profileType.Fields)
                {
                    ProfileTypeFieldValue checkFieldValue = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByPrimaryKeysAsync(fieldValue.ProfileTypeFieldId, fieldValue.UserId, fieldValue.ProfileTypeId);
                    if (fieldValue.Value == null)
                    {
                        fieldValue.Value = " ";
                    }
                    if (checkFieldValue == null)
                    {
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(fieldValue);
                    }
                    else
                    {
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).UpdateAsync(fieldValue);
                    }
                }
            }
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Delete)]
        public async Task<bool> DeleteAsync(User obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.Delete.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.UserId.GetIntValue()) { Value = obj.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                if (success)
                    obj = null;
                method.End();
                //method.WaitForOnAfterCompleted();
            }

            return success;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<int> GetUsersByProfileTypeCountAsync(ProfileType profileType)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.GetUsersByProfileTypeCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.ProfileTypeId.GetIntValue()) { Value = profileType.Id });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("UserCount");
            return count;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<User> GetIdByUserNameAsync(string username)
        {
            await AuthenticateAndAuthorizeAsync();

            if (Settings.Default.RootAdmin() != null &&
                string.Compare(Settings.Default.RootAdmin().Username, username).Equals(0))
            {
                return Settings.Default.RootAdmin();
            }

            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.GetIdByUserName.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Username.GetIntValue()) { Value = username });

            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public async Task<User> UpdateUserAsync(string idUser, string token1, string tokenDate)
        {
            await AuthenticateAndAuthorizeAsync();
            User userUpdate = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.UpdateUser.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.UserId.GetIntValue()) { Value = idUser });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Token.GetIntValue()) { Value = token1 });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.DateRefreshToken.GetIntValue()) { Value = tokenDate });
                method.ClearCache = true;

                userUpdate = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

                method.End();
                //method.WaitForOnAfterCompleted();
            }

            return userUpdate;

        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<User> GetIdByTokenAsync(string token)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.GetIdByToken.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Token.GetIntValue()) { Value = token });

            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public async Task<User> UpdateUserByTokenAsync(User user)
        {
            await AuthenticateAndAuthorizeAsync();
            User userId = await GetIdByTokenAsync(user.Token);
            string userID = userId.Id;
            User userUpdate = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.UpdateUserByToken.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.UserId.GetIntValue()) { Value = userID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Token.GetIntValue()) { Value = user.Token });
                //method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.Password.GetIntValue()) { Value = user.Password });
                method.ClearCache = true;

                userUpdate = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }

            return userUpdate;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        public async Task<List<User>> SearchAsync(string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();
            List<User> searchResults = new List<User>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.Search.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.User.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                searchResults.Add(await CreateAsync(row));
            }
            return searchResults;
        }

        [EntityPermission(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        public async Task<User> ResetPasswordAsync(User user)
        {
            if (user.Id.ToInt64(default).Equals(default(long)))
            {
                user = await this.Caller(Settings.Default.RootAdmin()).GetIdByUserNameAsync(user.Username);
            }

            string token1 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            byte[] data = Convert.FromBase64String(token);


            string token2 = Sha3Crypt.Encrypt(token1);


            /*MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.ASCII.GetBytes(token1);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in encodedBytes)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string token2 = s.ToString();*/

            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            string datumTokena = when.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(Settings.Default.RootAdmin()).UpdateUserAsync(user.Id, token2, datumTokena);
        }

        public async Task<int> GetCountAsync()
        {
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods.GetCount.GetIntValue();
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            int count = default;
            foreach (DataRow row in results.Rows)
            {
                count = row.GetValue<int>("TotalCount");
            }
            return count;
        }
    }
}
