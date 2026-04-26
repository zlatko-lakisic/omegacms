using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.Serialization;
using MD.Tools.Helpers.Core.TypeConversion;
using System.Linq;

namespace MD.Tools.Helpers.Core.Logging
{
    /// <summary>
    /// Encapsulates all information about a message
    /// </summary>
    [DataContract]
    [Serializable]
    public class LogMessage
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="exceptionToLog">The exception to log.</param>
        public LogMessage(Exception exceptionToLog)
            : this()
        {
            Level = TraceLevel.Error;
            ThrownException = exceptionToLog ?? throw new ArgumentNullException(nameof(exceptionToLog));

            ExtraInformation = new Dictionary<string, NameValueCollection>();
            NameValueCollection nvc = new NameValueCollection();
            foreach(string key in exceptionToLog.Data.Keys)
            {
                if (exceptionToLog.Data[key] != null)
                {
                    nvc.Add(key, exceptionToLog.Data[key].ToString());
                }
            }
            ExtraInformation.Add("Data", nvc);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="exceptionToLog">The exception to log.</param>
        public LogMessage(Type loggingType, Exception exceptionToLog)
            : this(exceptionToLog)
        {
            LoggingType = loggingType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="exceptionToLog">The exception to log.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageParameters">The message parameters.</param>
        public LogMessage(Exception exceptionToLog, string message, params object[] messageParameters)
            : this(exceptionToLog)
        {
            Message = message;
            MessageParameters = messageParameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="exceptionToLog">The exception to log.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageParameters">The message parameters.</param>
        public LogMessage(Type loggingType, Exception exceptionToLog, string message, params object[] messageParameters)
            : this(exceptionToLog, message, messageParameters)
        {
            LoggingType = loggingType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageParameters">The message parameters.</param>
        public LogMessage(TraceLevel level, string message, params object[] messageParameters)
            : this()
        {
            Level = level;
            Message = message;
            MessageParameters = messageParameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="loggingType">Type of the logging.</param>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageParameters">The message parameters.</param>
        public LogMessage(Type loggingType, TraceLevel level, string message, params object[] messageParameters)
            : this(level, message, messageParameters)
        {
            LoggingType = loggingType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        public LogMessage()
        {
            ExtraInformation = new Dictionary<string, NameValueCollection>();
            LoggedAt = DateTime.Now;
            PassedAssertCondition = false;
            Level = TraceLevel.Verbose;
        }



        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        [DataMember]
        public TraceLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [passed assert condition].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [passed assert condition]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool PassedAssertCondition { get; set; }

        /// <summary>
        /// Gets the formatted message.
        /// </summary>
        /// <value>The formatted message.</value>
        public string FormattedMessage
        {
            get
            {
                string frmMsg = Message.ToFormattedString(MessageParameters);
                if (LoggingType != null)
                    frmMsg = "{0}: {1}".ToFormattedString(LoggingType.ToReadableString(), frmMsg);
                if (ThrownException != null)
                {
                    frmMsg = string.Concat(frmMsg, Environment.NewLine, ThrownException.ToString()) + "\n" + GetInnerExceptions(ThrownException);
                }
                return frmMsg;
            }
        }

        private string GetInnerExceptions(Exception innerException)
        {
            string exceptionString = innerException.Message + ";\n";

            if (innerException.InnerException != null)
            {
                exceptionString += GetInnerExceptions(innerException.InnerException);
            }

            return exceptionString;
        }

        /// <summary>
        /// Gets or sets the message parameters.
        /// </summary>
        /// <value>The message parameters.</value>
        [DataMember]
        public object[] MessageParameters { get; set; }

        /// <summary>
        /// Gets or sets the type of the logging.
        /// </summary>
        /// <value>The type of the logging.</value>
        [DataMember]
        public Type LoggingType { get; set; }

        /// <summary>
        /// Gets or sets the thrown exception.
        /// </summary>
        /// <value>The thrown exception.</value>
        [DataMember]
        public Exception ThrownException { get; set; }

        /// <summary>
        /// Gets or sets the extra information.
        /// </summary>
        /// <value>The extra information.</value>
        [DataMember]
        public IDictionary<string, NameValueCollection> ExtraInformation { get; private set; }

        /// <summary>
        /// Gets or sets the logged at.
        /// </summary>
        /// <value>The logged at.</value>
        [DataMember]
        public DateTime LoggedAt { get; private set; }

    }
}
