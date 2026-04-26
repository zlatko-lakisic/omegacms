using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles.Auth
{
    public interface IBasicAuthenticationService
    {
        Task<bool> IsValidUserAsync(string user, string password);
    }
}
