using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    /// <summary>
    /// Unix time converter.
    /// </summary>
    public static class UnixTimeUtil
    {
        /// <summary>
        /// Get unix time now.
        /// </summary>
        public static double UnixTimeNow => GetUnixTyime(DateTime.Now);

        /// <summary>
        /// Convert unix time to DateTime.
        /// </summary>
        /// <param name="timestamp">Unix to convert.</param>
        /// <returns>DateTime of timestamp.</returns>
        public static DateTime GetDateTime(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        /// <summary>
        /// Convert DateTime to unix time.
        /// </summary>
        /// <param name="date">DateTime to convert.</param>
        /// <returns>Unix time of date.</returns>
        public static double GetUnixTyime(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = date - origin;
            return diff.TotalSeconds;
        }
    }
}
