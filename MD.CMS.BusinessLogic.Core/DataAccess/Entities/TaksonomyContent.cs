using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class TaksonomyContent : Content
    {
        #region Attributes
        private List<Taxonomy> _taxonomy;
       
        #endregion
         #region Properties
        public List<Taxonomy> Taxonomy
        {
            get { return _taxonomy; }
            set { _taxonomy = value; }
        }
         #endregion
    }
}
