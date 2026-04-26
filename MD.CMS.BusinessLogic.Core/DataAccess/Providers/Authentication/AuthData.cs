using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication
{
    public class AuthData
    {
        #region Attributes
        private Dictionary<string, string> _values;
        private string _authenticationProviderName;
        #endregion

        #region Properties
        public Dictionary<string, string> Values { get => _values; set => _values = value; }
        public string AuthenticationProviderName { get => _authenticationProviderName; set => _authenticationProviderName = value; }
        #endregion

        #region Methods
        public AuthData()
        {
            _values = new Dictionary<string, string>();
        }
        public T GetData<T>(string key, T defaultValue = default)
        {
            T returnValue = defaultValue;
            try
            {
                if (_values.ContainsKey(key) && !string.IsNullOrEmpty(_values[key]))
                {
                    switch (typeof(T).ToString())
                    {
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Boolean":
                        case "System.Decimal":
                            returnValue = MD.Tools.Helpers.Core.Helpers.Parser<T>.Parse(_values[key]);
                            break;
                        default:
                            returnValue = (T)Convert.ChangeType(_values[key], typeof(T));
                            break;
                    }
                }
            }
            catch
            {
                //Silent fail
            }
            return returnValue;
        }
        public void SetData<T>(string key, T value)
        {
            try
            {
                if (value != null)
                {
                    if (_values.ContainsKey(key))
                    {
                        _values[key] = value.ToString();
                    } 
                    else
                    {
                        _values.Add(key, value.ToString());
                    }
                }
            }
            catch
            {
                //Silent fail
            }
        }
        #endregion
    }
}
