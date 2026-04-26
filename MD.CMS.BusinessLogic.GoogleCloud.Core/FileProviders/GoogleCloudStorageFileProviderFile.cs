using MD.Tools.Helpers.Core.FileProvider;
using System;
using System.IO;

namespace MD.CMS.BusinessLogic.GoogleCloud.Core.FileProviders
{
    public class GoogleCloudStorageFileProviderFile : Microsoft.Extensions.FileProviders.IFileInfo
    {
        #region Attributes
        private bool _exists;
        private long _length;
        private string _physicalPath;
        private string _name;
        private DateTimeOffset _lastModified;
        private bool _isDirectory;
        private byte[] _bytes;
        #endregion

        #region Properties
        public bool Exists { get => _exists; set => _exists = value; }
        public long Length { get => _length; set => _length = value; }
        public string PhysicalPath { get => _physicalPath; set => _physicalPath = value; }
        public string Name { get => _name; set => _name = value; }
        public DateTimeOffset LastModified { get => _lastModified; set => _lastModified = value; }
        public bool IsDirectory { get => _isDirectory; set => _isDirectory = value; }
        #endregion

        #region Methods
        public GoogleCloudStorageFileProviderFile()
        {

        }
        public GoogleCloudStorageFileProviderFile(FileProviderFile file, string physicalPath = null)
        {
            _exists = true;
            _length = file.FileBytes.Length;
            _physicalPath = physicalPath;
            _name = file.FullFileName;
            _lastModified = file.LastModified;
            _isDirectory = false;
            _bytes = file.FileBytes;
        }
        public Stream CreateReadStream()
        {
            MemoryStream stream = new MemoryStream();
            if (_bytes != null)
            {
                stream.Read(_bytes);
            }
            else
            {
                FileProviderOptions options = new FileProviderOptions();
                options.FileRequestOptions.Add(new FileProviderFileOptions()
                {
                    Path = _physicalPath
                });
                stream.Read(DynamicFileProvider.Instance.SetFileProvider(new GoogleCloudStorageFileProvider().ProviderType).ReadFile(options).Result.FileBytes);
            }
            return stream;
        }
        #endregion
    }
}
