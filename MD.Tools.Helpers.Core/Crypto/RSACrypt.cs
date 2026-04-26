using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto;
using System.IO;
using Org.BouncyCastle.OpenSsl;
using System.Globalization;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Pkcs;

namespace MD.Tools.Helpers.Core.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    public static class RSACrypt
    {
        /// <summary>
        /// 
        /// </summary>
        public enum KeyPairType
        {
            /// <summary>
            /// 
            /// </summary>
            Public,
            /// <summary>
            /// 
            /// </summary>
            Private
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keySize"></param>
        /// <returns></returns>
        public static IDictionary<KeyPairType, string> GenerateGetPair(int keySize)
        {
            RsaKeyPairGenerator kpgen = new RsaKeyPairGenerator();
            kpgen.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
            AsymmetricCipherKeyPair keyPair = kpgen.GenerateKeyPair();

            Dictionary<KeyPairType, string> keys = new Dictionary<KeyPairType, string>();

            keys.Add(KeyPairType.Private, Convert.ToBase64String(PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private).ToAsn1Object().GetEncoded()));
            keys.Add(KeyPairType.Public, Convert.ToBase64String(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public).GetEncoded()));

            return keys;
        }

        /// <summary>
        /// Encrypt strings with the RSA algorythm
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string inputString, string publicKey)
        {
            if (string.IsNullOrEmpty(inputString))
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new ArgumentException("message", nameof(inputString));
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            if (string.IsNullOrEmpty(publicKey))
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new ArgumentException("message", nameof(publicKey));
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(inputString);

            Pkcs1Encoding encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (StringReader txtreader = new StringReader(string.Format(CultureInfo.InvariantCulture, "-----BEGIN PUBLIC KEY-----\n{0}\n-----END PUBLIC KEY-----\n", publicKey)))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            return Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));

        }

        /// <summary>
        /// Decrypt strings with the RSA algorythm
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string RSADecrypt(string inputString, string privateKey)
        {
            if (string.IsNullOrEmpty(inputString))
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new ArgumentException("message", nameof(inputString));
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            if (string.IsNullOrEmpty(privateKey))
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new ArgumentException("message", nameof(privateKey));
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            byte[] bytesToDecrypt = Encoding.UTF8.GetBytes(inputString);

            Pkcs1Encoding decryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (StringReader txtreader = new StringReader(string.Format(CultureInfo.InvariantCulture, "-----BEGIN PRIVATE KEY-----\n{0}\n-----END PRIVATE KEY-----\n", privateKey)))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                decryptEngine.Init(false, keyParameter);
            }

            return Convert.ToBase64String(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
        }
    }
}
