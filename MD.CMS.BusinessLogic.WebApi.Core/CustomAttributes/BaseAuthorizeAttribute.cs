using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes
{
    public abstract class BaseAuthorizeAttribute : BaseActionAttribute, IAuthorizationFilter
    {
        #region Methods
        public BaseAuthorizeAttribute() : base()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            OnAuthorize(context);
        }

        public virtual void OnAuthorize(AuthorizationFilterContext context)
        {
            //Do Nothing
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await OnAuthorizeAsync(context);
        }

        public virtual async Task OnAuthorizeAsync(AuthorizationFilterContext context)
        {
            //Do Nothing
        }
        #endregion
    }
}