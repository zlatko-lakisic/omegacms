using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using MD.Tools.Helpers.Core.FileProvider;
using MD.Tools.Helpers.Core.Extensions.Stream;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;

namespace MD.CMS.WebApi.Core.Controllers
{
    [TokenAuth]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content")]
    public class UploadController : BaseLoggedOnWebApiController
    {
        private readonly string savePath;
        public const int ImageMinimumBytes = 512;
        string[] imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".ai", ".bmp", ".ico", ".ps", ".psd", ".svg", ".tif", ".tiff" };
        string[] videoExtensions = new string[] { ".mp4", ".3g2", ".3gp", ".avi", ".flv", ".h264", ".m4v", "mkv", ".mov", ".mpg", ".rm", ".swf", ".vob", "wmv" };
        string[] audioExtensions = new string[] { ".mp3", ".aif", ".mid", ".midi", ".mpa", ".wav", ".wma", ".m4a" };
        string[] documentExtensions = new string[] { ".txt", ".doc", ".docx", ".pdf", ".odt", ".rtf", ".tex", ".wpd", ".wks", ".csv", ".xlsx", ".xls" };

        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public UploadController()
        {
            savePath = Properties.Settings.Default.FileUpload;
        }

        private static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

        private string GetExtensionFromMimeType(string mimeType)
        {
            string[] mimeParts = mimeType.Split('/');
            return mimeParts[1];
        }

        private bool IsAllowedToUpload(string extension, string fileType)
        {
            switch (fileType)
            {
                case "1":
                    return imageExtensions.Contains(extension);
                case "2":
                    return videoExtensions.Contains(extension);
                case "3":
                    return audioExtensions.Contains(extension);
                case "4":
                    return documentExtensions.Contains(extension);
                default:
                    return false;
            }
        }

        private string UploadToYoutube(string videoPath, MediaContent mediaContent)
        {
            /*VideosSnippet snippet = new VideosSnippet();
            snippet.Title = mediaContent.Name;
            snippet.Description = mediaContent.Description;

            VideosStatus status = new VideosStatus();
            status.privacyStatus = "public";

            RefreshAccessTokenResponse accessToken = YouTubeManager.Channel.GetAccessToken("1/tTd8l6vB9kKBDnrsSQ820AlBBZXmLY-xRv6C3ZjKcHo", YoutubeApi.Properties.Settings.Default.ClientId, YoutubeApi.Properties.Settings.Default.ClientSecret);
            UpdateVideosResponse response = YouTubeManager.Channel.UploadVideo(snippet, accessToken.AccessToken.Value, videoPath, status);
            return response.Video.Id;
            */
            return string.Empty;
        }

        public class FileUploadModel
        {
            public string path { get; set; }
            public string fileType { get; set; }
            public string mediaContentName { get; set; }
            public string mediaContentDescription { get; set; }
            public IFormFile file { get; set; }
        }

        [HttpPost]
        [Route("[action]")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent, PermissionAccessTypeEnum.Write)]
        public async Task<IActionResult> PostFormData([FromForm]FileUploadModel upload)
        {
            try
            {
                bool success = false;
                UploadResponse response = new UploadResponse();
                MediaContent mediaContent = new MediaContent();
                string extension = Path.GetExtension(upload.file.FileName);
                string randomFileName = Guid.NewGuid().ToString() + extension;

                if (IsAllowedToUpload(extension.ToLower(), upload.fileType))
                {

                    DynamicFileProvider provider = DynamicFileProvider.Default;
                    if (!MD.CMS.BusinessLogic.Core.Properties.Settings.Default.FileUploadProvider.Equals(default))
                    {
                        provider = new DynamicFileProvider();
                        provider.SetFileProvider(MD.CMS.BusinessLogic.Core.Properties.Settings.Default.FileUploadProvider);
                    }

                    string desiredFileLocation = provider.PathJoin(CMS.BusinessLogic.Core.Properties.Settings.Default.FileUploadPath, upload.path.Trim('\\', '/'));

                    FileProviderOptions fileProviderOptions = new FileProviderOptions();
                    fileProviderOptions.DirectoryRequestOptions = new FileProviderDirectoryOptions() { Path = desiredFileLocation };

                    if (!(await provider.DirectoryExists(fileProviderOptions)))
                    {
                        await provider.CreateDirectory(new FileProviderDirectory() { DirectoryPath = desiredFileLocation });
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        await upload.file.CopyToAsync(stream);
                        success = await provider.WriteFile(new FileProviderFile()
                        {
                            FileBytes = stream.ReadToEnd(),
                            FilePath = provider.PathJoin(desiredFileLocation, randomFileName)
                        });
                    }

                    if (!success)
                    {
                        throw new Exception("Error uploading file!");
                    }

                    response.PathToSaveToDatabase = string.Format("{0}/{1}", upload.path.Trim('\\', '/'), randomFileName);
                }
                else
                {
                    throw new HttpException((int)System.Net.HttpStatusCode.Forbidden);
                }

                return Ok(response);
            }
            catch (System.Exception e)
            {
                throw new HttpException((int)System.Net.HttpStatusCode.InternalServerError, "An error occured", e);
            }
        }
    }
}