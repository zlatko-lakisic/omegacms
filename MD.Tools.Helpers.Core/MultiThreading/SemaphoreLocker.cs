
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MD.Tools.Helpers.Core.MultiThreading
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>")]
    public class SemaphoreLocker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public async Task LockAsync(Func<Task> worker)
        {
            if (worker is null)
            {
                throw new ArgumentNullException(nameof(worker));
            }

            using(SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1))
            {
                await _semaphore.WaitAsync().ConfigureAwait(true);
                try
                {
                    await worker().ConfigureAwait(true);
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }
    }
}
