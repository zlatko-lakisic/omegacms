using MD.Tools.BaseDataAccess.Core.Enumerations;
using MD.Tools.BaseDataAccess.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MD.Tools.Helpers.Core.Extensions.StringExt;

namespace MD.Tools.BaseDataAccess.Core.Entities
{
    public class BaseWebRequest
    {
        #region Attributes
        private List<IBaseWebRequestProperty> _parameters;
        private string _methodName;
        private string _methodPath;
        private string _domain;
        private List<KeyValuePair<string, string>> _parametersAsKeyPairValues = null;
        private WebRequestEnum _methodType = new WebRequestEnum();
        private RequestBodyType _bodyType = new RequestBodyType();
        private string _jsonBody;
        private List<KeyValuePair<string, string>> _encodedUrlBody;
        private bool _overrideJsonBody;
        private bool _overrideEncodedUrlBody;
        #endregion

        #region Properties
        /// <summary>
        /// Request method type
        /// </summary>
        public WebRequestEnum MethodType
        {
            get { return _methodType; }
            set { _methodType = value; }
        }

        /// <summary>
        /// List of request properties
        /// </summary>
        public List<IBaseWebRequestProperty> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        /// <summary>
        /// Name of the API method that is to be called
        /// </summary>
        public string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }

        /// <summary>
        /// Domain on which the request is being sent
        /// </summary>
        public string Domain
        {
            get { return (!string.IsNullOrEmpty(_domain) && !_domain.EndsWith("/")) ? string.Format("{0}/", _domain) : _domain; }
            set { _domain = value; }
        }

        /// <summary>
        /// Path to the method you want to call
        /// </summary>
        public string MethodPath
        {
            get { return _methodPath; }
            set { _methodPath = value; }
        }
        /// <summary>
        /// HTTP request body type - determines in what way the data will be delivered - either as UrlEncoded values or as JSON
        /// </summary>
        public RequestBodyType BodyType
        {
            get { return _bodyType; }
            set { _bodyType = value; }
        }

        /// <summary>
        /// Gets the parameters for this YouTube request in form of a <string,string> dictionary
        /// </summary>
        public List<KeyValuePair<string, string>> ParametersDictionary
        {
            get
            {
                return _parametersAsKeyPairValues == null
                    ? GenerateParamDictionary()
                    : _parametersAsKeyPairValues;
            }
        }

        /// <summary>
        /// Request body - usualy JSON
        /// </summary>
        public string JSONBody
        {
            get
            {
                if (_overrideJsonBody)
                {
                    if (string.IsNullOrEmpty(_jsonBody))
                    {
                        _jsonBody = string.Empty;
                    }
                    return _jsonBody;
                }
                return GenerateJson();
            }
            set
            {
                _jsonBody = value;
            }
        }

        /// <summary>
        /// Override json body with custom data
        /// </summary>
        public bool OverrideJsonBody
        {
            get { return _overrideJsonBody; }
            set { _overrideJsonBody = value; }
        }

        /// <summary>
        /// Override json body with custom data
        /// </summary>
        public bool OverrideEncodedUrlBody
        {
            get { return _overrideEncodedUrlBody; }
            set { _overrideEncodedUrlBody = value; }
        }

        public List<KeyValuePair<string, string>> EncodedUrlBody
        {
            get
            {
                if (_overrideEncodedUrlBody)
                {
                    return _encodedUrlBody;
                }
                return GenerateEncodedUrlBody();
            }
            set 
            { 
                _encodedUrlBody = value; 
            }
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Generates a URL string from this request with query string included
        /// </summary>
        /// <returns></returns>
        public string ToRequestUrlWithQueryStrings()
        {
            string requestUrl = string.Format("{0}?", this.ToStandardRequestUrl());

            //check if any parameters are available
            if (Parameters != null && Parameters.Any())
            {
                bool isFirstParameter = true;
                foreach (IBaseWebRequestProperty param in Parameters)
                {
                    if (isFirstParameter)
                    {
                        //this will be called only once, on the first parameter
                        requestUrl += string.Format("{0}={1}", param.Name, HttpUtility.UrlEncode(param.Value as string));
                        isFirstParameter = false;
                    }
                    else
                        requestUrl += string.Format("&{0}={1}", param.Name, HttpUtility.UrlEncode(param.Value as string));
                }

                //return the generated string
                return requestUrl;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Generates a URL string for this request
        /// </summary>
        /// <returns></returns>
        public string ToStandardRequestUrl(bool includeQueryParameters = false)
        {
            string requestUrl = string.Empty;

            //check if domain already contains HTTP 
            if (this.Domain.Contains("http")){
                requestUrl = string.Format("{0}{1}{2}", this.Domain, this.MethodPath, this.MethodName);
            }
            else
            {
                requestUrl = string.Format(@"https://{0}{1}{2}", this.Domain, this.MethodPath, this.MethodName);
            }              

            //add query string parameters if needed           
            if (includeQueryParameters && Parameters.Where(p => p != null && p.IsQueryStringParam).Any())
            {
                requestUrl += "?";
                foreach (IBaseWebRequestProperty param in Parameters)
                {
                    if (param != null)
                    {
                        if (param.IsQueryStringParam)
                            requestUrl += string.Format("{0}={1}&", param.Name, HttpUtility.UrlEncode(param.Value == null ? string.Empty : param.Value.ToString()));
                    }

                }

                //trim the last &
                requestUrl = requestUrl.TrimEnd('&');
            }

            return requestUrl;
        }
        #endregion

        #region Private
        /// <summary>
        /// Generates a dictionary of parameters
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> GenerateParamDictionary()
        {
            List<KeyValuePair<string, string>> results = new List<KeyValuePair<string, string>>();

            //construct the keypairs
            foreach (var param in this.Parameters)
            {
                KeyValuePair<string, string> nextKeypair = new KeyValuePair<string, string>(param.Name, param.ToJson());
                results.Add(nextKeypair);
            }

            //return the results
            _parametersAsKeyPairValues = results;
            return results;
        }

        /// <summary>
        /// Creates a JSON string for the request body from all the paramneters contained in the request
        /// </summary>
        /// <returns>Returns a string</returns>
        private string GenerateJson()
        {
            if (this.Parameters != null)
            {
                //open starting brackets
                string bodyResponse = string.Empty;
                bodyResponse = "{";

                foreach (var param in Parameters.Where(p => p.IsQueryStringParam == false))
                {
                    bodyResponse += string.Format("{0}", param.ToJson());
                    bodyResponse += ",";
                }

                //trim last ","
                bodyResponse = bodyResponse.TrimEnd(',');

                //close ending brackets
                bodyResponse += "}";

                //return the JSON body
                return bodyResponse;
            }
            else return string.Empty;
        }


        private List<KeyValuePair<string, string>> GenerateEncodedUrlBody()
        {
            if (this.Parameters != null)
            {
                //open starting
                List<KeyValuePair<string, string>> bodyResponse = new List<KeyValuePair<string, string>>();

                //add all parameters as URL Encoded values
                foreach (var param in Parameters)
                {
                    bodyResponse.Add(new KeyValuePair<string, string>(param.Name, param.Value.ToString()));
                }

                return bodyResponse;
            }
            else return new List<KeyValuePair<string, string>>();
        }
        #endregion
        #endregion
    }
}
