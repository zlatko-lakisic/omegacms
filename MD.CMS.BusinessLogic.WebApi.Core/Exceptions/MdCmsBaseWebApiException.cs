using System;
using System.Net;

namespace MD.CMS.BusinessLogic.WebApi.Core.Exceptions
{
    public abstract class MdCmsBaseWebApiException : Exception
    {
        #region Properties
        public HttpStatusCode StatusCode { get; set; }
        #endregion

        #region Methods
        public MdCmsBaseWebApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
            this.Data.Add("Status Code", statusCode);
        }

        public MdCmsBaseWebApiException(HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
            this.Data.Add("Status Code", statusCode);
        }
        #endregion
    }
}
