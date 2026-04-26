using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public class ValidationType
    {
        #region Attributes
        private LengthType _minLength;
        private LengthType _maxLength;
        private CharacterType _characterTypes;
        private TypeValidationType _typeValidation;
        private string _regex;
        private Boolean _required;
        private Boolean _repeatable;
        #endregion

        #region Properties

        public LengthType MinLength
        {
            get { return _minLength; }
            set { _minLength = value; }
        }

        public LengthType MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        public CharacterType CharacterTypes
        {
            get { return _characterTypes; }
            set { _characterTypes = value; }
        }

        public TypeValidationType TypeValidation
        {
            get { return _typeValidation; }
            set { _typeValidation = value; }
        }

        public string Regex
        {
            get { return _regex; }
            set { _regex = value; }
        }

        public Boolean Required
        {
            get { return _required; }
            set { _required = value; }
        }

        public Boolean Repeatable
        {
            get { return _repeatable; }
            set { _repeatable = value; }
        }
        #endregion

        #region Methods
        public ValidationType()
        {
            _maxLength = new LengthType();
            _minLength = new LengthType();
            _characterTypes = new CharacterType();
            _typeValidation = new TypeValidationType();
        }
        #endregion
    }
}
