using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Helpers.Exceptions
{
    public class BaseDataAccessPluginException : BaseDataAccessPluginException<Exception>
    {
        public BaseDataAccessPluginException(BaseDataAccessPluginExceptionMapping mapping) : base(mapping)
        {
        }

        public BaseDataAccessPluginException(Exception innerException, BaseDataAccessPluginExceptionMapping mapping) : base(innerException, mapping)
        {
        }
    }

    public class BaseDataAccessPluginException<T> : Exception
        where T : Exception
    {
        #region Child Classes
        [Serializable]
        public class BaseDataAccessPluginExceptionMapping
        {
            #region Attributes
            private int _methodInt;
            private IEnumerable<IMethodProperty> _properties;
            private Entities _entity;
            private string _pluginSettings;
            #endregion

            #region Properties
            public int MethodInt { get => _methodInt; set => _methodInt = value; }
            public IEnumerable<IMethodProperty> Properties { get => _properties; set => _properties = value; }
            public string PluginSettings { get => _pluginSettings; set => _pluginSettings = value; }
            public Entities Entity { get => _entity; set => _entity = value; }
            #endregion
        }
        #endregion

        #region Attributes
        BaseDataAccessPluginExceptionMapping _mapping;
        #endregion

        #region Properties
        public BaseDataAccessPluginExceptionMapping Mapping => _mapping;
        #endregion

        #region Methods
        public BaseDataAccessPluginException(BaseDataAccessPluginExceptionMapping mapping) : base("The current user does not have sufficient permission to access this resource!")
        {
            _mapping = mapping;
            this.Data.Add("Mapping", mapping);
        }

        public BaseDataAccessPluginException(T innerException, BaseDataAccessPluginExceptionMapping mapping) : base("The current user does not have sufficient permission to access this resource!", innerException)
        {
            _mapping = mapping;
            this.Data.Add("Mapping", mapping);
        }
        #endregion
    }
}
