using System;

namespace Fishit.Logging
{
    public class Logger : ILogger
    {
        private const string LogLineFormat = "{0}; {1,-5}; {2}";

        private readonly string _loggerName;
        private readonly LogLevel _logLevel;

        private Logger()
        {
            _logLevel = LogLevel.Debug;
        }

        public Logger(string loggerName)
            : this()
        {
            _loggerName = string.IsNullOrEmpty(loggerName) ? "NULL" : loggerName;
        }

        public bool IsDebugEnabled => _logLevel == LogLevel.Debug;

        public void Error(object message)
        {
            if (_logLevel > LogLevel.Error)
            {
                return;
            }

            Console.WriteLine(LogLineFormat, _loggerName, LogLevel.Error.ToString(), message);
        }

        public void Error(object message, Exception exception)
        {
            if (_logLevel > LogLevel.Error)
            {
                return;
            }

            Console.WriteLine(LogLineFormat, _loggerName, LogLevel.Error.ToString(), message);
            Console.WriteLine(exception);
        }

        public void Warn(object message)
        {
            if (_logLevel > LogLevel.Warn)
            {
                return;
            }

            Console.WriteLine(LogLineFormat, _loggerName, LogLevel.Warn.ToString(), message);
        }

        public void Warn(object message, Exception exception)
        {
            if (_logLevel > LogLevel.Warn)
            {
                return;
            }

            Console.WriteLine(LogLineFormat, _loggerName, LogLevel.Warn.ToString(), message);
            Console.WriteLine(exception);
        }

        public void Info(object message)
        {
            if (_logLevel > LogLevel.Info)
            {
                return;
            }

            Console.WriteLine(LogLineFormat, _loggerName, LogLevel.Info.ToString(), message);
        }

        public void Info(object message, Exception exception)
        {
            if (_logLevel > LogLevel.Info)
            {
                return;
            }

            Console.WriteLine(LogLineFormat, _loggerName, LogLevel.Info.ToString(), message);
            Console.WriteLine(exception);
        }

        public void Debug(object message)
        {
            if (_logLevel > LogLevel.Debug)
            {
                return;
            }

            Console.WriteLine(LogLineFormat, _loggerName, LogLevel.Debug.ToString(), message);
        }

        public void Debug(object message, Exception exception)
        {
            if (_logLevel > LogLevel.Debug)
            {
                return;
            }

            Console.WriteLine(LogLineFormat, _loggerName, LogLevel.Debug.ToString(), message);
            Console.WriteLine(exception);
        }
    }
}