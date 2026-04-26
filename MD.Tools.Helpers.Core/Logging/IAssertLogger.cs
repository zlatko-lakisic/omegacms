using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.Helpers.Core.Logging
{
    /// <summary>
    /// Extends logger to allow for Assert Logging functionality
    /// </summary>
    public interface IAssertLogger : ILogger
    {

        /// <summary>
        /// Asserts the log.
        /// </summary>
        /// <param name="message">The message.</param>
        void AssertLog(LogMessage message);

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c> the message is not logged</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        void AssertLogInformation(bool expectedCondition, string message);

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c>  the message is not logged</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        void AssertLogWarning(bool expectedCondition, string message);

        /// <summary>
        /// Logs the verbose message.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c>  the message is not logged</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        void AssertLogVerbose(bool expectedCondition, string message);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="expectedCondition">if set to <c>true</c>  the message is not logged</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c> or empty</exception>
        void AssertLogError(bool expectedCondition, string message);
    }
}
