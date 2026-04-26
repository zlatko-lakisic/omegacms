using Microsoft.VisualStudio.TestTools.UnitTesting;
using MD.Tools.Helpers.Core.FileProvider;
using System;
using System.Collections.Generic;
using System.Text;
using MD.Tools.Helpers.Core.Properties;

namespace MD.Tools.Helpers.Core.FileProvider.Tests
{
    [TestClass()]
    public class DynamicFileProviderTests
    {
        [TestMethod()]
        public void ReadHostedFileTest()
        {
            FileProviderOptions options = new FileProviderOptions();
            options.FileRequestOptions.Add(new FileProviderFileOptions() { Path = @"\\nas2\IIS_Shares\CMS\Dev\Uploads\Root\0906d73b-88d9-409c-af59-62c8dd66d91f.xlsx" });
            DynamicFileProvider.Instance.SetFileProvider((int)FileProviderEnum.Hosted);
            FileProviderFile file = DynamicFileProvider.Instance.ReadFile(options).Result;
        }

        [TestMethod()]
        public void ReadAwsS3DirectoryTest()
        {
            FileProviderOptions options = new FileProviderOptions();
            options.DirectoryRequestOptions = new FileProviderDirectoryOptions()
            {
                LoadObjects = true
            };

            DynamicFileProvider.Instance.SetFileProvider(2);

            HelperSettings.Default.ProviderOptions.Add(DynamicFileProvider.Instance.GetSelectedFileProvider().ProviderName, "{ \"BucketName\": \"test-bucket\", \"AccessKey\": \"EXAMPLE_AWS_ACCESS_KEY_ID\", \"SecretKey\": \"EXAMPLE_AWS_SECRET_KEY\", \"RegionDisplayName\": \"us-east-1\", \"CacheFiles\": \"true\", \"CacheLocation\": \"E:\\\\tmp\\\\cached-files\"}");

            IEnumerable<FileProviderFile> files = DynamicFileProvider.Instance.ReadDirectoryFiles(options).Result;
        }
    }
}