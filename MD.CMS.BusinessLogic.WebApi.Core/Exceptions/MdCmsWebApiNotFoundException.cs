using System;
using System.Net;

namespace MD.CMS.BusinessLogic.WebApi.Core.Exceptions
{
    public class MdCmsWebApiNotFoundException : MdCmsBaseWebApiException
    {
        #region Methods
        public MdCmsWebApiNotFoundException(string ipAddress, string requestAddress) : base(HttpStatusCode.NotFound, "The requested resource was not found within the CMS!")
        {
            this.Data.Add("IP Address", ipAddress);
            this.Data.Add("Request Address", requestAddress);
        }

        public MdCmsWebApiNotFoundException(string ipAddress, string requestAddress, Exception innerException) : base(HttpStatusCode.NotFound, "The requested resource was not found within the CMS!", innerException)
        {
            this.Data.Add("IP Address", ipAddress);
            this.Data.Add("Request Address", requestAddress);
        }
        #endregion
    }
}
