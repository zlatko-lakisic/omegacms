using MD.CMS.BusinessLogic.WebApi.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles
{
    public class AliasModel<T>
    {
        #region Attributes
        private AliasType _aliasType;
        private string _id;
        private string _template;
        private T _content;
        #endregion

        #region Properties

        public AliasType AliasType
        {
            get { return _aliasType; }
            set { _aliasType = value; }
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Template
        {
            get { return _template; }
            set { _template = value; }
        }

        public T Content
        {
            get { return _content; }
            set { _content = value; }
        }
        #endregion
    }
}