using System;
using System.Net;

namespace MD.CMS.BusinessLogic.WebApi.Core.Exceptions
{
    public class MdCmsWebApiAuthenticationException : MdCmsBaseWebApiException
    {
        #region Methods
        public MdCmsWebApiAuthenticationException(string ipAddress) : base(HttpStatusCode.Unauthorized, "The API call was aborted bacause the user is not authenticated with the current credentials!")
        {
            this.Data.Add("IP Address", ipAddress);
        }

        public MdCmsWebApiAuthenticationException(string ipAddress, string authenticationCode) : this(ipAddress)
        {
            this.Data.Add("Authentication Code", authenticationCode);
        }

        public MdCmsWebApiAuthenticationException(string ipAddress, string authenticationCode, Exception innerException) : base(HttpStatusCode.Unauthorized, "The API call was aborted bacause the user is not authenticated with the current credentials!", innerException)
        {
            this.Data.Add("IP Address", ipAddress);
            this.Data.Add("Authentication Code", authenticationCode);
        }
        #endregion
    }
}
