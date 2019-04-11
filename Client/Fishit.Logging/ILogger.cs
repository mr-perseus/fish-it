using System;

namespace Fishit.Logging
{
    public interface ILogger
    {
        bool IsDebugEnabled { get; }

        void Error(object message);

        void Error(object message, Exception exception);

        void Warn(object message);

        void Warn(object message, Exception exception);

        void Info(object message);

        void Info(object message, Exception exception);

        void Debug(object message);

        void Debug(object message, Exception exception);
    }
}