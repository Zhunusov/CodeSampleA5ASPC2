using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    /// <summary>
    /// Thread-safe randomizer. Use MerlinUtils.ThreadSafeRandom.
    /// </summary>
    public static class Randomizer
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than System.Int32.MaxValue.</returns>
        public static int GetRandomNumber()
        {
            var threadSafeRandom = new ThreadSafeRandom();
            return threadSafeRandom.Next();
        }

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="max">The exclusive upper bound of the random number to be generated. maxValue must
        /// be greater than or equal to 0.</param>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than maxValue;
        /// that is, the range of return values ordinarily includes 0 but not maxValue. However,
        /// if maxValue equals 0, maxValue is returned.</returns>
        public static int GetRandomNumber(int max)
        {
            var threadSafeRandom = new ThreadSafeRandom();
            return threadSafeRandom.Next(max);
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned. maxValue must be greater
        /// than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue;
        /// that is, the range of return values includes minValue but not maxValue. If minValue
        /// equals maxValue, minValue is returned.</returns>
        public static int GetRandomNumber(int min, int max)
        {
            var threadSafeRandom = new ThreadSafeRandom();
            return threadSafeRandom.Next(min, max);
        }

        /// <summary>
        /// Returns a random System.String of the specified length, consisting of Latin characters and numbers.
        /// </summary>
        /// <param name="length">A random string of the specified length, consisting of Latin characters and numbers.</param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(Chars[GetRandomNumber(0, Chars.Length - 1)]);
            }

            return stringBuilder.ToString();
        }
    }
}
