using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.BaseDataAccess.Core.Entities;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class TaxonomyContent : BaseEntity<long>
    {
        #region Attributes
        private long _contentId;
        private int _LCID;
        private DateTime _dateCreated;
        private long _taxonomyId;
        private string _title;
        private string _alias;
        private string _path;
        private string _type;
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

        public long TaxonomyId
        {
            get { return _taxonomyId; }
            set { _taxonomyId = value; }
        }

        public long ContentId
        {
            get { return _contentId; }
            set { _contentId = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value;}
            }
            
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        
        #endregion
    }
}
