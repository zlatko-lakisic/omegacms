using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles
{
    public class TemplateScreenshot
    {
        #region Attributes
        private string _screenshotFile;
        private string _screenshotUrl;
        private int _screenshotWidth;
        private int _screenshotHeight;
        private Template _template;
        #endregion

        #region Properties
        public Template Template { get => _template; set => _template = value; }
        public string ScreenshotUrl { get => _screenshotUrl; set => _screenshotUrl = value; }
        public string ScreenshotFile { get => _screenshotFile; set => _screenshotFile = value; }
        public int ScreenshotWidth { get => _screenshotWidth; set => _screenshotWidth = value; }
        public int ScreenshotHeight { get => _screenshotHeight; set => _screenshotHeight = value; }
        #endregion
    }
}
