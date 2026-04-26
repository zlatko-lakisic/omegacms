using System;
using System.Collections;
using System.Data;
using MD.Tools.Helpers.Core.TypeConversion;
using System.Linq;
using System.Collections.Generic;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public class MethodProperty : MethodProperty<object>
    {

        #region Methods
        public MethodProperty(int id) : base(id)
        {
        }
        public MethodProperty(int id, object value) : base(id, value)
        {
        }
        public MethodProperty(int id, object value, DbType propertyType) : base(id, value, propertyType)
        {
        }
        #endregion
    }

    public class MethodProperty<T> : IMethodProperty
    {
        #region Attributes
        private int _id;
        private T _value;
        protected DbType _methodPropertyType;
        private bool _isArray;
        protected static string _arrayDelimiter = ",";
        #endregion

        #region Properties
        /// <summary>
        /// Property Id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Method property value
        /// </summary>
        public virtual object Value
        {
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            get
            {
                object result = _value;
                if(_methodPropertyType != null
                    && _methodPropertyType == DbType.String
                    && _value == null)
                {
                    result = DBNull.Value;
                }
                return result;
            }
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            set { _value = (T)value; }
        }

        public bool IsArray { get => _isArray; set => _isArray = value; }
        #endregion

        #region Methods
        public MethodProperty(int id)
        {
            _id = id;
        }
        public MethodProperty(int id, T value)
        {
            _id = id;
            _value = value;
        }
        public MethodProperty(int id, T value, DbType propertyType)
        {
            _id = id;
            _value = value;
            _methodPropertyType = propertyType;
        }
        public static string ArrayToValue<E>(E[] array)
        {
            return EnumToValue(array.ToList());
        }
        public static string EnumToValue<E>(IEnumerable<E> array)
        {
            return string.Join(_arrayDelimiter, array);
        }
        #endregion
    }
}
