using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.Tools.Helpers.Core.Properties;
using System.Globalization;
using System.Runtime.InteropServices;

namespace MD.Tools.Helpers.Core.Plugins
{
    /// <summary>
    /// Provides some useful helpers methods when dealing with reflection
    /// </summary>
    public static class ReflectionHelper
    {

        /// <summary>
        /// Initializes the <see cref="ReflectionHelper"/> class.
        /// </summary>
        static ReflectionHelper()
        {
            if (Properties.HelperSettings.Default.RefreshTypesOnAssemblyLoad)
            {
                AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoad);
            }
        }

        /// <summary>
        /// Gets the get default plugin path.
        /// </summary>
        /// <value>The get default plugin path.</value>
        public static string GetDefaultPluginPath
        {
            get
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                string[] paths = new string[] { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), AppDomain.CurrentDomain.RelativeSearchPath, AppDomain.CurrentDomain.BaseDirectory };
#pragma warning restore CS8601 // Possible null reference assignment.
                return string.Join(";", paths.Distinct().ToArray());
            }
        }

        private static IDictionary<string, List<Type>> _typesForPath = new Dictionary<string, List<Type>>();


        /// <summary>
        /// Alls the available types.
        /// </summary>
        /// <param name="fileProviderType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public static IList<Type> AllAvailableTypes(int fileProviderType, string path)
        {
            if (!_typesForPath.ContainsKey(path))
            {
                lock (_typesForPath)
                {
                    List<Type> types = new List<Type>();
                    try
                    {
                        IList<Assembly> assemblies = GetAllAssemblies(fileProviderType, path);
                        foreach (Assembly ass in assemblies)
                        {
                            try
                            {
                                foreach (Type t in ass.GetTypes())
                                {
                                    if (HelperSettings.Default.ReflectionHelperExclusions.Cast<string>().Any(ex => !string.IsNullOrEmpty(t.FullName) && t.FullName.Contains(ex, StringComparison.InvariantCultureIgnoreCase)))
                                    {
                                        continue;
                                    }
                                    types.Add(t);
                                }
                            }
                            catch (System.Reflection.ReflectionTypeLoadException ex)
                            {
                                IEnumerable<string> skippedTypes = HelperSettings.Default.ReflectionHelperExclusions.Cast<string>();
                                
                                if (!skippedTypes.Any(typeName =>
                                        ex.Message.Contains(typeName, StringComparison.InvariantCultureIgnoreCase)
                                    ) &&
                                    !ex.LoaderExceptions.Any(t =>
                                        skippedTypes.Any(typeName => 
                                            t.Message.Contains(typeName, StringComparison.InvariantCultureIgnoreCase)
                                        )
                                    )
                                )
                                {
                                    LogException(ex);
                                    System.Diagnostics.Trace.WriteLine(ex.ToString());
                                    if (ex.InnerException != null) System.Diagnostics.Trace.WriteLine(ex.InnerException.ToString());
                                    if (ex.LoaderExceptions != null)
                                    {
                                        foreach (Exception lex in ex.LoaderExceptions)
                                        {
                                            LogException(ex);
                                            System.Diagnostics.Trace.WriteLine(lex.ToString());
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogException(ex);
                            }
                        }
                    }
                    catch (UnauthorizedAccessException error)
                    {
                        Logger.Log(error);
                    }
                    catch (IOException error)
                    {
                        Logger.Log(error);
                    }
                    catch (Exception error)
                    {
                        Logger.Log(error);
                    }

                    _typesForPath[path] = types;
                }
            }

            if (!_typesForPath.ContainsKey(path))
            {
                return new List<Type>().AsReadOnly();
            }

            return _typesForPath[path].AsReadOnly();
        }

        private static object _lock = new object();

        /// <summary>
        /// Gets all assemblies.
        /// </summary>
        /// <param name="fileProviderType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IList<Assembly> GetAllAssemblies(int fileProviderType, string path)
        {
            lock (_lock)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return new List<Assembly>().AsReadOnly();
                }

                bool nonStandardPath = !string.CompareOrdinal(ReflectionHelper.GetDefaultPluginPath, path).Equals(0);

                if (Properties.HelperSettings.Default.RefreshTypesOnAssemblyLoad)
                {
                    AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(OnAssemblyLoad);
                }
                Dictionary<string, Assembly> assLookup = GetLoadedAssemblies();
                foreach (string p in path.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        LoadPluginsInFolder(fileProviderType, assLookup, p, nonStandardPath);
                    }
                }
                if (Properties.HelperSettings.Default.RefreshTypesOnAssemblyLoad)
                {
                    AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoad);
                }
                return new List<Assembly>(assLookup.Values).AsReadOnly();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private static void LoadPluginsInFolder(int fileProviderType, Dictionary<string, Assembly> assLookup, string p, bool nonStandardPath)
        {
            if (HelperSettings.Default.VerboseLoggingReflectionEnabled && Logging.Logger.IsAvailable && Logging.Logger.IsEnabledAtLevel(System.Diagnostics.TraceLevel.Info))
            {
                typeof(ReflectionHelper).LogInformation("Attempting to find new plugins in folder: '{0}''", p);
            }

            string path = p;

            if (nonStandardPath)
            {
                try
                {
                    if (Directory.Exists(HelperSettings.Default.TempAssembliesFolder))
                    {
                        Directory.Delete(HelperSettings.Default.TempAssembliesFolder, true);
                    }
                }
#pragma warning disable CS0168 // Variable is declared but never used
                catch (Exception error)
#pragma warning restore CS0168 // Variable is declared but never used
                {
                    //Silent fail
                }

                if (!Directory.Exists(HelperSettings.Default.TempAssembliesFolder))
                {
                    Directory.CreateDirectory(HelperSettings.Default.TempAssembliesFolder);
                }

                path = HelperSettings.Default.TempAssembliesFolder;

                FileProvider.FileProviderOptions options = new FileProvider.FileProviderOptions();
                options.DirectoryRequestOptions = new FileProvider.FileProviderDirectoryOptions()
                {
                    Path = p,
                    SearchPattern = @"\.(?i)(dll)$",
                    LoadObjects = true
                };

                IEnumerable<FileProvider.FileProviderFile> files = FileProvider.DynamicFileProvider.Instance.SetFileProvider(fileProviderType).ReadDirectoryFiles(options).Result;

                foreach (FileProvider.FileProviderFile file in files)
                {
                    string newFilePath = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}.dll", path, RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\\" : "/", file.FileName);

                    try
                    {
                        if (File.Exists(newFilePath))
                        {
                            File.SetAttributes(newFilePath, FileAttributes.Normal);
                            File.Delete(newFilePath);
                        }
                    }
                    catch (UnauthorizedAccessException error)
                    {
                        LogExceptionAsWarning(error);
                    }

                    if (!File.Exists(newFilePath))
                    {
                        File.WriteAllBytes(newFilePath, file.FileBytes);
                    }
                }
            }

            foreach(string filePath in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    if (!IsManagedAssembly(filePath))
                    {
                        continue;
                    }
                    AssemblyName an = AssemblyName.GetAssemblyName(filePath);
                    if (HelperSettings.Default.ReflectionHelperExclusions.Cast<string>().Any(ex => an.FullName.Contains(ex, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }
                    if (HelperSettings.Default.VerboseLoggingReflectionEnabled && Logging.Logger.IsAvailable && Logging.Logger.IsEnabledAtLevel(System.Diagnostics.TraceLevel.Info)) typeof(ReflectionHelper).LogInformation("Explicitly loading assembly '{0}' from '{1}'", an.FullName, filePath);
                    if (!assLookup.ContainsKey(an.FullName))
                    {
                        if (nonStandardPath)
                        {
                            LoadAssembly(assLookup, an, filePath);
                        }
                        else
                        {
                            LoadAssembly(assLookup, an);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException(ex);
                }
            }
        }

        private static void LoadAssembly(Dictionary<string, Assembly> assLookup, AssemblyName an)
        {
            LoadAssembly(assLookup, an, string.Empty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private static void LoadAssembly(Dictionary<string, Assembly> assLookup, AssemblyName an, string filePath)
        {
            try
            {
                if (!assLookup.ContainsKey(an.FullName))
                {
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        Assembly assembly = Assembly.LoadFrom(filePath);
                        an = assembly.GetName();
                    }
                    assLookup[an.FullName] = AppDomain.CurrentDomain.Load(an);
                }
            }
            catch (System.IO.FileNotFoundException fex)
            {
                LogExceptionAsWarning(fex);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private static void LogExceptionAsWarning(Exception ex)
        {
            System.Diagnostics.Trace.WriteLine(ex.ToString());
            typeof(ReflectionHelper).LogWarning(ex.ToString());
            if (ex.InnerException != null)
            {
                System.Diagnostics.Trace.WriteLine(ex.InnerException.ToString());
                typeof(ReflectionHelper).LogWarning(ex.InnerException.ToString());
            }
        }

        private static void LogException(Exception ex)
        {
            System.Diagnostics.Trace.WriteLine(ex.ToString());
            typeof(ReflectionHelper).Log(ex);
            if (ex.InnerException != null)
            {
                System.Diagnostics.Trace.WriteLine(ex.InnerException.ToString());
                typeof(ReflectionHelper).Log(ex.InnerException);
            }
        }

        private static Dictionary<string, Assembly> GetLoadedAssemblies()
        {
            Dictionary<string, Assembly> assLookup = new Dictionary<string, Assembly>();
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
                assLookup[ass.FullName] = ass;
            AssemblyName[] names = assLookup.Values
                .SelectMany(asc => asc.GetReferencedAssemblies())
                .Where(asl => !assLookup.ContainsKey(asl.FullName)).ToArray();
            foreach (AssemblyName assN in names)
                LoadAssembly(assLookup, assN);
            if (HelperSettings.Default.VerboseLoggingReflectionEnabled && Logging.Logger.IsAvailable && Logging.Logger.IsEnabledAtLevel(System.Diagnostics.TraceLevel.Info)) typeof(ReflectionHelper).LogInformation("Already Loaded Assemblies:\n\n{0}", string.Join("\n", assLookup.Keys.ToArray()));
            return assLookup;
        }

        private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            if (HelperSettings.Default.VerboseLoggingReflectionEnabled && Logging.Logger.IsAvailable && Logging.Logger.IsEnabledAtLevel(System.Diagnostics.TraceLevel.Info)) typeof(ReflectionHelper).LogInformation("Clearing Cached Types as new Assembly Loaded '{0}'", args.LoadedAssembly.FullName);
            lock (_typesForPath)
            {
                _typesForPath.Clear();
            }
        }

        /// <summary>
        /// Determines whether is managed assmebly the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// 	<c>true</c> if is managed assmebly the specified path otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// <para>Taken from:</para>
        /// <para>http://geekswithblogs.net/rupreet/archive/2005/11/02/58873.aspx</para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "timestamp"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "sections"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "peHeaderSignature"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "pSymbolTable"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "optionalHeaderSize"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "noOfSymbol"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "machine"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "characteristics")]
        public static bool IsManagedAssembly(string path)
        {
            uint peHeader;
            uint peHeaderSignature;
            ushort machine;
            ushort sections;
            uint timestamp;
            uint pSymbolTable;
            uint noOfSymbol;
            ushort optionalHeaderSize;
            ushort characteristics;
            ushort dataDictionaryStart;
            uint[] dataDictionaryRVA = new uint[16];
            uint[] dataDictionarySize = new uint[16];

            using (Stream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {

                    //PE Header starts @ 0x3C (60). Its a 4 byte header.
                    fs.Position = 0x3C;
                    peHeader = reader.ReadUInt32();
                    //Moving to PE Header start location...
                    fs.Position = peHeader;
                    peHeaderSignature = reader.ReadUInt32();

                    //We can also show all these value, but we will be       
                    //limiting to the CLI header test.

                    machine = reader.ReadUInt16();
                    sections = reader.ReadUInt16();
                    timestamp = reader.ReadUInt32();
                    pSymbolTable = reader.ReadUInt32();
                    noOfSymbol = reader.ReadUInt32();
                    optionalHeaderSize = reader.ReadUInt16();
                    characteristics = reader.ReadUInt16();
                    /*
                    Now we are at the end of the PE Header and from here, the PE Optional Headers starts...
                    To go directly to the datadictionary, we'll increase the      
                    stream’s current position to with 96 (0x60). 96 because,
                     28 for Standard fields
                     68 for NT-specific fields
                    From here DataDictionary starts...and its of total 128 bytes. DataDictionay has 16 directories in total,
                    doing simple maths 128/16 = 8.
                    So each directory is of 8 bytes.
                             In this 8 bytes, 4 bytes is of RVA and 4 bytes of Size.
               
                    btw, the 15th directory consist of CLR header! if its 0, its not a CLR file :)
                    */
                    try
                    {
                        dataDictionaryStart = Convert.ToUInt16(Convert.ToUInt16(fs.Position) + 0x60);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        dataDictionaryStart = default(ushort);
                    }
                    fs.Position = dataDictionaryStart;
                    for (int i = 0; i < 15; i++)
                    {
                        dataDictionaryRVA[i] = reader.ReadUInt32();
                        dataDictionarySize[i] = reader.ReadUInt32();
                    }
                    if (dataDictionaryRVA[14] == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }


    }
}
