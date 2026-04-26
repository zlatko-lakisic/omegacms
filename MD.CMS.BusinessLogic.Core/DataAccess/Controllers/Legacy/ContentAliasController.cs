using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;


namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentAliasController : BaseController<ContentAliasController>
    {
        [Obsolete("Deprecated", true)]
        public List<ContentAlias> GetAll(int lcid = default(int))
        {
            return GetAllAsync(lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentAlias GetById(long id, int lcid = default(int))
        {
            return GetByIdAsync(id, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ContentAlias> GetByContent(long id, int lcid = default(int))
        {
            return GetByContentAsync(id, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(ContentAlias obj)
        {
            return DeleteAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool DeleteByContent(Content obj)
        {
            return DeleteByContentAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentAlias GetByContentId(long id, int lcid = default(int))
        {
            return GetByContentIdAsync(id, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public string GetAliasByContent(Content content)
        {
            return GetAliasByContentAsync(content).Result;
        }

        [Obsolete("Deprecated", true)]
        public ContentAlias Save(Content content, string alias)
        {
            return SaveAsync(content, alias).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<ContentAlias> GetAllAliasesByContent(Content content)
        {
            return GetAllAliasesByContentAsync(content).Result;
        }
    }
}