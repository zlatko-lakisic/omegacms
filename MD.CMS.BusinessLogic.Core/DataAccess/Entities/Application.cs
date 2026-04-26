using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class Application : BaseEntity<string>
    {
        #region Attributes
        private string _publicApiKey;
        #endregion

        #region Properties
        public string PublicApiKey { get => _publicApiKey; set => _publicApiKey = value; }
        #endregion
    }
}
