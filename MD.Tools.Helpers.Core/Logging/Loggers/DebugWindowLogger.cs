using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MD.Tools.Helpers.Core.Logging.Loggers
{
    /// <summary>
    /// Simply outputs the logged messages to the debug console
    /// </summary>
    public sealed class DebugWindowLogger : ILogger
    {
        private static CustomTraceSwitch _switch => new CustomTraceSwitch(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.DebugLoggerTraceSwitch, "Defines which messages the DebugWindow Logger should record", "4");

        /// <summary>
        /// The message format
        /// </summary>
        private const string MessageFormat = "{0:HH:mm:ss} - {1}";

        /// <summary>
        /// 
        /// </summary>
        public string LoggerName => "DebugWindowLogger";

        #region ILogger Members

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public void Log(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (_switch.TraceError) LogMessage(exception.ToString());
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public void Log(string message, Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (_switch.TraceError) LogMessage(string.Concat(message, Environment.NewLine, exception.ToString()));
        }

        [Conditional("DEBUG")]
        private static void LogMessage(string message)
        {
#if DEBUG
            string messageRW = string.Format(System.Globalization.CultureInfo.InvariantCulture, MessageFormat, DateTime.Now, message);
            Debug.WriteLine(messageRW);
#endif
        }

        [Conditional("DEBUG")]
        private static void LogMessage(string message, DateTime logTime)
        {
#if DEBUG
            string messageRW = string.Format(System.Globalization.CultureInfo.InvariantCulture, MessageFormat, logTime, message);
            Debug.WriteLine(messageRW);
#endif
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogInformation(string message)
        {
            if (_switch.TraceInfo) LogMessage(message);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogWarning(string message)
        {
            if (_switch.TraceWarning) LogMessage(message);
        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogVerbose(string message)
        {
            if (_switch.TraceVerbose) LogMessage(message);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogError(string message)
        {
            if (_switch.TraceError) LogInformation(message);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is ready to log information
        /// </summary>
        /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
        public bool IsAvailable
        {
            get
            {
#if DEBUG
                return System.Diagnostics.Debugger.IsAttached;
#else
                return false;
#endif
            }
        }
        #endregion


        /// <summary>
        /// Determines whether [is enabled at level] [the specified level].
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>
        /// 	<c>true</c> if [is enabled at level] [the specified level]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEnabledAtLevel(TraceLevel level)
        {
            return IsAvailable && _switch.Level >= level;
        }



        #region ILogger Members


        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(LogMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (IsEnabledAtLevel(message.Level)) LogMessage(message.FormattedMessage, message.LoggedAt);
        }

        #endregion
    }
}
