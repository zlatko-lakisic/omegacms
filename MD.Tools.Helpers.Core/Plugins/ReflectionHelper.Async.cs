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
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace MD.Tools.Helpers.Core.Plugins
{
    /// <summary>
    /// Provides some useful helpers methods when dealing with reflection
    /// </summary>
    public static partial class ReflectionHelperAsync
    {
        private static SemaphoreSlim @lock = new SemaphoreSlim(1);
        private static IDictionary<string, ConcurrentQueue<Type>> _typesForPathAsync = new ConcurrentDictionary<string, ConcurrentQueue<Type>>();


        /// <summary>
        /// Initializes the <see cref="ReflectionHelper"/> class.
        /// </summary>
        static ReflectionHelperAsync()
        {
            if (Properties.HelperSettings.Default.RefreshTypesOnAssemblyLoad)
            {
                AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoadAsync);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileProviderType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<IList<Type>> AllAvailableTypesAsync(int fileProviderType, string path)
        {
            if (!_typesForPathAsync.ContainsKey(path))
            {
                ConcurrentQueue<Type> types = new ConcurrentQueue<Type>();
                try
                {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                    IList<Assembly> assemblies = await GetAllAssembliesAsync(fileProviderType, path);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
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
                                types.Enqueue(t);
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
#pragma warning disable CA1031 // Do not catch general exception types
                        catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                        {
                            LogException(ex);
                        }
                    }
                }
                catch (UnauthorizedAccessException error)
                {
                    typeof(ReflectionHelper).Log(error);
                }
                catch (IOException error)
                {
                    typeof(ReflectionHelper).Log(error);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    typeof(ReflectionHelper).Log(error);
                }

                _typesForPathAsync.TryAdd(path, types);
            }

            if (!_typesForPathAsync.ContainsKey(path))
            {
                return new List<Type>().AsReadOnly();
            }

            return _typesForPathAsync[path].ToList().AsReadOnly();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileProviderType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<IList<Assembly>> GetAllAssembliesAsync(int fileProviderType, string path)
        {
            ConcurrentDictionary<string, Assembly> assLookup = new ConcurrentDictionary<string, Assembly>();
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            await @lock.WaitAsync();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return new List<Assembly>().AsReadOnly();
                }

                bool nonStandardPath = !string.CompareOrdinal(ReflectionHelper.GetDefaultPluginPath, path).Equals(0);

                if (Properties.HelperSettings.Default.RefreshTypesOnAssemblyLoad)
                {
                    AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(OnAssemblyLoadAsync);
                }
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                assLookup = await GetLoadedAssembliesAsync();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
                foreach (string p in path.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrEmpty(p))
                    {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                        await LoadPluginsInFolderAsync(fileProviderType, assLookup, p, nonStandardPath);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
                    }
                }
                if (Properties.HelperSettings.Default.RefreshTypesOnAssemblyLoad)
                {
                    AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoadAsync);
                }
            }
            finally
            {
                @lock.Release();
            }
            return assLookup.Values.ToList();
        }
        private static async Task LoadPluginsInFolderAsync(int fileProviderType, ConcurrentDictionary<string, Assembly> assLookup, string p, bool nonStandardPath)
        {
            if (HelperSettings.Default.VerboseLoggingReflectionEnabled)
            {
                typeof(ReflectionHelper).LogInformation("Attempting to find {1} new plugins in folder: '{0}'", p, nonStandardPath ? "non-standard" : "standard");
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
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
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

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                IEnumerable<FileProvider.FileProviderFile> files = await FileProvider.DynamicFileProvider.Instance.SetFileProvider(fileProviderType).ReadDirectoryFiles(options);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task

                if (HelperSettings.Default.VerboseLoggingReflectionEnabled)
                {
                    typeof(ReflectionHelper).LogInformation("Retreived {1} files from '{0}''", p, files.Count());
                }

                foreach (FileProvider.FileProviderFile file in files)
                {
                    string newFilePath = Path.Join(path, file.FileName);

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
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception error)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        LogExceptionAsWarning(error);
                    }

                    if (!File.Exists(newFilePath))
                    {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                        await File.WriteAllBytesAsync(newFilePath, file.FileBytes);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
                    }
                }
            }

            if (HelperSettings.Default.VerboseLoggingReflectionEnabled) typeof(ReflectionHelper).LogInformation("Found {1} plugin DLLs in folder: '{0}''", path, Directory.GetFiles(path, "*.dll").Length);

            foreach (string filePath in Directory.GetFiles(path, "*.dll"))
            {
                if (!IsManagedAssembly(filePath))
                {
                    continue;
                }
                try
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(filePath);
                    if (HelperSettings.Default.ReflectionHelperExclusions.Cast<string>().Any(ex => an.FullName.Contains(ex, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }
                    if (HelperSettings.Default.VerboseLoggingReflectionEnabled) typeof(ReflectionHelper).LogInformation("Explicitly loading assembly '{0}' from '{1}'", an.FullName, filePath);
                    if (!assLookup.ContainsKey(an.FullName))
                    {
                        if (nonStandardPath)
                        {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                            await LoadAssemblyAsync(assLookup, an, filePath);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
                        }
                        else
                        {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                            await LoadAssemblyAsync(assLookup, an);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
                        }
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    LogException(ex);
                }
            }
        }

        private static async Task<ConcurrentDictionary<string, Assembly>> GetLoadedAssembliesAsync()
        {
            ConcurrentDictionary<string, Assembly> assLookup = new ConcurrentDictionary<string, Assembly>();
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
                assLookup[ass.FullName] = ass;
            AssemblyName[] names = assLookup.Values
                .SelectMany(asc => asc.GetReferencedAssemblies())
                .Where(asl => !assLookup.ContainsKey(asl.FullName)).ToArray();
            foreach (AssemblyName assN in names)
            {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                await LoadAssemblyAsync(assLookup, assN);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            }
            if (HelperSettings.Default.VerboseLoggingReflectionEnabled) typeof(ReflectionHelper).LogInformation("Already Loaded Assemblies:\n\n{0}", string.Join("\n", assLookup.Keys.ToArray()));
            return assLookup;
        }


        private static Task LoadAssemblyAsync(ConcurrentDictionary<string, Assembly> assLookup, AssemblyName an)
        {
            return LoadAssemblyAsync(assLookup, an, string.Empty);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task LoadAssemblyAsync(ConcurrentDictionary<string, Assembly> assLookup, AssemblyName an, string filePath)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
                    assLookup.TryAdd(an.FullName, AppDomain.CurrentDomain.Load(an));
                }
            }
            catch (System.IO.FileNotFoundException fex)
            {
                LogExceptionAsWarning(fex);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                LogException(ex);
            }
        }

        private static void OnAssemblyLoadAsync(object sender, AssemblyLoadEventArgs args)
        {
            if (HelperSettings.Default.VerboseLoggingReflectionEnabled) typeof(ReflectionHelper).LogInformation("Clearing Cached Types as new Assembly Loaded '{0}'", args.LoadedAssembly.FullName);
            _typesForPathAsync.Clear();
        }

        private static void LogExceptionAsWarning(Exception ex)
        {
            if (Logging.Logger.IsEnabledAtLevel(System.Diagnostics.TraceLevel.Warning))
            {
                typeof(ReflectionHelper).LogWarning(ex.ToString());
                if (ex.InnerException != null) typeof(ReflectionHelper).LogWarning(ex.InnerException.ToString());
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                if (ex.InnerException != null) System.Diagnostics.Trace.WriteLine(ex.InnerException.ToString());
            }
        }

        private static void LogException(Exception ex)
        {
            if (Logging.Logger.IsAvailable)
            {
                typeof(ReflectionHelper).Log(ex);
                if (ex.InnerException != null) typeof(ReflectionHelper).Log(ex.InnerException);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                if (ex.InnerException != null) System.Diagnostics.Trace.WriteLine(ex.InnerException.ToString());
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
