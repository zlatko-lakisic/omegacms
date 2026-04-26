using MD.CMS.Installer.Hosted.Core.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MD.CMS.Installer.Hosted.Core.Tools
{
    internal enum DownloadPlatformType : int
    {
        Win = 1,
        Linux = 2,
        Osx = 3
    }
    internal enum DownloadArtifactType
    {
        Administration,
        WebApi,
        AsyncTask
    }

    internal enum DownloadHostedType
    {
        Hosted,
        AwsLambda,
        AzureFunctions
    }

    internal enum DownloadReleaseType
    {
        Debug,
        Release
    }

    internal delegate void DownloadArtifactProgressChangedHandler(DownloadProgressChangedEventArgs e);
    internal delegate void DownloadArtifactOnCompleteHandler(Stream stream);

    internal class Downloader
    {
        public static async Task DownloadArtifact(DownloadPlatformType platformType, DownloadArtifactType artifactType, DownloadHostedType hostedType, DownloadReleaseType releaseType, string version, string authorization, DownloadArtifactProgressChangedHandler downloadArtifactProgressChanged, DownloadArtifactOnCompleteHandler downloadArtifactOnComplete)
        {
            if (string.IsNullOrEmpty(version))
            {
                throw new ArgumentException("message", nameof(version));
            }

            if (string.IsNullOrEmpty(authorization))
            {
                throw new ArgumentException("message", nameof(authorization));
            }

            if (downloadArtifactProgressChanged is null)
            {
                throw new ArgumentNullException(nameof(downloadArtifactProgressChanged));
            }

            if (downloadArtifactOnComplete is null)
            {
                throw new ArgumentNullException(nameof(downloadArtifactOnComplete));
            }

            string artifactFileFormat = string.Empty;
            switch (artifactType)
            {
                case DownloadArtifactType.Administration:
                    switch (hostedType)
                    {
                        case DownloadHostedType.Hosted:
                            artifactFileFormat = Properties.Resources.Artifact_Hosted_AdminFileFormat;
                            break;
                    }
                    break;
                case DownloadArtifactType.WebApi:
                    switch (hostedType)
                    {
                        case DownloadHostedType.Hosted:
                            artifactFileFormat = Properties.Resources.Artifact_Hosted_WebApiFileFormat;
                            break;
                    }
                    break;
            }

            string artifactFormat = string.Empty;
            switch (artifactType)
            {
                case DownloadArtifactType.Administration:
                    switch (hostedType)
                    {
                        case DownloadHostedType.Hosted:
                            artifactFormat = Properties.Resources.Artifact_Hosted_AdminFormat;
                            break;
                    }
                    break;
                case DownloadArtifactType.WebApi:
                    switch (hostedType)
                    {
                        case DownloadHostedType.Hosted:
                            artifactFormat = Properties.Resources.Artifact_Hosted_WebApiFormat;
                            break;
                    }
                    break;
            }

            string platformTypeString = string.Empty;
            switch (platformType)
            {
                case DownloadPlatformType.Win:
                    platformTypeString = Properties.Resources.PlatformType_Win;
                    break;
                case DownloadPlatformType.Linux:
                    platformTypeString = Properties.Resources.PlatformType_Linux;
                    break;
                case DownloadPlatformType.Osx:
                    platformTypeString = Properties.Resources.PlatformType_Osx;
                    break;
            }

            string releaseTypeString = string.Empty;
            switch (releaseType)
            {
                case DownloadReleaseType.Debug:
                    releaseTypeString = Properties.Resources.ReleaseType_Debug;
                    break;
                case DownloadReleaseType.Release:
                    releaseTypeString = Properties.Resources.ReleaseType_Release;
                    break;
            }

            string artifactFile = string.Format(artifactFileFormat, platformTypeString, version, releaseTypeString);
            string url = string.Format(artifactFormat, Properties.Resources.Artifact_Url, artifactFile);

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.Authorization, authorization);
                    webClient.DownloadProgressChanged += (sender, e) =>
                    {
                        downloadArtifactProgressChanged(e);
                    };

                    Stream downloadedFile = new MemoryStream(await webClient.DownloadDataTaskAsync(url));
                    downloadArtifactOnComplete(downloadedFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while downloading {artifactFile}. The error message is: {ex.Message}");
            }
        }
    }
}
