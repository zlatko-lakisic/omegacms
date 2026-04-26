using MD.CMS.Installer.Hosted.Core.Tools;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MD.CMS.Installer.Hosted.Core.BusinessLogic.Installers
{
    internal class LinuxInstaller : BaseInstaller, IInstaller
    {
        #region Properties
        public OSPlatform Platform => OSPlatform.Linux;
        #endregion

        public Task Run(InstallerOptions opts, DownloadArtifactType artifactType)
        {
            return base.RunInstaller(opts, artifactType);
        }
    }
}
