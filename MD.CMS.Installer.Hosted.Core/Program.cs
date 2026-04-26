using MD.CMS.Installer.Hosted.Core.Tools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using MD.CMS.Installer.Hosted.Core.BusinessLogic;
using MD.CMS.Installer.Hosted.Core.BusinessLogic.Installers;

namespace MD.CMS.Installer.Hosted.Core
{
    public class Program
    {
        #region Properties
        private static Dictionary<DownloadPlatformType, IInstaller> Installers = new Dictionary<DownloadPlatformType, IInstaller>
        {
            { DownloadPlatformType.Win, new WindowsInstaller() },
            { DownloadPlatformType.Linux, new LinuxInstaller() },
            { DownloadPlatformType.Osx, new OsxInstaller() }
        };
        #endregion

        static async Task Main(string[] args)
        {
            (await CommandLine.Parser.Default.ParseArguments<InstallerOptions>(args)
                .WithParsedAsync(RunOptions))
                .WithNotParsed(HandleParseError);
        }

        static async Task RunOptions(InstallerOptions opts)
        {
            Visuals.Intro();

            InstallerOptions.ValidateOptions(opts);

            bool atLeastOneInstalled = false;
            if (!string.IsNullOrEmpty(opts.AdminInstallPath))
            {
                atLeastOneInstalled = true;
                await Installers[(DownloadPlatformType)opts.Platform].Run(opts, DownloadArtifactType.Administration);
            }
            if (!string.IsNullOrEmpty(opts.WebApiInstallPath))
            {
                atLeastOneInstalled = true;
                await Installers[(DownloadPlatformType)opts.Platform].Run(opts, DownloadArtifactType.WebApi);
            }
            if (!string.IsNullOrEmpty(opts.AsyncTaskProcessorPath))
            {
                atLeastOneInstalled = true;
                await Installers[(DownloadPlatformType)opts.Platform].Run(opts, DownloadArtifactType.AsyncTask);
            }
            if (!atLeastOneInstalled)
            {
                Console.WriteLine("Nothing to install.");
            }
            else
            {
                Console.WriteLine("The installation has finished.");
            }
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("An error occured parsing input options, please try again.");
        }
    }
}
