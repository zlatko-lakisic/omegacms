using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MD.Tools.Helpers.Core.FileProvider;
using MD.Tools.Helpers.Core.Properties;
using MD.Tools.Helpers.Core.Serializer;

namespace MD.CMS.BusinessLogic.GoogleCloud.Core.FileProviders.Tests
{
    [TestClass()]
    public class GoogleCloudStorageFileProviderTests
    {
        [TestMethod()]
        public void ReadDirectoryFilesTest()
        {
            var credsJson = Environment.GetEnvironmentVariable("OMEGA_GCS_TEST_CREDENTIALS_JSON");
            if (string.IsNullOrWhiteSpace(credsJson))
            {
                Assert.Inconclusive("Set OMEGA_GCS_TEST_CREDENTIALS_JSON to a service account JSON (see root .env.example). Do not commit real keys.");
            }

            DynamicFileProvider.AddFileProvider<GoogleCloudStorageFileProvider>();
            HelperSettings.Default.ProviderOptions[new GoogleCloudStorageFileProvider().ProviderName] = new GoogleCloudStorageFileProviderOptions()
            {
                Bucket = Environment.GetEnvironmentVariable("OMEGA_GCS_TEST_BUCKET") ?? "test-bucket",
                ProjectId = Environment.GetEnvironmentVariable("OMEGA_GCS_TEST_PROJECT") ?? "test-project",
                CacheFiles = true,
                CacheLocation = "/tmp/GoogleCloudStorageFileProviderFiles",
                CredentialsJson = OmegaJsonSerializer.DeserializeObject<dynamic>(credsJson)
            };

            using (DynamicFileProvider provider = DynamicFileProvider.Instance.SetFileProvider(new GoogleCloudStorageFileProvider().ProviderType))
            {
                IEnumerable<FileProviderFile> files = provider.ReadDirectoryFiles(new FileProviderOptions()
                {
                    DirectoryRequestOptions = new FileProviderDirectoryOptions()
                    {
                        Path = "containers/images",
                        LoadObjects = true
                    }
                }).Result;
            }
        }
    }
}
