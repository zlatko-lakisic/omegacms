using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System.Linq;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ProfileTypeController : BaseController<ProfileTypeController>
    {
        public async Task<ProfileType> CreateAsync(DataRow row, bool transformExpression = true)
        {
            ProfileType obj = base.Create<ProfileType, long>(row, "ProfileTypeId");
            if (obj != null)
            {
                obj.Name = row.GetValue<string>("Name");
                obj.Icon = row.GetValue<string>("Icon");

                List<ProfileTypeFieldValue> fieldValues = new List<ProfileTypeFieldValue>();

                List<ProfileTypeField> fieldsByProfileType = await ProfileTypeFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByProfileTypeAsync(obj, transformExpression: transformExpression);
                if (fieldsByProfileType != null && fieldsByProfileType.Any())
                {
                    foreach (ProfileTypeField field in fieldsByProfileType)
                    {
                        ProfileTypeFieldValue fieldValue = new ProfileTypeFieldValue(field);
                        fieldValues.Add(fieldValue);
                    }
                }
                obj.Fields = fieldValues;
            }
            return obj;
        }

        public async Task<ProfileType> GetByIdAsync(long id, bool transformExpression = true)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.Id.GetIntValue()) { Value = id });
            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), transformExpression: transformExpression);
        }

        public async Task<List<ProfileType>> GetAllWithPaginationAsync(long pageIndex, long pageSize, string searchTerm, string sort = "Name ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            List<ProfileType> list = new List<ProfileType>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.GetAllWithPagination.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = pageSize });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = pageIndex });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                list.Add(await CreateAsync(row));
            }
            return list;
        }

        public async Task<long> GetAllCountAsync(string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ProfileType> list = new List<ProfileType>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.GetAllCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            long count = row.GetValue<long>("ProfileTypesCount");
            return count;
        }

        public async Task<List<ProfileType>> GetAllAsync(string sort = "Name ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            List<ProfileType> list = new List<ProfileType>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.GetAll.GetIntValue();
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                list.Add(await CreateAsync(row));
            }
            return list;
        }

        public async Task<List<ProfileType>> GetByUserAsync(User user)
        {
            return await GetByUserIdAsync(user.Id);
        }

        public async Task<List<ProfileType>> GetByUserIdAsync(string id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ProfileType> profileTypes = new List<ProfileType>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.GetByUser.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.UserId.GetIntValue()) { Value = id });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            foreach (DataRow row in results.Rows)
            {
                profileTypes.Add(await CreateAsync(row));
            }

            return profileTypes;
        }

        public async Task<int> GetByUserCountAsync(string userId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.GetByUserCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.UserId.GetIntValue()) { Value = userId });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("ProfileTypesByUserCount");
            return count;
        }

        public async Task<List<ProfileType>> GetNotBelongingProfileTypesByUserAsync(User user)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ProfileType> profileTypes = new List<ProfileType>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.GetNotBelongingProfileTypesByUser.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.UserId.GetIntValue()) { Value = user.Id });
            //This need to go in mysql
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                profileTypes.Add(await CreateAsync(row));
            }
            return profileTypes;
        }

        public async Task<ProfileType> SaveAsync(ProfileType profileType)
        {
            await AuthenticateAndAuthorizeAsync();
            ProfileType newProfileType = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.Name.GetIntValue()) { Value = profileType.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.Icon.GetIntValue()) { Value = profileType.Icon });

                if (profileType.IsNew)
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.Insert.GetIntValue();
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.Id.GetIntValue()) { Value = profileType.Id });
                }
                method.ClearCache = true;

                newProfileType = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

                List<ProfileTypeField> oldFields = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().Caller(UserMakingTheCall).GetByProfileTypeAsync(profileType);
                List<ProfileTypeField> newFields = new List<ProfileTypeField>();

                if (profileType.Fields != null)
                {
                    foreach (ProfileTypeField field in profileType.Fields.Select(pt => pt.ToProfileTypeField()))
                    {
                        field.ProfileTypeId = newProfileType.Id;
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().Caller(UserMakingTheCall).SaveAsync(field);
                        newFields.Add(field);
                    }
                }

                if (!profileType.IsNew)
                {
                    if (newFields != null)
                    {
                        foreach (ProfileTypeField oldField in oldFields)
                        {
                            var found = false;
                            foreach (ProfileTypeField newField in newFields)
                            {
                                if (oldField.Id == newField.Id)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().Caller(UserMakingTheCall).DeleteAsync(oldField);
                            }
                        }
                    }
                }
                method.End();
                //method.WaitForOnAfterCompleted();


            }
            return newProfileType;
        }

        public async Task<bool> DeleteAsync(ProfileType obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.Id.GetIntValue()) { Value = obj.Id });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                //method.WaitForOnBeforeCompleted();
                if (success)
                {
                    success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().Caller(UserMakingTheCall).DeleteAllByProfileTypeIdAsync(obj.Id);
                    if (!success)
                    {
                        return false;
                    }
                    return success;
                }
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<List<ProfileType>> SearchAsync(string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ProfileType> searchResults = new List<ProfileType>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods.Search.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                searchResults.Add(await CreateAsync(row));
            }
            return searchResults;

        }
    }
}
