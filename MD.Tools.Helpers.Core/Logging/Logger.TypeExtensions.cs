using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using MD.Tools.Helpers.Core.TypeConversion;
using System.Web;

namespace MD.Tools.Helpers.Core.Logging
{
    public static partial class Logger
    {
        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public static void Log(this Type loggingType, Exception exception)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, exception));
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public static void Log(this Type loggingType, string message, Exception exception)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, exception, message));
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="exception">The exception to log</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <c>null</c></exception>
        public static void Log(this Type loggingType, Exception exception, string messageFormat, params object[] arguments)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, exception, messageFormat, arguments));
        }

        /// <summary>
        /// Logs the specified logging type.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="level">The level.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Log(this Type loggingType, TraceLevel level, string messageFormat, params object[] arguments)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, level, messageFormat, arguments));
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="message">The message.</param>
        public static void LogInformation(this Type loggingType, string message)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, TraceLevel.Info, message));
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogInformation(this Type loggingType, string messageFormat, params object[] arguments)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, TraceLevel.Info, messageFormat, arguments));

        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="message">The message.</param>
        public static void LogWarning(this Type loggingType, string message)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, TraceLevel.Warning, message));

        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogWarning(this Type loggingType, string messageFormat, params object[] arguments)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, TraceLevel.Warning, messageFormat, arguments));

        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="message">The message.</param>
        public static void LogVerbose(this Type loggingType, string message)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, TraceLevel.Verbose, message));
        }

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogVerbose(this Type loggingType, string messageFormat, params object[] arguments)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, TraceLevel.Verbose, messageFormat, arguments));

        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="message">The message.</param>
        public static void LogError(this Type loggingType, string message)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion

            Log(new LogMessage(loggingType, TraceLevel.Error, message));
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogError(this Type loggingType, string messageFormat, params object[] arguments)
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
            if (loggingType == null)
            {
                if (IsFailStop)
                {
                    return;
                }
                else
                {
                    throw new ArgumentNullException(nameof(loggingType));
                }
            }
            #endregion
            Log(new LogMessage(loggingType, TraceLevel.Error, messageFormat, arguments));

        }
    }
}
