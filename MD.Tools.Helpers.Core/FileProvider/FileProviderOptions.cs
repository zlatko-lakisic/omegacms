using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.Helpers.Core.FileProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class FileProviderOptions : FileProviderParallelOptions
    {
        #region Attributes
        private List<FileProviderFileOptions> _fileRequestOptions;
        private FileProviderDirectoryOptions _directoryRequestOptions;
        private Dictionary<string, FileProviderFile> _files;
        private Dictionary<string, FileProviderDirectory> _directories;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public List<FileProviderFileOptions> FileRequestOptions { get => _fileRequestOptions; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, FileProviderFile> Files { get => _files; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, FileProviderDirectory> Directories { get => _directories; }
        /// <summary>
        /// 
        /// </summary>
        public FileProviderDirectoryOptions DirectoryRequestOptions { get => _directoryRequestOptions; set => _directoryRequestOptions = value; }
        #endregion


        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public FileProviderOptions() : base()
        {
            _fileRequestOptions = new List<FileProviderFileOptions>();
            _files = new Dictionary<string, FileProviderFile>();
            _directories = new Dictionary<string, FileProviderDirectory>();
        }
        #endregion
    }
}