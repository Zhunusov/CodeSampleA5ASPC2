using System;
using Domain.Logging;
using Microsoft.Extensions.Logging;
using Utils;

namespace Web.Logging.DataBaseLogger
{
    class DataBaseLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly SqlHelper _helper;
        private const int MessageMaxLength = 4000;

        public DataBaseLogger(string categoryName, Func<string, LogLevel, bool> filter, string connectionString)
        {
            _categoryName = categoryName;
            _filter = filter;
            _helper = new SqlHelper(connectionString);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            if (exception != null)
            {
                message += "\n" + exception;
            }
            message = message.Length > MessageMaxLength ? message.Substring(0, MessageMaxLength) : message;
            ServerEvent serverEvent = new ServerEvent
            {
                Message = message,
                EventId = eventId.Id,
                LogLevel = (int)logLevel,
                Time = DateTime.Now
            };
            _helper.InsertLog(serverEvent);
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter == null || _filter(_categoryName, logLevel);
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
