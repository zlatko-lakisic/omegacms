using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.CMS.BusinessLogic.AwsLambda.Core.Properties
{
    /// <summary>
    /// Application settings
    /// </summary>
    public class Settings
    {
        #region Attributes
        private static Settings defaultInstance = new Settings();
        private string _webAppPath;
        private string _appReferencePath;
        private bool _debugMode;
        private List<string> _supportedMimeTypes;
        #endregion

        #region Properties
        /// <summary>
        /// Default static settings instance
        /// </summary>
        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
            set
            {
                defaultInstance = value;
            }
        }
        /// <summary>
        /// Folder path to the web application files
        /// </summary>
        public string WebAppPath { get => _webAppPath; set => _webAppPath = value; }
        /// <summary>
        /// Reference path for the application in the format of "{dll_file_name}.{namespace}"
        /// </summary>
        public string AppReferencePath { get => _appReferencePath; set => _appReferencePath = value; }
        /// <summary>
        /// Toggle debug mode to display debug information in CloudWatch
        /// </summary>
        public bool DebugMode { get => _debugMode; set => _debugMode = value; }
        /// <summary>
        /// Supported MimeTypes
        /// </summary>
        public List<string> SupportedMimeTypes { get => _supportedMimeTypes; }
        #endregion

        #region Methods
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Settings()
        {
            _supportedMimeTypes = new List<string> {
                "video/mp4", 
                "application/octet-stream", 
                "font/woff2", 
                "font/woff", 
                "font/ttf", 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };
        }

        /// <summary>
        /// Parse settings from environmental variables
        /// </summary>
        public void ParseConfig()
        {
            _webAppPath = Environment.GetEnvironmentVariable("MD_CMS_AwsLambda_Container_Core_WebAppPath");

            _appReferencePath = Environment.GetEnvironmentVariable("MD_CMS_AwsLambda_Container_Core_AppReferencePath");

            bool debugMode = _debugMode;
            bool.TryParse(Environment.GetEnvironmentVariable("MD_CMS_AwsLambda_Container_Core_DebugMode"), out debugMode);
            _debugMode = debugMode;

            string supportedMimeTypesString = Environment.GetEnvironmentVariable("MD_CMS_AwsLambda_Container_Core_SupportedMimeTypes");
            if (!string.IsNullOrEmpty(supportedMimeTypesString))
            {
                _supportedMimeTypes.AddRange(supportedMimeTypesString.Split(',').Where(element => !string.IsNullOrEmpty(element) && !string.IsNullOrWhiteSpace(element)));
            }
        }
        #endregion
    }
}
