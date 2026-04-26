using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using System.Linq;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.CMS.BusinessLogic.Core.DataAccess;
using Microsoft.AspNetCore.Mvc.Filters;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;

namespace MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes
{
    public class LcidAttribute : BaseActionAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext != null && (context.HttpContext.Request.Headers.ContainsKeyName(Settings.Default.LCIDHeaderName) || context.HttpContext.Request.Query.ContainsKeyName(Settings.Default.LCIDHeaderName)))
            {
                string lcid = context.HttpContext.Request.Headers.GetValue(Settings.Default.LCIDHeaderName);
                if (string.IsNullOrEmpty(lcid))
                {
                    lcid = context.HttpContext.Request.Query.GetValue(Settings.Default.LCIDHeaderName);
                }


                if (string.IsNullOrEmpty(lcid))
                {
                    DataAccessSettings.SelectedLcid = lcid.ToInt32(default(int));
                }
            }
          
            base.OnActionExecuting(context);
        }
    }
}