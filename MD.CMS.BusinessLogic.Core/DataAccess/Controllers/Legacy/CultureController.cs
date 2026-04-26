using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class CultureController : BaseController<CultureController>
    {
        [Obsolete("Deprecated", true)]
        public Culture Save(Culture obj)
        {
            return SaveAsync(obj).Result;
        }

        [Obsolete("Deprecated", true)]
        public void Update(Culture obj)
        {
            Task.Run(async () => { await UpdateAsync(obj); }).Wait();
        }
        [Obsolete("Deprecated", true)]
        public bool Delete(Culture obj)
        {
            return DeleteAsync(obj).Result;
        }
        [Obsolete("Deprecated", true)]
        public Culture GetByLCID(int lcid, bool selectFromAll = false)
        {
            return GetByLCIDAsync(lcid, selectFromAll).Result;
        }
        //
        [Obsolete("Deprecated", true)]
        public Culture GetByCode(string code, bool selectFromAll = false)
        {
            return GetByCodeAsync(code, selectFromAll).Result;
        }
        //SelectAll
        [Obsolete("Deprecated", true)]
        public IEnumerable<Culture> GetAll()
        {
            return GetAllAsync().Result;
        }

        [Obsolete("Deprecated", true)]
        public IEnumerable<Culture> GetAllAvailableForContentId(long contentId)
        {
            return GetAllAvailableForContentIdAsync(contentId).Result;
        }

        //SelectAll
        [Obsolete("Deprecated", true)]
        public List<Culture> GetApproved()
        {
            return GetApprovedAsync().Result;
        }

        [Obsolete("Deprecated", true)]
        public DataSet SearchCms(string searchTerm)
        {
            return SearchCmsAsync(searchTerm).Result;
        }
    }
}
