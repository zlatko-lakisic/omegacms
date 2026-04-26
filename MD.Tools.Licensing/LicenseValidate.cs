using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MD.Tools.Licensing
{
    public class LicenseValidate
    {
        public static bool ValidateLicense(string license, string serverKey, ComponentEnum component, string majorVersion, string clientKey, int users, params string[] domains)
        {
            if (string.IsNullOrEmpty(license))
            {
                throw new ArgumentException("License cannot be null or empty!", nameof(license));
            }

            if (string.IsNullOrEmpty(serverKey))
            {
                throw new ArgumentException("Server key cannot be null or empty!", nameof(serverKey));
            }

            try
            {
                License purchasedLicense = JsonConvert.DeserializeObject<License>(Helpers.Core.Crypto.AESCrypt.Decrypt(license, ClientId.GetClientId(majorVersion, clientKey) + serverKey));

                if (!purchasedLicense.Domains.Select(domain => $"^{domain.Replace(".", @"\.").Replace("*", @"(.+)")}$").Any(domain => domains.Any(d => new Regex(domain).IsMatch(d))))
                {
                    throw new LicensingException(LicensingException.LicensingExceptionErrorType.DomainMismatch);
                }

                return new Regex(purchasedLicense.VersionRegex).IsMatch(majorVersion) &&
                    purchasedLicense.ClientId == ClientId.GetClientId(majorVersion, clientKey) &&
                    purchasedLicense.Users >= users &&
                    purchasedLicense.Component == component &&
                    purchasedLicense.Expiration > DateTime.Now.Ticks;
            }
            catch
            {
                return false;
            }
        }
    }
}