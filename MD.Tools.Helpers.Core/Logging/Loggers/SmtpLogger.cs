using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Net.Mail;
using MD.Tools.Helpers.Core.TypeConversion;

namespace MD.Tools.Helpers.Core.Logging.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    public class SmtpLogger : ILogger
    {
        private static CustomTraceSwitch _switch => new CustomTraceSwitch(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.SmtpLoggerTraceSwitch, "Defines which messages the SMTP Logger should record", "0");
        private static SmtpClient _client = new SmtpClient();
        /// <summary>
        /// 
        /// </summary>
        public string LoggerName => "SmtpLogger";

        #region ILogger Members

        private static void SendEmail(TraceLevel level, string message)
        {
            if (string.IsNullOrEmpty(message)) message = string.Empty;
            SendEmail(level, message, message, Math.Abs(message.GetHashCode(StringComparison.OrdinalIgnoreCase)).ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="detail">The detail.</param>
        /// <param name="category">The category.</param>
        private static void SendEmail(TraceLevel level, string message, string detail, string category)
        {
            if (string.IsNullOrEmpty(message)) message = string.Empty;
            if (string.IsNullOrEmpty(detail)) detail = message;
            if (string.IsNullOrEmpty(category)) category = Math.Abs(message.GetHashCode(StringComparison.OrdinalIgnoreCase)).ToString(CultureInfo.InvariantCulture);
            using (MailMessage msg = new MailMessage())
            {
                foreach (string s in MD.Tools.Helpers.Core.Properties.HelperSettings.Default.SmtpToAddress)
                {
                    msg.To.Add(s);
                }
                msg.Subject = "{0}:{1}/{2} - {3} {4:s}".ToFormattedString(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.SmtpApplication, level, category, message, DateTime.Now);
                msg.Body = detail;
                _client.Send(msg);
            }
        }

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

            if (_switch.TraceError) SendEmail(TraceLevel.Error, exception.Message, exception.ToString(), "{0}/{1}".ToFormattedString(Math.Abs(exception.Message.GetHashCode(StringComparison.OrdinalIgnoreCase)), Math.Abs(exception.ToString().GetHashCode(StringComparison.OrdinalIgnoreCase))));
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

            if (_switch.TraceError) SendEmail(TraceLevel.Error, message, exception.ToString(), "{0}/{1}".ToFormattedString(Math.Abs(exception.Message.GetHashCode(StringComparison.OrdinalIgnoreCase)), Math.Abs(exception.ToString().GetHashCode(StringComparison.OrdinalIgnoreCase))));
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogInformation(string message)
        {
            if (_switch.TraceInfo) SendEmail(TraceLevel.Info, message);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogWarning(string message)
        {
            if (_switch.TraceWarning) SendEmail(TraceLevel.Warning, message);
        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogVerbose(string message)
        {
            if (_switch.TraceVerbose) SendEmail(TraceLevel.Verbose, message);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogError(string message)
        {
            if (_switch.TraceError) SendEmail(TraceLevel.Error, message);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is ready to log information
        /// </summary>
        /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
        public bool IsAvailable
        {
            get
            {
                return MD.Tools.Helpers.Core.Properties.HelperSettings.Default.SmtpLoggerIsEnabled
                    &&
                    (
                        (_client.DeliveryMethod == SmtpDeliveryMethod.Network && !string.IsNullOrEmpty(_client.Host))
                        ||
                        (_client.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory && !string.IsNullOrEmpty(_client.PickupDirectoryLocation))
                    );
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
            return _switch.Level >= level;
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(LogMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (IsAvailable && IsEnabledAtLevel(message.Level)) SendEmail(message.Level, message.FormattedMessage);
        }

    }
}
