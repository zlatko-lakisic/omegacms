using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public class CharacterType : Editable
    {
        #region Attributes
        private bool _letters;
        private CasingType _casing;
        private SpecialCharactersType _specialCharacters;
        private NumbersType _numbers;
        #endregion

        #region Properties

        public bool Letters
        {
            get { return _letters; }
            set { _letters = value; }
        }

        public CasingType Casing
        {
            get { return _casing; }
            set { _casing = value; }
        }

        public SpecialCharactersType SpecialCharacters
        {
            get { return _specialCharacters; }
            set { _specialCharacters = value; }
        }

        public NumbersType Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
        }
        #endregion

        #region Methods
        public CharacterType()
        {
            _casing = new CasingType();
            _specialCharacters = new SpecialCharactersType();
            _numbers = new NumbersType();
        }
        #endregion
    }
}
