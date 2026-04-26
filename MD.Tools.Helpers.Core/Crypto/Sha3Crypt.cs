using System;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;
using System.Globalization;

namespace MD.Tools.Helpers.Core.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    public static class Sha3Crypt
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toEncrypt">String to encrypt</param>
        /// <param name="bitLength">Default to 512 bits</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, int bitLength = 512)
        {
            Sha3Digest hashAlgorithm = new Sha3Digest(bitLength);

            // Choose correct encoding based on your usecase
            byte[] input = Encoding.ASCII.GetBytes(toEncrypt);

            hashAlgorithm.BlockUpdate(input, 0, input.Length);

            byte[] result = new byte[bitLength/8];
            hashAlgorithm.DoFinal(result, 0);

            string hashString = BitConverter.ToString(result);
#pragma warning disable CA1308 // Normalize strings to uppercase
            return hashString.Replace("-", "", true, CultureInfo.InvariantCulture).ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
        }
        #endregion
    }
}
