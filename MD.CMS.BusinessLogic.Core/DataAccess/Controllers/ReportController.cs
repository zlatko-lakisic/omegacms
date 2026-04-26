using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Data;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportController : BaseController<ReportController>
    {
        public async Task<DataTable> GetDataAsync(ReportDefinition definition)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Report;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Report.Methods.GetSampleData.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Report.Parameters.ReportDefinition.GetIntValue()) { Value = definition.Definition });
            return await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
        }

        public async Task<DataTable> GetSampleDataAsync(ReportDefinition definition)
        {
            definition.Definition.Limit = new Limit() { From = 0, To = 100 };
            return await GetDataAsync(definition);
        }
    }
}
