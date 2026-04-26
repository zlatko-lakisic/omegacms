using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;

namespace MD.CMS.BusinessLogic.Core.Helpers.Extensions
{
    public static class ControllerExtensionMethods
    {
        public static T DefaultPlugin<T>(this T controller, bool useDefaultPlugin)
            where T : BaseController<T>, new()
        {
            controller.UseDefaultPlugin = useDefaultPlugin;
            return controller;
        }
        public static T Caller<T>(this T controller, User user)
            where T : BaseController<T>, new()
        {
            controller.UserMakingTheCall = user;

            return controller;
        }
        public static T Caller<T>(this T controller, string userId)
            where T : BaseController<T>, new()
        {
            if(userId == Properties.Settings.Default.RootId())
            {
                controller.UserMakingTheCall = Properties.Settings.Default.RootAdmin();
            }

            return controller;
        }
    }
}
