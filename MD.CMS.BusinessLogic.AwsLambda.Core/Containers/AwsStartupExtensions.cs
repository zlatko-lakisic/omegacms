using Microsoft.AspNetCore.Hosting;
using System.Collections;

namespace MD.CMS.BusinessLogic.AwsLambda.Core.Containers
{
    /// <summary>
    /// AWS startup extension methods
    /// </summary>
    public static class AwsStartupExtensions
    {
        /// <summary>
        /// Use AWS Startup method
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="startup"></param>
        /// <param name="path"></param>
        /// <param name="environmentalVariables"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseAwsStartup(this IWebHostBuilder hostBuilder, IAwsStartup startup, string path, IDictionary environmentalVariables)
        {
            if (hostBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(hostBuilder));
            }

            if (startup is null)
            {
                throw new System.ArgumentNullException(nameof(startup));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new System.ArgumentException($"'{nameof(path)}' cannot be null or empty", nameof(path));
            }

            if (environmentalVariables is null)
            {
                throw new System.ArgumentNullException(nameof(environmentalVariables));
            }

            return startup.UseAwsStartup(hostBuilder, path, environmentalVariables);
        }
    }
}
