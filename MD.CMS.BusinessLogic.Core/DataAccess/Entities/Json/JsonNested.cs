using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Json
{
    public class JsonNested
    {
        #region Attributes
        private string _value;
        private string _name;
        private string _parentId;
        private IEnumerable<JsonNested> _children;
        #endregion

        #region Properties

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public IEnumerable<JsonNested> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public string ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }
        #endregion
    }
}
