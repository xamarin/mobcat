using System;

namespace Microsoft.MobCAT.Services
{
    public enum LogType
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        FATAL
    }

    public interface ILoggingService
    {
        /// <summary>
        /// Logs the specified message under the specified log type.
        /// </summary>
        /// <returns>The log.</returns>
        /// <param name="logType">Log type.</param>
        /// <param name="message">Message.</param>
        void Log(LogType logType, string message);

        /// <summary>
        /// Logs the specified message and exception under the specified log type.
        /// </summary>
        /// <returns>The log.</returns>
        /// <param name="logType">Log type.</param>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exception.</param>
        void Log(LogType logType, string message, Exception exception);
    }
}