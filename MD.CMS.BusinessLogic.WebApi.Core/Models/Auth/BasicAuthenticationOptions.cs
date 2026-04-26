using Microsoft.AspNetCore.Authentication;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles.Auth
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }
    }
}
