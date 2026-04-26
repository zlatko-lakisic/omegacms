using MD.CMS.BusinessLogic.WebApi.Core.Exceptions;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using MD.Tools.BaseDataAccess.Plugins.Core.Helpers.Exceptions;
using MD.Tools.Helpers.Core.Exceptions;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Licensing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.BusinessLogic
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            try
            {
                await next(context);
            }
            catch (LicensingException ex)
            {
                MD.Tools.Helpers.Core.Logging.Logger.LogWarning(ex.Message, ex);
                await HandleExceptionAsync(context, ex, HttpStatusCode.PaymentRequired);
            }
            catch (MdCmsWebApiAuthenticationException ex)
            {
                MD.Tools.Helpers.Core.Logging.Logger.LogWarning(ex.Message, ex);
                await HandleExceptionAsync(context, ex, ex.StatusCode);
            }
            catch (MdCmsWebApiAuthorizationException ex)
            {
                MD.Tools.Helpers.Core.Logging.Logger.LogWarning(ex.Message, ex);
                await HandleExceptionAsync(context, ex, ex.StatusCode);
            }
            catch (MdCmsWebApiNotFoundException ex)
            {
                MD.Tools.Helpers.Core.Logging.Logger.LogWarning(ex.Message, ex);
                await HandleExceptionAsync(context, ex, ex.StatusCode);
            }
            catch (MDEntityUnauthorizedException ex)
            {
                MD.Tools.Helpers.Core.Logging.Logger.LogWarning(ex.Message, ex);
                await HandleExceptionAsync(context, ex, HttpStatusCode.Forbidden);
            }
            catch (BaseDataAccessPluginException ex)
            {
                MD.Tools.Helpers.Core.Logging.Logger.LogWarning(ex.Message, ex);
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                MD.Tools.Helpers.Core.Logging.Logger.Log(ex);
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync<T>(HttpContext context, T exception, HttpStatusCode resultCode)
            where T : Exception
        {
            typeof(ErrorHandlingMiddleware).Log(exception);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)resultCode;
            return context.Response.WriteAsync(new ErrorDetails(exception, (int)resultCode).ToString());
        }
    }
}
