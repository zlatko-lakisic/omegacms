using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MD.Tools.Helpers.Core.FileProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class FileProviderFile
    {
        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Need to return array of bytes")]
        public byte[] FileBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileName { 
            get
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    return default;
                }

                return Path.GetFileNameWithoutExtension(FilePath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FileExtension
        {
            get
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    return default;
                }

                return Path.GetExtension(FilePath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FullFileName
        {
            get
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    return default;
                }

                return Path.GetFileName(FilePath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastModified { get; set; }
    }
}
