using System;
using System.Collections;
using System.Data;
using MD.Tools.Helpers.Core.TypeConversion;
using System.Linq;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public class ExtendedMethodProperty : MethodProperty, IExtendedMethodProperty
    {
        #region Attributes
        private string _propertyNameValue;
        private DbType _propertyType;
        private bool _transformValueArrayToCsv;
        #endregion

        #region Properties
        /// <summary>
        /// Property name value
        /// </summary>
        public string PropertyNameValue
        {
            get { return _propertyNameValue; }
        }
        /// <summary>
        /// Property value type
        /// </summary>
        public DbType PropertyType
        {
            get { return _propertyType; }
        }
        #endregion

        #region Methods
        public ExtendedMethodProperty(string propertyNameValue, DbType propertyType, int id)
            : base(id)
        {
            _propertyNameValue = propertyNameValue;
            _propertyType = propertyType;
            _methodPropertyType = propertyType;
        }
        public ExtendedMethodProperty(string propertyNameValue, DbType propertyType, int id, object value)
            : base(id, value, propertyType)
        {
            _propertyNameValue = propertyNameValue;
            _propertyType = propertyType;
        }

        public override object Value 
        {
            get
            {
                if (!_transformValueArrayToCsv)
                {
                    if (base.Value != null && base.Value != DBNull.Value)
                    {
                        switch (_propertyType)
                        {
                            case DbType.Int16:
                                return base.Value.ToString().ToInt16(default(int));
                            case DbType.Int32:
                                return base.Value.ToString().ToInt32(default(int));
                            case DbType.Int64:
                                return base.Value.ToString().ToInt64(default(int));
                        }
                    }
                }
                else
                {
                    if (base.IsArray || base.Value.GetType().IsArray)
                    {
                        base.Value = string.Join(_arrayDelimiter, ((IEnumerable)base.Value).Cast<object>().Select((obj) => {
                            if (obj != null && obj != DBNull.Value)
                            {
                                switch (_propertyType)
                                {
                                    case DbType.Int16:
                                        return obj.ToString().ToInt16(default(int));
                                    case DbType.Int32:
                                        return obj.ToString().ToInt32(default(int));
                                    case DbType.Int64:
                                        return obj.ToString().ToInt64(default(int));
                                }
                            }
                            return obj;
                        }).ToArray());
                    }
                }
                return base.Value;
            } 
            set => base.Value = value; 
        }

        public bool TransformValueArrayToCsv { get => _transformValueArrayToCsv; set => _transformValueArrayToCsv = value; }
        public string ArrayDelimiter 
        { 
            get
            {
                if (string.IsNullOrEmpty(_arrayDelimiter))
                {
                    _arrayDelimiter = ",";
                }
                return _arrayDelimiter;
            }
            set => _arrayDelimiter = value; 
        }
        #endregion

    }
}
