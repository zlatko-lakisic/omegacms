using System;
using System.IO;
using MD.CMS.Administration.Core.Models;
using MD.CMS.BusinessLogic.Administration.Core.Addons;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Licensing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MD.CMS.Administration.Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                if (Startup.GetCheckLicense() &&
                    !(
                        HttpContext.Connection.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                        HttpContext.Connection.RemoteIpAddress.ToString() == "127.0.0.1"
                    ) && !(
                        HttpContext.Connection.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 &&
                        (HttpContext.Connection.RemoteIpAddress.IsIPv6LinkLocal || HttpContext.Connection.RemoteIpAddress.IsIPv6SiteLocal)
                    ) && !Tools.Licensing.LicenseValidate.ValidateLicense(Tools.Licensing.License.ReadLicenseFile(Directory.GetCurrentDirectory()),
                        Tools.Licensing.ServerKey.ReadServerKeyFile(Directory.GetCurrentDirectory()),
                        Tools.Licensing.ComponentEnum.Administration,
                        AdminAddonAppBuilder.Default.AdminSystemVersion,
                        Tools.Licensing.ClientKey.ReadClientKeyFile(Directory.GetCurrentDirectory()),
                        1,
                        HttpContext.Request.Host.Value))
                {
                    throw new Tools.Licensing.LicensingException(Tools.Licensing.LicensingException.LicensingExceptionErrorType.LicenseInvalid);
                }

                HttpContext current = HttpContext;
                return View(new Layout(HttpContext));
            }
            catch (LicensingException e)
            {
                if (Logger.IsAvailable)
                {
                    typeof(HomeController).Log(e);
                }
                else
                {
                    Console.WriteLine("A licensing error occured! Please check your license and restart this application!");
                }
                return RedirectToAction("Index", "Error402");
            }
            catch (Exception e)
            {
                if (Logger.IsAvailable)
                {
                    typeof(HomeController).Log(e);
                }
                else
                {
                    Console.WriteLine($"A general error occured while processing this request, the error message is '{e.Message}' and the stack trace is '{e.StackTrace}'");
                }
                return RedirectToAction("Index", "Error500");
            }
        }
    }
}
