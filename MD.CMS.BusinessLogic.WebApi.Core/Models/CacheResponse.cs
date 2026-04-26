using MD.Tools.Helpers.Core.Caching;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.WebApi.Core.Models
{
    public class CacheResponse
    {
        public string ProviderName { get; set; }
        public IEnumerable<OmegaCachingObject> CacheObjects { get; set; }

        public CacheResponse()
        {
            CacheObjects = new List<OmegaCachingObject>();
        }
    }
}
