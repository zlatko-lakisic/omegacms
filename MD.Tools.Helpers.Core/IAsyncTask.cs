using System;
using System.Threading.Tasks;

namespace MD.Tools.Helpers.Core
{
    /// <summary>
    /// Interface for classes executable by the Async Tasks Processor Service.
    /// </summary>
    public interface IAsyncTask : IDisposable
    {
        /// <summary>
        /// Contains the next scheduled execute time for this class
        /// </summary>
        DateTime NextExecuteTime
        {
            get;
        }

        /// <summary>
        /// The primary 'entry-point' for this class - excecuted by the AsyncTasks windows service
        /// </summary>
        Task ExecuteAsync();

        /// <summary>
        /// Any initalisation logic required
        /// </summary>
        Task InitializeAsync();


    }
}
