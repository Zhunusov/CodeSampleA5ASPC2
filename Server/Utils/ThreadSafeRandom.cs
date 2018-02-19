using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    /// <summary>
    /// Thread-safe wrapper for System.Random.
    /// </summary>
    public sealed class ThreadSafeRandom
    {
        private static readonly Random Global = new Random();
        [ThreadStatic] private static Random _local;

        /// <summary>
        /// Initializes a new instance of the MerlinUtils.ThreadSafeRandom class.
        /// </summary>
        public ThreadSafeRandom()
        {
            if (_local == null)
            {
                int seed;
                lock (Global)
                {
                    seed = Global.Next();
                }
                _local = new Random(seed);
            }
        }

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than System.Int32.MaxValue.</returns>
        public int Next()
        {
            return _local.Next();
        }

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="max">The exclusive upper bound of the random number to be generated. maxValue must
        /// be greater than or equal to 0.</param>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than maxValue;
        /// that is, the range of return values ordinarily includes 0 but not maxValue. However,
        /// if maxValue equals 0, maxValue is returned.</returns>
        public int Next(int max)
        {
            return _local.Next(max);
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
        public int Next(int min, int max)
        {
            return _local.Next(min, max);
        }
    }
}
