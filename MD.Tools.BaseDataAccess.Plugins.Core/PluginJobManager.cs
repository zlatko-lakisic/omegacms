using MD.Tools.Helpers.Core.FileProvider;
using MD.Tools.Helpers.Core.MessageQueue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public class PluginJobManager
    {
        #region Methods
        /// <summary>
        /// Add a plugin job to the job que
        /// </summary>
        /// <param name="pluginClassType">The Plugin class type</param>
        /// <param name="pluginJobMessage">The plugin job message</param>
        /// <returns>Job thet was added to the hashset</returns>
        public static PluginJob AddJob(Type pluginClassType, string pluginJobMessage)
        {
            return new PluginJob(PluginJobManager<string>.AddJob(pluginClassType, pluginJobMessage, string.Empty));
        }
        /// <summary>
        /// Get all plugin jobs currently in the que
        /// </summary>
        /// <param name="hashFileLocation">The location of the hash file</param>
        /// <returns>List of plugin jobs currently in the que</returns>
        public static IEnumerable<PluginJob> GetAllJobs()
        {
            return PluginJobManager<string>.GetAllJobs().Select(job => new PluginJob(job));
        }
        /// <summary>
        /// Get all plugin jobs currently in the que
        /// </summary>
        /// <param name="pluginClassType">The Plugin class type</param>
        /// <returns>List of plugin jobs currently in the que</returns>
        public static IEnumerable<PluginJob> GetAllJobs(Type pluginClassType)
        {
            return PluginJobManager<string>.GetAllJobs(pluginClassType).Select(job => new PluginJob(job));
        }
        /// <summary>
        /// Remove a job from the que
        /// </summary>
        /// <param name="jobId">The job id to search for</param>
        /// <returns>Boolean value wether the removal was successful</returns>
        public static bool RemoveJob(string jobId)
        {
            return PluginJobManager<string>.RemoveJob(jobId);
        }
        #endregion
    }

    /// <summary>
    /// Plugin job que manager class
    /// </summary>
    public class PluginJobManager<T>
    {
        #region Attributes
        private static MessageQueueManager _queueManager;

        public static MessageQueueManager QueueManager
        { 
            get
            {
                return _queueManager = new MessageQueueManager(Properties.Settings.Default.BaseDataAccessPluginsFileProviderType, Properties.Settings.Default.BaseDataAccessPluginsDirectory, Properties.Settings.Default.PluginJobManagerQueueSettings);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a plugin job to the job que
        /// </summary>
        /// <param name="pluginClassType">The Plugin class type</param>
        /// <param name="pluginJobMessage">The plugin job message</param>
        /// <param name="paramaters">Plugin job paramaters</param>
        /// <returns>Job thet was added to the hashset</returns>
        public static PluginJob<T> AddJob(Type pluginClassType, string pluginJobMessage, T paramaters)
        {
            if (pluginClassType == null)
            {
                throw new ArgumentNullException("pluginClassType");
            }

            if (string.IsNullOrEmpty(pluginJobMessage))
            {
                throw new ArgumentNullException("pluginJobMessage");
            }

            PluginJob<T> job = new PluginJob<T>(paramaters);
            job.PluginName = pluginClassType.Name;
            job.Message = pluginJobMessage;

            QueueManager.SendMessage(new Message()
            {
                Id = job.Id,
                MessageBody = JsonConvert.SerializeObject(job)
            });

            return job;
        }
        /// <summary>
        /// Get all plugin jobs currently in the que
        /// </summary>
        /// <returns>List of plugin jobs currently in the que</returns>
        public static IEnumerable<PluginJob<T>> GetAllJobs()
        {
            return LoadList();
        }
        /// <summary>
        /// Get all plugin jobs currently in the que
        /// </summary>
        /// <param name="pluginClassType">The Plugin class type</param>
        /// <returns>List of plugin jobs currently in the que</returns>
        public static IEnumerable<PluginJob<T>> GetAllJobs(Type pluginClassType)
        {
            return LoadList().Where(job => string.Compare(job.PluginName, pluginClassType.Name, true).Equals(0));
        }
        /// <summary>
        /// Remove a job from the que
        /// </summary>
        /// <param name="jobId">The job id to search for</param>
        /// <returns>Boolean value wether the removal was successful</returns>
        public static bool RemoveJob(string jobId)
        {
            return QueueManager.DeleteMessage(new Message()
            {
                Id = jobId
            });
        }
        /// <summary>
        /// Load plugin queue list from file
        /// </summary>
        /// <param name="hashFileLocation">The location of the hash file</param>
        /// <returns>List of jobs</returns>
        private static HashSet<PluginJob<T>> LoadList()
        {
            HashSet<PluginJob<T>> jobs = new HashSet<PluginJob<T>>(QueueManager.ReadAllMessages().Select(message => JsonConvert.DeserializeObject<PluginJob<T>>(message.MessageBody)));

            return jobs;
        }
        #endregion
    }
}
