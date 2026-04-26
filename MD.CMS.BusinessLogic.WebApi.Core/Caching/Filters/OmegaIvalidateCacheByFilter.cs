using MD.Tools.Helpers.Core.Caching.CacheKeys;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using MD.Tools.Helpers.Core.Caching;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching.Filters
{
    public class OmegaIvalidateCacheByFilter : IAsyncActionFilter, IFilterMetadata
    {
        #region Attributes
        private readonly IEnumerable<OmegaCacheProfile> _cacheProfiles;
        private readonly ILogger _logger;
        private ICacheKeyGenerator _cacheKeyGenerator;
        #endregion

        #region Properties
        #endregion

        #region Methods
        public OmegaIvalidateCacheByFilter(IEnumerable<OmegaCacheProfile> cacheProfiles, ILoggerFactory loggerFactory, ICacheKeyGenerator cacheKeyGenerator = null)
        {
            _cacheProfiles = cacheProfiles;
            _logger = loggerFactory.CreateLogger(GetType());
            if(cacheKeyGenerator == null)
            {
                cacheKeyGenerator = new DefaultCacheKeyGenerator();
            }
            _cacheKeyGenerator = cacheKeyGenerator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await OnBeforeActionExecutionAsync(context).ConfigureAwait(true);
            await next();
        }

        private async Task OnBeforeActionExecutionAsync(ActionExecutingContext context)
        {
            foreach(OmegaCacheProfile _cacheProfile in _cacheProfiles)
            {
                _cacheProfile.CacheKey = _cacheKeyGenerator.MakeCacheKey($"{_cacheProfile.ControllerName}-{_cacheProfile.MethodName}");
                await OmegaCacheController.GetNewInstance().InvalidateCacheAsync(_cacheProfile.CachingProvider, _cacheProfile.CacheKey, true);
            }
        }
        #endregion
    }
}
