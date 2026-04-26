using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.BaseDataAccess.Core.Entities;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class MenuContent : BaseEntity<long>
    {
        #region Attributes
        private int _LCID;
        private DateTime _dateCreated;
        private long _menuId;
        private string _title;
        private string _menucontentpath;
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

        public string DateCreated
        {
            get { return _dateCreated.ToString("yyyy-MM-dd H:mm:ss", CultureInfo.InvariantCulture); }
            set { _dateCreated = DateTime.Parse(value, CultureInfo.InvariantCulture); }
        }

        public long MenuId
        {
            get { return _menuId; }
            set { _menuId = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string MenuContentPath
        {
            get { return _menucontentpath; }
            set { _menucontentpath = value; }
        }
        #endregion

        #region Methods

        public override string GetPermissionEntityId()
        {
            return string.Format("{0}-{1}", Id, LCID);
        }
        #endregion
    }
}
