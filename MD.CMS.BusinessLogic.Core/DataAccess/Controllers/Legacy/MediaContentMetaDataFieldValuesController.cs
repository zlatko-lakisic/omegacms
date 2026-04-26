using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MediaContentMetaDataFieldValuesController : BaseController<MediaContentMetaDataFieldValuesController>
    {
        [Obsolete("Deprecated", true)]
        public MediaContentMetaDataFieldValues Save(MediaContentMetaDataFieldValues obj)
        {
            return SaveAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MediaContentMetaDataFieldValues> GetByMediaContent(MediaContent obj)
        {
            return GetByMediaContentAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteByMediaContent(MediaContent mediaContent)
        {
            return DeleteByMediaContentAsync(mediaContent).Result;
        }

    }
}
