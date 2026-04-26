using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class MetaDataFieldValue : MetaDataField
    {
        #region Attributes
        private string _contentId;
        private long _metaDataFieldId;
        private long _lCID;
        private DateTime _dateCreated;
        private string _value;
        #endregion

        #region Properties

        public string ContentId
        {
            get { return _contentId; }
            set { _contentId = value; }
        }
        public long LCID
        {
            get { return _lCID; }
            set { _lCID = value; }
        }
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }


        public string DateCreated
        {
            get
            {
                return _dateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            set
            {
                if (value != null)
                {
                    _dateCreated = DateTime.Parse(value, CultureInfo.InvariantCulture);
                }
                else
                {
                    _dateCreated = DateTime.UtcNow;
                }
            }
        }

        public long MetaDataFieldId
        {
            get { return _metaDataFieldId; }
            set { _metaDataFieldId = value; }
        }
        #endregion

        public MetaDataFieldValue() : base()
        {
        }

        public MetaDataFieldValue(MetaDataField obj) :
            base(obj)
        {
            this._metaDataFieldId = obj.Id;
        }
    }
}
