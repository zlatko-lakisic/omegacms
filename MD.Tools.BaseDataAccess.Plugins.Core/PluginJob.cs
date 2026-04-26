using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{

    public class PluginJob : PluginJob<string>
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public PluginJob(PluginJob<string> obj)
        {
            this.Id = obj.Id;
            this.Message = obj.Message;
            this.PluginName = obj.PluginName;
            this.StartedOn = obj.StartedOn;
            this.Paramaters = obj.Paramaters;
        }
    }

    public class PluginJob<T>
    {
        #region Attributes
        private string _id;
        private string _pluginName;
        private string _message;
        private DateTime _startedOn;
        private T _paramaters;
        #endregion

        #region Properties
        /// <summary>
        /// Job Id
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Job plugin name
        /// </summary>
        public string PluginName
        {
            get { return _pluginName; }
            set { _pluginName = value; }
        }
        /// <summary>
        /// Job message
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        /// <summary>
        /// When the job was started
        /// </summary>
        public DateTime StartedOn
        {
            get { return _startedOn; }
            set { _startedOn = value; }
        }
        /// <summary>
        /// Special paramaters to pass to the message system
        /// </summary>
        public T Paramaters
        {
            get { return _paramaters; }
            set { _paramaters = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Default constructor for job, initiates the Id and StartedOn properties
        /// </summary>
        public PluginJob()
        {
            _id = Guid.NewGuid().ToString();
            _startedOn = DateTime.Now;
            _paramaters = default(T);
        }
        /// <summary>
        /// Default constructor for job, initiates the Id and StartedOn properties
        /// </summary>
        public PluginJob(T paramaters)
        {
            _id = Guid.NewGuid().ToString();
            _startedOn = DateTime.Now;
            _paramaters = paramaters;
        }
        #endregion
    }
}
