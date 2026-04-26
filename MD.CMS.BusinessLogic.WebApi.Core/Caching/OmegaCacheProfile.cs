using MD.Tools.Helpers.Core.Caching.Providers;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching
{
    public class OmegaCacheProfile
    {
        public class CacheProfile
        {
            public int Duration { get; set; }
            public bool Enabled { get; set; }
        }

        public CacheProfile Client { get; set; }
        public CacheProfile Server { get; set; }
        public string CacheKey { get; set; }
        public IOmegaServerCachingProvider CachingProvider { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
    }
}
