using Google.Cloud.Logging.V2;
using Google.Cloud.Logging.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using Google.Api;
using Google.Api.Gax.Grpc;
using System.Diagnostics;
using System.Text;
using MD.Tools.Helpers.Core.Serializer;
using MD.Tools.Helpers.Core.Logging;
using MD.CMS.BusinessLogic.GoogleCloud.Core.Properties;

namespace MD.CMS.BusinessLogic.GoogleCloud.Core.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    public class GoogleCloudLogger : ILogger
    {

        private static CustomTraceSwitch _switch => new CustomTraceSwitch("GoogleCloudLogger", "Defines which messages the AWS CloudWatch Logger should record", "0");

        /// <summary>
        /// 
        /// </summary>
        public bool IsAvailable => true;

        /// <summary>
        /// 
        /// </summary>
        public string LoggerName => "GoogleCloudLogger";

        private static string SerializeException(Exception error)
        {
            return OmegaJsonSerializer.SerializeObject(error);
        }
        /// <summary>
        /// 
        /// </summary>
        public GoogleCloudLogger()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool IsEnabledAtLevel(TraceLevel level)
        {
            return _switch.Level >= level;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public void Log(Exception exception)
        {
            if (exception != null && _switch.TraceError)
            {
                LogString(SerializeException(exception));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Log(LogMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (IsAvailable && IsEnabledAtLevel(message.Level))
            {
                StringBuilder msge = new StringBuilder(message.FormattedMessage);

                foreach (string key in message.ExtraInformation.Keys)
                {
                    msge.AppendLine("\n");
                    AddParameters(message.ExtraInformation[key], msge, key);
                }

                switch (message.Level)
                {
                    case TraceLevel.Error:
                    case TraceLevel.Warning:
                        Log(msge.ToString(), message.ThrownException);
                        break;
                    case TraceLevel.Info:
                    case TraceLevel.Verbose:
                        LogInformation(msge.ToString());
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Log(string message, Exception exception)
        {
            if (exception != null && _switch.TraceError)
            {
                LogString(message);
                LogString(SerializeException(exception));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            LogString(message, LogSeverity.Error);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogInformation(string message)
        {
            LogString(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogVerbose(string message)
        {
            LogString(message, LogSeverity.Notice);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message)
        {
            LogString(message, LogSeverity.Warning);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private static void LogString(string message, LogSeverity severity = LogSeverity.Info)
        {
            Console.WriteLine(message);
            LoggingServiceV2Client client = LoggingServiceV2Client.Create();
            LogName logName = LogName.FromProjectLog(Settings.Default.ProjectId, Settings.Default.LogId);
            LogEntry logEntry = new LogEntry
            {
                LogName = logName.ToString(),
                Severity = severity,
                TextPayload = message,
            };
            IEnumerable<LogEntry> entries = new LogEntry[] { logEntry, };
            MonitoredResource resource = new MonitoredResource { };
            IDictionary<string, string> entryLabels = new Dictionary<string, string>
            {
                { "omegacms", Settings.Default.Product.ToLowerInvariant() }
            };
            client.WriteLogEntries(logName, resource, entryLabels, entries);
            Console.WriteLine($"Created log entry in log-id: {logName.LogId}.");
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
    }
}
