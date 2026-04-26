using CommandLine;
using MD.CMS.Installer.Hosted.Core.Tools;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MD.CMS.Installer.Hosted.Core.BusinessLogic
{
    internal class InstallerOptions
    {
        #region Properties
        [Option('l', "license", Required = false, HelpText = "By using this Wizard you agree to the terms and " +
                       "\nconditions described at http://omegacms.io/legal/terms/ and " +
                       "\nthose of the Omega CMS End User Lcense Agreement." +
                       "\n\t1. Agree and go forward" +
                       "\n\t2. Cancel installation")]
        public int License { get; set; }

        [Option('p', "platform", Required = false, HelpText = "Please chose your desired platform (default platform running on)" +
                "\n\t1. Windows" +
                "\n\t2. Linux" +
                "\n\t3. Osx")]
        public int Platform { get; set; }

        [Option('v', "version", Required = false, HelpText = "Please specify the desired version (\"x.x.x\" format)")]
        public string Version { get; set; }

        [Option('a', "admin", Required = false, HelpText = "Please specify install path for Omega Administration")]
        public string AdminInstallPath { get; set; }

        [Option('w', "webapi", Required = false, HelpText = "Please specify install path for Omega WebApi")]
        public string WebApiInstallPath { get; set; }

        [Option('t', "asynctaskprocessor", Required = false, HelpText = "Please specify install path for Omega Async Task Processor")]
        public string AsyncTaskProcessorPath { get; set; }
        #endregion

        #region Methods
        public static void ValidateOptions(InstallerOptions opts)
        {
            int eulaAgree = opts.License;

            while (eulaAgree.Equals(default))
            {
                Console.WriteLine("By using this Wizard you agree to the terms and " +
                    "\nconditions described at http://omegacms.io/legal/terms/ and " +
                    "\nthose of the Omega CMS End User Lcense Agreement.");
                Console.WriteLine("\t1. Agree and go forward");
                Console.WriteLine("\t2. Cancel installation");
                Console.Write("Your choice: ");
                eulaAgree = Convert.ToInt32(Console.ReadLine());
                if (!(eulaAgree == 1 || eulaAgree == 2))
                {
                    eulaAgree = default;
                    Console.WriteLine("\tInvalid choice!");
                }
            }

            opts.License = eulaAgree;

            if (opts.License == 2)
            {
                return;
            }

            int platformTypeInt = opts.Platform;
            int defaultPlatformTypeInt = (int)DownloadPlatformType.Win;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                defaultPlatformTypeInt = (int)DownloadPlatformType.Linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                defaultPlatformTypeInt = (int)DownloadPlatformType.Osx;
            }

            while (platformTypeInt.Equals(default))
            {
                Console.WriteLine("Please chose your desired platform:");
                Console.WriteLine("\t1. Windows");
                Console.WriteLine("\t2. Linux");
                Console.WriteLine("\t3. Osx");
                Console.Write(string.Format("Your choice [{0}]: ", defaultPlatformTypeInt));
                platformTypeInt = Convert.ToInt32(Console.ReadLine());
                if (platformTypeInt.Equals(default))
                {
                    platformTypeInt = defaultPlatformTypeInt;
                }
                if (!Enum.IsDefined(typeof(DownloadPlatformType), platformTypeInt))
                {
                    platformTypeInt = default;
                    Console.WriteLine("\tInvalid choice!");
                }
            }

            opts.Platform = platformTypeInt;

            string version = opts.Version;

            while (string.IsNullOrEmpty(version))
            {
                Console.Write("Please specify the desired version (\"x.x.x\" format): ");
                version = Console.ReadLine();
                if (string.IsNullOrEmpty(version) || !new Regex("^\\d(\\d|(?<!\\.)\\.)*\\d$|^\\d$").IsMatch(version))
                {
                    version = string.Empty;
                    Console.WriteLine("\tInvalid version!");
                }
            }

            opts.Version = version;
        }

        public static void ValidateProductInstallPath(InstallerOptions opts, DownloadArtifactType artifactType, out string installPath, out string productName)
        {
            productName = string.Empty;
            installPath = string.Empty;

            switch (artifactType)
            {
                case DownloadArtifactType.Administration:
                    productName = Properties.Resources.Artifact_Name_Admin;
                    installPath = opts.AdminInstallPath;
                    break;
                case DownloadArtifactType.WebApi:
                    productName = Properties.Resources.Artifact_Name_WebApi;
                    installPath = opts.WebApiInstallPath;
                    break;
                case DownloadArtifactType.AsyncTask:
                    productName = Properties.Resources.Artifact_Name_AsyncTask;
                    installPath = opts.AsyncTaskProcessorPath;
                    break;
            }

            while (string.IsNullOrEmpty(installPath))
            {
                Console.Write(string.Format("Please specify install path for {0}: ", productName));
                installPath = Console.ReadLine();
                if (string.IsNullOrEmpty(installPath))
                {
                    installPath = string.Empty;
                    Console.WriteLine("\tInvalid path!");
                }
            }

            switch (artifactType)
            {
                case DownloadArtifactType.Administration:
                    opts.AdminInstallPath = installPath;
                    break;
                case DownloadArtifactType.WebApi:
                    opts.WebApiInstallPath = installPath;
                    break;
                case DownloadArtifactType.AsyncTask:
                    opts.AsyncTaskProcessorPath = installPath;
                    break;
            }
        }
        #endregion
    }
}
