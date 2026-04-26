using MD.Tools.Helpers.Core.FileProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.Tools.Helpers.Core.MessageQueue
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageQueueManager
    {
        #region Attributes
        private IMessageQueue _queue;
        #endregion

        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileProvider"></param>
        /// <param name="fileProviderLocation"></param>
        /// <param name="settings"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public MessageQueueManager(int fileProvider, string fileProviderLocation, string settings)
        {
            _queue = Plugins.PluginLoader<IMessageQueue>.GetAll(fileProvider, fileProviderLocation, true).FirstOrDefault();
            if(_queue == null)
            {
                throw new Exception("Could not find message queue!");
            }
            _queue.Init(settings);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SendMessage(Message message)
        {
            return _queue.SendMessage(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool DeleteMessage(Message message)
        {
            return _queue.DeleteMessage(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Message> ReadAllMessages()
        {
            return _queue.ReadAllMessages();
        }
        #endregion
    }
}
