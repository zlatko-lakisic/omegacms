using MD.CMS.Installer.Hosted.Core.Tools;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MD.CMS.Installer.Hosted.Core.BusinessLogic.Installers
{
    internal class WindowsInstaller : BaseInstaller, IInstaller
    {
        #region Properties
        public OSPlatform Platform => OSPlatform.Windows;
        #endregion

        public Task Run(InstallerOptions opts, DownloadArtifactType artifactType)
        {
            return base.RunInstaller(opts, artifactType);
        }
    }
}
