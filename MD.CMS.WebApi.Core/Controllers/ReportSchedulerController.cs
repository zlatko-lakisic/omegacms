using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ReportScheduler")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Reporting")]
    public class ReportSchedulerController : BaseLoggedOnWebApiController
    {
        /// <summary>
        /// Gets ReportScheduler by id
        /// </summary>
        /// <param name="id">Id of wanted ReportScheduler object</param>
        /// <returns>If found, "Ok" status code with found ReportScheduler object, else "NotFound" or "BadRequest" status code</returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportScheduler GetById")]
        [ActionName("GetById")]        
        public async Task<IActionResult> GetById(long id)
        {
            if (id != default(long))
            {
                ReportScheduler scheduler = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
                if (scheduler != null)
                {
                    return Ok(scheduler);
                }
                else
                {
                    return NotFound();
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Gets all ReportScheduler objects which belong to ReportDefinition object with given id
        /// </summary>
        /// <param name="id">Report Definition Id</param>
        /// <returns>If found, "Ok" status code with list of found ReportScheduler objects, else "NotFound" or "BadRequest" status code</returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportScheduler GetByReportDefinitionId")]
        [ActionName("GetByReportDefinitionId")]
        public async Task<IActionResult> GetByReportDefinitionId(int id)
        {
            if (id != default(long))
            {
                ReportDefinition reportDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
                if (reportDefinition != null)
                {
                    List<ReportScheduler> schedulers = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByReportDefinitionAsync(reportDefinition);
                    return Ok(schedulers);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }
                
        /// <summary>
        /// Gets all ReportScheduler objects
        /// </summary>
        /// <returns>If everything went fine, "Ok" status code with list of ReportScheduler objects, InternalServerError otherwise</returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportScheduler GetAll")]     
        [ActionName("GetAll")]
        public async Task<IActionResult> GetAll(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "Name ASC";
            }
            List<ReportScheduler> schedulers = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(id);
            if (schedulers != null)
            {
                return Ok(schedulers);
            }
            else
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets all ReportScheduler objects with pagination
        /// </summary>
        /// <returns>If everything went fine, "Ok" status code with list of ReportScheduler objects, InternalServerError otherwise</returns>
        [HttpGet]
        [Route("[action]")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportScheduler GetAllWithPagination")]     
        [ActionName("GetAllWithPagination")]
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
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllWithPaginationAsync(pageIndex, pageSize, searchTerm, searchColumn, sort));
        }

        /// <summary>
        /// Gets count of objects
        /// </summary>
        /// <returns>If everything went fine, "Ok" status code with numer of ReportScheduler objects, InternalServerError otherwise</returns>
        [HttpGet]
        [Route("[action]")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ReportScheduler GetAllCount")]
        [ActionName("GetAllCount")]
        public async Task<IActionResult> GetAllCount([FromQuery] string searchTerm, [FromQuery] string searchColumn)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            searchColumn = System.Web.HttpUtility.UrlDecode(searchColumn);
            long count = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllCountAsync(searchTerm, searchColumn);
            if (count != null)
            {
                return Ok(count);
            }
            else
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Saves ReportScheduler object with all its properties (that might include other objects or lists of other objects
        /// </summary>
        /// <param name="scheduler">ReportScheduler object to save</param>
        /// <returns>"Ok" status code with saved ReportScheduler object or "BadRequest" with ModelState in case that ModelState is not valid</returns>
        [HttpPost]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByReportDefinitionId")]
        [OmegaInvalidateCache("GetAll")]
        [ActionName("Save")]
        public async Task<IActionResult> Save([FromBody]ReportScheduler scheduler)
        {
            scheduler.AuthorId = (await GetLoggedOnUser()).Id;
            if (scheduler.ReportId == default(int))
            {
                return BadRequest("You have to create report definition if you don't have one!");
            }
            ReportScheduler newScheduler = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(scheduler);
            return Ok(newScheduler);
        }

        /// <summary>
        /// Deletes ReportScheduler object with coresponding id
        /// </summary>
        /// <param name="id">Id of ReportScheduler object to delete</param>
        /// <returns>"Ok" Status code if everything went fine, "NotFound" or "BadRequest" status codes otherwise</returns>
        [HttpDelete]
        [Route("[action]/{id?}")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByReportDefinitionId")]
        [OmegaInvalidateCache("GetAll")]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            if (id != default(long))
            {
                ReportScheduler toDelete = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
                if (toDelete != null)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportSchedulerController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(toDelete);
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

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetReportSchedulerActionTypes")]
        public async Task<IActionResult> GetReportSchedulerActionTypes()
        {
            var allReportSchedulerActionTypes = Enum.GetNames(typeof(ReportSchedulerAction.EnumAction));
            return Ok(allReportSchedulerActionTypes);
        }
    }
}