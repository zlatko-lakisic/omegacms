using MD.Tools.BaseDataAccess.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MD.CMS.BusinessLogic.WebApi.Core.BaseControllers
{
    public class SecureApiWebRequestController : MD.Tools.BaseDataAccess.Core.Controllers.BaseController<SecureApiWebRequestController>, IBaseControllerSettings
    {
        #region Attributes
        private string _method;
        private string _url;
        private bool _isJsonArray;
        private List<KeyValuePair<string, string>> _headers;
        private string _data;
        #endregion

        #region Methods
        public SecureApiWebRequestController()
        {
        }

        public SecureApiWebRequestController(string method, string url, bool isJsonArray, IEnumerable<KeyValuePair<string, string>> headers, string data)
        {
            _method = method;
            _url = url;
            _isJsonArray = isJsonArray;
            _headers = headers.ToList();
            _data = data;
        }

        public string Execute()
        {
            //BaseWebRequest request = new BaseWebRequest();

            //request.BodyType = Tools.BaseDataAccess.Enumerations.RequestBodyType.JSON;

            //switch (_method.ToLowerInvariant())
            //{
            //    case "post":
            //        request.MethodType = Tools.BaseDataAccess.Enumerations.WebRequestEnum.Post;
            //        break;
            //    case "put":
            //        request.MethodType = Tools.BaseDataAccess.Enumerations.WebRequestEnum.Put;
            //        break;
            //    case "delete":
            //        request.MethodType = Tools.BaseDataAccess.Enumerations.WebRequestEnum.Delete;
            //        break;
            //    default:
            //        request.MethodType = Tools.BaseDataAccess.Enumerations.WebRequestEnum.Get;
            //        break;
            //}

            //if (!string.IsNullOrEmpty(_contentType))
            //{
            //    _headers.Add(new KeyValuePair<string, string>("Content-Type", "application/json; charset=UTF-8"));
            //    _headers.Add(new KeyValuePair<string, string>("Accept", "application/json, text/javascript, */*; q=0.01"));
            //}
            //else
            //{
            //    _headers.Add(new KeyValuePair<string, string>("Accept", "*/*"));
            //}

            //List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
            //foreach (KeyValuePair<string, string> header in _headers)
            //{
            //    if (!string.IsNullOrEmpty(header.Key) && !string.IsNullOrEmpty(header.Value))
            //    {
            //        KeyValuePair<string, string> newHeader = new KeyValuePair<string, string>(header.Key, header.Value);
            //        if (headers.Contains(newHeader))
            //        {
            //            headers.Remove(newHeader);
            //        }
            //        headers.Add(newHeader);
            //    }
            //}
            //_headers = headers;
            
            //request.Domain = _url;
            //request.Parameters = new List<IBaseWebRequestProperty>();
            //request.JSONBody = _data;
            //request.OverrideJsonBody = true;

            //return this.ExecuteWebRequest(request, _headers.ToArray());
            return string.Empty;
        }
        #endregion

        public string ConnectionString
        {
            get { return string.Empty; }
        }
    }
}