using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ProfileTypeFieldValueController : BaseController<ProfileTypeFieldValueController>
    {
        public async Task<ProfileTypeFieldValue> CreateAsync(DataRow row)
        {
            ProfileTypeFieldValue obj = null;
            if (row != null)
            {
                obj = new ProfileTypeFieldValue(await ProfileTypeFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(row.GetValue<long>("ProfileTypeFieldId")));
                obj.Id = row.GetValue<long>("ProfileTypeFieldId");
                obj.ProfileTypeFieldId = row.GetValue<long>("ProfileTypeFieldId");
                obj.ProfileTypeId = row.GetValue<long>("ProfileTypeId");
                obj.UserId = row.GetValue<string>("UserId");
                obj.Name = row.GetValue<string>("Name");
                obj.Value = row.GetValue<string>("Value");
            }
            return obj;
        }

        public async Task<ProfileTypeFieldValue> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Id.GetIntValue()) { Value = id });
            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }


        public async Task<ProfileTypeFieldValue> GetByPrimaryKeysAsync(long profileTypeFieldId, string userId, long profileTypeId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeFieldValue;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Methods.GetByPrimaryKeys.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.ProfileTypeFieldId.GetIntValue()) { Value = profileTypeFieldId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.UserId.GetIntValue()) { Value = userId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.ProfileTypeId.GetIntValue()) { Value = profileTypeId });

            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }


        public async Task<List<ProfileTypeFieldValue>> GetByUserAsync(User obj)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ProfileTypeFieldValue> list = new List<ProfileTypeFieldValue>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeFieldValue;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Methods.GetByUser.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.UserId.GetIntValue()) { Value = obj.Id });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in result.Rows)
            {
                list.Add(await CreateAsync(row));
            }
            return list;
        }

        public async Task<List<ProfileTypeFieldValue>> GetByUserAndProfileTypeAsync(User user, ProfileType profileType)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ProfileTypeFieldValue> list = new List<ProfileTypeFieldValue>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeFieldValue;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Methods.GetByUserAndProfileType.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.UserId.GetIntValue()) { Value = user.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.ProfileTypeId.GetIntValue()) { Value = profileType.Id });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in result.Rows)
            {
                list.Add(await CreateAsync(row));
            }
            return list;
        }

        public async Task<ProfileTypeFieldValue> SaveAsync(ProfileTypeFieldValue obj)
        {
            await AuthenticateAndAuthorizeAsync();
            ProfileTypeFieldValue profiletypeField = null;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeFieldValue;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.ProfileTypeFieldId.GetIntValue()) { Value = obj.ProfileTypeFieldId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.UserId.GetIntValue()) { Value = obj.UserId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.ProfileTypeId.GetIntValue()) { Value = obj.ProfileTypeId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.Value.GetIntValue()) { Value = obj.Value });

                method.ClearCache = true;

                profiletypeField = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return profiletypeField;
        }

        public async Task<ProfileTypeFieldValue> UpdateAsync(ProfileTypeFieldValue obj)
        {
            await AuthenticateAndAuthorizeAsync();
            ProfileTypeFieldValue profiletypeField = null;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeFieldValue;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Methods.Update.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.ProfileTypeFieldId.GetIntValue()) { Value = obj.ProfileTypeFieldId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.UserId.GetIntValue()) { Value = obj.UserId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.ProfileTypeId.GetIntValue()) { Value = obj.ProfileTypeId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Parameters.Value.GetIntValue()) { Value = obj.Value });

                method.ClearCache = true;

                profiletypeField = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return profiletypeField;
        }
    }
}
