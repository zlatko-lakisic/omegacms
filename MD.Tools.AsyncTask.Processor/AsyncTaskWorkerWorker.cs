using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MD.Tools.Helpers.Core;
using Microsoft.Extensions.Hosting;
using MD.Tools.Helpers.Core.Plugins;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.AsyncTask.Processor.Properties;

namespace MD.Tools.AsyncTask.Processor
{
    public class AsyncTaskWorkerWorker : BackgroundService
    {
        #region Attributes
        private System.Timers.Timer _timer;
        private IList<IAsyncTask> _tasks;
        private static object _lock = new object();
        #endregion

        #region Background Service Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            typeof(AsyncTaskWorkerWorker).LogVerbose("{0} is starting".ToFormattedString(Settings.Default.ServiceName));
            await EnsureTasksAreInitialised();
            StartTimer();
            typeof(AsyncTaskWorkerWorker).LogVerbose("{0} has started".ToFormattedString(Settings.Default.ServiceName));
            await base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            StopTimer();
            EnsureTasksAreClosed();
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            base.Dispose();
            StopTimer();
            EnsureTasksAreClosed();
        }
        #endregion

        #region Private worker methods

        private void StartTimer()
        {
            StopTimer();
            int ms = (int)Settings.Default.Period.TotalMilliseconds;
            lock (_lock)
            {
                _timer = new System.Timers.Timer(ms);
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerElapsed);
                _timer.Enabled = true;
            }
        }

        private void StopTimer()
        {
            lock (_lock)
            {
                if (_timer != null)
                {
                    _timer.Enabled = false;
                    _timer.Elapsed -= new System.Timers.ElapsedEventHandler(OnTimerElapsed);
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        private async void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StopTimer();
            foreach (IAsyncTask task in _tasks)
            {
                if (task.NextExecuteTime <= DateTime.Now)
                {
                    typeof(AsyncTaskWorkerWorker).LogVerbose("{0} attempting to execute task {1}".ToFormattedString(Settings.Default.ServiceName, task.GetType().FullName));
                    try
                    {
                        await task.ExecuteAsync();
                    }
                    catch (Exception ex)
                    {
                        typeof(AsyncTaskWorkerWorker).Log(ex);
                    }
                    typeof(AsyncTaskWorkerWorker).LogVerbose("{0} has executed task {1}".ToFormattedString(Settings.Default.ServiceName, task.GetType().FullName));
                }
            }
            StartTimer();
        }

        private async Task EnsureTasksAreInitialised()
        {
            EnsureTasksAreClosed();
            try
            {
                typeof(AsyncTaskWorkerWorker).LogVerbose("{0} looking for tasks in {1}".ToFormattedString(Settings.Default.ServiceName, Settings.Default.PluginsDirectory));
                _tasks = PluginLoader<IAsyncTask>.GetAll((int)MD.Tools.Helpers.Core.FileProvider.FileProviderEnum.Hosted, Settings.Default.PluginsDirectory);
                typeof(AsyncTaskWorkerWorker).LogVerbose("{0} has found {1} tasks".ToFormattedString(Settings.Default.ServiceName, _tasks.Count));
                foreach (IAsyncTask task in _tasks)
                {
                    typeof(AsyncTaskWorkerWorker).LogVerbose("{0} attempting to initialise task {1}".ToFormattedString(Settings.Default.ServiceName, task.GetType().FullName));
                    typeof(AsyncTaskWorkerWorker).LogVerbose(MD.Tools.BaseDataAccess.Plugins.Core.Properties.Settings.Default.BaseDataAccessPluginsDirectory);
                    await task.InitializeAsync();
                    typeof(AsyncTaskWorkerWorker).LogVerbose("{0} has initialised task {1}".ToFormattedString(Settings.Default.ServiceName, task.GetType().FullName));
                }
            }
            catch (Exception ex)
            {
                typeof(AsyncTaskWorkerWorker).Log(ex);
                EnsureTasksAreClosed();
                throw;
            }
        }

        private void EnsureTasksAreClosed()
        {
            if (_tasks != null)
            {
                foreach (IAsyncTask task in _tasks)
                {
                    typeof(AsyncTaskWorkerWorker).LogVerbose("{0} attempting to close task {1}".ToFormattedString(Settings.Default.ServiceName, task.GetType().FullName));
                    try
                    {
                        task.Dispose();
                    }
                    catch (Exception ex)
                    {
                        typeof(AsyncTaskWorkerWorker).Log(ex);
                    }
                    typeof(AsyncTaskWorkerWorker).LogVerbose("{0} has closed task {1}".ToFormattedString(Settings.Default.ServiceName, task.GetType().FullName));
                }
                _tasks = null;
            }
        }
        #endregion
    }
}
