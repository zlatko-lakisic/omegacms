using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;

using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{

    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Culture")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "General")]
    public class CultureController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Route("[action]")]
        [ActionName("SelectCulture")]
        [OmegaInvalidateCache("PaginationGetFolderByPath", typeof(FolderController))]
        [OmegaInvalidateCache("GetByFolderIdCount", typeof(ContentController))]
        [OmegaInvalidateCache("GetTaxonomyByPath", typeof(TaxonomyController))]
        [OmegaInvalidateCache("PaginationGetMenuByPath", typeof(MenuController))]        
        //this method is called only to MdInvalidateCache
        public async Task<IActionResult> SelectCulture()
        {
            return Ok();
        }


        //SelectByLCID
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{lcid}")]
        [ActionName("GetByLCID")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Culture GetByLCID")]
        public async Task<IActionResult> GetByLCID(int lcid)
        {
            Culture cultures = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByLCIDAsync(lcid);
            if (cultures == null)
                return NotFound();

            return Ok(cultures);
        }

        //SelectByCode
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{code}")]
        [ActionName("GetByCode")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Culture GetByCode")]
        public async Task<IActionResult> GetByCode(string code)
        {
            Culture cultures = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByCodeAsync(code);
            if (cultures == null)
                return NotFound();

            return Ok(cultures);
        }
        //SelectAll
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Culture GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync());
        }
        //SelectAll
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetApproved")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Culture GetApproved")]
        public async Task<IActionResult> GetApproved()
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetApprovedAsync());
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetAllForContentId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Culture GetAllForContentId")]
        public async Task<IActionResult> GetAllForContentId(long id)
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAvailableForContentIdAsync(id));
        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture, PermissionAccessTypeEnum.Delete)]
        [Route("[action]")]
        [OmegaInvalidateCache("GetByLCID")]
        [OmegaInvalidateCache("GetByCode")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetApproved")]
        [OmegaInvalidateCache("GetAllForContentId")]
        public async Task<IActionResult> Delete(Culture culture)
        {
            Culture cultures = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByLCIDAsync(culture.LCID);
            if (cultures == null)
                return NotFound();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(cultures))
                return Ok(new GenericResponse { Success = true });

            return BadRequest("Something went wrong");
        }
        
        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Culture, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetByLCID")]
        [OmegaInvalidateCache("GetByCode")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetApproved")]
        [OmegaInvalidateCache("GetAllForContentId")]
        public async Task<IActionResult> Post([FromBody]Culture cultures)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            cultures = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(cultures);

            if (cultures != null)
                return Ok(cultures);

            return Ok("Culture added");
        }

    }
}