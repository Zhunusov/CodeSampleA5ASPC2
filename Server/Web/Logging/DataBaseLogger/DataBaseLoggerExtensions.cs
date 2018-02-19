using System;
using Microsoft.Extensions.Logging;

namespace Web.Logging.DataBaseLogger
{
    static class DataBaseLoggerExtensions
    {
        public static ILoggerFactory AddContext(this ILoggerFactory factory,
            Func<string, LogLevel, bool> filter = null, string connectionStr = null)
        {
            factory.AddProvider(new DataBaseLoggerProvider(filter, connectionStr));
            return factory;
        }

        public static ILoggerFactory AddContext(this ILoggerFactory factory, LogLevel minLevel, string connectionStr)
        {
            return AddContext(
                factory,
                (_, logLevel) => logLevel >= minLevel, connectionStr);
        }
    }
}
