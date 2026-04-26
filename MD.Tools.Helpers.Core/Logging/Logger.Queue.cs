using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace MD.Tools.Helpers.Core.Logging
{
    public static partial class Logger
    {

        #region Logging Queue Implementation

        private static InnerLogQueues _loggingQueue = new InnerLogQueues();

        /// <summary>
        /// Closes the queue.
        /// </summary>
        public static void CloseQueue()
        {
            if (_loggingQueue != null) _loggingQueue.Dispose();
        }

        /// <summary>
        /// Internal Logging queue
        /// </summary>
        private class InnerLogQueues : IDisposable
        {
            private bool _isInitialised;
            private static object _lock = new object();

            /// <summary>
            /// Initializes a new instance of the <see cref="InnerLogQueues"/> class.
            /// </summary>
            public InnerLogQueues()
            {
                if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
                {
                    AppDomain.CurrentDomain.DomainUnload += new EventHandler(OnDomainUnload);
                }
                else
                {
                    AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnDomainUnload);
                }

            }

            public void Initialise()
            {
                lock (_lock)
                {
                    if (_isInitialised) return;
                    _isInitialised = true;
                    InitialiseMessageBuffer();
                    InitialiseAssertMessageBuffer();
                }
            }

            private void OnDomainUnload(object sender, EventArgs e)
            {
                Dispose();
            }

            private Collections.BlockingQueue<LogMessage> _logBuffer;

            private Collections.BlockingQueue<LogMessage> _assertBuffer;

            private Thread _loggingThread;

            private Thread _assertThread;

            private void InitialiseMessageBuffer()
            {
                if (_logBuffer != null) return;
                _logBuffer = new MD.Tools.Helpers.Core.Collections.BlockingQueue<LogMessage>();
                _loggingThread = new Thread(() =>
                {
                    //Log(new LogMessage(TraceLevel.Info, "Started Logging Thread"));
                    while (_logBuffer != null
                    && _logBuffer.IsOpen
                    && (!_isDisposing || _logBuffer.Count > 0))
                    {
                        try
                        {
                            LogMessage msg = _logBuffer.Dequeue();
                            if (msg != null) InnerLog(msg);
                        }
                        catch
                        {
                            //swallow
                        }
                    }
                    //InnerLog(new LogMessage(TraceLevel.Info, "Stopped Logging Thread")); // don't use queue!
                });
                _loggingThread.IsBackground = true;
                _loggingThread.Name = "Background Logging Thread";
                _loggingThread.Start();
                Log(new LogMessage(TraceLevel.Info, "Loaded '{0}' Loggers", InternalLoggers.Count));
            }

            private void InitialiseAssertMessageBuffer()
            {
                if (_assertBuffer != null) return;
                _assertBuffer = new MD.Tools.Helpers.Core.Collections.BlockingQueue<LogMessage>();
                _assertThread = new System.Threading.Thread(() =>
                {
                    //Log(new LogMessage(TraceLevel.Info, "Started Assert Logging Thread"));
                    while (_assertBuffer != null
                        && _assertBuffer.IsOpen
                        && (!_isDisposing || _assertBuffer.Count > 0))
                    {
                        try
                        {
                            LogMessage msg = _assertBuffer.Dequeue();
                            if (msg != null) InnerAssertLog(msg);
                        }
                        catch
                        {
                            //swallow
                        }
                    }
                    //InnerLog(new LogMessage(TraceLevel.Info, "Stopped Assert Logging Thread")); // don't use queue!
                });
                _assertThread.IsBackground = true;
                _assertThread.Name = "Background Assert Logging Thread";
                _assertThread.Start();
            }

            private static void InnerLog(LogMessage message)
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    foreach (ILogger logger in InternalLoggers.Where(lgr => lgr.IsAvailable && lgr.IsEnabledAtLevel(message.Level)))
                    {
                        try
                        {
                            logger.Log(message);
                        }
                        catch
                        {
                            if (!IsFailStop) throw;
                        }
                    }
                    scope.Complete();
                }
            }

            private static void InnerAssertLog(LogMessage message)
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    foreach (ILogger logger in InternalLoggers
                        .Where(lgr => lgr.IsAvailable && lgr.IsEnabledAtLevel(message.Level)))
                    {
                        IAssertLogger al = logger as IAssertLogger;
                        if (al == null)
                        {
                            if (logger.IsAvailable && !message.PassedAssertCondition) logger.Log(message);
                        }
                        else
                        {
                            al.AssertLog(message);
                        }
                    }
                    scope.Complete();
                }
            }


            #region IDisposable Members

            private bool _isDisposing;

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_isDisposing) return;
                System.Diagnostics.Trace.WriteLine("Disposing Queues!");
                _isDisposing = true;
                AppDomain.CurrentDomain.DomainUnload -= new EventHandler(OnDomainUnload);

                DrainAndCloseQueue(_assertBuffer, _assertThread);
                _assertBuffer = null;

                DrainAndCloseQueue(_logBuffer, _loggingThread);
                _logBuffer = null;


                GC.SuppressFinalize(this);

            }

            private static void DrainAndCloseQueue(Collections.BlockingQueue<LogMessage> queue, System.Threading.Thread thread)
            {
                try
                {
                    queue.Enqueue(null);//force a Dequeue;
                    while (thread != null
                        && thread.IsAlive) System.Threading.Thread.Sleep(5);

                    if (queue != null)
                    {
                        foreach (LogMessage lm in queue.Contents)
                            InnerLog(lm);// log any remaining messages
                        queue.Close();
                    }
                }
                catch { }
            }

            /// <summary>
            /// Releases unmanaged resources and performs other cleanup operations before the
            /// <see cref="InnerLogQueues"/> is reclaimed by garbage collection.
            /// </summary>
            ~InnerLogQueues()
            {
                Dispose();
            }

            #endregion

            /// <summary>
            /// Logs the specified message.
            /// </summary>
            /// <param name="message">The message.</param>
            public void Log(LogMessage message)
            {
                if (_isDisposing) return;
                if (!_isInitialised) Initialise();
                #region Validate Parameters
                if (message == null)
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
                _logBuffer.Enqueue(message);


#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                EventLogEntryType entryType = EventLogEntryType.Information;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                switch (message.Level)
                {
                    case TraceLevel.Error:
                        entryType = EventLogEntryType.Error;
                        break;
                    case TraceLevel.Info:
                        entryType = EventLogEntryType.Information;
                        break;
                    case TraceLevel.Warning:
                        entryType = EventLogEntryType.Warning;
                        break;
                }
            }

            /// <summary>
            /// Asserts the log.
            /// </summary>
            /// <param name="message">The message.</param>
            public void AssertLog(LogMessage message)
            {

                if (_isDisposing) return;
                if (!_isInitialised) Initialise();
                #region Validate Parameters
                if (message == null)
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
                _assertBuffer.Enqueue(message);
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(LogMessage message)
        {
            _loggingQueue.Log(message);
        }

        /// <summary>
        /// Asserts the log.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void AssertLog(LogMessage message)
        {
            _loggingQueue.AssertLog(message);
        }
        #endregion
    }
}
