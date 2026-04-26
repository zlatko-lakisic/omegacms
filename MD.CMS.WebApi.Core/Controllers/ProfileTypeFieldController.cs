using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ProfileTypeField")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Profile Type")]
    public class ProfileTypeFieldController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileTypeField GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            ProfileTypeField profileTypeField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (profileTypeField == null)
                return NotFound();

            return Ok(profileTypeField);
        }

        //id = profileTypeId
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByProfileType")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileTypeField GetByProfileType")]
        public async Task<IActionResult> GetByProfileType (long id)
        {
            ProfileType profileType = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (profileType == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByProfileTypeAsync(profileType));
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByProfileType")]
        [OmegaInvalidateCache("GetById", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetAll", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetAllProfileTypesWithPermissions", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetByUser", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetNotBelonging", typeof(ProfileTypeController))]
        public async Task<IActionResult> Post([FromBody]ProfileTypeField profileTypeField)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           profileTypeField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(profileTypeField);

           if (profileTypeField == null)
               return BadRequest();

           return Ok(profileTypeField);
        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("GetByProfileType")]
        [OmegaInvalidateCache("GetById", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetAll", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetAllProfileTypesWithPermissions", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetByUser", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetNotBelonging", typeof(ProfileTypeController))]
        public async Task<IActionResult> Delete(long id)
        {
            ProfileTypeField profileTypeField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (profileTypeField == null)
                return NotFound();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(profileTypeField))
                return Ok();

            return BadRequest();
        }
    }    
}
