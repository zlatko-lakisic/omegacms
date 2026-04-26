using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using MD.Tools.Helpers.Core.TypeConversion;

namespace MD.Tools.Helpers.Core.Logging
{
    /// <summary>
    /// Static class to call when logging messages and exceptions
    /// </summary>
    /// <remarks>
    /// 	<para>
    /// This class can be configured to be FailStop which means that any exceptions
    /// thrown internally within the logging mechanism will not affect the flow of the
    /// host application.
    /// </para>
    /// 	<para>
    /// As each logger implementation may have different logic depending on the <see ref="M:Log"/>
    /// overload used there is a degree of code duplication calling the relevant overload
    /// on each loaded instance.
    /// </para>
    /// </remarks>
    public static partial class Logger
    {

        private static object _lock = new object();


        /// <summary>
        /// Gets or sets a value indicating logging should be fail stop.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if logging should be is fail stop; otherwise, <c>false</c>.
        /// </value>
        public static bool IsFailStop
        {
            get
            {
                return Properties.HelperSettings.Default.LoggerIsFailStop;
            }
        }

        private static IList<ILogger> _internalLoggers;

        /// <summary>
        /// Gets a value indicating whether this instance is available.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        public static bool IsAvailable
        {
            get
            {
                return _internalLoggers != null;
            }
        }

        /// <summary>
        /// Determines whether [is logging for level availabled] [the specified level].
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>
        /// 	<c>true</c> if [is logging for level availabled] [the specified level]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEnabledAtLevel(System.Diagnostics.TraceLevel level)
        {
            if (level == TraceLevel.Off) return false;
            return InternalLoggers.Select(l => l.IsAvailable && l.IsEnabledAtLevel(level)).Any();
        }

        /// <summary>
        /// Gets or sets the internal loggers.
        /// </summary>
        /// <value>The internal loggers.</value>
        internal static IList<ILogger> InternalLoggers
        {
            get
            {
                lock (_lock)
                {
                    if (_internalLoggers == null)
                    {
                        InitLoggers();
                    }
                }
                return _internalLoggers;
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void InitLoggers(string path = null)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _internalLoggers = Plugins.PluginLoader<ILogger>.GetAll((int)FileProvider.FileProviderEnum.Hosted, path);
            _loggingQueue.Initialise();
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public static void Log(Exception exception)
        {
            #region Validate Parameters
            if (exception == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(exception));
                }
            }
            #endregion
            Log(new LogMessage(exception));
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public static void Log(string message, Exception exception)
        {
            #region Validate Parameters
            if (exception == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(exception));
                }
            }
            if (string.IsNullOrEmpty(message))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(message));
                }
            }
            #endregion
            Log(new LogMessage(exception, message));
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public static void Log(Exception exception, string messageFormat, params object[] arguments)
        {
            #region Validate Parameters
            if (exception == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(exception));
                }
            }
            if (string.IsNullOrEmpty(messageFormat))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(messageFormat));
                }
            }
            if (arguments == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(arguments));
                }
            }

            #endregion
            Log(new LogMessage(exception, messageFormat, arguments));
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public static void LogInformation(string message)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(message))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(message));
                }
            }
            #endregion
            Log(new LogMessage(TraceLevel.Info, message));
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogInformation(string messageFormat, params object[] arguments)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(messageFormat))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(messageFormat));
                }
            }
            if (arguments == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(arguments));
                }
            }
            #endregion
            Log(new LogMessage(TraceLevel.Info, messageFormat, arguments));

        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public static void LogWarning(string message)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(message))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(message));
                }
            }
            #endregion
            Log(new LogMessage(TraceLevel.Warning, message));

        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogWarning(string messageFormat, params object[] arguments)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(messageFormat))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(messageFormat));
                }
            }
            if (arguments == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(arguments));
                }
            }
            #endregion
            Log(new LogMessage(TraceLevel.Warning, messageFormat, arguments));

        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public static void LogVerbose(string message)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(message))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(message));
                }
            }
            #endregion
            Log(new LogMessage(TraceLevel.Verbose, message));
        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogVerbose(string messageFormat, params object[] arguments)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(messageFormat))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(messageFormat));
                }
            }
            if (arguments == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(arguments));
                }
            }
            #endregion
            Log(new LogMessage(TraceLevel.Verbose, messageFormat, arguments));

        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public static void LogError(string message)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(message))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(message));
                }
            }
            #endregion

            Log(new LogMessage(TraceLevel.Error, message));
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogError(string messageFormat, params object[] arguments)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(messageFormat))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(messageFormat));
                }
            }
            if (arguments == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(arguments));
                }
            }
            #endregion
            Log(new LogMessage(TraceLevel.Error, messageFormat, arguments));

        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> the message is not logged.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public static void AssertLogInformation(bool expectedCondition, string message)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(message))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(message));
                }
            }
            #endregion
            LogMessage msg = new LogMessage(TraceLevel.Info, message);
            msg.PassedAssertCondition = expectedCondition;
            AssertLog(msg);

        }

        /// <summary>
        /// Asserts the log information.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> [expected condition].</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void AssertLogInformation(bool expectedCondition, string messageFormat, params object[] arguments)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(messageFormat))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(messageFormat));
                }
            }
            if (arguments == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(arguments));
                }
            }
            #endregion
            LogMessage msg = new LogMessage(TraceLevel.Info, messageFormat, arguments);
            msg.PassedAssertCondition = expectedCondition;
            AssertLog(msg);

        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> the message is not logged.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public static void AssertLogWarning(bool expectedCondition, string message)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(message))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(message));
                }
            }
            #endregion
            LogMessage msg = new LogMessage(TraceLevel.Warning, message);
            msg.PassedAssertCondition = expectedCondition;
            AssertLog(msg);
        }

        /// <summary>
        /// Asserts the log warning.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> [expected condition].</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void AssertLogWarning(bool expectedCondition, string messageFormat, params object[] arguments)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(messageFormat))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(messageFormat));
                }
            }
            if (arguments == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(arguments));
                }
            }
            #endregion
            LogMessage msg = new LogMessage(TraceLevel.Warning, messageFormat, arguments);
            msg.PassedAssertCondition = expectedCondition;
            AssertLog(msg);
        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> the message is not logged.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public static void AssertLogVerbose(bool expectedCondition, string message)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(message))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(message));
                }
            }
            #endregion
            LogMessage msg = new LogMessage(TraceLevel.Verbose, message);
            msg.PassedAssertCondition = expectedCondition;
            AssertLog(msg);
        }

        /// <summary>
        /// Asserts the log verbose.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> [expected condition].</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void AssertLogVerbose(bool expectedCondition, string messageFormat, params object[] arguments)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(messageFormat))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(messageFormat));
                }
            }
            if (arguments == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(arguments));
                }
            }
            #endregion
            LogMessage msg = new LogMessage(TraceLevel.Verbose, messageFormat, arguments);
            msg.PassedAssertCondition = expectedCondition;
            AssertLog(msg);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> the message is not logged.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        public static void AssertLogError(bool expectedCondition, string message)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(message))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(message));
                }
            }
            #endregion
            LogMessage msg = new LogMessage(TraceLevel.Error, message);
            msg.PassedAssertCondition = expectedCondition;
            AssertLog(msg);
        }

        /// <summary>
        /// Asserts the log error.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> [expected condition].</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void AssertLogError(bool expectedCondition, string messageFormat, params object[] arguments)
        {
            #region Validate Parameters
            if (string.IsNullOrEmpty(messageFormat))
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(messageFormat));
                }
            }
            if (arguments == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(arguments));
                }
            }
            #endregion
            LogMessage msg = new LogMessage(TraceLevel.Error, messageFormat, arguments);
            msg.PassedAssertCondition = expectedCondition;
            AssertLog(msg);
        }

        /// <summary>
        /// Logs the calling method call.
        /// </summary>
        [Conditional("DEBUG")]
        public static void LogMethodCall()
        {
            try
            {
                if (!Properties.HelperSettings.Default.AllowMethodLogging || !Logger.IsEnabledAtLevel(TraceLevel.Verbose)) return;
                StackTrace stackTrace = new StackTrace();
                if (stackTrace.FrameCount <= 1) return;
                #region Detect Recursion
                bool isRecursing = false;
                StackFrame stackFrame = stackTrace.GetFrame(1);
                MethodBase methodBase = stackFrame.GetMethod();
                foreach (StackFrame sf in stackTrace.GetFrames().Skip(2))
                {
                    isRecursing = (sf.GetMethod() == methodBase);
                    if (isRecursing) break;
                }
                if (isRecursing) return;
                #endregion
                string signatureMessage = "{0}:  Called '{1}'".ToFormattedString(methodBase.DeclaringType.ToReadableString(), methodBase.ToString());
                LogVerbose(signatureMessage);
            }
            catch (Exception ex)
            {
                Log(ex);
            }

        }

        /// <summary>
        /// Logs the calling method.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public static string LogCallingMethod(string template)
        {
            if (string.IsNullOrEmpty(template)) throw new ArgumentNullException(nameof(template));
            if (!Properties.HelperSettings.Default.AllowMethodLogging || !Logger.IsEnabledAtLevel(TraceLevel.Verbose)) return string.Empty;
            try
            {
                StackTrace stackTrace = new StackTrace();
                if (stackTrace.FrameCount <= 2) return string.Empty; ;
                #region Detect Recursion
                bool isRecursing = false;
                StackFrame stackFrame = stackTrace.GetFrame(2);
                MethodBase methodBase = stackFrame.GetMethod();
                foreach (StackFrame sf in stackTrace.GetFrames().Skip(3))
                {
                    isRecursing = (sf.GetMethod() == methodBase);
                    if (isRecursing) break;
                }
                if (isRecursing) return string.Empty; ;
                #endregion
                return template.ToFormattedString(methodBase.DeclaringType.ToReadableString(), methodBase.ToString());
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            return string.Empty;
        }


    }
}
