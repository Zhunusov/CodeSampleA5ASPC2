using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Web.Logging.FileLogger
{
    class FileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly string _categoryName;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly object _lock = new object();
        public FileLogger(string categoryName, Func<string, LogLevel, bool> filter, string path)
        {
            _filePath = path;
            _filter = filter;
            _categoryName = categoryName;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (_filter == null || _filter(_categoryName, logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (formatter != null)
            {
                lock (_lock)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Enum.GetName(typeof(LogLevel), logLevel));
                    sb.Append(": ");
                    sb.Append(formatter(state, exception));
                    sb.Append(Environment.NewLine);
                    File.AppendAllText(_filePath, sb.ToString());
                }
            }
        }
    }
}
