using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles
{
    public class TemplateFile
    {
        #region Attributes
        private string _path;
        private string _name;
        #endregion

        #region Properties
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion
    }
}