using MD.CMS.Installer.Hosted.Core.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MD.CMS.Installer.Hosted.Core.BusinessLogic
{
    internal interface IInstaller
    {
        OSPlatform Platform { get; }
        Task Run(InstallerOptions opts, DownloadArtifactType artifactType);
    }
}
