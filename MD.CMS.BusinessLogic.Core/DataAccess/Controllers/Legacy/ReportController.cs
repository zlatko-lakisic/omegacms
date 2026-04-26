using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Data;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportController : BaseController<ReportController>
    {
        public DataTable GetData(ReportDefinition definition)
        {
            return GetDataAsync(definition).Result;
        }

        public DataTable GetSampleData(ReportDefinition definition)
        {
            return GetSampleDataAsync(definition).Result;
        }
    }
}
