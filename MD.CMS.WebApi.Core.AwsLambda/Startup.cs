using MD.CMS.BusinessLogic.Aws.Core.ConfigParsers;
using MD.CMS.BusinessLogic.Aws.Core.FileProviders.S3;
using MD.Tools.Helpers.Core.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.AwsLambda
{
    public class Startup : Core.Startup
    {
        public static string StartupPath
        {
            get
            {
                return _basePath;
            }
            set
            {
                _basePath = value;
            }
        }

        internal static string SwaggerXmlFilePath
        {
            set
            {
                _swaggerXmlFilePath = value;
            }
        }

        public Startup(IConfiguration configuration) : base(configuration)
        {
            Tools.Helpers.Core.FileProvider.DynamicFileProvider.AddFileProvider<AWSS3FileProvider>();
            ConfigParser.Providers.Add(new LambdaConfigParser());
            LicenseValidSetter = new LicenseValidDelegate(AwsLicenseValidator);
        }

        public static async Task<bool> AwsLicenseValidator(HttpContext context)
        {
            return true;
        }
    }
}
