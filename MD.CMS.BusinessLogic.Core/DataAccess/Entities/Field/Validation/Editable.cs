using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public abstract class Editable
    {
        #region Attributes
        private bool _edit;
        #endregion

        #region Properties

        public bool Edit
        {
            get { return _edit; }
            set { _edit = value; }
        }
        #endregion
    }
}
