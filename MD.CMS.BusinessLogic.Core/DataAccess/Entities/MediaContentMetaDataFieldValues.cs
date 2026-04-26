using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class MediaContentMetaDataFieldValues : MetaDataField
    {
        #region Attributes
        private long _mediaContentId;
        private long _metaDataFieldId;

        private DateTime _dateCreated;
        private string _value;
        #endregion

        #region Properties

        public long MediaContentId
        {
            get { return _mediaContentId; }
            set { _mediaContentId = value; }
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

        public MediaContentMetaDataFieldValues()
        {
        }

        public MediaContentMetaDataFieldValues(MetaDataField obj) :
            base(obj)
        {
        }
    }
}
