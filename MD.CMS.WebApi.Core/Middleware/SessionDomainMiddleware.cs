using MD.CMS.BusinessLogic.Core.DataAccess.Controllers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Middleware
{
    public class SessionDomainMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionDomainMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            SessionController.SessionDomain = context.Request.Host.ToString();
            await _next.Invoke(context);
        }
    }
}
