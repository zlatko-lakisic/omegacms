using Microsoft.Extensions.Options;
using System;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles.Auth
{
    public class BasicAuthenticationPostConfigureOptions : IPostConfigureOptions<BasicAuthenticationOptions>
    {
        public void PostConfigure(string name, BasicAuthenticationOptions options)
        {
            if (string.IsNullOrEmpty(options.Realm))
            {
                throw new InvalidOperationException("Realm must be provided in options");
            }
        }
    }
}
