using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MD.Tools.Licensing
{
    public class License
    {
        public ComponentEnum Component { get; set; }
        public int Users { get; set; }
        public string[] Domains { get; set; }
        public string ClientId { get; set; }
        public string VersionRegex { get; set; }
        public long Expiration { get; set; }

        public static string ReadLicenseFile(string filePath, string licenseFileName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("message", nameof(filePath));
                }

                if (string.IsNullOrEmpty(licenseFileName))
                {
                    licenseFileName = Properties.Resources.licenseFileName;
                }

                return File.ReadAllText(Path.Join(filePath, Properties.Resources.licenseDirectory, licenseFileName));
            }
            catch (FileNotFoundException ex)
            {
                throw new LicensingException(LicensingException.LicensingExceptionErrorType.LicenseFileMissing, ex);
            }
            catch (Exception ex)
            {
                throw new LicensingException(LicensingException.LicensingExceptionErrorType.LicenseFileCorrupt, ex);
            }
        }
    }
}
