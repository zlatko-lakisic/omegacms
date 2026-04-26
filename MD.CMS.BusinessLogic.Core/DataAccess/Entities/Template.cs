using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class Template : BaseEntity<long>
    {
        #region Attributes
        private string _name;
        private string _description;
        private string _templateUrl;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string TemplateUrl
        {
            get { return _templateUrl; }
            set { _templateUrl = value; }
        }

        public bool IsNew
        {
            get
            {
                return Id.Equals(default(long));
            }
        }
        #endregion
    }
}
