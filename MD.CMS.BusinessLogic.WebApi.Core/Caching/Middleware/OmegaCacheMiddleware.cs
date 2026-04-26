using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching.Middleware
{
    public class OmegaCacheMiddleware
    {
        private readonly RequestDelegate _next;

        public OmegaCacheMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);
        }
    }
}
