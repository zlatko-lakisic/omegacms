using Microsoft.Extensions.FileProviders;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.GoogleCloud.Core.FileProviders
{
    public class GoogleCloudStorageFileProviderDirectoryContents : IDirectoryContents
    {
        #region Attributes
        private bool _exists;
        private ConcurrentQueue<GoogleCloudStorageFileProviderFile> _queue;
        private string _subpath;
        private MD.Tools.Helpers.Core.FileProvider.IFileProvider _provider;
        #endregion

        #region Properties
        public bool Exists { get => _exists; set => _exists = value; }
        #endregion

        #region Methods
        public GoogleCloudStorageFileProviderDirectoryContents(MD.Tools.Helpers.Core.FileProvider.IFileProvider provider, string subpath)
        {
            _provider = provider;
            _subpath = subpath;
        }

        private void EnsureFilesAreInitialized()
        {
            if (_queue == null)
            {
                _queue = new ConcurrentQueue<GoogleCloudStorageFileProviderFile>();
                foreach(Tools.Helpers.Core.FileProvider.FileProviderFile file in _provider.ReadDirectoryFiles(new Tools.Helpers.Core.FileProvider.FileProviderDirectoryOptions()
                {
                    Path = _subpath,
                    LoadObjects = true
                }).Result)
                {
                    _queue.Enqueue(new GoogleCloudStorageFileProviderFile(file));
                }
            }
        }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            EnsureFilesAreInitialized();
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            EnsureFilesAreInitialized();
            return _queue.GetEnumerator();
        }
        #endregion
    }
}
