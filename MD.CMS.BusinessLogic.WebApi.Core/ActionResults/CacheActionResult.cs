using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MD.CMS.BusinessLogic.WebApi.Core.ActionResults
{
    public class CacheActionResult : IActionResult
    {
        #region Attributes
        private object _value;
        #endregion

        #region Methods
        public CacheActionResult(object value)
        {
            _value = value;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            ObjectResult objectResult = new ObjectResult(_value)
            {
                StatusCode = (int)HttpStatusCode.OK
            };

            await objectResult.ExecuteResultAsync(context);
        }
        #endregion
    }
}