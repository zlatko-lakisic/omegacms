using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFieldController : BaseController<ContentTypeDefinitionFieldController>
    {
        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionField GetById(long id, bool transformExpression = true)
        {
            return GetByIdAsync(id, transformExpression).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ContentTypeDefinitionField> GetByContentTypeDefinitionId(long contentTypeDefinitionId, bool transformExpression = true)
        {
            return GetByContentTypeDefinitionIdAsync(contentTypeDefinitionId, transformExpression).Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentTypeDefinitionField Save(ContentTypeDefinitionField obj)
        {
            return SaveAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(ContentTypeDefinitionField obj)
        {
            return DeleteAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(long id)
        {
            return DeleteAsync(id).Result;
        }
    }
}
