using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles
{
    public class ErrorDetails
    {
        private Exception _innerException;
        private int _statusCode;

        public int StatusCode { get{ return _statusCode; } }
        public string Message
        {
            get
            {
                return InnerException.Message;
            }
        }

        internal Exception InnerException { get => _innerException; }

        public ErrorDetails(Exception innerException, int statusCode)
        {
            _innerException = innerException;
            _statusCode = statusCode;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
