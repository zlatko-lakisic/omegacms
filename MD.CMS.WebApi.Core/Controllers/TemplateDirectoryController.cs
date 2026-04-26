using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using Microsoft.AspNetCore.Hosting;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;

namespace MD.CMS.WebApi.Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content")]
    public class TemplateDirectoryController : BaseLoggedOnWebApiController
    {
        private IWebHostEnvironment _env;
        public TemplateDirectoryController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("GetTemplateDirectoryByPath")]
        public IActionResult GetTemplateDirectoryByPath([FromBody]Template template)
        {
            string webRoot = _env.WebRootPath;
            string path = template.TemplateUrl;
            string rootPath = Path.GetFullPath(Path.Combine(webRoot, @"..\"));
            string fullPathToDirectory = rootPath + "\\" + path;


            TemplateDirectory templateDirectory = new TemplateDirectory() { Path = path};
            templateDirectory.Children = new List<TemplateDirectory>();
            templateDirectory.Files = new List<TemplateFile>();

            string [] directoryFullPaths = Directory.GetDirectories(fullPathToDirectory);
            string[] fileFullPaths = Directory.GetFiles(fullPathToDirectory, "*.html");

            for (int i = 0; i < directoryFullPaths.Length; i++)
			{
                TemplateDirectory child = new TemplateDirectory(){
                    Path = MakeShortPath(directoryFullPaths[i], rootPath),
                    Name = MakeDirectoryName(directoryFullPaths[i])
                };
                templateDirectory.Children.Add(child);
			}

            for (int i = 0; i < fileFullPaths.Length; i++)
            {
                TemplateFile file = new TemplateFile()
                {
                    Path = MakeShortPath(fileFullPaths[i], rootPath),
                    Name = MakeDirectoryName(fileFullPaths[i])
                };
                templateDirectory.Files.Add(file);
            }
     
            return Ok(templateDirectory);
        }


        private string MakeShortPath(string fullPath, string rootPath)
        {
            int index = rootPath.Length + 1;
            return fullPath.Substring(index);
        }

        private string MakeDirectoryName(string fullPath)
        {
            string[] pathParts = fullPath.Split('\\');
            return pathParts[pathParts.Length - 1];
        }


    }
}