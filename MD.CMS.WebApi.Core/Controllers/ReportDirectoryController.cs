using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using Microsoft.AspNetCore.Hosting;
using MD.CMS.WebApi.Core.Properties;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;

namespace MD.CMS.WebApi.Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Reporting")]
    public class ReportDirectoryController : BaseLoggedOnWebApiController
    {
        private IWebHostEnvironment _env;
        public ReportDirectoryController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("GetReportDirectoryByPath")]  
        public IActionResult GetReportDirectoryByPath([FromBody]GenericJsonSingleObject<string> objWithPath)
        {
            string webRoot = _env.WebRootPath;
            string path = objWithPath.ValueName;
            string rootPath = Path.GetFullPath(Path.Combine(webRoot, Settings.Default.TemplateDirectoryRoot));
            string fullPathToDirectory = rootPath + path;


            ReportDirectory reportDirectory = new ReportDirectory() { Path = path };
            reportDirectory.Children = new List<ReportDirectory>();

            if (!Directory.Exists(fullPathToDirectory))
            {
                fullPathToDirectory = rootPath;
                reportDirectory.Path = "";
            }
            string[] directoryFullPaths = Directory.GetDirectories(fullPathToDirectory);         

            for (int i = 0; i < directoryFullPaths.Length; i++)
            {
                ReportDirectory child = new ReportDirectory()
                {
                    Path = MakeShortPath(directoryFullPaths[i], rootPath),
                    Name = MakeDirectoryName(directoryFullPaths[i])
                };
                reportDirectory.Children.Add(child);
            }
            reportDirectory.Path = path;
            return Ok(reportDirectory);
        }


        private string MakeShortPath(string fullPath, string rootPath)
        {
            int index = rootPath.Length;
            return fullPath.Substring(index).TrimStart('\\');
        }

        private string MakeDirectoryName(string fullPath)
        {
            string[] pathParts = fullPath.Split('\\');
            return pathParts[pathParts.Length - 1];
        }


    }
}