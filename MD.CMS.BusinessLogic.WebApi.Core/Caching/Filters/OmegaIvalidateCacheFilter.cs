using MD.Tools.Helpers.Core.Caching;
using MD.Tools.Helpers.Core.Caching.CacheKeys;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching.Filters
{
    public class OmegaInvalidateCacheFilter : IAsyncActionFilter, IFilterMetadata
    {
        #region Attributes
        private readonly OmegaCacheProfile _cacheProfile;
        private readonly ILogger _logger;
        private ICacheKeyGenerator _cacheKeyGenerator;
        #endregion

        #region Properties
        #endregion

        #region Methods
        public OmegaInvalidateCacheFilter(OmegaCacheProfile cacheProfile, ILoggerFactory loggerFactory, ICacheKeyGenerator cacheKeyGenerator = null)
        {
            _cacheProfile = cacheProfile;
            _logger = loggerFactory.CreateLogger(GetType());
            if(cacheKeyGenerator == null)
            {
                cacheKeyGenerator = new DefaultCacheKeyGenerator();
            }
            _cacheKeyGenerator = cacheKeyGenerator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _cacheProfile.CacheKey = _cacheKeyGenerator.MakeCacheKey($"{_cacheProfile.ControllerName}-{_cacheProfile.MethodName}", context.HttpContext.Request.GetEncodedPathAndQuery());
            await OnBeforeActionExecutionAsync();
            await next();
        }

        private async Task OnBeforeActionExecutionAsync()
        {
            await OmegaCacheController.GetNewInstance().InvalidateCacheAsync(_cacheProfile.CachingProvider, _cacheProfile.CacheKey, true);
        }
        #endregion
    }
}
