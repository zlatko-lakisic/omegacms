using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.WebApi.Core.Properties;

namespace MD.CMS.WebApi.Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GoogleController : BaseLoggedOnWebApiController
    {
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("Translate")]
        public IActionResult Translate(string text, string source, string target="en")
        {
            // example URL: www.googleapis.com/language/translate/v2?key=YOUR_API_KEY&source=en&target=de&q=Hello%20world

            string protocol = "https";
            string server = "www.googleapis.com";
            string service = "language";
            string action = "translate";
            string version = "v2";
            string key = Settings.Default.GoogleApiKey;
            string q =  HttpUtility.UrlEncode(text);

            string response = "";
            
            string URL = string.Format("{0}://{1}/{2}/{3}/{4}?key={5}&source={6}&target={7}&q={8}", protocol, server, service, action, version, key, source, target, q);

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers["Content-Type"] = "application/json;charset=UTF-8";
                    response = wc.DownloadString(URL);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    HttpWebResponse resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        //Handle it
                        return NotFound();
                    }
                }
                //Handle it
                return BadRequest();
            }


            if (response != "")
            {
                return Ok(response); 
            }
            else
            {
                return BadRequest();
            }
            
        }

    }
}