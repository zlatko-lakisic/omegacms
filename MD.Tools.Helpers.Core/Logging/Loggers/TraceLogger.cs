using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MD.Tools.Helpers.Core.Logging.Loggers
{
    /// <summary>
    /// Provides Trace functionality for logging.  Can be configured to trigger Trace.Assert statements
    /// </summary>
    public class TraceLogger : MD.Tools.Helpers.Core.Logging.IAssertLogger
    {
        private static CustomTraceSwitch _switch => new CustomTraceSwitch(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TraceLoggerTraceSwitch, "Defines which messages the Trace Logger should record", "4");

        private static CustomTraceSwitch _assertSwitch => new CustomTraceSwitch(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TraceAssertLoggerTraceSwitch, "Defines which messages the Trace Logger should call Trace.Assert for", "1");
        /// <summary>
        /// 
        /// </summary>
        public string LoggerName => "TraceLogger";

        /// <summary>
        /// The message format
        /// </summary>
        private const string MessageFormat = "{0:HH:mm:ss} - {1}";

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

            if (_switch.TraceError) Trace.TraceError(FormatMessage(exception.ToString()));
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

            if (_switch.TraceError) Trace.TraceError(FormatMessage(string.Concat(message, Environment.NewLine, exception.ToString())));
        }

        private static string FormatMessage(string message)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, MessageFormat, DateTime.Now, message);

        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogInformation(string message)
        {
            if (_switch.TraceInfo) Trace.TraceInformation(FormatMessage(message));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogWarning(string message)
        {
            if (_switch.TraceWarning) Trace.TraceWarning(FormatMessage(message));
        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogVerbose(string message)
        {
            if (_switch.TraceVerbose) Trace.TraceInformation(FormatMessage(message));
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogError(string message)
        {
            if (_switch.TraceError) Trace.TraceError(FormatMessage(message));
        }

        /// <summary>
        /// Gets a value indicating whether this instance is ready to log information
        /// </summary>
        /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
        public bool IsAvailable
        {
            get
            {
                return MD.Tools.Helpers.Core.Properties.HelperSettings.Default.TraceLoggerEnabled;

            }
        }

        #endregion

        #region IAssertLogger Members

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> the message is not logged</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void AssertLogInformation(bool expectedCondition, string message)
        {
            if (!expectedCondition) LogInformation(message);
            if (_assertSwitch.TraceInfo) Trace.Assert(expectedCondition, message);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c>  the message is not logged</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void AssertLogWarning(bool expectedCondition, string message)
        {
            if (!expectedCondition) LogWarning(message);
            if (_assertSwitch.TraceWarning) Trace.Assert(expectedCondition, message);
        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c>  the message is not logged</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void AssertLogVerbose(bool expectedCondition, string message)
        {
            if (!expectedCondition) LogVerbose(message);
            if (_assertSwitch.TraceVerbose) Trace.Assert(expectedCondition, message);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c>  the message is not logged</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void AssertLogError(bool expectedCondition, string message)
        {
            if (!expectedCondition) LogError(message);
            if (_assertSwitch.TraceError) Trace.Assert(expectedCondition, message);
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
            return _switch.Level >= level || _assertSwitch.Level >= level;
        }

        #region ILogger Members


        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(LogMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (IsAvailable && IsEnabledAtLevel(message.Level)) Trace.WriteLine(message.FormattedMessage);
        }

        #endregion

        #region IAssertLogger Members

        /// <summary>
        /// Asserts the log.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AssertLog(LogMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (IsAvailable && IsEnabledAtLevel(message.Level)) Trace.Assert(message.PassedAssertCondition, message.FormattedMessage);
        }

        #endregion
    }
}
