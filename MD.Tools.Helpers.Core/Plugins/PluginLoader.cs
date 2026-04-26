using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MD.Tools.Helpers.Core.TypeConversion;
using System.IO;
using MD.Tools.Helpers.Core.Properties;

namespace MD.Tools.Helpers.Core.Plugins
{
    /// <summary>
    /// Handles loading all classes that implement a particualr interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PluginLoader<T>
    {
        /// <summary>
        /// Occurs when a plugin is loaded that doesn't have a default constructor.  If there are no
        /// subscribers for this event then the plugin will not be loaded
        /// </summary>
        public static event EventHandler<PluginLoaderEventArgs<T>> LoadingComplexPlugin;

        private static List<T> _allPlugins;
        private static object _lock = new object();

        /// <summary>
        /// Get all of the plugins that implement the given interface
        /// </summary>
        /// <param name="fileProviderType"></param>
        /// <param name="path"></param>
        /// <param name="reloadIfEmpty"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        public static IList<T> GetAll(int fileProviderType = 0, string path = null, bool reloadIfEmpty = false)
        {
            if(fileProviderType.Equals(default))
            {
                fileProviderType = Properties.HelperSettings.Default.DefaultFileProvider;
            }

            lock (_lock)
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = ReflectionHelper.GetDefaultPluginPath;
                }
                string typeName = typeof(T).Name;

                if(_allPlugins != null && !_allPlugins.Any() && reloadIfEmpty)
                {
                    _allPlugins = null;
                }

                if (_allPlugins == null)
                {
                    _allPlugins = new List<T>();
                    foreach (Type t in ReflectionHelper.AllAvailableTypes(fileProviderType, path))
                    {
                        AddRequiredTypeToList(_allPlugins, t);
                    }
                    if (Properties.HelperSettings.Default.RefreshTypesOnAssemblyLoad)
                    {
                        AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(OnAssemblyLoad);
                        AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoad);
                    }
                }
                return _allPlugins;
            }
        }

        private static void AddRequiredTypeToList(List<T> plugins, Type t)
        {
            try
            {
                if (plugins != null && t != null && !t.IsInterface && !t.IsAbstract)
                {
                    if (typeof(T).IsAssignableFrom(t))
                    {
                        AddTypeToList(plugins, t);
                    }
                    else if (Properties.HelperSettings.Default.CheckForMisloadedPlugins && Logging.Logger.IsAvailable)
                    {
                        CheckForMisloadedType(t);
                    }

                }

            }
            catch (Exception ex)
            {
                LogExceptionSafely(ex);
            }
        }

        private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            lock (_lock)
            {
                if (HelperSettings.Default.VerboseLoggingReflectionEnabled && Logging.Logger.IsAvailable) Logging.Logger.LogInformation("Clearing All '{0}' Plugins as new assembly loaded '{1}' from '{2}'", typeof(T).ToReadableString(), args.LoadedAssembly.FullName, args.LoadedAssembly.Location);
                _allPlugins = null;
            }
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

        private static void AddTypeToList(List<T> plugins, Type t)
        {
            ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
            if (ci != null)
            {
                plugins.Add((T)ci.Invoke(null));
            }
            else if (LoadingComplexPlugin != null)
            {
                PluginLoaderEventArgs<T> clea = new PluginLoaderEventArgs<T> { PluginType = t };
                LoadingComplexPlugin(null, clea);
                if (clea.Plugins != null) plugins.AddRange(clea.Plugins);
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

    }
}
