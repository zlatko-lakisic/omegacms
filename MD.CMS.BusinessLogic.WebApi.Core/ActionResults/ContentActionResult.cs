using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MD.CMS.BusinessLogic.WebApi.Core.ActionResults
{
    public class ContentActionResult<T> : IActionResult, IContentActionResult
    {
        #region Attributes
        private HttpStatusCode _httpStatusCode;
        private T _value;
        private HttpRequest _request;
        #endregion

        #region Methods
        public ContentActionResult(HttpStatusCode httpStatusCode, ControllerBase controller = null, T value = default(T))
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            _httpStatusCode = httpStatusCode;
            _request = controller.Request;
            _value = value;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            ObjectResult objectResult = new ObjectResult(_value)
            {
                StatusCode = (int)_httpStatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }

        public string GetValue()
        {
            return JsonConvert.SerializeObject(_value);
        }
        #endregion
    }
}