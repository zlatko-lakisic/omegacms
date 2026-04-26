using System;
using System.IO;

namespace MD.Tools.Licensing
{
    public class ClientKey
    {
        public static string New()
        {
            return Guid.NewGuid().ToString();
        }

        public static void SaveToFile(string filePath, string keyFileName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("message", nameof(filePath));
                }

                if (string.IsNullOrEmpty(keyFileName))
                {
                    keyFileName = Properties.Resources.clientKeyFileName;
                }

                if (!Directory.Exists(Path.Join(filePath, Properties.Resources.licenseDirectory)))
                {
                    Directory.CreateDirectory(Path.Join(filePath, Properties.Resources.licenseDirectory));
                }

                if (!File.Exists(Path.Join(filePath, Properties.Resources.licenseDirectory, keyFileName)))
                {
                    File.WriteAllText(Path.Join(filePath, Properties.Resources.licenseDirectory, keyFileName), New());
                }
            }
            catch(Exception ex)
            {
                throw new LicensingException(LicensingException.LicensingExceptionErrorType.ClientKeyFileSave, ex);
            }
        }
        public static string ReadClientKeyFile(string filePath, string keyFileName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("message", nameof(filePath));
                }

                if (string.IsNullOrEmpty(keyFileName))
                {
                    keyFileName = Properties.Resources.clientKeyFileName;
                }

                return File.ReadAllText(Path.Join(filePath, Properties.Resources.licenseDirectory, keyFileName));
            }
            catch (FileNotFoundException ex)
            {
                throw new LicensingException(LicensingException.LicensingExceptionErrorType.ClientKeyFileMissing, ex);
            }
            catch (Exception ex)
            {
                throw new LicensingException(LicensingException.LicensingExceptionErrorType.ClientKeyFileCorrupt, ex);
            }
        }
    }
}
