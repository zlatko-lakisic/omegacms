using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "ProfileType")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Profile Type")]
    public class ProfileTypeController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileType GetById")]
        public async Task<IActionResult> GetById(long id)
        {

            ProfileType profileType = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, transformExpression: false);

            if (profileType == null)
                return NotFound();

            return Ok(profileType);
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetByIdAndTransformExpression")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileType GetByIdAndTransformExpression")]
        public async Task<IActionResult> GetByIdAndTransformExpression(long id, [FromQuery] bool transform = true)
        {

            ProfileType profileType = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, transformExpression: transform);

            if (profileType == null)
                return NotFound();

            return Ok(profileType);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileType GetAll")]
        public async Task<IActionResult> GetAll(string id = null)
        {
            string sort = id;
            if (string.IsNullOrEmpty(sort))
            {
                sort = "Name ASC";
            }
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(sort: sort));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAllCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileType GetAllCount")]
        public async Task<IActionResult> GetAllCount([FromQuery] string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllCountAsync(searchTerm));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAllWitPagination")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileType GetAllWitPagination")]
        public async Task<IActionResult> GetAllWitPagination([FromQuery] long pageIndex, [FromQuery] long pageSize, [FromQuery] string searchTerm, [FromQuery] string sort = "Name ASC")
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
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllWithPaginationAsync(pageIndex, pageSize, searchTerm, sort: sort));
        }

        //id - userid
        //[HttpGet]
        //[Permissions(PerrmissionsEnum.ProfileTypeControllerGetByUser)]
        //[ActionName("GetByUser")]
        //public IEnumerable<ProfileType> GetByUser(long id)
        //{
        //    User user = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetById(id);
        //    if (user == null)
        //        return null;

        //    return MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByUser(user);
        //}

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetNotBelonging")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileType GetNotBelongingProfileTypesByUser")]
        public async Task<IActionResult> GetNotBelongingProfileTypesByUser(string id)
        {
            User user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (user == null)
                return null;

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetNotBelongingProfileTypesByUserAsync(user));
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("SaveProfileTypeWithProfileTypeFieldValues")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("Search")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllProfileTypesWithPermissions")]
        [OmegaInvalidateCache("GetByUser")]
        [OmegaInvalidateCache("GetNotBelonging")]
        [OmegaInvalidateCache("GetByIdAndTransformExpression")]
        [OmegaInvalidateCache("GetById", typeof(ProfileTypeFieldController))]
        [OmegaInvalidateCache("GetByProfileType", typeof(ProfileTypeFieldController))]
        public async Task<IActionResult> SaveProfileTypeWithProfileTypeFieldValues([FromBody]ProfileType profileType)
        {
            if (profileType.Fields == null)
            {
                //In case of no fields in profile type just return success
                return Ok(profileType);
            }
            foreach (ProfileTypeFieldValue fieldValue in profileType.Fields)
            {
                ProfileTypeFieldValue checkFieldValue = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByPrimaryKeysAsync(fieldValue.ProfileTypeFieldId, fieldValue.UserId, fieldValue.ProfileTypeId);
                if (fieldValue.Value == null)
                {
                    fieldValue.Value = " ";
                }
                if (checkFieldValue == null || checkFieldValue.UserId == "0")
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(fieldValue);
                }
                else
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldValueController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateAsync(fieldValue);
                }
            }
            return Ok(profileType);
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("Search")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllProfileTypesWithPermissions")]
        [OmegaInvalidateCache("GetByUser")]
        [OmegaInvalidateCache("GetByIdAndTransformExpression")]
        [OmegaInvalidateCache("GetNotBelonging")]
        [OmegaInvalidateCache("GetById", typeof(ProfileTypeFieldController))]
        [OmegaInvalidateCache("GetByProfileType", typeof(ProfileTypeFieldController))]
        [OmegaInvalidateCache("GetByUser", typeof(ProfileTypeFieldValueController))]
        [OmegaInvalidateCache("GetById", typeof(UserController))]
        public async Task<IActionResult> Post([FromBody]ProfileType profileType)
        {
            ProfileType newProfileType = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(profileType);
            return Ok(newProfileType);
        }


        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileType, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("Search")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllProfileTypesWithPermissions")]
        [OmegaInvalidateCache("GetByUser")]
        [OmegaInvalidateCache("GetNotBelonging")]
        [OmegaInvalidateCache("GetByIdAndTransformExpression")]
        [OmegaInvalidateCache("GetById", typeof(ProfileTypeFieldController))]
        [OmegaInvalidateCache("GetByProfileType", typeof(ProfileTypeFieldController))]
        public async Task<IActionResult> Delete(long id)
        {
            ProfileType profileType = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            int userCount = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetUsersByProfileTypeCountAsync(profileType);
            if (userCount > 0)
                throw new HttpException((int)HttpStatusCode.Forbidden, "Cannot delete this profile type because this type belongs to some of the users");

            if (profileType == null)
                return NotFound();

            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(profileType))
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [Route("[action]")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "ProfileType Search")]
        [ActionName("Search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            List<ProfileType> searchResults = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm);
            return Ok(searchResults);
        }
    }
}
