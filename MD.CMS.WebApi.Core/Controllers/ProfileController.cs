using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Profile")]
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("AssignProfileTypeToUser")]       
        [OmegaInvalidateCache("GetNotBelongingProfileTypesByUser", typeof(ProfileTypeController))]
        public async Task<IActionResult> AssignProfileTypeToUser([FromQuery] string userId, [FromQuery] long profileTypeId, [FromQuery] bool assigned = true)
        {
            bool success = false;
            User user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(userId);
            if (user == null)
                return BadRequest("There is no user with this id");

            ProfileType profileType = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(profileTypeId);
            if (profileType == null)
                return BadRequest("There is no profile type with this id");

            if (assigned)
            {
                success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(user, profileType);
            }
            else
            {
                if (await CanDelete(user))
                    success = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(user, profileType);
                else
                    throw new HttpException((int)HttpStatusCode.Forbidden, "User must have at least one profile type");
            }

            if (success)
                return Ok();
            else
                throw new HttpException((int)HttpStatusCode.BadRequest, "Changes not applied");
        }

        private async Task<bool> CanDelete(User user)
        {
            int numberOfProfileTypes = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByUserCountAsync(user.Id);
            if (numberOfProfileTypes > 1)
                return true;

            return false;
        }
    }

}
