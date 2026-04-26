using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ContentAlias : BaseEntity<long>
    {
        #region Attributes
        private long _contentId;
        private int _lcid;
        private DateTime _dateCreated;
        private string _alias;
        #endregion
        #region Properties

        public long ContentId {
            get { return _contentId; }
            set { _contentId = value; }
        }
        public int LCID
        {
            get { return _lcid; }
            set { _lcid = value; }
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
        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }
        public bool IsNew
        {
            get
            {
                return ContentId.Equals(default(long));
            }
        }
        #endregion
    }
}
