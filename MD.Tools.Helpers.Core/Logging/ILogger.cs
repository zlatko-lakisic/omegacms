using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.Helpers.Core.Logging
{
    /// <summary>
    /// Defines the methods that all loggers must implement
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        void Log(Exception exception);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Log(LogMessage message);

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        void Log(string message, Exception exception);

        /// <summary>
        /// Determines whether [is enabled at level] [the specified level].
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>
        /// 	<c>true</c> if [is enabled at level] [the specified level]; otherwise, <c>false</c>.
        /// </returns>
        bool IsEnabledAtLevel(System.Diagnostics.TraceLevel level);

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        void LogInformation(string message);

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        void LogWarning(string message);

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        void LogVerbose(string message);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        void LogError(string message);

        /// <summary>
        /// Gets a value indicating whether this instance is ready to log information
        /// </summary>
        /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
        bool IsAvailable { get; }

        /// <summary>
        /// Gets the logger name
        /// </summary>
        string LoggerName { get; }
    }
}
