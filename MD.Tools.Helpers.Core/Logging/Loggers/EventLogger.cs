using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Web;
using MD.Tools.Helpers.Core.TypeConversion;

namespace MD.Tools.Helpers.Core.Logging.Loggers
{
    /// <summary>
    /// Publishes received messages to the event log
    /// </summary>
    public sealed class EventLogger : ILogger
    {
        private static CustomTraceSwitch _switch => new CustomTraceSwitch(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.EventLoggerTraceSwitch, "Defines which messages the  Event Log Logger should record", "2");

        /// <summary>
        /// The shared event log object
        /// </summary>
        private static EventLog _eventLog;

        /// <summary>
        /// 
        /// </summary>
        public string LoggerName => "EventLogger";

        /// <summary>
        /// Resolves the event log.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static EventLog ResolveEventLog()
        {
            if (_eventLog == null)
            {
                string sourceName = MD.Tools.Helpers.Core.Properties.HelperSettings.Default.EventLoggerSourceName;
                string logName = MD.Tools.Helpers.Core.Properties.HelperSettings.Default.EventLoggerLogName;
                try
                {
                    if (!EventLog.Exists(logName))
                    {
                        EventSourceCreationData escd = new EventSourceCreationData(sourceName, logName);
                        EventLog.CreateEventSource(escd);
                        SetLog(sourceName, logName, true);
                    }
                    else
                    {
                        SetLog(sourceName, logName, false);
                    }
                }
                catch //HACK: No permissions to create event log
                {
                    Trace.Write(string.Format(System.Globalization.CultureInfo.InvariantCulture, @"Failed to create log {0}\{1}", sourceName, logName));
                }

            }
            return _eventLog;
        }

        private static void SetLog(string sourceName, string logName, bool isNew)
        {

            _eventLog = new EventLog { Source = sourceName, Log = logName };
            if (isNew && _eventLog.OverflowAction != OverflowAction.OverwriteAsNeeded) _eventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 1);
            _eventLog.WriteEntry("Initialised Log", EventLogEntryType.Information);
        }  

        /// <summary>
        /// Writes the entry.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        private static void WriteEntry(string message, EventLogEntryType type)
        {
            EventLog log = ResolveEventLog();
            if (log == null) return;
            log.WriteEntry(message, type);
        }



        #region ILogger Members

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public void Log(Exception exception)
        {
            if (exception != null && _switch.TraceError) WriteEntry(exception.ToString(), EventLogEntryType.Error);
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public void Log(string message, Exception exception)
        {
            if (exception != null && _switch.TraceError) WriteEntry(string.Concat(message, Environment.NewLine, exception.ToString()), EventLogEntryType.Error);
        }


        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogInformation(string message)
        {
            if (_switch.TraceInfo) WriteEntry(message, EventLogEntryType.Information);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogWarning(string message)
        {
            if (_switch.TraceWarning) WriteEntry(message, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogVerbose(string message)
        {
            if (_switch.TraceVerbose) WriteEntry(message, EventLogEntryType.Information);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public void LogError(string message)
        {
            if (_switch.TraceError) WriteEntry(message, EventLogEntryType.Error);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is ready to log information
        /// </summary>
        /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
        public bool IsAvailable
        {
            get { return MD.Tools.Helpers.Core.Properties.HelperSettings.Default.EventLoggerIsEnabled /*&& CanCreateEventLog.GetValueOrDefault(true)*/; }
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

        #region ILogger Members


        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Log(LogMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (IsAvailable && IsEnabledAtLevel(message.Level))
            {
               StringBuilder msge = new StringBuilder(message.FormattedMessage);
                
               foreach(string key in message.ExtraInformation.Keys)
               {
                   msge.AppendLine("\n");
                   AddParameters(message.ExtraInformation[key], msge , key);
               }
               WriteEntry(msge.ToString(), EntryType(message.Level));
            }
        }

        private static EventLogEntryType EntryType(TraceLevel level)
        {
            if (level == TraceLevel.Error) return EventLogEntryType.Error;
            if (level == TraceLevel.Warning) return EventLogEntryType.Warning;
            return EventLogEntryType.Information;
        }

        private static string _messageFormat = "\t{0:-15} : {1}\n";

        private static void AddExtraDetail(StringBuilder extraInfo, string key, string value)
        {
            extraInfo.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, _messageFormat, key, value);
        }


        private static void AddParameters(System.Collections.Specialized.NameValueCollection nvc, StringBuilder extraInfo, string title)
        {
            extraInfo.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "\n{0}\n\n", title);
            foreach (string key in nvc.AllKeys.OrderBy(s => s))
            {
                AddExtraDetail(extraInfo, key, nvc[key]);
            }
        }

        #endregion
    }
}
