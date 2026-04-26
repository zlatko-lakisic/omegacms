using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using Microsoft.AspNetCore.Mvc.Filters;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.Tools.Helpers.Core.TypeConversion;

namespace MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes
{
    public class BaseActionAttribute : ActionFilterAttribute
    {
        #region Attributes
        private bool _isAdministration;
        #endregion

        #region Properties
        /// <summary>
        /// Is this an administration api call?
        /// </summary>
        public bool IsAdministration { get => _isAdministration; set => _isAdministration = value; }
        #endregion

        #region Methods
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.HttpContext != null && (context.HttpContext.Request.Headers.ContainsKeyName(Settings.Default.IsAdministrationHeaderName) || context.HttpContext.Request.Query.ContainsKeyName(Settings.Default.IsAdministrationHeaderName)))
            {
                string isAdministrationValue = context.HttpContext.Request.Headers.GetValue(Settings.Default.IsAdministrationHeaderName);
                if (string.IsNullOrEmpty(isAdministrationValue))
                {
                    isAdministrationValue = context.HttpContext.Request.Query.GetValue(Settings.Default.IsAdministrationHeaderName);
                }


                if (string.IsNullOrEmpty(isAdministrationValue))
                {
                    _isAdministration = isAdministrationValue.ToBoolean(false);
                }
            }
            _isAdministration = false;
        }
        #endregion
    }
}