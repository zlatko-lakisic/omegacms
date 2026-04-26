using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Core.Entities.WebRequestProperties
{
    /// <summary>
    /// Represents a generic, childless property for YouTube API.
    /// This class allows you to specify your own custom properties
    /// </summary>
    public class GenericChildlessProperty : GenericChildlessProperty<string>
    {
        public GenericChildlessProperty(string name, string value = "", bool isQueryStringParam = true)
            : base(name, value, isQueryStringParam)
        {
        }
    }
    /// <summary>
    /// Represents a generic, childless property for YouTube API.
    /// This class allows you to specify your own custom properties
    /// </summary>
    /// <typeparam name="T">Type of value to store</typeparam>
    public class GenericChildlessProperty<T> : ChildlessProperty
    {
        //FIELDS
        private string _name = string.Empty;
        private T _value = default(T);
        private bool _isQueryStringParam = false;

        //PROPERTIES
        /// <summary>
        /// Name of the property. Must match the name of the properties that the YouTube API is expecting.
        /// Find a list of all valid property names on https://developers.google.com/youtube/v3/docs/
        /// </summary>
        public override string Name
        {
            get { return _name; }
        }
        /// <summary>
        /// If true, this property will be sent via query string. If false, it will be sent in the HTTP request body
        /// </summary>
        public override bool IsQueryStringParam
        {
            get { return _isQueryStringParam; }
        }

        /// <summary>
        /// Value that will be delivered to YouTube API
        /// </summary>
        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = (T)value;
            }
        }

        //CONSTRUCTORS
        /// <summary>
        /// Creates a new instance of the GenericChildlessProperty
        /// </summary>
        /// <param name="name">Name of the property, as expected by the YouTube API</param>
        /// <param name="value">Value of the property that will be delivered to YouTube API. Optional initialy, but must be set before sending the parameter to YouTube API</param>
        /// <param name="isQueryStringParam">If true, the this property will be delievered in the query string of the HTTP request, otherwise it will be delivered in request body as JSON</param>
        public GenericChildlessProperty(string name, T value = default(T), bool isQueryStringParam = true)
        {
            this._name = name;
            this.Value = value;
            this._isQueryStringParam = isQueryStringParam;
        }
    }
}
