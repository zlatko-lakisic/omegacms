using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public class WebAddressType : Editable
    {
        #region Attributes
        private List<string> _includes;
        private List<string> _protocols;
        #endregion

        #region Properties

        public List<string> Includes
        {
            get 
            {
                if (_includes == null)
                {
                    _includes = new List<string>();
                }
                return _includes; 
            }
            set { _includes = value; }
        }

        public List<string> Protocols
        {
            get 
            {
                if (_protocols == null)
                {
                    _protocols = new List<string>();
                }
                return _protocols; 
            }
            set { _protocols = value; }
        }
        #endregion
    }
}
