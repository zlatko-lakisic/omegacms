using MD.CMS.BusinessLogic.Core.Properties;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options
{
    public class ContentOptions
    {
        #region Attributes
        private List<string> _contentIds;
        private int _lcid;
        #endregion

        #region Properties
        public List<string> ContentIds 
        { 
            get
            {
                if(_contentIds == null)
                {
                    _contentIds = new List<string>();
                }
                return _contentIds;
            } 
            set => _contentIds = value;
        }
        public string[] ContentIdsArray
        {
            get
            {
                return ContentIds.ToArray();
            }
        }
        public bool LoadAuthor { get; set; }
        public bool FillFields { get; set; }
        public bool FillMetaData { get; set; }
        public int Lcid 
        {
            get
            {
                if (_lcid.Equals(default))
                {
                    _lcid = Settings.Default.DefaultLcid;
                }
                return _lcid;
            } 
            set => _lcid = value; 
        }
        #endregion
    }
}
