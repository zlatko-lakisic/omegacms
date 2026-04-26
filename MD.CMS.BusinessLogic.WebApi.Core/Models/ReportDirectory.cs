using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles
{
    public class ReportDirectory
    {
        #region Attributes
        private string _path;
        private List<ReportDirectory> _children;      
        private const string _root = "C:\\Projects\\MD.CMS\\MD.CMS.Website\\src\\scripts";
        private string _name;
        #endregion

        #region Properties
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public List<ReportDirectory> Children
        {
            get { return _children; }
            set { _children = value; }
        }       
        public static string RootPath
        {
            get { return _root; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion
    }
}