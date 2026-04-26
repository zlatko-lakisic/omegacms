using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System;
using MD.Tools.Helpers.Core.Data;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFieldValueController : BaseController<ContentTypeDefinitionFieldValueController>
    {
        [Obsolete("Deprecated", true)]
        public List<ContentTypeDefinitionFieldValue> GetByContent(Content obj)
        {
            return GetByContentAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ContentTypeDefinitionFieldValue> GetByContentId(Content obj)
        {
            return GetByContentIdAsync(obj.Id, obj.LCID, obj.DateCreated).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ContentTypeDefinitionFieldValue> GetByValue(string value, long contentTypeDefinitionId = default, long contentTypeDefinitionFieldId = default, ComparerTypeEnum comparer = ComparerTypeEnum.Equals, DataTransformEnum transform = DataTransformEnum.ToString)
        {
            return GetByValueAsync(value, contentTypeDefinitionId, contentTypeDefinitionFieldId, comparer, transform).Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionFieldValue Save(ContentTypeDefinitionFieldValue obj)
        {
            return SaveAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionFieldValue Update(ContentTypeDefinitionFieldValue obj)
        {
            return UpdateAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionFieldValue Select(ContentTypeDefinitionFieldValue obj)
        {
            return SelectAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(ContentTypeDefinitionFieldValue obj)
        {
            return DeleteAsync(obj).Result;
        }
    }
}
