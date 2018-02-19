using System;
using Microsoft.Extensions.Logging;

namespace Web.Logging.FileLogger
{
    static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath, Func<string, LogLevel, bool> filter = null)
        {
            factory.AddProvider(new FileLoggerProvider(filter, filePath));
            return factory;
        }

        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath,LogLevel minLevel)
        {
            factory.AddProvider(new FileLoggerProvider((_, logLevel) => logLevel >= minLevel, filePath));
            return factory;
        }
    }
}
