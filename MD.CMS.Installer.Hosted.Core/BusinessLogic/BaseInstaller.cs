using MD.CMS.Installer.Hosted.Core.Tools;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.Installer.Hosted.Core.BusinessLogic
{
    internal abstract class BaseInstaller
    {
        protected async Task RunInstaller(InstallerOptions opts, DownloadArtifactType artifactType)
        {
            string productName = string.Empty;
            string installPath = string.Empty;

            InstallerOptions.ValidateProductInstallPath(opts, artifactType, out installPath, out productName);

            Console.WriteLine("Deleting all files from {0}...", productName);
            if (Directory.Exists(installPath))
            {
                foreach (string subDirectory in Directory.GetDirectories(installPath))
                {
                    if (!subDirectory.ToLowerInvariant().Contains("license"))
                    {
                        Directory.Delete(subDirectory, true);
                    }
                }

                foreach (string file in Directory.GetFiles(installPath))
                {
                    if (!file.ToLowerInvariant().Contains("appsettings.json") && !file.ToLowerInvariant().Contains("web.config"))
                    {
                        File.Delete(file);
                    }
                }
            }

            Console.WriteLine("Installing {0}...", productName);
            await Tools.Downloader.DownloadArtifact((DownloadPlatformType)opts.Platform, artifactType, Tools.DownloadHostedType.Hosted, Tools.DownloadReleaseType.Release, opts.Version, "none", (e) => {
                Console.WriteLine("{0} downloaded {1} of {2} bytes. {3} % complete...",
                                    productName,
                                    e.BytesReceived,
                                    e.TotalBytesToReceive,
                                    e.ProgressPercentage);
            }, (result) => {
                try
                {
                    if (!Directory.Exists(installPath))
                    {
                        Console.WriteLine($"Creating the folder {installPath}...");
                        Directory.CreateDirectory(installPath);
                    }

                    using (SevenZipArchive archive = SevenZipArchive.Open(result))
                    {
                        Console.WriteLine($"Extracting archive {productName}");
                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                        {
                            Console.WriteLine($"Invlating file {entry.Key}...");
                            entry.WriteToDirectory(installPath, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while installing {productName}. The error message is: {ex.Message}");
                }
            });
        }
    }
}
