using MD.CMS.BusinessLogic.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using MD.Tools.BaseDataAccess.Core.Entities;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class Culture : BaseEntity<long>
    {
        #region Attributes
        private int _LCID;
        private string _code;
        private string _isoCode;
        private string _name;
        private bool _isApproved;
        #endregion

        #region Properties
        public int LCID
        {
            get
            {
                if (_LCID.Equals(default(int)))
                {
                    _LCID = Settings.Default.DefaultLcid;
                }
                return _LCID;
            }
            set { _LCID = value; }
        }
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }
        internal string GoogleCode
        {
            get
            {
                if (!string.IsNullOrEmpty(_code))
                {
                    string[] codesArray = _code.Split('-');
                    if (codesArray.Length > 1)
                    {
                        return codesArray[0];
                    }
                }
                return _code;
            }
        }
        public string IsoCode
        {
            get
            {
                return _isoCode;
            }
            set
            {
                _isoCode = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public bool IsApproved
        {
            get
            {
                return _isApproved;
            }
            set
            {
                _isApproved = value;
            }
        }
        #endregion
    }
}
