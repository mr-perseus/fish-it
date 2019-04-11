using System;

namespace Fishit.Logging
{
    public interface ILoggerProvider
    {
        void Configure();

        ILogger GetLogger(string loggerName);

        ILogger GetLogger(Type type);
    }
}