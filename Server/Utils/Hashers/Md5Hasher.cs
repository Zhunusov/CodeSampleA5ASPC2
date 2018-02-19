using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Hashers
{
    /// <summary>
    /// MD5 hasher.
    /// </summary>
    public static class Md5Hasher
    {
        /// <summary>
        /// Hesher a string using the algorithm md5.
        /// </summary>
        /// <param name="input">String for hashing.</param>
        /// <returns>MD 5 hash string.</returns>
        public static string Hash(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Checks if the hash matches the resulting string.
        /// </summary>
        /// <param name="hash">Hash string.</param>
        /// <param name="input">The string to check.</param>
        /// <returns>The result of the hash and string matching.</returns>
        public static bool VerifyHashe(string hash, string input)
        {
            return hash == Hash(input);
        }
    }
}
