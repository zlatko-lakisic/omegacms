using MD.Tools.Helpers.Core.FileProvider;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication
{
    public class AuthenticationProviderLoader
    {
        #region Methods
        public static void Load(int provider = (int)FileProviderEnum.Hosted, string path = null)
        {
            AuthenticationProviders.LoadProviders(provider, path);
        }
        #endregion
    }
}
