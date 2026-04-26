using Microsoft.AspNetCore.Hosting;
using System.Collections;

namespace MD.CMS.BusinessLogic.AwsLambda.Core.Containers
{
    /// <summary>
    /// AWSStartup Interface
    /// </summary>
    public interface IAwsStartup
    {
        /// <summary>
        /// Configuration method for the aws startup object
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path"></param>
        /// <param name="environmentalVariables"></param>
        void Configure(IWebHostBuilder builder, string path, IDictionary environmentalVariables);

        /// <summary>
        /// UseAwsStartup Object
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="path"></param>
        /// <param name="environmentalVariables"></param>
        /// <returns></returns>
        IWebHostBuilder UseAwsStartup(IWebHostBuilder hostBuilder, string path, IDictionary environmentalVariables);
    }
}
