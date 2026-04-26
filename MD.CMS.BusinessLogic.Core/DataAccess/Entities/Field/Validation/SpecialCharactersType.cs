using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public class SpecialCharactersType : Editable
    {
        #region Attributes
        private List<string> _included;
        #endregion

        #region Properties

        public List<string> Included
        {
            get 
            {
                if (_included == null)
                {
                    _included = new List<string>();
                }
                return _included; 
            }
            set { _included = value; }
        }
        #endregion
    }
}
