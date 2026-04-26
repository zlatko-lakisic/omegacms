using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "TaxonomyContent")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Taxonomy")]
    public class TaxonomyContentController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByTaxonomyId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "TaxonomyContent GetByTaxonomyId")]
        public async Task<IActionResult> GetByTaxonomyId(long id)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyIdAsync(id));
        }

        //id2 = lcid
        [HttpGet]
        [Route("[action]")]
        [ActionName("GetByTaxonomyIdCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "TaxonomyContent GetByTaxonomyIdCount")]
        public async Task<IActionResult> GetByTaxonomyIdCount([FromQuery] long taxonomyId, [FromQuery] int lcid, [FromQuery] string searchTerm)
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = HttpUtility.UrlDecode(searchTerm);
            int taxonomyContentCount = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyIdCountAsync(taxonomyId, searchTerm, lcid);
            return Ok(taxonomyContentCount);
        }

        [HttpGet]
        [Route("[action]/{id?}/{id2?}/{id3?}")]
        [ActionName("Search")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "TaxonomyContent Search")]
        public async Task<IActionResult> Search(string id, long id2, int id3)
        {
            string searchTerm = id;
            long parentId = id2;
            int lcid = id3;
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm, parentId, lcid));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("PaginationGetByTaxonomyId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "TaxonomyContent PaginationGetByTaxonomyId")]
        public virtual async Task<IActionResult> PaginationGetByTaxonomyId([FromQuery] long taxonomyId, [FromQuery] int lcid, [FromQuery] int currentPageIndex, [FromQuery] int maxNumberOfRows, [FromQuery] string searchTerm, [FromQuery] string sort = "Order ASC")
        {
            if (searchTerm == null) {
                searchTerm = "";
            }
            searchTerm = HttpUtility.UrlDecode(searchTerm);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyIdWithPaginationAsync(taxonomyId, currentPageIndex, maxNumberOfRows, searchTerm, lcid, sort));
        }

        [HttpDelete]
        //id2 = taxonomyId    
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetByTaxonomyId")]
        [OmegaInvalidateCache("GetByTaxonomyIdCount")]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId")]
        [OmegaInvalidateCache("GetById", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentIdCount", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetAll", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetHierarchyByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomySearchByName", typeof(TaxonomyController))]
        public async Task<IActionResult> Delete(long id, long id2)
        {
            IEnumerable<TaxonomyContent> taxonomyContents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByTaxonomyIdAsync(id2);
            TaxonomyContent taxonomyContent = new TaxonomyContent();


            foreach (TaxonomyContent tc in taxonomyContents)
            {
                if (tc.Id == id)
                {
                    taxonomyContent = tc;
                }
            }    

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(taxonomyContent))
                return Ok();

            return BadRequest();
        }

        //POST: ws/TaxonomyContent/Delete
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent, PermissionAccessTypeEnum.Delete)]
        [Route("[action]")]
        [ActionName("DeleteTaxonomyContent")]
        [OmegaInvalidateCache("GetByTaxonomyId")]
        [OmegaInvalidateCache("GetByTaxonomyIdCount")]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId")]
        [OmegaInvalidateCache("GetById", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentIdCount", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetAll", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetHierarchyByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomySearchByName", typeof(TaxonomyController))]
        public async Task<IActionResult> DeleteTaxonomy([FromBody]Content content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //TaxonomyContent result = new TaxonomyContent();
             bool  result;
            foreach (Taxonomy taxonomy in content.Taxonomy)
            {
             result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteTaxonomyAsync(content, taxonomy);
             if (result)
                 return Ok();
            }        

          

            return BadRequest();

        }


        //POST: ws/TaxonomyContent/Delete
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetByTaxonomyId")]
        [OmegaInvalidateCache("GetByTaxonomyIdCount")]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId")]
        [OmegaInvalidateCache("GetById", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentIdCount", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetAll", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetHierarchyByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomySearchByName", typeof(TaxonomyController))]
        public async Task<IActionResult> Post([FromBody]Content content)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            content.AuthorId = "1";
            User Author = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(content.AuthorId);

            Content newContent = new Content();
            newContent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(content);
            if (content.ContentType != null && content.ContentType.Fields != null && content.ContentType.Fields.Any())
            {
                foreach (ContentTypeDefinitionFieldValue field in content.ContentType.Fields)
                {
                    if (field.Value != null)
                    {
                        field.ContentId = newContent.Id;
                        field.LCID = newContent.LCID;
                        field.DateCreated = Convert.ToDateTime(newContent.DateCreated);
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(field);
                    }
                }
            }


            TaxonomyContent result = new TaxonomyContent();
            for (var i = 0; i < content.Taxonomy.Count; i++)
            {
                int order = i;
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().Caller(await GetLoggedOnUser()).SaveAsync(newContent, content.Taxonomy[i], order);
            }
            //foreach (Taxonomy taxonomy in content.Taxonomy)
            //{
            //    result = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Save(newContent, taxonomy);
            //}

            Content c = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { newContent.Id }
            })).FirstOrDefault();

            if (c == null)
                return BadRequest();

            return Ok(c);

        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("SaveTaxonomyContent")]
        [OmegaInvalidateCache("GetByTaxonomyId")]
        [OmegaInvalidateCache("GetByTaxonomyIdCount")]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId")]
        [OmegaInvalidateCache("GetById", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentIdCount", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetAll", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetHierarchyByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomySearchByName", typeof(TaxonomyController))]
        public async Task<IActionResult> Post([FromBody]Taxonomy taxonomy)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
  

            Taxonomy newtaxonomy = new Taxonomy();
            newtaxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(taxonomy);
            if (newtaxonomy != null && taxonomy.Contents != null)
            {
                TaxonomyContent result = new TaxonomyContent();
                for (var i = 0; i < taxonomy.Contents.Count; i++)
                {
                    int order = i;
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().Caller(await GetLoggedOnUser()).SaveAsync(taxonomy.Contents[i], newtaxonomy, order);
                }
               
            }



            if (newtaxonomy == null)
                return BadRequest();

            return Ok(newtaxonomy);

        }
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent, PermissionAccessTypeEnum.Write)]
        [Route("[action]/{id?}")]
        [ActionName("Update")]
        [OmegaInvalidateCache("GetByTaxonomyId")]
        [OmegaInvalidateCache("GetByTaxonomyIdCount")]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId")]
        [OmegaInvalidateCache("GetById", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentIdCount", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetAll", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetHierarchyByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomySearchByName", typeof(TaxonomyController))]
        public async Task<IActionResult> Update([FromBody]Taxonomy taxonomy, int id)
        {
            int order = id;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (taxonomy != null && taxonomy.Contents != null)
            {
                TaxonomyContent result = new TaxonomyContent();
                for (var i = 0; i < taxonomy.Contents.Count; i++)
                {               
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().Caller(await GetLoggedOnUser()).UpdateAsync(taxonomy.Contents[i], taxonomy, order);
                    order++;
                }
            }
            if (taxonomy == null)
                return BadRequest();
            return Ok(taxonomy);
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent, PermissionAccessTypeEnum.Delete)]
        [Route("[action]")]
        [ActionName("DeleteContent")]
        [OmegaInvalidateCache("GetByTaxonomyId")]
        [OmegaInvalidateCache("GetByTaxonomyIdCount")]
        [OmegaInvalidateCache("PaginationGetByTaxonomyId")]
        [OmegaInvalidateCache("GetById", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByParentIdCount", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomyContentGetTaxonomyByContent", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetAll", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetHierarchyByParentId", typeof(TaxonomyController))]
        [OmegaInvalidateCache("GetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("PaginationGetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("TaxonomySearchByName", typeof(TaxonomyController))]
        public async Task<IActionResult> DeleteContent([FromBody]Taxonomy taxonomy)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //TaxonomyContent result = new TaxonomyContent();
            foreach (Content content in taxonomy.Contents)
            {
              bool  result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteTaxonomyAsync(content, taxonomy);
            }
            return Ok(new GenericResponse { Success = true });
        

        }

    }
}
