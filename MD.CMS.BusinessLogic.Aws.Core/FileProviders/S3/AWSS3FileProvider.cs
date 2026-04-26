using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using MD.Tools.Helpers.Core.Extensions.Stream;
using System.Text.RegularExpressions;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.Serializer;
using MD.Tools.Helpers.Core.FileProvider;
using Newtonsoft.Json;

namespace MD.CMS.BusinessLogic.Aws.Core.FileProviders.S3
{
    public partial class AWSS3FileProvider : IFileProvider
    {
        #region Attributes
        private AWSS3FileProviderOptions _awsS3Options;
        #endregion

        #region Properties
        public bool CanWriteMultipleFiles => false;

        public bool CanReadMultipleFiles => false;

        public bool CanCheckMultipleFiles => false;

        public int ProviderType => 2;

        public string ProviderName => "AWSS3FileProvider";
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static DateTime StartLog(string name)
        {
            typeof(AWSS3FileProvider).LogVerbose($"AWS S3 FileProvider event {name} started...");
            return DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="start"></param>
        private static void EndLog(string name, DateTime start)
        {
            typeof(AWSS3FileProvider).LogVerbose($"AWS S3 FileProvider event {name} completed, elapsed time {DateTime.Now.Subtract(start).TotalMilliseconds} ms.");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        private AmazonS3Client GetClient()
        {
            try
            {
                if (_awsS3Options is null)
                {
                    throw new ArgumentNullException("_awsS3Options");
                }

                return new AmazonS3Client(_awsS3Options.AccessKey, _awsS3Options.SecretKey, Amazon.RegionEndpoint.EnumerableAllRegions.FirstOrDefault(region => string.CompareOrdinal(region.SystemName, _awsS3Options.RegionDisplayName).Equals(0)));
            }
            catch (AmazonS3Exception e)
            {
                typeof(AWSS3FileProvider).Log("Error occured while creating an AWS S3 Client", e);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                typeof(AWSS3FileProvider).Log("Error occured while creating an AWS S3 Client", e);
            }
            return null;
        }

        public async Task<bool> FileExists(FileProviderFileOptions options)
        {
            using(AmazonS3Client client  = GetClient())
            {
                try
                {
                    GetObjectRequest request = new GetObjectRequest()
                    {
                        BucketName = _awsS3Options.BucketName
                    };
                    DateTime start = StartLog("FileExists");
                    GetObjectResponse response = await client.GetObjectAsync(request).ConfigureAwait(true);
                    EndLog("FileExists", start);
                    return response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK;
                }
                catch (AmazonS3Exception e)
                {
                    typeof(AWSS3FileProvider).Log("Error occured while validating file exists", e);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    typeof(AWSS3FileProvider).Log("Error occured while validating file exists", e);
                }
                return false;
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IDictionary<string, bool>> FilesExist(IEnumerable<FileProviderFileOptions> options)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return new Dictionary<string, bool>();
        }

        public async Task<IEnumerable<FileProviderDirectory>> ReadChildDirectoryInfo(FileProviderDirectoryOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            using (AmazonS3Client client = GetClient())
            {
                try
                {
                    DateTime start = StartLog("ReadChildDirectoryInfo");
                    ListObjectsV2Request request = new ListObjectsV2Request()
                    {
                        BucketName = _awsS3Options.BucketName
                    };

                    ListObjectsV2Response response = await client.ListObjectsV2Async(request).ConfigureAwait(true);
                    EndLog("ReadChildDirectoryInfo", start);

                    return response.CommonPrefixes.Select(prefix => new FileProviderDirectory()
                    {
                        DirectoryPath = prefix
                    });
                }
                catch (AmazonS3Exception e)
                {
                    typeof(AWSS3FileProvider).Log("Error occured while reading AWS S3 directory info", e);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    typeof(AWSS3FileProvider).Log("Error occured while reading AWS S3 directory info", e);
                }
                return null;
            }
        }

        public async Task<IEnumerable<FileProviderFile>> ReadDirectoryFiles(FileProviderDirectoryOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            using (AmazonS3Client client = GetClient())
            {
                try
                {
                    DateTime start = StartLog("ReadDirectoryFiles");
                    ListObjectsV2Request request = new ListObjectsV2Request()
                    {
                        BucketName = _awsS3Options.BucketName
                    };

                    ListObjectsV2Response response = await client.ListObjectsV2Async(request).ConfigureAwait(true);

                    if (options.LoadObjects)
                    {
                        List<FileProviderFile> files = new List<FileProviderFile>();
                        typeof(AWSS3FileProvider).LogVerbose($"AWS S3 FileProvider event ReadDirectoryFiles attempting to load {response.S3Objects.Count} files before filter...");

                        foreach (S3Object obj in response.S3Objects)
                        {
                            typeof(AWSS3FileProvider).LogVerbose($"AWS S3 FileProvider event ReadDirectoryFiles starting match for {obj.Key}...");
                            bool isMatch = true;

                            if (!string.IsNullOrEmpty(options.SearchPattern))
                            {
                                isMatch = Regex.IsMatch(obj.Key, options.SearchPattern);
                            }

                            if (isMatch)
                            {
                                byte[] fileBytes = await GetFromCache(obj.Key).ConfigureAwait(true);
                                if (fileBytes != null)
                                {
                                    typeof(AWSS3FileProvider).LogVerbose($"AWS S3 FileProvider event ReadDirectoryFiles loaded from cache for {obj.Key}...");
                                    files.Add(new FileProviderFile()
                                    {
                                        FileBytes = fileBytes,
                                        FilePath = obj.Key,
                                        LastModified = obj.LastModified
                                    });
                                }
                                else
                                {
                                    typeof(AWSS3FileProvider).LogVerbose($"AWS S3 FileProvider event ReadDirectoryFiles starting download for {obj.Key}...");
                                    GetObjectRequest fileRequest = new GetObjectRequest()
                                    {
                                        BucketName = _awsS3Options.BucketName,
                                        Key = obj.Key
                                    };

                                    GetObjectResponse fileResponse = await client.GetObjectAsync(fileRequest).ConfigureAwait(true);
                                    fileBytes = fileResponse.ResponseStream.ReadToEnd();
                                    files.Add(new FileProviderFile()
                                    {
                                        FileBytes = fileBytes,
                                        FilePath = fileResponse.Key,
                                        LastModified = fileResponse.LastModified
                                    });
                                    StoreToCache(fileResponse.Key, fileBytes);
                                    typeof(AWSS3FileProvider).LogVerbose($"AWS S3 FileProvider event ReadDirectoryFiles {obj.Key} downloaded.");
                                }
                            }
                        }

                        EndLog("ReadDirectoryFiles", start);
                        typeof(AWSS3FileProvider).LogVerbose($"AWS S3 FileProvider event ReadDirectoryFiles loaded {files.Count} files...");
                        return files;
                    }
                    else
                    {
                        EndLog("ReadDirectoryFiles", start);
                        return response.S3Objects.Select(obj => new FileProviderFile()
                        {
                            FilePath = obj.Key
                        });
                    }
                }
                catch (AmazonS3Exception e)
                {
                    typeof(AWSS3FileProvider).Log("Error occured while reading AWS S3 directory files", e);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    typeof(AWSS3FileProvider).Log("Error occured while reading AWS S3 directory files", e);
                }
                return null;
            }
        }

        public async Task<FileProviderDirectory> ReadDirectoryInfo(FileProviderDirectoryOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            using (AmazonS3Client client = GetClient())
            {
                try
                {
                    DateTime start = StartLog("ReadDirectoryInfo");
                    ListObjectsV2Request request = new ListObjectsV2Request()
                    {
                        BucketName = _awsS3Options.BucketName
                    };

                    ListObjectsV2Response response = await client.ListObjectsV2Async(request).ConfigureAwait(true);
                    EndLog("ReadDirectoryInfo", start);

                    return new FileProviderDirectory()
                    {
                        DirectoryPath = response.Prefix
                    };
                }
                catch (AmazonS3Exception e)
                {
                    typeof(AWSS3FileProvider).Log("Error occured while reading AWS S3 directory info", e);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    typeof(AWSS3FileProvider).Log("Error occured while reading AWS S3 directory info", e);
                }
                return null;
            }
        }

        public async Task<FileProviderFile> ReadFile(FileProviderFileOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            using (AmazonS3Client client = GetClient())
            {
                try
                {
                    FileProviderFile result = null;
                    DateTime start = StartLog("ReadFile");
                    byte[] fileBytes = await GetFromCache(options.Path).ConfigureAwait(true);
                    if (fileBytes != null)
                    {
                        result = new FileProviderFile();
                        result.FileBytes = fileBytes;
                        result.FilePath = options.Path;
                        result.LastModified = File.GetLastWriteTime(options.Path);
                    }
                    else
                    {
                        GetObjectRequest request = new GetObjectRequest()
                        {
                            BucketName = _awsS3Options.BucketName,
                            Key = options.Path
                        };

                        GetObjectResponse response = await client.GetObjectAsync(request).ConfigureAwait(true);


                        if (response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        {
                            bool isMatch = true;

                            if (!string.IsNullOrEmpty(options.SearchPattern))
                            {
                                isMatch = Regex.IsMatch(response.Key, options.SearchPattern);
                            }

                            if (isMatch)
                            {
                                using (Stream resultStream = response.ResponseStream)
                                {
                                    result = new FileProviderFile();
                                    result.FileBytes = resultStream.ReadToEnd();
                                    result.FilePath = response.Key;
                                    result.LastModified = response.LastModified;
                                }
                            }

                        }
                    }
                    EndLog("ReadFile", start);

                    return result;
                }
                catch (AmazonS3Exception e)
                {
                    typeof(AWSS3FileProvider).Log("Error occured while reading AWS S3 file", e);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    typeof(AWSS3FileProvider).Log("Error occured while reading AWS S3 file", e);
                }
                return null;
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IDictionary<string, FileProviderFile>> ReadFiles(IEnumerable<FileProviderFileOptions> options)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return new Dictionary<string, FileProviderFile>();
        }

        public async Task<bool> WriteFile(FileProviderFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using (AmazonS3Client client = GetClient())
            {
                try
                {
                    DateTime start = StartLog("WriteFile");
                    PutObjectRequest request = new PutObjectRequest()
                    {
                        BucketName = _awsS3Options.BucketName,
                        Key = file.FilePath
                    };

                    request.InputStream = new MemoryStream(file.FileBytes);

                    PutObjectResponse response = await client.PutObjectAsync(request).ConfigureAwait(true);
                    EndLog("WriteFile", start);

                    return response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK;
                }
                catch (AmazonS3Exception e)
                {
                    typeof(AWSS3FileProvider).Log("Error occured while writing AWS S3 file", e);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    typeof(AWSS3FileProvider).Log("Error occured while writing AWS S3 file", e);
                }
                return false;
            }
        }

        public void ParseOptions(dynamic options)
        {
            if (options != null)
            {
                string optionsString = OmegaJsonSerializer.SerializeObject(options);
                if (!string.IsNullOrEmpty(optionsString))
                {
                    try
                    {
                        _awsS3Options = new AWSS3FileProviderOptions();
                        _awsS3Options = OmegaJsonSerializer.DeserializeObject<AWSS3FileProviderOptions>(optionsString);

                        if (_awsS3Options != null)
                        {
                            typeof(AWSS3FileProvider).LogVerbose("Options parsed for AWS S3 FileProvider (Bucket: {0}, Region: {1})", _awsS3Options.BucketName, _awsS3Options.RegionDisplayName);
                        }
                    }
                    catch (JsonSerializationException e)
                    {
                        typeof(AWSS3FileProvider).Log("Error while deserializing the AWS S3 FileProvider options.", e);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        typeof(AWSS3FileProvider).Log("Error while deserializing the AWS S3 FileProvider options.", e);
                    }
                }
            }
        }

        public async Task<bool> DirectoryExists(FileProviderDirectoryOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            using (AmazonS3Client client = GetClient())
            {
                try
                {
                    DateTime start = StartLog("DirectoryExists");
                    ListObjectsV2Request request = new ListObjectsV2Request()
                    {
                        BucketName = _awsS3Options.BucketName
                    };

                    ListObjectsV2Response response = await client.ListObjectsV2Async(request).ConfigureAwait(true);
                    EndLog("DirectoryExists", start);

                    return response.CommonPrefixes.Any(prefix => string.CompareOrdinal(prefix, options.Path).Equals(0));
                }
                catch (AmazonS3Exception e)
                {
                    typeof(AWSS3FileProvider).Log("Error occured while validating AWS S3 directory exists", e);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    typeof(AWSS3FileProvider).Log("Error occured while validating AWS S3 directory exists", e);
                }
                return false;
            }
        }

        public async Task<FileProviderDirectory> CreateDirectory(FileProviderDirectory directory)
        {
            if (directory is null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            using (AmazonS3Client client = GetClient())
            {
                try
                {
                    DateTime start = StartLog("CreateDirectory");
                    PutObjectRequest request = new PutObjectRequest()
                    {
                        BucketName = _awsS3Options.BucketName,
                        Key = directory.DirectoryPath
                    };

                    await client.PutObjectAsync(request).ConfigureAwait(true);
                    EndLog("CreateDirectory", start);

                    return directory;
                }
                catch (AmazonS3Exception e)
                {
                    typeof(AWSS3FileProvider).Log("Error occured while creating AWS S3 directory", e);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    typeof(AWSS3FileProvider).Log("Error occured while creating AWS S3 directory", e);
                }
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        private void ClearFromCache(string fileName)
        {
            if (_awsS3Options.CacheFiles)
            {
                File.Delete(Path.Combine(_awsS3Options.CacheLocation, fileName));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearAllFromCache()
        {
            if (_awsS3Options.CacheFiles)
            {
                if (Directory.Exists(_awsS3Options.CacheLocation))
                {
                    Directory.Delete(_awsS3Options.CacheLocation, true);
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
            if (_awsS3Options.CacheFiles)
            {
                if (!Directory.Exists(_awsS3Options.CacheLocation))
                {
                    Directory.CreateDirectory(_awsS3Options.CacheLocation);
                }

                if (File.Exists(Path.Combine(_awsS3Options.CacheLocation, fileName)))
                {
                    using (FileStream stream = File.Open(Path.Combine(_awsS3Options.CacheLocation, fileName), FileMode.Open))
                    {
                        if(stream.Length.Equals(default))
                        {
                            ClearFromCache(fileName);
                            return null;
                        }

                        byteResult = new byte[stream.Length];
                        await stream.ReadAsync(byteResult.AsMemory(0, (int)stream.Length)).ConfigureAwait(true);
                    }
                }
            }
            return byteResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bytes"></param>
        private async void StoreToCache(string fileName, byte[] bytes)
        {
            if (_awsS3Options.CacheFiles)
            {
                await File.WriteAllBytesAsync(Path.Combine(_awsS3Options.CacheLocation, fileName), bytes).ConfigureAwait(true);
            }
        }

        public void Dispose()
        {
            ClearAllFromCache();
        }

        public string PathJoin(params string[] paths)
        {
            return string.Join('/', paths.Select(path => path.Trim('/', '\\')));
        }
        #endregion
    }
}
