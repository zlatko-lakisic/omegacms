using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles
{
    public class TemplateDirectory
    {
        #region Attributes
        private string _path;
        private List<TemplateDirectory> _children;
        private List<TemplateFile> _files;
        private const string _root = "C:\\Projects\\MD.CMS\\MD.CMS.Website\\src\\scripts";
        private string _name;
        #endregion

        #region Properties
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public List<TemplateDirectory> Children
        {
            get { return _children; }
            set { _children = value; }
        }
        public List<TemplateFile> Files
        {
            get { return _files; }
            set { _files = value; }
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