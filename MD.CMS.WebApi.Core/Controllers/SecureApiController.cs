/*using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using System;
using System.IO;

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;

namespace MD.CMS.WebApi.Core.Controllers
{
    public class SecureApiController : BaseLoggedOnWebApiController
    {
        string key = "I/YGVv0Toc81seeRd+CipEsNGFXQhaCb1HVIlkKd8vY=";
        string iv = "OytxFiJFA6PzjbaovbzaDg==";


        [HttpPost]
        [ActionName("GenerateToken")]
        public IActionResult GenerateToken(ClientRSAModel obj)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

            //string key = Convert.ToBase64String(aes.Key);
            //string iv = Convert.ToBase64String(aes.IV);

            string stringToEncrypt = "{\"key\":\"" + key + "\", \"iv\":\"" + iv + "\"}";

            string encrypted = RsaEncryptWithPublic(stringToEncrypt, obj.pem);
            
            return Ok(encrypted);
        }

        private string RsaEncryptWithPublic(string clearText, string publicKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;

        }

    }
}
*/