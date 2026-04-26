using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace MD.CMS.BusinessLogic.WebApi.Core.WebSockets
{
    public class OmegaSocketContext
    {
        #region Attributes
        private ConcurrentDictionary<string, string> _queryStrings;
        private Stream _body;
        private HttpStatusCode _result;
        #endregion

        #region Properties
        public ConcurrentDictionary<string, string> QueryStrings { get => _queryStrings; set => _queryStrings = value; }
        public Stream Body { get => _body; set => _body = value; }
        public HttpStatusCode Result { get => _result; set => _result = value; }
        public string AuthorizationCode
        {
            get
            {
                return WebSocketHelpers.GetAuthenticationHeader(_queryStrings);
            }
        }
        public string ConnectionId
        {
            get
            {
                return WebSocketHelpers.GetConnectionIdHeader(_queryStrings);
            }
        }
        #endregion

        #region Methods
        public OmegaSocketContext()
        {
        }

        public OmegaSocketContext(HttpContext context)
        {
            _queryStrings = WebSocketHelpers.QueryStringsToDictionary(context.Request.Query);
        }
        #endregion
    }
}
