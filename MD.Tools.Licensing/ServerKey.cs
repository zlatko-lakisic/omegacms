using System;
using System.IO;

namespace MD.Tools.Licensing
{
    public class ServerKey
    {
        public static string ReadServerKeyFile(string filePath, string keyFileName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("message", nameof(filePath));
                }

                if (string.IsNullOrEmpty(keyFileName))
                {
                    keyFileName = Properties.Resources.serverKeyFileName;
                }

                return File.ReadAllText(Path.Join(filePath, Properties.Resources.licenseDirectory, keyFileName));
            }
            catch (FileNotFoundException ex)
            {
                throw new LicensingException(LicensingException.LicensingExceptionErrorType.ServerFileMissing, ex);
            }
            catch (Exception ex)
            {
                throw new LicensingException(LicensingException.LicensingExceptionErrorType.ServerFileCorrupt, ex);
            }
        }
    }
}
