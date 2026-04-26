using System;
using System.Text;
using System.Security.Cryptography;

namespace MD.Tools.Helpers.Core.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    public static class AESCrypt
    {
        /// <summary>
        /// 
        /// </summary>
        public enum KeySize: int
        {
#pragma warning disable CA1707 // Identifiers should not contain underscores
            /// <summary>
            /// 
            /// </summary>
            _128 = 128,
            /// <summary>
            /// 
            /// </summary>
            _192 = 192,
            /// <summary>
            /// 
            /// </summary>
            _256 = 256
#pragma warning restore CA1707 // Identifiers should not contain underscores
        }

        #region Methods
        private static RijndaelManaged GetRijndaelManaged(string secretKey, KeySize keySize)
        {
            int keySizeNumber = 0;
            switch (keySize)
            {
                case KeySize._128:
                    keySizeNumber = 16;
                    break;
                case KeySize._192:
                    keySizeNumber = 24;
                    break;
                case KeySize._256:
                    keySizeNumber = 36;
                    break;
            }
            var keyBytes = new byte[keySizeNumber];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = (int)keySize,
                BlockSize = (int)keySize,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            if (plainBytes == null)
            {
                throw new ArgumentNullException(nameof(plainBytes));
            }
            if (rijndaelManaged == null)
            {
                throw new ArgumentNullException(nameof(rijndaelManaged));
            }

            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        private static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            if (encryptedData == null)
            {
                throw new ArgumentNullException(nameof(encryptedData));
            }
            if (rijndaelManaged == null)
            {
                throw new ArgumentNullException(nameof(rijndaelManaged));
            }

            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="keySize"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string key, KeySize keySize = KeySize._128)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            using (RijndaelManaged rijnadel = GetRijndaelManaged(key, keySize))
            {
                return Convert.ToBase64String(Encrypt(plainBytes, rijnadel));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="key"></param>
        /// <param name="keySize"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedText, string key, KeySize keySize = KeySize._128)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            using (RijndaelManaged rijnadel = GetRijndaelManaged(key, keySize)) 
            {
                return Encoding.UTF8.GetString(Decrypt(encryptedBytes, rijnadel));
            }
        }
        #endregion
    }
}
