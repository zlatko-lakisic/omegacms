using MD.CMS.BusinessLogic.AwsLambda.Core.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MD.CMS.BusinessLogic.AwsLambda.Core.Containers
{
    /// <summary>
    /// Aws Startup Tools
    /// </summary>
    public class AwsStartupTools
    {
        #region Attributes
        private static IAwsStartup _awsStartup;
        private static IAwsStartupSockets _awsStartupSockets;
        private static ConcurrentDictionary<string, Assembly> assLookup = new ConcurrentDictionary<string, Assembly>();
        #endregion

        #region Properties
        /// <summary>
        /// Print assembly loading messages
        /// </summary>
        public static bool PrintLoadingMessages { get; set; }
        #endregion

        #region Methods
        private static void LoadAssemblies(Assembly parentAssembly, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty", nameof(path));
            }

            if (parentAssembly != null)
            {
                foreach (AssemblyName ass in parentAssembly.GetReferencedAssemblies())
                {
                    try
                    {
                        if (assLookup.ContainsKey(ass.FullName))
                        {
                            continue;
                        }

                        if (PrintLoadingMessages)
                        {
                            Console.WriteLine($"Loading reference assembly: {ass.FullName}!");
                        }
                        Assembly refAss = Assembly.LoadFrom($"{path}{ass.Name}.dll");
                        assLookup[ass.FullName] = AppDomain.CurrentDomain.Load(refAss.GetName());
                        LoadAssemblies(refAss, path);
                    }
#pragma warning disable CS0168 // Variable is declared but never used
                    catch (FileNotFoundException e)
#pragma warning restore CS0168 // Variable is declared but never used
                    {
                        try
                        {
                            if (!assLookup.ContainsKey(ass.FullName))
                            {
                                if (PrintLoadingMessages)
                                {
                                    Console.WriteLine($"Trying to load from GAC: {ass.FullName}!");
                                }
                                assLookup[ass.FullName] = AppDomain.CurrentDomain.Load(ass);
                            }
                        }
                        catch (FileNotFoundException err)
                        {
                            Console.WriteLine($"FileNotFoundException Error Occured:");
                            Console.WriteLine(JsonConvert.SerializeObject(err));
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine($"Exception Error Occured:");
                            Console.WriteLine(JsonConvert.SerializeObject(err));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Exception Error Occured:");
                        Console.WriteLine(JsonConvert.SerializeObject(e));
                    }
                }
            }
        }

        private static Assembly LoadApp(string path, string referencePath)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty", nameof(path));
            }

            if (string.IsNullOrEmpty(referencePath))
            {
                throw new ArgumentException($"'{nameof(referencePath)}' cannot be null or empty", nameof(referencePath));
            }

            string dllName = referencePath.Split(".dll.").First();

            Assembly assembly = Assembly.LoadFrom($"{path}{dllName}.dll");

            if (!assLookup.ContainsKey(assembly.FullName))
            {
                assLookup[assembly.FullName] = AppDomain.CurrentDomain.Load(assembly.GetName());
            }

            LoadAssemblies(assembly, path);

            return assembly;
        }

        /// <summary>
        /// Get the AWS startup object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="referencePath"></param>
        /// <returns></returns>
        public static IAwsStartup GetAwsStartup(string path, string referencePath)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty", nameof(path));
            }

            if (string.IsNullOrEmpty(referencePath))
            {
                throw new ArgumentException($"'{nameof(referencePath)}' cannot be null or empty", nameof(referencePath));
            }

            if (_awsStartup == null)
            {
                string typeName = referencePath.Split(".dll.").Last();
                Type type = LoadApp(path, referencePath).GetType(typeName);
                _awsStartup = Activator.CreateInstance(type) as IAwsStartup;

                if (Settings.Default.DebugMode)
                {
                    if (_awsStartup != null)
                    {
                        Console.WriteLine("_awsStartup found!");
                    }
                    else
                    {
                        Console.WriteLine("_awsStartup not found!");
                    }
                }
            }

            if (_awsStartup == null)
            {
                throw new NullReferenceException($"Could not locate AWS Startup object in the provided dll from path {path} and referencePath {referencePath}");
            }

            return _awsStartup;
        }

        /// <summary>
        /// Get the AWS startup sockets object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="referencePath"></param>
        /// <returns></returns>
        public static IAwsStartupSockets GetAwsStartupSockets(string path, string referencePath)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty", nameof(path));
            }

            if (string.IsNullOrEmpty(referencePath))
            {
                throw new ArgumentException($"'{nameof(referencePath)}' cannot be null or empty", nameof(referencePath));
            }

            if (_awsStartupSockets == null)
            {
                string typeName = referencePath.Split(".dll.").Last();
                Type type = LoadApp(path, referencePath).GetType(typeName);
                _awsStartupSockets = Activator.CreateInstance(type) as IAwsStartupSockets;

                if (Settings.Default.DebugMode)
                {
                    if (_awsStartupSockets != null)
                    {
                        Console.WriteLine("_awsStartupSockets found!");
                    }
                    else
                    {
                        Console.WriteLine("_awsStartupSockets not found!");
                    }
                }
            }

            if (_awsStartupSockets == null)
            {
                throw new NullReferenceException($"Could not locate AWS Startup Sockets object in the provided dll from path {path} and referencePath {referencePath}");
            }

            return _awsStartupSockets;
        }
        #endregion
    }
}
