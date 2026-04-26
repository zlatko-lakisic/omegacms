using MD.CMS.BusinessLogic.WebApi.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MD.CMS.BusinessLogic.WebApi.Core.BaseControllers
{
    public abstract class BaseController : ControllerBase
    {
        public override NotFoundResult NotFound()
        {
            throw new MdCmsWebApiNotFoundException(HttpContext.Connection.RemoteIpAddress.ToString(), string.Format("{0}://{1}{2}{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path, HttpContext.Request.QueryString));
        }

        public override UnauthorizedResult Unauthorized()
        {
            throw new MdCmsWebApiAuthenticationException(HttpContext.Connection.RemoteIpAddress.ToString());
        }

        public override ForbidResult Forbid()
        {
            throw new MdCmsWebApiAuthorizationException(HttpContext.Connection.RemoteIpAddress.ToString(), string.Format("{0}://{1}{2}{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path, HttpContext.Request.QueryString));
        }
    }
}
