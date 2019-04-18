using System;

namespace Fishit.Logging
{
    public static class LogManager
    {
        private static bool _configured;

        private static ILoggerProvider LoggerProvider => null;

        public static ILogger GetLogger(string name)
        {
            Configure();

            return LoggerProvider == null ? new Logger(name) : LoggerProvider.GetLogger(name);
        }

        public static ILogger GetLogger(Type type)
        {
            Configure();

            return LoggerProvider == null ? new Logger(type.FullName) : LoggerProvider.GetLogger(type);
        }

        private static void Configure()
        {
            if (LoggerProvider == null || _configured)
                return;

            LoggerProvider.Configure();
            _configured = true;
        }
    }
}