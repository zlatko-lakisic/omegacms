using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MetaDataFieldValueController : BaseController<MetaDataFieldValueController>
    {
        [Obsolete("Deprecated", true)]
        public List<MetaDataFieldValue> GetByContent(Content obj)
        {
            return GetByContentAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public MetaDataFieldValue Save(MetaDataFieldValue obj)
        {
            return SaveAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public MetaDataFieldValue Update(MetaDataFieldValue obj)
        {
            return UpdateAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteByContent(MetaDataFieldValue obj)
        {
            return DeleteByContentAsync(obj).Result;
        }
    }
}
