using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication
{
    public interface IAuthUser : IUser
    {
        string AuthDataString { get; set; }
    }
}
