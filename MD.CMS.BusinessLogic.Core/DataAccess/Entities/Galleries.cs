#region Usings

using System;

#endregion 

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class Galleries 
    {
        #region Fields 

        private string _gallerieId;
        private DateTime _dateCreated;
        private string _path;
        private string _name;

        #endregion

        #region Properties

        public string GallerieId
        {
            get { return _gallerieId; }
            set { if (!Equals(_gallerieId, value))_gallerieId = value; }
        }

        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set{if (!Equals(_dateCreated, value))_dateCreated = value;}
        }
        public string Path
        {
            get { return _path; }
            set{if (!Equals(_path, value))_path = value;}
        }
        public string Name
        {
            get { return _name; }
            set{if (!Equals(_name, value))_name = value;}
        }
        #endregion 
    }
}
