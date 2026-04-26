using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent
{
    public abstract class GenericContentFieldValue: GenericContentField
    {
        #region Attributes
        private string _value;
        #endregion

        #region Properties

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion

        public GenericContentFieldValue() : base()
        {
        }

        public GenericContentFieldValue(GenericContentFieldValue obj) : base(obj)
        {
            _value = Value;
        }

        public GenericContentFieldValue(GenericContentField obj) : base(obj)
        {
        }
    }
}
