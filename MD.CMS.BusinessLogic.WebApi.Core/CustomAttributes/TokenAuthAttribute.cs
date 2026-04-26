using MD.CMS.BusinessLogic.WebApi.Core.Session;
using Microsoft.AspNetCore.Mvc.Filters;
using MD.CMS.BusinessLogic.WebApi.Core.Exceptions;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes
{
    public class TokenAuthAttribute : BaseAuthorizeAttribute
    {
        public override async Task OnAuthorizeAsync(AuthorizationFilterContext context)
        {
            string authenticationCode = context.HttpContext.Request.Headers.GetValue(Properties.Settings.Default.AuthenticateHeaderName);
            bool authorized = false;
            if (!string.IsNullOrEmpty(authenticationCode))
            {
                authorized = await SessionTable.UserAuthenticatedAsync(authenticationCode);
            }

            if (!authorized)
            {
                throw new MdCmsWebApiAuthenticationException(context.HttpContext.Connection.RemoteIpAddress.ToString(), string.Empty);
            }
        }
    }
}