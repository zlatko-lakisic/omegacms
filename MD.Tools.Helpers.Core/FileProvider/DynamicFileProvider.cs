using MD.Tools.Helpers.Core.FileProvider.Providers;
using MD.Tools.Helpers.Core.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.Tools.Helpers.Core.FileProvider
{
    /// <summary>
    /// 
    /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class DynamicFileProvider : Singleton<DynamicFileProvider>, IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        #region Attributes
        private int _fileProvider;
        private static ConcurrentDictionary<string, IFileProvider> _availablePoviders = new ConcurrentDictionary<string, IFileProvider>(new List<KeyValuePair<string, IFileProvider>> { new KeyValuePair<string, IFileProvider>(new HostedFileProvider().ProviderName, new HostedFileProvider()) });
        private static DynamicFileProvider _defaultFileProvider;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public static DynamicFileProvider Default
        {
            get
            {
                if(_defaultFileProvider == null)
                {
                    _defaultFileProvider = new DynamicFileProvider();
                    _defaultFileProvider.SetFileProvider(Properties.HelperSettings.Default.DefaultFileProvider);
                }
                return _defaultFileProvider;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool AddFileProvider<T>()
            where T : IFileProvider, new()
        {
            return AddFileProvider(new T());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool AddFileProvider(IFileProvider provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (!_availablePoviders.ContainsKey(provider.ProviderName))
            {
                return _availablePoviders.TryAdd(provider.ProviderName, provider);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public DynamicFileProvider()
        {
            _fileProvider = (int)FileProviderEnum.None;

            ParseOptions();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public DynamicFileProvider SetFileProvider(int provider)
        {
            _fileProvider = provider;

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        public IFileProvider GetSelectedFileProvider()
        {
            IFileProvider provider = _availablePoviders.Select(pro => pro.Value).FirstOrDefault(pro => pro.ProviderType == _fileProvider);

            if (provider == null)
            {
                throw new ArgumentOutOfRangeException(nameof(provider), "The selected provider does not exist!");
            }

            return provider;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Need to check FileProviderType property")]
        public async Task<bool> DirectoryExists(FileProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            return await GetSelectedFileProvider().DirectoryExists(options.DirectoryRequestOptions).ConfigureAwait(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public async Task<FileProviderDirectory> CreateDirectory(FileProviderDirectory directory)
        {
            if (directory is null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            ParseOptions();

            return await GetSelectedFileProvider().CreateDirectory(directory).ConfigureAwait(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Need to check FileProviderType property")]
        public async Task<bool> FileExists(FileProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            return await GetSelectedFileProvider().FileExists(options.FileRequestOptions.FirstOrDefault()).ConfigureAwait(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Need to check FileProviderType property")]
        public async Task<IDictionary<string, bool>> FilesExist(FileProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            if (GetSelectedFileProvider().CanCheckMultipleFiles)
            {
                return await GetSelectedFileProvider().FilesExist(options.FileRequestOptions).ConfigureAwait(true);
            }
            else
            {
                ConcurrentDictionary<string, bool> result = new ConcurrentDictionary<string, bool>();

                await Task.WhenAll(options.FileRequestOptions.Select(async option => result.TryAdd(option.Path, await GetSelectedFileProvider().FileExists(option).ConfigureAwait(true)))).ConfigureAwait(true);

                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Need to check FileProviderType property")]
        public async Task<FileProviderFile> ReadFile(FileProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            return await GetSelectedFileProvider().ReadFile(options.FileRequestOptions.FirstOrDefault()).ConfigureAwait(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Need to check FileProviderType property")]
        public async Task<IDictionary<string, FileProviderFile>> ReadFiles(FileProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            if (GetSelectedFileProvider().CanReadMultipleFiles)
            {
                return await GetSelectedFileProvider().ReadFiles(options.FileRequestOptions).ConfigureAwait(true);
            }
            else
            {
                ConcurrentDictionary<string, FileProviderFile> result = new ConcurrentDictionary<string, FileProviderFile>();

                await Task.WhenAll(options.FileRequestOptions.Select(async option => result.TryAdd(option.Path, await GetSelectedFileProvider().ReadFile(option).ConfigureAwait(true)))).ConfigureAwait(true);

                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Need to check FileProviderType property")]
        public async Task<bool> WriteFile(FileProviderFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            return await GetSelectedFileProvider().WriteFile(file).ConfigureAwait(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Need to check FileProviderType property")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IDictionary<string, bool>> WriteFiles(IEnumerable<FileProviderFile> files, FileProviderParallelOptions options)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            ConcurrentDictionary<string, bool> result = new ConcurrentDictionary<string, bool>();

            await Task.WhenAll(files.Select(async file => result.TryAdd(file.FilePath, await GetSelectedFileProvider().WriteFile(file).ConfigureAwait(true)))).ConfigureAwait(true);

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        public Task<IEnumerable<FileProviderDirectory>> ReadChildDirectoryInfo(FileProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.DirectoryRequestOptions is null)
            {
                throw new ArgumentNullException("options.DirectoryRequestOptions");
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            return GetSelectedFileProvider().ReadChildDirectoryInfo(options.DirectoryRequestOptions);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        public Task<IEnumerable<FileProviderFile>> ReadDirectoryFiles(FileProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.DirectoryRequestOptions is null)
            {
                throw new ArgumentNullException("options.DirectoryRequestOptions");
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            return GetSelectedFileProvider().ReadDirectoryFiles(options.DirectoryRequestOptions);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        public Task<FileProviderDirectory> ReadDirectoryInfo(FileProviderOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.DirectoryRequestOptions is null)
            {
                throw new ArgumentNullException("options.DirectoryRequestOptions");
            }

            if (_fileProvider == (int)FileProviderEnum.None)
            {
                throw new ArgumentOutOfRangeException("FileProviderType");
            }

            ParseOptions();

            return GetSelectedFileProvider().ReadDirectoryInfo(options.DirectoryRequestOptions);
        }
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CA1822 // Mark members as static
        private void ParseOptions()
#pragma warning restore CA1822 // Mark members as static
        {
            foreach (IFileProvider provider in _availablePoviders.Select(pro => pro.Value))
            {
                if (HelperSettings.Default.ProviderOptions.ContainsKey(provider.ProviderName))
                {
                    provider.ParseOptions(HelperSettings.Default.ProviderOptions[provider.ProviderName]);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            if(_availablePoviders != null)
            {
                foreach(IFileProvider provider in _availablePoviders.Select(pro => pro.Value))
                {
                    provider.Dispose();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public string PathJoin(params string[] paths)
        {
            return GetSelectedFileProvider().PathJoin(paths);
        }
        #endregion
    }
}
