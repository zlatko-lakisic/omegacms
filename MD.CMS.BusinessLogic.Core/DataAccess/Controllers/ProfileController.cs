using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ProfileController : BaseController<ProfileController>
    {
        public async Task<bool> SaveAsync(User user, ProfileType profileType)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Profile;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Profile.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Profile.Parameters.UserId.GetIntValue()) { Value = user.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Profile.Parameters.ProfileTypeId.GetIntValue()) { Value = profileType.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<bool> DeleteAsync(User user, ProfileType profileType)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {

                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Profile;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Profile.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Profile.Parameters.UserId.GetIntValue()) { Value = user.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Profile.Parameters.ProfileTypeId.GetIntValue()) { Value = profileType.Id });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }
        public async Task<bool> DeleteAllAsync(string id)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {

                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Profile;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Profile.Methods.DeleteAll.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Profile.Parameters.UserId.GetIntValue()) { Value = id });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }
    }
}
