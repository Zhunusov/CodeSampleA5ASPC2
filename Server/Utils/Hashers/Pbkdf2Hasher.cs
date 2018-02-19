using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Utils.Hashers
{
    /// <summary>
    /// PBKDF2 hasher.
    /// </summary>
    public static class Pbkdf2Hasher
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int HasingIterationsCount = 10101;

        /// <summary>
        /// Hesher a string using the algorithm PBKDF2.
        /// </summary>
        /// <param name="input">String for hashing.</param>
        /// <returns></returns>
        public static string Hash(string input)
        {
            byte[] salt;
            byte[] buffer2;
            if (input == null)
            {
                throw new ArgumentNullException("password");
            }
            using (var bytes = new Rfc2898DeriveBytes(input, SaltByteSize, HasingIterationsCount))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(HashByteSize);
            }
            byte[] dst = new byte[(SaltByteSize + HashByteSize) + 1];
            Buffer.BlockCopy(salt, 0, dst, 1, SaltByteSize);
            Buffer.BlockCopy(buffer2, 0, dst, SaltByteSize + 1, HashByteSize);
            return Convert.ToBase64String(dst);
        }

        /// <summary>
        /// Checks if the hash matches the resulting string.
        /// </summary>
        /// <param name="hash">Hash string.</param>
        /// <param name="input">The string to check.</param>
        /// <returns>The result of the hash and string matching.</returns>
        public static bool VerifyHashe(string hash, string input)
        {
            byte[] inputHashBytes;

            var arrayLen = (SaltByteSize + HashByteSize) + 1;

            if (hash == null)
            {
                return false;
            }

            if (input == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] src = Convert.FromBase64String(hash);

            if (src.Length != arrayLen || src[0] != 0)
            {
                return false;
            }

            var currentSaltBytes = new byte[SaltByteSize];
            Buffer.BlockCopy(src, 1, currentSaltBytes, 0, SaltByteSize);

            var currentHashBytes = new byte[HashByteSize];
            Buffer.BlockCopy(src, SaltByteSize + 1, currentHashBytes, 0, HashByteSize);

            using (var bytes = new Rfc2898DeriveBytes(input, currentSaltBytes, HasingIterationsCount))
            {
                inputHashBytes = bytes.GetBytes(SaltByteSize);
            }

            return AreHashesEqual(currentHashBytes, inputHashBytes);
        }

        private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            var minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (var i = 0; i < minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }
    }
}
