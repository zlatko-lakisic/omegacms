using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace MD.Tools.Helpers.Core.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    public static class MD5Crypt
    {
        /// <summary>
        /// Encrypt input as MD5 hash string.
        /// Cryptographic hash algorithms such as MD2, MD4, MD5, MD6, HAVAL-128, HMAC-MD5, DSA (which uses SHA-1), RIPEMD, RIPEMD-128, RIPEMD-160, HMACRIPEMD160 and SHA-1 are no longer considered secure, because it is too easy to create hash collisions with them (little computational effort is enough to find two or more different inputs that produces the same hash).
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string input)
        {
            StringBuilder sb = new StringBuilder();
            // step 1, calculate MD5 hash from input
#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
            using (MD5 md5 = System.Security.Cryptography.MD5.Create())
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2", CultureInfo.InvariantCulture));
                }
            }
            return sb.ToString();
        }
    }
}
