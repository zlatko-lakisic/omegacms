using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.Helpers.Core.MessageQueue
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageQueue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        void Init(string settings);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool SendMessage(Message message);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Message> ReadAllMessages();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool DeleteMessage(Message message);
    }
}
