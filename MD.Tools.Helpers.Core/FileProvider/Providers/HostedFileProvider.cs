using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MD.Tools.Helpers.Core.FileProvider.Providers
{
    internal class HostedFileProvider : IFileProvider
    {
        #region Properties
        public bool CanWriteMultipleFiles => false;

        public bool CanReadMultipleFiles => false;

        public bool CanCheckMultipleFiles => false;

        public int ProviderType => (int)FileProviderEnum.Hosted;

        public string ProviderName => "HostedFileProvider";

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<FileProviderDirectory> CreateDirectory(FileProviderDirectory directory)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (directory is null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            DirectoryInfo info = Directory.CreateDirectory(directory.DirectoryPath);

            return new FileProviderDirectory()
            {
                DirectoryPath = info.FullName
            };
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<bool> DirectoryExists(FileProviderDirectoryOptions options)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return Directory.Exists(options.Path);
        }

        public void Dispose()
        {
            //Do Nothing
        }
        #endregion

        #region Methods
        public async Task<bool> FileExists(FileProviderFileOptions options)
        {
            KeyValuePair<string, bool> result = (await FilesExist(new FileProviderFileOptions[] { options }).ConfigureAwait(true)).First();

            return result.Value;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IDictionary<string, bool>> FilesExist(IEnumerable<FileProviderFileOptions> options)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (!options.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(options));
            }

            Dictionary<string, bool> result = new Dictionary<string, bool>();

            foreach(FileProviderFileOptions option in options)
            {

                result.Add(option.Path, File.Exists(option.Path));
            }

            return result;
        }

        public void ParseOptions(dynamic options)
        {
            //Do Nothing
        }

        public string PathJoin(params string[] paths)
        {
            return Path.Join(paths.Select((path, i) => {
                path = path.TrimEnd('\\', '/');
                if (i > 0)
                {
                    path = path.TrimStart('\\', '/');
                }
                return path;
            }).ToArray());
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IEnumerable<FileProviderDirectory>> ReadChildDirectoryInfo(FileProviderDirectoryOptions options)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return Directory.GetDirectories(options.Path, "*.*").
                Where(path => Regex.IsMatch(path, options.SearchPattern)).
                Select(path => new FileProviderDirectory() { DirectoryPath = path });
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IEnumerable<FileProviderFile>> ReadDirectoryFiles(FileProviderDirectoryOptions options)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            List<FileProviderFile> result = new List<FileProviderFile>();

            string[] fileNames = Directory.GetFiles(options.Path, "*.*").
                                    Where(path => Regex.IsMatch(path, options.SearchPattern)).ToArray();

            foreach(string fileName in fileNames)
            {
                result.Add(new FileProviderFile
                {
                    FileBytes = (await File.ReadAllBytesAsync(fileName).ConfigureAwait(true)),
                    FilePath = fileName,
                    LastModified = File.GetLastWriteTime(fileName)
                });
            }

            return result;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<FileProviderDirectory> ReadDirectoryInfo(FileProviderDirectoryOptions options)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            DirectoryInfo info = new DirectoryInfo(options.Path);

            return new FileProviderDirectory() {
                DirectoryPath = info.FullName
            };
        }

        public async Task<FileProviderFile> ReadFile(FileProviderFileOptions options)
        {
            KeyValuePair<string, FileProviderFile> result = (await ReadFiles(new FileProviderFileOptions[] { options }).ConfigureAwait(true)).First();

            return result.Value;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IDictionary<string, FileProviderFile>> ReadFiles(IEnumerable<FileProviderFileOptions> options)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (!options.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(options));
            }

            Dictionary<string, FileProviderFile> result = new Dictionary<string, FileProviderFile>();

            byte[] byteResult;
            foreach (FileProviderFileOptions option in options)
            {
                using (FileStream stream = File.Open(option.Path, FileMode.Open))
                {
                    byteResult = new byte[stream.Length];
                    await stream.ReadAsync(byteResult.AsMemory(0, (int)stream.Length)).ConfigureAwait(true);
                    result.Add(option.Path, new FileProviderFile
                    {
                        FileBytes = byteResult,
                        FilePath = option.Path,
                        LastModified = File.GetLastWriteTime(option.Path)
                    });
                }
            }

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public async Task<bool> WriteFile(FileProviderFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            bool result = false;
            try
            {
                await File.WriteAllBytesAsync(file.FilePath, file.FileBytes).ConfigureAwait(true);
                result = true;
            }
            catch (ArgumentNullException error)
            {
                Logging.Logger.Log(error);
            }
            catch (ArgumentException error)
            {
                Logging.Logger.Log(error);
            }
            catch (PathTooLongException error)
            {
                Logging.Logger.Log(error);
            }
            catch (DirectoryNotFoundException error)
            {
                Logging.Logger.Log(error);
            }
            catch (IOException error)
            {
                Logging.Logger.Log(error);
            }
            catch (UnauthorizedAccessException error)
            {
                Logging.Logger.Log(error);
            }
            catch (NotSupportedException error)
            {
                Logging.Logger.Log(error);
            }
            catch (System.Security.SecurityException error)
            {
                Logging.Logger.Log(error);
            }
            catch (Exception error)
            {
                Logging.Logger.Log(error);
            }
            return result;
        }
        #endregion
    }
}
