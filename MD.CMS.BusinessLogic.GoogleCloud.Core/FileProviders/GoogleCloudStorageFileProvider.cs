using Google.Api.Gax;
using Google.Cloud.Storage.V1;
using MD.Tools.Helpers.Core.FileProvider;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.Serializer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.GoogleCloud.Core.FileProviders
{
    public class GoogleCloudStorageFileProvider : Tools.Helpers.Core.FileProvider.IFileProvider, Microsoft.Extensions.FileProviders.IFileProvider
    {
        #region Attributes
        private StorageClient _storageClient;
        private GoogleCloudStorageFileProviderOptions _options;
        private string _rootPath;
        private static object _lock = new object();
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        #endregion

        #region Properties
        public bool CanWriteMultipleFiles => false;

        public bool CanReadMultipleFiles => false;

        public bool CanCheckMultipleFiles => false;

        public int ProviderType => 3;

        public string ProviderName => "GoogleCloudStorageFileProvider";
        #endregion

        #region Methods
        public GoogleCloudStorageFileProvider()
        {

        }

        public GoogleCloudStorageFileProvider(string rootPath)
        {
            _rootPath = rootPath;
        }

        private StorageClient GetClient()
        {
            if(_storageClient == null)
            {
                _storageClient = StorageClient.Create(_options.GetCredentials());
            }
            return _storageClient;
        }

        public async Task<FileProviderDirectory> CreateDirectory(FileProviderDirectory directory)
        {
            return directory;
        }

        public async Task<bool> DirectoryExists(FileProviderDirectoryOptions options)
        {
            return (await GetClient().GetObjectAsync(_options.Bucket, PathJoin(_rootPath, options.Path))) != null;
        }

        public void Dispose()
        {
            if(_storageClient != null)
            {
                _storageClient.Dispose();
            }
            ClearAllFromCache();
        }

        public async Task<bool> FileExists(FileProviderFileOptions options)
        {
            if ((await GetFromCache(options.Path)) == null)
            {
                return (await GetClient().GetObjectAsync(_options.Bucket, PathJoin(_rootPath, options.Path))) != null;
            }
            return true;
        }

        public async Task<IDictionary<string, bool>> FilesExist(IEnumerable<FileProviderFileOptions> options)
        {
            return new Dictionary<string, bool>(await Task.WhenAll(options.Select(async opt => new KeyValuePair<string, bool>(opt.Path, await FileExists(opt)))));
        }

        public void ParseOptions(dynamic options)
        {
            if (options != null)
            {
                string optionsString = OmegaJsonSerializer.SerializeObject(options);
                if (!string.IsNullOrEmpty(optionsString))
                {
                    _options = OmegaJsonSerializer.DeserializeObject(optionsString, new GoogleCloudStorageFileProviderOptions());

                    if (_options != null)
                    {
                        typeof(GoogleCloudStorageFileProvider).LogVerbose("Options parsed for GCP Cloud Storage FileProvider (Bucket: {0}, ProjectId: {1})", _options.Bucket, _options.ProjectId);
                    }
                }
            }
        }

        private string PathCleanup(string path)
        {
            return path.Trim('/', '\\');
        }

        public string PathJoin(params string[] paths)
        {
            return string.Join('/', paths.Where(path => !string.IsNullOrEmpty(path)).Select(path => PathCleanup(path)));
        }

        public async Task<IEnumerable<FileProviderDirectory>> ReadChildDirectoryInfo(FileProviderDirectoryOptions options)
        {
            return new List<FileProviderDirectory>();
        }

        public async Task<IEnumerable<FileProviderFile>> ReadDirectoryFiles(FileProviderDirectoryOptions options)
        {
            typeof(GoogleCloudStorageFileProvider).LogVerbose($"Attempting to read directory files from {options.Path}.");
            Page<Google.Apis.Storage.v1.Data.Object> result = await (GetClient().ListObjectsAsync(_options.Bucket, PathJoin(_rootPath, options.Path), new ListObjectsOptions() { 
                PageSize = 9999
            })).ReadPageAsync(9999);
            typeof(GoogleCloudStorageFileProvider).LogVerbose($"{result.Count()} directory files retreived from {options.Path}.");

            return await Task.WhenAll(result.Select(async obj => {
                FileProviderFile file = new FileProviderFile();
                file.FilePath = obj.Name;
                file.LastModified = obj.Updated.HasValue ? obj.Updated.Value : new DateTime();
                if (options.LoadObjects)
                {
                    file.FileBytes = await GetFromCache(file.FilePath).ConfigureAwait(true);
                    if (file.FileBytes == null)
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            await GetClient().DownloadObjectAsync(obj, stream);
                            file.FileBytes = stream.ToArray();
                            await StoreToCache(file.FilePath, file.FileBytes);
                        }
                    }
                }
                return file;
            }));
        }

        public async Task<FileProviderDirectory> ReadDirectoryInfo(FileProviderDirectoryOptions options)
        {
            return new FileProviderDirectory()
            {
                DirectoryPath = options.Path
            };
        }

        public async Task<FileProviderFile> ReadFile(FileProviderFileOptions options)
        {
            typeof(GoogleCloudStorageFileProvider).LogVerbose($"Attempting to read file from {options.Path}.");
            Google.Apis.Storage.v1.Data.Object result = await GetClient().GetObjectAsync(_options.Bucket, PathJoin(_rootPath, options.Path));
            typeof(GoogleCloudStorageFileProvider).LogVerbose($"Files retreived from {options.Path}.");
            if (result != null)
            {
                FileProviderFile file = new FileProviderFile();
                file.FilePath = options.Path;
                file.FileBytes = await GetFromCache(file.FilePath).ConfigureAwait(true);
                file.LastModified = result.Updated.HasValue ? result.Updated.Value : new DateTime();
                if (file.FileBytes == null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await GetClient().DownloadObjectAsync(result, stream);
                        file.FileBytes = stream.ToArray();
                        await StoreToCache(file.FilePath, file.FileBytes);
                    }
                }
                return file;
            }
            return null;
        }

        public async Task<IDictionary<string, FileProviderFile>> ReadFiles(IEnumerable<FileProviderFileOptions> options)
        {
            return new Dictionary<string, FileProviderFile>(await Task.WhenAll(options.Select(async opt => new KeyValuePair<string, FileProviderFile>(opt.Path, await ReadFile(opt)))));
        }

        public async Task<bool> WriteFile(FileProviderFile file)
        {
            return (await GetClient().UploadObjectAsync(_options.Bucket, file.FilePath, null, new MemoryStream(file.FileBytes))) != null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        private void ClearFromCache(string fileName)
        {
            if (_options.CacheFiles)
            {
                lock (_lock)
                {
                    File.Delete(Path.Combine(GetDirectoryWithoutFile(fileName), GetFileName(fileName)));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearAllFromCache()
        {
            if (_options.CacheFiles)
            {
                if (Directory.Exists(_options.CacheLocation))
                {
                    lock (_lock)
                    {
                        Directory.Delete(_options.CacheLocation, true);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private async Task<byte[]> GetFromCache(string fileName)
        {
            byte[] byteResult = null;
            if (_options.CacheFiles)
            {
                if (!Directory.Exists(GetDirectoryWithoutFile(fileName)))
                {
                    await _semaphore.WaitAsync();
                    try
                    {
                        typeof(GoogleCloudStorageFileProvider).LogVerbose($"Creating cache directory {GetDirectoryWithoutFile(fileName)}.");
                        Directory.CreateDirectory(GetDirectoryWithoutFile(fileName));
                        typeof(GoogleCloudStorageFileProvider).LogVerbose($"Cache directory {GetDirectoryWithoutFile(fileName)} created.");
                    }
                    catch (InvalidOperationException error)
                    {
                        typeof(GoogleCloudStorageFileProvider).Log(error);
                    }
                    catch (Exception error)
                    {
                        typeof(GoogleCloudStorageFileProvider).Log(error);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }

                if (File.Exists(Path.Combine(GetDirectoryWithoutFile(fileName), GetFileName(fileName))))
                {
                    typeof(GoogleCloudStorageFileProvider).LogVerbose($"Retreiving cache file {Path.Combine(GetDirectoryWithoutFile(fileName), GetFileName(fileName))}.");
                    using (FileStream stream = File.Open(Path.Combine(GetDirectoryWithoutFile(fileName), GetFileName(fileName)), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        if (stream.Length.Equals(default))
                        {
                            typeof(GoogleCloudStorageFileProvider).LogVerbose($"Cache file {Path.Combine(GetDirectoryWithoutFile(fileName), GetFileName(fileName))} has byte size of 0, clearing...");
                            ClearFromCache(fileName);
                            return null;
                        }

                        byteResult = new byte[stream.Length];
                        await stream.ReadAsync(byteResult.AsMemory(0, (int)stream.Length)).ConfigureAwait(true);
                    }
                    typeof(GoogleCloudStorageFileProvider).LogVerbose($"Cache file {Path.Combine(GetDirectoryWithoutFile(fileName), GetFileName(fileName))} retreived.");
                }
            }
            return byteResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bytes"></param>
        private async Task StoreToCache(string fileName, byte[] bytes)
        {
            if (_options.CacheFiles)
            {
                if (!Directory.Exists(GetDirectoryWithoutFile(fileName)))
                {
                    await _semaphore.WaitAsync();
                    try
                    {
                        typeof(GoogleCloudStorageFileProvider).LogVerbose($"Creating cache directory {GetDirectoryWithoutFile(fileName)}.");
                        Directory.CreateDirectory(GetDirectoryWithoutFile(fileName));
                        typeof(GoogleCloudStorageFileProvider).LogVerbose($"Cache directory {GetDirectoryWithoutFile(fileName)} created.");
                    }
                    catch (InvalidOperationException error)
                    {
                        typeof(GoogleCloudStorageFileProvider).Log(error);
                    }
                    catch (Exception error)
                    {
                        typeof(GoogleCloudStorageFileProvider).Log(error);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
                await _semaphore.WaitAsync();
                try
                {
                    typeof(GoogleCloudStorageFileProvider).LogVerbose($"Creating cache file {Path.Combine(GetDirectoryWithoutFile(fileName), GetFileName(fileName))}.");
                    await File.WriteAllBytesAsync(Path.Combine(GetDirectoryWithoutFile(fileName), GetFileName(fileName)), bytes).ConfigureAwait(true);
                    typeof(GoogleCloudStorageFileProvider).LogVerbose($"Cache file {Path.Combine(GetDirectoryWithoutFile(fileName), GetFileName(fileName))} created.");
                }
                catch (InvalidOperationException error)
                {
                    typeof(GoogleCloudStorageFileProvider).Log(error);
                }
                catch(Exception error)
                {
                    typeof(GoogleCloudStorageFileProvider).Log(error);
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }
        private string GetDirectoryWithoutFile(string fileName)
        {
            return Path.Combine(_options.CacheLocation, string.Join('/', fileName.Split('/').Take(fileName.Split('/').Length - 1)));
        }
        private string GetFileName(string fileName)
        {
            string name = fileName.Split('/').LastOrDefault();
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (subpath is null)
            {
                throw new ArgumentNullException(nameof(subpath));
            }

            FileProviderOptions options = new FileProviderOptions();
            options.FileRequestOptions.Add(new FileProviderFileOptions()
            {
                Path = PathJoin(_rootPath, subpath)
            });
            FileProviderFile file = DynamicFileProvider.Instance.SetFileProvider(ProviderType).ReadFile(options).Result;
            if (file != null)
            {
                string filePath = default;
                GoogleCloudStorageFileProvider provider = new GoogleCloudStorageFileProvider();
                provider.ParseOptions(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.ProviderOptions[ProviderName]);
                if (provider._options.CacheFiles)
                {
                    filePath = Path.Join(Path.GetFullPath(provider._options.CacheLocation), PathJoin(_rootPath, subpath));
                }
                return new GoogleCloudStorageFileProviderFile(file, filePath);
            }
            return new NotFoundFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            
            if (subpath is null)
            {
                throw new ArgumentNullException(nameof(subpath));
            }

            return new GoogleCloudStorageFileProviderDirectoryContents(this, PathJoin(_rootPath, subpath));
        }

        public IChangeToken Watch(string filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return NullChangeToken.Singleton;
        }
        #endregion
    }
}
