using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Template")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content")]
    public class TemplateController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Template GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            Template template = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (template == null)
                return NotFound();

            return Ok(template);
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Template GetAll")]
        public async Task<IActionResult> GetAll(string id = null)
        {
            string sort = id;
            if (string.IsNullOrEmpty(sort))
            {
                sort = "Name ASC";
            }
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(sort));
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAllCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Template GetAllCount")]
        public async Task<IActionResult> GetAllCount([FromQuery] string searchTerm, [FromQuery] string searchColumn)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            searchColumn = System.Web.HttpUtility.UrlDecode(searchColumn);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllCountAsync(searchTerm, searchColumn));
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAllWithPagination")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Template GetAllWithPagination")]
        public async Task<IActionResult> GetAllWithPagination([FromQuery] string sort, [FromQuery] long pageIndex, [FromQuery] long pageSize, [FromQuery] string searchTerm, [FromQuery] string searchColumn)
        {
            if (string.IsNullOrEmpty(sort))
            {
                sort = "Name ASC";
            }
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            searchColumn = System.Web.HttpUtility.UrlDecode(searchColumn);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllWithPaginationAsync(sort, pageIndex, pageSize, searchTerm, searchColumn));
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetByContent")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Template GetByContent")]
        public async Task<IActionResult> GetByContent(Content content)
        {
            Template template = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByContentAsync(content);
            if (template == null)
                return NotFound();

            return Ok(template);
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetByFolder")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Template GetByFolder")]
        public async Task<IActionResult> GetByFolder(long id)
        {
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (folder == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderAsync(folder));
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("GetByFolder")]
        [OmegaInvalidateCache("Search")]
        public async Task<IActionResult> Post([FromBody]Template template)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (template.IsNew)
            {
                template = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(template);
            }
            else
            {
                template = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateAsync(template);
            }

            return Ok(template);
        }



        [HttpDelete]
        [Route("[action]/{id?}")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("Search")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("GetByFolder")]
        public async Task<IActionResult> Delete(long id)
        {
            Template template = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (template == null)
                throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("Template {0} does not exist ", template.Name));
            else
            {
                bool success = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(template);
                if (!success)
                {
                    throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("Template {0} is not deleted", template.Name));
                }
            }
            return Ok("Deleted");
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("Search")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Template Search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm, [FromQuery] string searchColumn)
        {
            List<Template> searchResults = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm, searchColumn);
            return Ok(searchResults);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetTemplateScreenshot([FromBody]TemplateScreenshot request)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");
            ChromeDriver driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);
            driver.Manage().Window.Size = new System.Drawing.Size(request.ScreenshotWidth, request.ScreenshotHeight);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(webDriver => ((IJavaScriptExecutor)webDriver).ExecuteScript("return document.readyState").Equals("complete"));

            driver.Navigate().GoToUrl(request.ScreenshotUrl);
            Screenshot screenshot = (driver as ITakesScreenshot).GetScreenshot();
            string randomFileName = string.Format("{0}.png", MD.Tools.Helpers.Core.Crypto.MD5Crypt.MD5Encrypt(request.ScreenshotUrl));
            string desiredFileLocation = string.Format("{0}\\{1}\\{2}", CMS.BusinessLogic.Core.Properties.Settings.Default.FileUploadPath, "screenshots", randomFileName);
            bool success = await Tools.Helpers.Core.FileProvider.DynamicFileProvider.Default.WriteFile(new Tools.Helpers.Core.FileProvider.FileProviderFile()
            {
                FileBytes = screenshot.AsByteArray,
                FilePath = desiredFileLocation
            });
            driver.Close();
            driver.Quit();

            request.ScreenshotFile = randomFileName;

            return Ok(request);
        }
    }
}
