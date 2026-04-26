using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MD.Tools.Helpers.Core.FileProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class FileProviderDirectory
    {
        /// <summary>
        /// 
        /// </summary>
        public string DirectoryPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DirectoryName
        {
            get
            {
                if (string.IsNullOrEmpty(DirectoryPath))
                {
                    return default;
                }

                return Path.GetDirectoryName(DirectoryPath);
            }
        }
    }
}
