using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.Licensing
{
    public class LicensingException : Exception
    {
        public enum LicensingExceptionErrorType
        {
            LicenseFileMissing,
            LicenseFileCorrupt,
            LicenseFileSave,
            ClientKeyFileSave,
            ClientKeyFileMissing,
            ClientKeyFileCorrupt,
            ServerFileMissing,
            ServerFileCorrupt,
            ServerFileSave,
            LicenseInvalid,
            TooMayUsers,
            DomainMismatch,
            ClientIdFileSave
        }

        private void SetExceptionData(LicensingExceptionErrorType type)
        {
            switch (type)
            {
                case LicensingExceptionErrorType.LicenseFileMissing:
                    this.Data.Add("License Error", "The license file is missing!");
                    break;
                case LicensingExceptionErrorType.LicenseFileCorrupt:
                    this.Data.Add("License Error", "Cannot read or open license file!");
                    break;
                case LicensingExceptionErrorType.LicenseFileSave:
                    this.Data.Add("License Error", "Cannot save license file!");
                    break;
                case LicensingExceptionErrorType.ClientKeyFileMissing:
                    this.Data.Add("License Error", "The client key file is missing!");
                    break;
                case LicensingExceptionErrorType.ClientKeyFileCorrupt:
                    this.Data.Add("License Error", "Cannot read or open client key file!");
                    break;
                case LicensingExceptionErrorType.ClientKeyFileSave:
                    this.Data.Add("License Error", "Cannot save client key file!");
                    break;
                case LicensingExceptionErrorType.ClientIdFileSave:
                    this.Data.Add("License Error", "Cannot save client id file!");
                    break;
                case LicensingExceptionErrorType.ServerFileMissing:
                    this.Data.Add("License Error", "The server key file is missing!");
                    break;
                case LicensingExceptionErrorType.ServerFileCorrupt:
                    this.Data.Add("License Error", "Cannot read or open server key file!");
                    break;
                case LicensingExceptionErrorType.ServerFileSave:
                    this.Data.Add("License Error", "Cannot save server key file!");
                    break;
                case LicensingExceptionErrorType.LicenseInvalid:
                    this.Data.Add("License Error", "The provided license is invalid for this installation!");
                    break;
                case LicensingExceptionErrorType.TooMayUsers:
                    this.Data.Add("License Error", "You have too many users for the license level you are using!");
                    break;
                case LicensingExceptionErrorType.DomainMismatch:
                    this.Data.Add("License Error", "The domains you are using do not match the license you are using!");
                    break;
            }
        }

        public LicensingException(LicensingExceptionErrorType type) : base("A licensing error occured!")
        {
            SetExceptionData(type);
        }

        public LicensingException(LicensingExceptionErrorType type, Exception innerException) : base("A licensing error occured!", innerException)
        {
            SetExceptionData(type);
        }
    }
}
