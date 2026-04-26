using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.Helpers.Core.FileProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class FileProviderParallelOptions
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public bool UseMultiThreading { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MaximumNumberOfThreads { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public FileProviderParallelOptions()
        {
            MaximumNumberOfThreads = 10;
        }
        #endregion
    }
}