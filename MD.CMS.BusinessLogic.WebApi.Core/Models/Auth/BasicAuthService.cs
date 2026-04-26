using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles.Auth
{
    public class BasicAuthService : IBasicAuthenticationService
    {
        public Task<bool> IsValidUserAsync(string user, string password)
        {
            return null;
        }
    }
}
