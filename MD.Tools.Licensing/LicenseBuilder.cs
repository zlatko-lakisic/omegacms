using System;
using System.Runtime.InteropServices;

namespace MD.Tools.Licensing
{
    public class LicenseBuilder
    {
        public static License BuildLicense(ComponentEnum component, string majorVersion, string clientKey, int users, params string[] domains)
        {
            if (string.IsNullOrEmpty(majorVersion))
            {
                throw new ArgumentException("Major version cannot be null or empty!", nameof(majorVersion));
            }

            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentException("Client key cannot be null or empty!", nameof(clientKey));
            }

            if (domains is null)
            {
                throw new ArgumentNullException(nameof(domains));
            }

            return new License()
            {
                ClientId = ClientId.GetClientId(majorVersion, clientKey),
                Users = users,
                Domains = domains,
                Component = component
            };
        }
    }
}
