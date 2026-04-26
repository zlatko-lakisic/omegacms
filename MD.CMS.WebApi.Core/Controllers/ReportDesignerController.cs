using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    //  [MdOutputCacheAttribute(OutputCacheType = MdOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ReportDesigner")]
    [TokenAuth]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Reporting")]
    public class ReportDesignerController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Route("[action]")]
        [ActionName("GetEntities")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportDesigner GetEntities")]
        public async Task<IActionResult> GetEntities()
        {
            List<Entity> entities = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllEntitiesAsync();
            return Ok(entities);
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("GenerateSampleReportdata")]
        public async Task<IActionResult> GenerateSampleReportdata([FromBody]ReportDefinition obj)
        {
            DataTable results = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetSampleDataAsync(obj);
            dynamic resultsTable = new ExpandoObject();
            resultsTable.columns = results.Columns.Cast<DataColumn>().Select(column =>
            {
                dynamic c = new ExpandoObject();
                c.title = column.ColumnName;
                return c;
            });
            resultsTable.rows = results.Rows.Cast<DataRow>().Select(row =>
            {
                List<dynamic> rowValues = new List<dynamic>();
                foreach (DataColumn column in results.Columns)
                {
                    dynamic rowValue = new ExpandoObject();
                    rowValue.value = row[column];
                    rowValues.Add(rowValue);
                }
                return rowValues;
            });
            return Ok(resultsTable);
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("GetAllColumns")]
        public async Task<IActionResult> GetAllColumns([FromBody]ReportDefinition obj)
        {
            return Ok(obj.Definition.Entities.Select(entity =>
            {
                dynamic column = new ExpandoObject();
                column.entity = entity.Name;
                column.columns = entity.Fields;
                return column;
            }));
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("DeleteDefinition")]
        [OmegaInvalidateCache("GetAllDefinitions")]
        [OmegaInvalidateCache("GetDefinitionById")]
        [OmegaInvalidateCache("Search")]
        public async Task<IActionResult> DeleteDefinition([FromBody]ReportDefinition obj)
        {
            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(obj);
            return Ok();
        }

      

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetAllDefinitions")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportDesigner GetAllDefinitions")]
        public async Task<IActionResult> GetAllDefinitions(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "Name ASC";
            }
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(id));

        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAllWithPagination")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportDesigner GetAllWithPagination")]
        public async Task<IActionResult> GetAllWithPagination([FromQuery] long pageIndex, [FromQuery] long pageSize, [FromQuery] string searchTerm, [FromQuery] string searchColumn, [FromQuery] string sort)
        {
            if (string.IsNullOrEmpty(sort))
            {
                sort = "Name ASC";
            }
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            searchColumn = System.Web.HttpUtility.UrlDecode(searchColumn);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllWithPaginationAsync(pageIndex, pageSize, searchTerm, searchColumn, sort));
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAllCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportDesigner GetAllCount")]
        public async Task<IActionResult> GetAllCount([FromQuery] string searchTerm, [FromQuery] string searchColumn)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            searchColumn = System.Web.HttpUtility.UrlDecode(searchColumn);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllCountAsync(searchTerm, searchColumn));
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetDefinitionById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportDesigner GetDefinitionById")]
        public async Task<IActionResult> GetDefinitionById(int id)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id));
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("Search")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportDesigner Search")]    
        public async Task<IActionResult> Search([FromQuery] string searchTerm, [FromQuery] string searchColumn)
        {
            List<ReportDefinition> searchResults = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm, searchColumn);
            return Ok(searchResults);
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("SaveDefinition")]
        [OmegaInvalidateCache("GetAllDefinitions")]
        [OmegaInvalidateCache("GetDefinitionById")]
        [OmegaInvalidateCache("GetAllCount")]
        [OmegaInvalidateCache("GetAllWithPagination")]
        [OmegaInvalidateCache("Search")]
        public async Task<IActionResult> SaveDefinition([FromBody]ReportDefinition obj)
        {
            obj.AuthorId = (await GetLoggedOnUser()).Id;
            return Ok(await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(obj));
        }

        [HttpDelete]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetAllDefinitions")]
        [OmegaInvalidateCache("GetDefinitionById")]
        [OmegaInvalidateCache("GetAllCount")]
        [OmegaInvalidateCache("GetAllWithPagination")]
        [OmegaInvalidateCache("Search")]
        public async Task<IActionResult> Delete(long id)
        {
            if (id != default(long))
            {
                ReportDefinition toDelete = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
                if (toDelete != null)
                {
                    await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(toDelete);
                    return Ok(toDelete);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest("Invalid id");
            }
        }
    }
}
