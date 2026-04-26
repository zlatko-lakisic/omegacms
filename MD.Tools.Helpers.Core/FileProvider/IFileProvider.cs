using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MD.Tools.Helpers.Core.FileProvider
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFileProvider : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<FileProviderFile> ReadFile(FileProviderFileOptions options);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<IDictionary<string, FileProviderFile>> ReadFiles(IEnumerable<FileProviderFileOptions> options);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<bool> WriteFile(FileProviderFile file);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> FileExists(FileProviderFileOptions options);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<bool> DirectoryExists(FileProviderDirectoryOptions options);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        Task<FileProviderDirectory> CreateDirectory(FileProviderDirectory directory);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<IDictionary<string, bool>> FilesExist(IEnumerable<FileProviderFileOptions> options);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<FileProviderDirectory> ReadDirectoryInfo(FileProviderDirectoryOptions options);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<IEnumerable<FileProviderDirectory>> ReadChildDirectoryInfo(FileProviderDirectoryOptions options);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<IEnumerable<FileProviderFile>> ReadDirectoryFiles(FileProviderDirectoryOptions options);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        void ParseOptions(dynamic options);
        /// <summary>
        /// 
        /// </summary>
        bool CanWriteMultipleFiles { get; }
        /// <summary>
        /// 
        /// </summary>
        bool CanReadMultipleFiles { get; }
        /// <summary>
        /// 
        /// </summary>
        bool CanCheckMultipleFiles { get; }
        /// <summary>
        /// 
        /// </summary>
        int ProviderType { get; }
        /// <summary>
        /// 
        /// </summary>
        string ProviderName { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        string PathJoin(params string[] paths);
    }
}
