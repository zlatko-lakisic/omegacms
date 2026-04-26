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
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ProfileTypeFieldValue")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Profile Type")]
    public class ProfileTypeFieldValueController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeFieldValue, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByUser")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileTypeFieldValue GetByUser")]
        public async Task<IActionResult> GetByUser(string id)
        {
            User user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (user == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByUserAsync(user));
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeFieldValue, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetByUser")]
        [OmegaInvalidateCache("GetById", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetAll", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetAllProfileTypesWithPermissions", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetByUser", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetNotBelonging", typeof(ProfileTypeController))]
        [OmegaInvalidateCache("GetById", typeof(ProfileTypeFieldController))]
        [OmegaInvalidateCache("GetByProfileType", typeof(ProfileTypeFieldController))]
        public async Task<IActionResult> Post([FromBody]ProfileTypeFieldValue profileTypeFieldValue)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            ProfileTypeFieldValue checkFieldValue = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByPrimaryKeysAsync(profileTypeFieldValue.ProfileTypeFieldId, profileTypeFieldValue.UserId, profileTypeFieldValue.ProfileTypeId);


            if (checkFieldValue == null)
            {
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(profileTypeFieldValue);                
            }
            else
            {
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateAsync(profileTypeFieldValue);
            }

            return Ok();        
          
        }      
    }
}
