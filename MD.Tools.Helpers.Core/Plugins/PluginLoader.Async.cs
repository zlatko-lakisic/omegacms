using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MD.Tools.Helpers.Core.TypeConversion;
using System.IO;
using MD.Tools.Helpers.Core.Properties;
using MD.Tools.Helpers.Core.Logging;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace MD.Tools.Helpers.Core.Plugins
{
    /// <summary>
    /// Handles loading all classes that implement a particualr interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static partial class PluginLoaderAsync<T>
    {
        private static ConcurrentQueue<T> _allPluginsQueue;
        private static SemaphoreSlim @lock = new SemaphoreSlim(1);

        /// <summary>
        /// Occurs when a plugin is loaded that doesn't have a default constructor.  If there are no
        /// subscribers for this event then the plugin will not be loaded
        /// </summary>
        public static event EventHandler<PluginLoaderEventArgs<T>> LoadingComplexPlugin;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileProviderType"></param>
        /// <param name="path"></param>
        /// <param name="reloadIfEmpty"></param>
        /// <returns></returns>
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static async Task<IList<T>> GetAllAsync(int fileProviderType = 0, string path = null, bool reloadIfEmpty = false)
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            if (fileProviderType.Equals(default))
            {
                fileProviderType = Properties.HelperSettings.Default.DefaultFileProvider;
            }

            await @lock.WaitAsync().ConfigureAwait(true);
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = ReflectionHelper.GetDefaultPluginPath;
                }
                string typeName = typeof(T).Name;

                if (_allPluginsQueue != null && !_allPluginsQueue.Any() && reloadIfEmpty)
                {
                    _allPluginsQueue = null;
                }

                if (_allPluginsQueue == null)
                {
                    _allPluginsQueue = new ConcurrentQueue<T>();
                    foreach (Type t in await ReflectionHelperAsync.AllAvailableTypesAsync(fileProviderType, path).ConfigureAwait(true))
                    {
                        await AddRequiredTypeToListAsync(_allPluginsQueue, t).ConfigureAwait(true);
                    }
                    if (Properties.HelperSettings.Default.RefreshTypesOnAssemblyLoad)
                    {
                        AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(OnAssemblyLoadAsync);
                        AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoadAsync);
                    }
                }
            }
            finally
            {
                @lock.Release();
            }
            return _allPluginsQueue.ToList();
        }

        private static async Task AddRequiredTypeToListAsync(ConcurrentQueue<T> plugins, Type t)
        {
            try
            {
                if (plugins != null && t != null && !t.IsInterface && !t.IsAbstract)
                {
                    if (typeof(T).IsAssignableFrom(t))
                    {
                        await AddTypeToListAsync(plugins, t).ConfigureAwait(true);
                    }
                    else if (Properties.HelperSettings.Default.CheckForMisloadedPlugins)
                    {
                        CheckForMisloadedType(t);
                    }

                }

            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                LogExceptionSafely(ex);
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task AddTypeToListAsync(ConcurrentQueue<T> plugins, Type t)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
            if (ci != null)
            {
                plugins.Enqueue((T)ci.Invoke(null));
            }
            else if (LoadingComplexPlugin != null)
            {
                PluginLoaderEventArgs<T> clea = new PluginLoaderEventArgs<T> { PluginType = t };
                LoadingComplexPlugin(null, clea);
                if (clea.Plugins != null)
                {
                    foreach(T plugin in clea.Plugins)
                    {
                        plugins.Enqueue(plugin);
                    }
                }
            }
        }

        private static void OnAssemblyLoadAsync(object sender, AssemblyLoadEventArgs args)
        {
            if (HelperSettings.Default.VerboseLoggingReflectionEnabled) typeof(PluginLoader<T>).LogInformation("Clearing All '{0}' Plugins as new assembly loaded '{1}' from '{2}'", typeof(T).ToReadableString(), args.LoadedAssembly.FullName, args.LoadedAssembly.Location);
            _allPluginsQueue.Clear();
            _allPluginsQueue = null;
        }

        private static void CheckForMisloadedType(Type t)
        {
            Type intert = FindImplementedInterfaceWithSameName(t);
            Type baset = FindBaseTypeWithSameName(t, typeof(T));
            if (intert != null)
            {
                Logging.Logger.LogWarning("{0}: Type '{1}' implements an interface named '{2}', but is not the requested type!  Found type loaded from assembly '{3}' expected '{4}'", typeof(PluginLoader<T>).ToReadableString(), t.FullName, typeof(T).FullName, intert.AssemblyQualifiedName, typeof(T).AssemblyQualifiedName);
            }
            else if (baset != null)
            {
                Logging.Logger.LogWarning("{0}: Type '{1}' inherits from a type named '{2}', but is not the requested type!", typeof(PluginLoader<T>).ToReadableString(), t.FullName, typeof(T).FullName, baset.AssemblyQualifiedName, typeof(T).AssemblyQualifiedName);
            }
        }

        private static Type FindImplementedInterfaceWithSameName(Type t)
        {
            return (t.GetInterfaces() ?? Enumerable.Empty<Type>())
            .Where(i => string.Equals(i.FullName, t.FullName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the name of the base type with same.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        private static Type FindBaseTypeWithSameName(Type parent, Type targetType)
        {
            if (parent.BaseType == null || parent.BaseType == typeof(object)) return null;
            if (string.Equals(parent.BaseType.FullName, targetType.FullName, StringComparison.OrdinalIgnoreCase)) return parent.BaseType;
            return FindBaseTypeWithSameName(parent.BaseType, targetType);
        }

        private static void LogExceptionSafely(Exception ex)
        {
            if (Logging.Logger.IsAvailable)
            {
                Logging.Logger.Log(ex);
                if (ex.InnerException != null) Logging.Logger.Log(ex.InnerException);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                if (ex.InnerException != null) System.Diagnostics.Trace.WriteLine(ex.InnerException.ToString());
            }
        }
    }
}
