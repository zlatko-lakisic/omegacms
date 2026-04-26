using MD.CMS.BusinessLogic.WebApi.Core.Addons;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Licensing;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.BusinessLogic
{
    public class LicenseMiddleware
    {
        private readonly RequestDelegate next;
        public LicenseMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!(
                        context.Connection.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                        context.Connection.RemoteIpAddress.ToString() == "127.0.0.1"
                    ) && !(
                        context.Connection.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 &&
                        (context.Connection.RemoteIpAddress.IsIPv6LinkLocal || context.Connection.RemoteIpAddress.IsIPv6SiteLocal)
                    ) && !Tools.Licensing.LicenseValidate.ValidateLicense(Tools.Licensing.License.ReadLicenseFile(Directory.GetCurrentDirectory()),
                        Tools.Licensing.ServerKey.ReadServerKeyFile(Directory.GetCurrentDirectory()),
                        Tools.Licensing.ComponentEnum.WebApi,
                        WebApiAddonAppBuilder.Default.WebApiSystemVersion,
                        Tools.Licensing.ClientKey.ReadClientKeyFile(Directory.GetCurrentDirectory()),
                        1,
                        context.Request.Host.Value))
                {
                    throw new LicensingException(Tools.Licensing.LicensingException.LicensingExceptionErrorType.LicenseInvalid);
                }

                await next(context);
            }
            catch (LicensingException ex)
            {
                if (Logger.IsAvailable)
                {
                    typeof(LicenseMiddleware).Log(ex);
                } 
                else
                {
                    Console.WriteLine("A licensing error occured! Please check your license and restart this application!");
                }
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.PaymentRequired;
                await context.Response.WriteAsync(new ErrorDetails(new Exception("A licensing error occured! Please check your license and restart this application!"), context.Response.StatusCode).ToString()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
