using MD.CMS.BusinessLogic.Core.DataAccess;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;

namespace MD.CMS.WebApi.Core.Controllers
{
    [TokenAuth]
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Taxonomy")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Taxonomy")]
    public class TaxonomyController : BaseLoggedOnWebApiController
    {

        //GET: ws/Taxonomy/GetById/1        
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            bool fillParent = false;
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers.GetValue("fillParent")))
                fillParent = HttpContext.Request.Headers.GetValue("fillParent").ToBoolean(false);

            Taxonomy taxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, fillParent);

            if (taxonomy == null)
                return NotFound();

            taxonomy.Items = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyIdAsync(taxonomy.Id);
            return Ok(taxonomy);
        }


        //GET: ws/Taxonomy/GetByParentId/1      
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetByParentId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy GetByParentId")]
        public async Task<IActionResult> GetByParentId(long id, bool id2 = false)
        {
            int depth = 0;
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers.GetValue("depth")))
                depth = HttpContext.Request.Headers.GetValue("depth").ToInt32(default(int));

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdAsync(id, depth, loadContents: id2));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}/{id3?}")]
        [ActionName("Search")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy Search")]
        public async Task<IActionResult> Search(string id, long id2, bool id3)
        {
            string searchTerm = id;
            long parentId = id2;
            bool recursive = id3;
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm, parentId, recursive));
        }


        //id2 = lcid
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [ActionName("GetByParentIdCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy GetByParentIdCount")]
        public async Task<IActionResult> GetByParentIdCount([FromQuery] long taxonomyId, [FromQuery] int lcid, [FromQuery] string searchTerm)
        {
            lcid = DataAccessSettings.SelectedLcid;
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = HttpUtility.UrlDecode(searchTerm);
            int taxonomyCount = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdCountAsync(taxonomyId, lcid, searchTerm);
            return Ok(taxonomyCount);
        }


        //GET: ws/Taxonomy/GetByContent/1       
        //id - contentId
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("GetByContent")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy GetByContent")]
        public async Task<IActionResult> GetByContent(string id)
        {
            int depth = int.MaxValue;
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers.GetValue("depth")))
                depth = HttpContext.Request.Headers.GetValue("depth").ToInt32(int.MaxValue);

            Content content = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { id }
            })).FirstOrDefault();

            if (content == null)
                return Ok(new List<Taxonomy>());

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByContentAsync(content, depth));
        }
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [ActionName("TaxonomyContentGetTaxonomyByContent")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy TaxonomyContentGetTaxonomyByContent")]
        public async Task<IActionResult> TaxonomyContentGetTaxonomyByContent([FromBody]Content content)
        {
            int depth = int.MaxValue;
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers.GetValue("depth")))
                depth = HttpContext.Request.Headers.GetValue("depth").ToInt32(int.MaxValue);
            Content contents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByAllAsync(content);

            if (contents == null)
                return null;
            return Ok(await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).TaxonomyContentGetTaxonomyByContentAsync(contents));
        }
        //GET: ws/Taxonomy/GetAll
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy GetAll")]
        public async Task<IActionResult> GetAll()
        {
            int lcid = DataAccessSettings.SelectedLcid;
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(lcid));
        }

        //GET: ws/Taxonomy/GetHierarchyByParentId/1      
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetHierarchyByParentId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy GetHierarchyByParentId")]
        public async Task<IActionResult> GetHierarchyByParentId(long id, bool id2 = false)
        {
            List<Taxonomy> result = new List<Taxonomy>();
            int depth = int.MaxValue;
            if (!string.IsNullOrEmpty(HttpContext.Request.Headers.GetValue("depth")))
                depth = HttpContext.Request.Headers.GetValue("depth").ToInt32(int.MaxValue);

            result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetHierarchyByParentIdAsync(id, depth, id2);
            return Ok(result);
        }

        //[HttpPost]
        //[Permissions(PerrmissionsEnum.TaxonomyControllerGetTaxonomyByPath)]
        //[Lcid]
        //[ActionName("GetTaxonomyByPath")]
        //public IActionResult GetTaxonomyByPath(GenericJsonSingleObject<long> obj)
        //{
        //    int lcid = DataAccessSettings.SelectedLcid;
        //    Taxonomy taxonomy = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetTaxonomyByPath(obj.ValueName, true, true, lcid);
        //    if (taxonomy != null)
        //    {
        //        taxonomy.Children = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentId(taxonomy.Id, 1, lcid);
        //        //  taxonomy.Items = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyId(taxonomy.Id, lcid);
        //        taxonomy.Contents = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyId(taxonomy.Id, lcid);
        //        return Ok(taxonomy);
        //    }

        //    return NotFound();
        //}

        //[HttpPost]
        //[ActionName("PaginationGetTaxonomyByPath")]
        //[Permissions(PerrmissionsEnum.TaxonomyControllerPaginationGetTaxonomyByPath)]
        //[Lcid]
        //public IActionResult PaginationGetTaxonomyByPath(GenericJsonSingleObject<long> obj)
        //{
        //    int lcid = DataAccessSettings.SelectedLcid;
        //    Taxonomy taxonomy = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetTaxonomyByPath(obj.ValueName, true, true, lcid);
        //    if (taxonomy != null)
        //    {
        //        if (obj.ValueArray.Length >= 2)
        //        {
        //            long currentPageIndex = obj.ValueArray[0];
        //            long maxNumberOfRows = obj.ValueArray[1];
        //            taxonomy.Children = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdWithPagination(taxonomy.Id, currentPageIndex, maxNumberOfRows, 0, lcid);
        //        }

        //        return Ok(taxonomy);
        //    }
        //    return NotFound();
        //}

        //GET: ws/Taxonomy/TaxonomySearchByName
        //[HttpGet]
        //[Permissions(PerrmissionsEnum.TaxonomyControllerTaxonomySerachByName)]
        //[ActionName("TaxonomySearchByName")]
        //public IEnumerable<Taxonomy> TaxonomySearchByName(string searchTerm, int currentPage, int pageSize, string orderColumn, bool reverseOrder)
        //{
        //    return MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).TaxonomySearchByName(searchTerm, currentPage, pageSize, orderColumn, reverseOrder);
        //}

        //POST: ws/Taxonomy/Save
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetTaxonomyByPath")]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath")]
        [OmegaInvalidateCache("TaxonomySearchByName")]
        [OmegaInvalidateCache("GetByTaxonomyId", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("GetByTaxonomyIdCount", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy", typeof(ContentController))]
        [OmegaInvalidateCache("GetByTaxonomyIdCount", typeof(TaxonomyContentController))]
        [ActionName("Save")]
        public async Task<IActionResult> Post([FromBody]Taxonomy taxonomy)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await TaxonomyExist(taxonomy.ParentId))
                return BadRequest("Taxonomy parent doesn't exist");

            if (taxonomy.ParentId == taxonomy.Id)
                return BadRequest();



            Taxonomy newTaxonomy = new Taxonomy();
            newTaxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(taxonomy);

            //Taxonomy c = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetById(newTaxonomy.Id);


            if (newTaxonomy == null)
                return BadRequest();

            return Ok(newTaxonomy);

        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("DeleteContent")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetTaxonomyByPath")]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath")]
        [OmegaInvalidateCache("TaxonomySearchByName")]
        [OmegaInvalidateCache("GetByTaxonomyId", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("GetByTaxonomyIdCount", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy",typeof(ContentController))]
        public async Task<IActionResult> DeleteContent(string id, [FromBody]GenericJsonSingleObject<long> obj)
        {
            Taxonomy taxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetTaxonomyByPathAsync(obj.ValueName);
            
            List<TaxonomyContent> taxonomyContents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyIdAsync(taxonomy.Id);
            if (taxonomy != null && taxonomyContents != null)
            {
                //TaxonomyContent result = new TaxonomyContent();
                bool result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteTaxonomyAsync(taxonomyContents.Where(c => c.Id.ToString() == id).Select(c => new Content() { Id = id, DateCreated = c.DateCreated, LCID = c.LCID }).First(), taxonomy);

                if (result == true)
                    return Ok(new GenericResponse { Success = true });

               throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("Contents from {0} are not deleted successfully", taxonomy.Name));
               
            }


            throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("Contents from {0} are not deleted successfully", taxonomy.Name));


        }



        //DELETE: ws/Taxonomy/Delete/1
        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetTaxonomyByPath")]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath")]
        [OmegaInvalidateCache("TaxonomySearchByName")]
        [OmegaInvalidateCache("GetByTaxonomyId", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("GetByTaxonomyIdCount", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId", typeof(TaxonomyContentController))]
        public async Task<IActionResult> Delete(long id)
        {
            Taxonomy taxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (taxonomy != null)
            {
               
              bool  success = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(taxonomy);
                if (!success)
                {
                    throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("{0} taxonomy is not deleted. Please try again.", taxonomy.Name));
                }
            }
            return Ok();
        }


        //POST: ws/Taxonomy/AssignContentToTaxonomy/1/1
        //id=taxonomyId, id2 = contentId
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Write)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("AssignContentToTaxonomy")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetTaxonomyByPath")]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath")]
        [OmegaInvalidateCache("TaxonomySearchByName")]
        [OmegaInvalidateCache("GetByTaxonomyId", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("GetByTaxonomyIdCount", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId", typeof(TaxonomyContentController))]
        public async Task<IActionResult> AssignContentToTaxonomy(long id, string id2)
        {
            if (id2 == default(long).ToString())
                return BadRequest();

            Taxonomy taxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            Content content = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { id2 }
            })).FirstOrDefault();

            if (taxonomy == null || content == null)
                return BadRequest();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).AssignContentToTaxonomyAsync(taxonomy, content))
                return Ok();

            return BadRequest();
        }

        public class taxonomyModel
        {
            public string contentId { get; set; }
            public long[] taxonomyIds { get; set; }
        }

        //POST: ws/Taxonomy/AssignContentToTaxonomy/1/1
        //id=taxonomyId, id2 = contentId
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("AssignContentToTaxonomies")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByParentId")]
        [OmegaInvalidateCache("GetByParentIdCount")]
        [OmegaInvalidateCache("GetByContent")]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetHierarchyByParentId")]
        [OmegaInvalidateCache("GetTaxonomyByPath")]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath")]
        [OmegaInvalidateCache("TaxonomySearchByName")]
        [OmegaInvalidateCache("GetByTaxonomyId", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("GetByTaxonomyIdCount", typeof(TaxonomyContentController))]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId", typeof(TaxonomyContentController))]
        public async Task<IActionResult> AssignContentToTaxonomies([FromBody]taxonomyModel model)
        {
            foreach(long taxonomyId in model.taxonomyIds)
            {
                await AssignContentToTaxonomy(taxonomyId, model.contentId);
            }

            return Ok();
        }


        private async Task<bool> TaxonomyExist(long id)
        {
            Taxonomy taxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (taxonomy == null)
                return false;

            return true;
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [ActionName("GetTaxonomyByPath")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy GetTaxonomyByPath")]
        public async Task<IActionResult> GetTaxonomyByPath([FromBody]GenericJsonSingleObject<long> obj)
        {           
            int lcid = DataAccessSettings.SelectedLcid;
            Taxonomy taxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetTaxonomyByPathAsync(obj.ValueName, true, true, lcid);
            if (taxonomy != null)
            {
                taxonomy.Children = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdAsync(taxonomy.Id, 1, lcid);
            //    taxonomy.Items = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyId(taxonomy.Id, lcid);
                taxonomy.Contents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyIdAsync(taxonomy.Id, lcid);
                return Ok(taxonomy);
            }

            return NotFound();
        }



        [HttpGet]
        [ActionName("GetTaxonomyWithPaginationByPath")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy GetTaxonomyWithPaginationByPath")]
        public async Task<IActionResult> GetTaxonomyWithPaginationByPath([FromQuery] string path, [FromQuery] long pageIndex, [FromQuery] long pageSize, [FromQuery] string searchTerm)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = HttpUtility.UrlDecode(searchTerm);
            Taxonomy taxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetTaxonomyByPathAsync(path, true, true, lcid);
            if (taxonomy != null)
            {
                MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<Taxonomy> childrenPagination = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdWithPaginationAsync(taxonomy.Id, pageIndex, pageSize, searchTerm, 0, lcid);
                taxonomy.Children = childrenPagination.Items;
                taxonomy.ChildrenTotalCount = childrenPagination.TotalCount;
                MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<TaxonomyContent> itemsPagination = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyIdWithPaginationAsync(taxonomy.Id, pageIndex, pageSize, searchTerm, lcid);
                taxonomy.Items = itemsPagination.Items;
                taxonomy.ItemsTotalCount = itemsPagination.TotalCount;
                return Ok(taxonomy);
            }
            return NotFound();
        }

        [HttpGet]
        [ActionName("GetByParentIdWithPagination")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy GetTaxonomyWithPaginationByPath")]
        public async Task<IActionResult> GetByParentIdWithPagination([FromQuery] long parentId, [FromQuery] long pageIndex, [FromQuery] long pageSize, [FromQuery] string searchTerm)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = HttpUtility.UrlDecode(searchTerm);
            Taxonomy taxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(parentId);
            if (taxonomy != null)
            {
                return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentIdWithPaginationAsync(taxonomy.Id, pageIndex, pageSize, searchTerm, 0, lcid));
            }
            return NotFound();
        }

        [HttpPost]
        [ActionName("UpdateChildren")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Write)]
        [Route("[action]/{id?}")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy UpdateChildren")]
        public async Task<IActionResult> UpdateChildren([FromBody]Taxonomy taxonomy, long id)
        {
            long order = id;
            if (taxonomy != null && taxonomy.Children != null)
            {
                for (var i = 0; i < taxonomy.Children.Count; i++)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateAsync(taxonomy.Children[i], order);
                    order++;
                }

            }

            return Ok(taxonomy);
        }

        //GET: ws/Taxonomy/TaxonomySearchByName
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("TaxonomySearchByName")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Taxonomy TaxonomySearchByName")]
        public async Task<IActionResult> TaxonomySearchByName([FromQuery] string searchTerm, [FromQuery] int currentPage, [FromQuery] int pageSize, [FromQuery] string orderColumn, [FromQuery] bool reverseOrder)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).TaxonomySearchByNameAsync(searchTerm, currentPage, pageSize, orderColumn, reverseOrder));
        }
    }
}
