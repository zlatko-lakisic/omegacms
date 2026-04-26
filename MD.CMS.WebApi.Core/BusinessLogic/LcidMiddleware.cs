using MD.CMS.BusinessLogic.Core.DataAccess;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.Properties;

namespace MD.CMS.WebApi.Core.BusinessLogic
{
    public class LcidMiddleware
    {
        private readonly RequestDelegate next;
        public LcidMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context != null && (context.Request.Headers.ContainsKey(Settings.Default.LCIDHeaderName) || context.Request.Query.ContainsKey(Settings.Default.LCIDHeaderName)))
            {
                string lcid = context.Request.Headers.GetValue(Settings.Default.LCIDHeaderName);
                if (string.IsNullOrEmpty(lcid))
                {
                    lcid = context.Request.Query.GetValue(Settings.Default.LCIDHeaderName);
                }


                if (string.IsNullOrEmpty(lcid))
                {
                    DataAccessSettings.SelectedLcid = lcid.ToInt32(default(int));
                    System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo(DataAccessSettings.SelectedLcid);
                }
            }

            await next(context);
        }
    }
}
