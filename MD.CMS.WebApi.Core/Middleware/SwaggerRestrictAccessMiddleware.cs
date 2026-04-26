using MD.CMS.BusinessLogic.WebApi.Core.Exceptions;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.Session;
using MD.CMS.WebApi.Core.Filters.Swagger;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Middleware
{
    public class SwaggerRestrictAccessMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerRestrictAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.ToLowerInvariant().StartsWith(string.Format("{0}{1}", PathString.FromUriComponent("/"), "swagger")))
            {
                string authenticationCode = context.Request.Query.GetValue(MD.CMS.BusinessLogic.WebApi.Core.Properties.Settings.Default.AuthenticateHeaderName);
                bool authorized = false;
                if (!string.IsNullOrEmpty(authenticationCode))
                {
                    authorized = await SessionTable.UserAuthenticatedAsync(authenticationCode);
                } 
                else
                {
                    throw new MdCmsWebApiAuthenticationException(context.Connection.RemoteIpAddress.ToString(), string.Empty);
                }

                if (!authorized)
                {
                    throw new MdCmsWebApiAuthorizationException(context.Connection.RemoteIpAddress.ToString(), string.Format("{0}://{1}{2}{3}", context.Request.Scheme, context.Request.Host, context.Request.Path, context.Request.QueryString));
                } 
                else
                {
                    SwaggerAuthorizationFilter.Token = authenticationCode;
                    await _next.Invoke(context);
                }
            } 
            else
            {
                await _next.Invoke(context);
            }

        }

        void context_BeginRequest(HttpContext context)
        {
        }
    }
}
