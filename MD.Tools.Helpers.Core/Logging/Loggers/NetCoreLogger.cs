using MD.Tools.Helpers.Core.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;

namespace MD.Tools.Helpers.Core.Logging.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    public class NetCoreLogger : MD.Tools.Helpers.Core.Logging.ILogger
    {
        private static CustomTraceSwitch _switch => new CustomTraceSwitch(MD.Tools.Helpers.Core.Properties.HelperSettings.Default.NetCoreLoggerTraceSwitch, "Defines which messages the NetCore Logger should record", "0");

        private Microsoft.Extensions.Logging.ILogger _netCoreLogger;
        /// <summary>
        /// 
        /// </summary>
        public bool IsAvailable => HelperSettings.Default.NetCoreLoggerIsEnabled;
        /// <summary>
        /// 
        /// </summary>
        public string LoggerName => "CustomTraceSwitch";
        /// <summary>
        /// 
        /// </summary>
        public NetCoreLogger()
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                _ = builder
                    .AddFilter("Microsoft", _switch.LogLevel)
                    .AddFilter("System", _switch.LogLevel)
                    .AddFilter("LoggingConsoleApp.Program", _switch.LogLevel)
                    .AddConsole()
                    .AddDebug();
            });
#pragma warning restore CA2000 // Dispose objects before losing scope

            _netCoreLogger = loggerFactory.CreateLogger<NetCoreLogger>();
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
                _netCoreLogger.LogError(exception, exception.Message);
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
                _netCoreLogger.LogError(message, exception.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            _netCoreLogger.LogError(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogInformation(string message)
        {
            _netCoreLogger.LogInformation(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogVerbose(string message)
        {
            _netCoreLogger.LogInformation(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message)
        {
            _netCoreLogger.LogWarning(message);
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
