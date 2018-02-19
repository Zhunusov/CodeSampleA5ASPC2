using System;
using Microsoft.Extensions.Logging;

namespace Web.Logging.DataBaseLogger
{
    class DataBaseLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _connectionString;
        public DataBaseLoggerProvider(Func<string, LogLevel, bool> filter, string connectionStr)
        {
            _filter = filter;
            _connectionString = connectionStr;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DataBaseLogger(categoryName, _filter, _connectionString);
        }
        public void Dispose()
        {
        }
    }
}
