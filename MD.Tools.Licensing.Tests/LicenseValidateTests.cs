using MD.Tools.Helpers.Core.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MD.Tools.Licensing.Tests
{
    [TestClass()]
    public class LicenseValidateTests
    {
        [TestMethod()]
        public void ValidateLicenseTest()
        {
            ComponentEnum component = ComponentEnum.Administration;
            int users = 20;
            string[] domains = new string[] { "omegacms.io" };
            string majorVersion = "1.0.0";

            //Get Client Key
            string clientKey = Licensing.ClientKey.New();

            //Get Client Key
            string clientId = Licensing.ClientId.GetClientId(majorVersion, clientKey);

            //Get Generated License
            License generatedLicense = LicenseBuilder.BuildLicense(component, majorVersion, clientKey, users, domains);

            //Generate Server Key
            string serverKey = Guid.NewGuid().ToString();
            string serverEncryptKey = clientId + serverKey;

            //Generate License
            string license = AESCrypt.Encrypt(JsonConvert.SerializeObject(generatedLicense), serverEncryptKey);

            Assert.IsTrue(LicenseValidate.ValidateLicense(license, serverKey, component, majorVersion, clientKey, users, domains));
        }
    }
}