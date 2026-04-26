using MD.CMS.BusinessLogic.WebApi.Core.ActionResults;
using MD.Tools.Helpers.Core.Caching.CacheKeys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using MD.Tools.Helpers.Core.Caching;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching.Filters
{
    public class OmegaOutputCacheFilter : IAsyncActionFilter, IFilterMetadata
    {
        public class OmegaCacheResult : ObjectResult
        {
            public OmegaCacheResult(object value)
                    : base(value)
            {
                StatusCode = StatusCodes.Status200OK;
            }
        }

        #region Attributes
        private readonly OmegaCacheProfile _cacheProfile;
        private readonly ILogger _logger;
        private ICacheKeyGenerator _cacheKeyGenerator;
        #endregion

        #region Properties
        #endregion

        #region Methods
        public OmegaOutputCacheFilter(OmegaCacheProfile cacheProfile, ILoggerFactory loggerFactory, ICacheKeyGenerator cacheKeyGenerator = null)
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
            bool continueExecution = await OnBeforeActionExecutionAsync(context);
            if (continueExecution)
            {
                ActionExecutedContext resultContext = await next();
                await OnAfterActionExecutionAsync(resultContext);
            }
        }

        private async Task<bool> OnBeforeActionExecutionAsync(ActionExecutingContext context)
        {
            List<string> cacheHaderValues = new List<string>();
            if (_cacheProfile.Client.Enabled)
            {
                cacheHaderValues.Add("public");
                cacheHaderValues.Add($"max-age={_cacheProfile.Client.Duration}");
                SetEtag(context.HttpContext, _cacheProfile.CacheKey);
            }
            else
            {
                cacheHaderValues.Add("no-store");
            }
            SetHeader(context.HttpContext, "Cache-Control", string.Join(", ", cacheHaderValues));

            if (_cacheProfile.Server.Enabled)
            {
                string result = await OmegaCacheController.GetNewInstance().GetFromCacheAsync(_cacheProfile.CachingProvider, _cacheProfile.CacheKey);
                if (!string.IsNullOrEmpty(result))
                {
                    DateTime resultTime = await OmegaCacheController.GetNewInstance().GetDateFromCacheAsync(_cacheProfile.CachingProvider, _cacheProfile.CacheKey);
                    context.Result = new CacheActionResult(JsonConvert.DeserializeObject(result));
                    SetEtag(context.HttpContext, $"{_cacheProfile.CacheKey}-{resultTime.ToUniversalTime().Ticks}");
                    return false;
                }
            }
            return true;
        }

        private async Task OnAfterActionExecutionAsync(ActionExecutedContext context)
        {
            if (_cacheProfile.Server.Enabled && context.HttpContext.Response.StatusCode == StatusCodes.Status200OK)
            {
                switch (context.HttpContext.Response.StatusCode)
                {
                    case StatusCodes.Status200OK:
                        if (context.Result is IContentActionResult result)
                        {
                            string stringToCache = result.GetValue();
                            await OmegaCacheController.GetNewInstance().AddToCacheAsync(_cacheProfile.CachingProvider, TimeSpan.FromSeconds(_cacheProfile.Server.Duration), MD.CMS.BusinessLogic.WebApi.Core.Properties.Settings.Default.CacheSource, _cacheProfile.CacheKey, stringToCache);
                        }
                        else if (context.Result is OkObjectResult result2)
                        {
                            string stringToCache = JsonConvert.SerializeObject(result2.Value);
                            await OmegaCacheController.GetNewInstance().AddToCacheAsync(_cacheProfile.CachingProvider, TimeSpan.FromSeconds(_cacheProfile.Server.Duration), MD.CMS.BusinessLogic.WebApi.Core.Properties.Settings.Default.CacheSource, _cacheProfile.CacheKey, stringToCache);
                        }
                        break;
                }
            }
        }

        private void SetHeader(HttpContext context, string key, StringValues values)
        {
            if (!context.Response.Headers.ContainsKey(key))
            {
                context.Response.Headers.Add(key, values);
            }
            else
            {
                context.Response.Headers[key] = values;
            }
        }

        private void SetEtag(HttpContext context, string etag)
        {
            if (etag != null)
            {
                etag = $"{etag}:response-etag";
                SetHeader(context, "Etag", MD.Tools.Helpers.Core.Crypto.MD5Crypt.MD5Encrypt(etag).ToLowerInvariant());
            }
        }
        #endregion
    }
}
