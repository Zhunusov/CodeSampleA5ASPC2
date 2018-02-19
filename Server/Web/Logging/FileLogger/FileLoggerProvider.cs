using System;
using Microsoft.Extensions.Logging;

namespace Web.Logging.FileLogger
{
    class FileLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;

        private readonly string _path;

        public FileLoggerProvider(Func<string, LogLevel, bool> filter, string path)
        {
            _path = path;
            _filter = filter;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, _filter, _path);
        }

        public void Dispose()
        {
        }
    }
}
