using MD.CMS.BusinessLogic.Core.DataAccess;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [TokenAuth]
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ContentAlias")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content")]
    public class ContentAliasController : BaseLoggedOnWebApiController
    {

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [Lcid]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentAlias GetById")]
        public async Task<IActionResult> GetById(long id)
        {

            int lcid = DataAccessSettings.SelectedLcid;


            ContentAlias contentAlias = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentAliasController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, lcid);
            if (contentAlias == null)
                return NotFound();

            return Ok(contentAlias);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentAlias GetAll")]
        public async Task<IActionResult> GetAll()
        {
            int lcid = DataAccessSettings.SelectedLcid;
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentAliasController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(lcid));
        }


        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAllAliasesByContent")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType= OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ContentAlias GetAllAliasesByContent")]
        public async Task<IActionResult> GetAllAliasesByContent([FromBody]Content content)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            return Ok((await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentAliasController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAliasesByContentAsync(content)).Select(alias => alias.Alias));
        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllAliasesByContent")]
        [OmegaInvalidateCache("GetById", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetAll", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetAllVersion", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetByAll", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("MenuContentGetContentByMenu", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetByFolderId", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("PaginationGetByFolderId", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("GetBySearchTerm", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        [OmegaInvalidateCache("Translate", typeof(MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.BaseContentController<Content>))]
        public async Task<IActionResult> Delete(long id)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            ContentAlias contentAlias = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ContentAliasController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, lcid);
            if (contentAlias == null)
                return NotFound();

            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentAliasController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(contentAlias);

            return Ok(new GenericResponse { Success = true });
        }
    }
}