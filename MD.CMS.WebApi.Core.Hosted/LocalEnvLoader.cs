using System.IO;
using DotNetEnv;

namespace MD.CMS.WebApi.Core.Hosted
{
    internal static class LocalEnvLoader
    {
        /// <summary>Loads a solution-root or working-directory <c>.env</c> so environment variables map into ASP.NET Core configuration.</summary>
        internal static void Load()
        {
            foreach (var path in new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), ".env"),
                Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", ".env")),
            })
            {
                if (File.Exists(path))
                {
                    Env.Load(path);
                    return;
                }
            }
        }
    }
}
