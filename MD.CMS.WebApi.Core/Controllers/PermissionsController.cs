using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.Tools.Helpers.Core.TypeConversion;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Permissions")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "User")]
    public class PermissionsController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetUserPermissionssByObject")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Permissions GetUserPermissionssByObject")]
        public async Task<IActionResult> GetUserPermissionssByObject(int id, string id2 = "")
        {
            if (id.Equals(default(int)) || string.IsNullOrEmpty(id2))
            {
                return Ok(new List<UserPermissions>());
            }
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetUserPermissionssByObjectAsync((Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)id, id2));
        }

        [HttpGet]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetUserPermissionssByEntity")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Permissions GetUserPermissionssByEntity")]
        public async Task<IActionResult> GetUserPermissionssByEntity(int id, string id2 = "")
        {
            if (id.Equals(default(int)) || string.IsNullOrEmpty(id2))
            {
                return Ok(new List<UserPermissions>());
            }
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetUserPermissionsByEntityIdAsync((Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)id, id2));
        }

        [HttpGet]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetUserPermissionsByEntities")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Permissions GetProfileTypePermissionsByEntities")]
        public async Task<IActionResult> GetUserPermissionsByEntities(int id, string id2 = "")
        {
            List<UserPermissions> permissions = new List<UserPermissions>();
            if (id.Equals(default(int)) || string.IsNullOrEmpty(id2))
            {
                return Ok(permissions);
            }

            IEnumerable<string> entityIds = id2.Split('-').Where(id => !string.IsNullOrEmpty(id));

            foreach (string entityId in entityIds)
            {
                IEnumerable<UserPermissions> tempPermissions = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetUserPermissionsByEntityAsync((Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)id, (Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)entityId.ToInt32());
                foreach (UserPermissions tempPermission in tempPermissions)
                {
                    if (permissions.Select(p => p.UserId).Contains(tempPermission.UserId))
                    {
                        permissions.First(p => p.UserId == tempPermission.UserId).EntityPermissions.AddRange(tempPermission.EntityPermissions);
                    }
                    else
                    {
                        permissions.Add(tempPermission);
                    }
                }
            }

            return Ok(permissions);
        }

        [HttpGet]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetProfileTypePermissionsByObject")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Permissions GetProfileTypePermissionsByObject")]
        public async Task<IActionResult> GetProfileTypePermissionsByObject(int id, string id2 = "")
        {
            if (id.Equals(default(int)) || string.IsNullOrEmpty(id2))
            {
                return Ok(new List<ProfileTypePermissions>());
            }
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetProfileTypePermissionsByObjectAsync((Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)id, id2));
        }

        [HttpGet]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetProfileTypePermissionsByEntity")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Permissions GetProfileTypePermissionsByEntity")]
        public async Task<IActionResult> GetProfileTypePermissionsByEntity(int id, string id2 = "")
        {
            if (id.Equals(default(int)) || string.IsNullOrEmpty(id2))
            {
                return Ok(new List<ProfileTypePermissions>());
            }
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetProfileTypePermissionsByEntityIdAsync((Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)id, id2));
        }

        [HttpGet]
        [Route("[action]/{id?}/{id2?}")]
        [ActionName("GetProfileTypePermissionsByEntities")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Permissions GetProfileTypePermissionsByEntities")]
        public async Task<IActionResult> GetProfileTypePermissionsByEntities(int id, string id2 = "")
        {
            List<ProfileTypePermissions> permissionsPreFiltered = new List<ProfileTypePermissions>();
            if (id.Equals(default(int)) || string.IsNullOrEmpty(id2))
            {
                return Ok(permissionsPreFiltered);
            }

            IEnumerable<string> entityIds = id2.Split('-').Where(id => !string.IsNullOrEmpty(id));

            foreach(string entityId in entityIds)
            {
                IEnumerable<ProfileTypePermissions> tempPermissions = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetApiProfileTypePermissionsByEntityObjectAsync((Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)id, (Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)entityId.ToInt32());
                foreach(ProfileTypePermissions tempPermission in tempPermissions)
                {
                    if(permissionsPreFiltered.Select(p => p.ProfileId).Contains(tempPermission.ProfileId))
                    {
                        permissionsPreFiltered.First(p => p.ProfileId == tempPermission.ProfileId).EntityPermissions.AddRange(tempPermission.EntityPermissions);
                    }
                    else
                    {
                        permissionsPreFiltered.Add(tempPermission);
                    }
                }
            }

            List<ProfileTypePermissions> permissions = new List<ProfileTypePermissions>();
            IEnumerable<IGrouping<long, ProfileTypePermissions>> groupedResult = permissionsPreFiltered.GroupBy(p => p.ProfileId);
            foreach(IGrouping<long, ProfileTypePermissions> group in groupedResult)
            {
                ProfileTypePermissions permission = permissionsPreFiltered.FirstOrDefault(p => p.ProfileId == group.Key);
                if (permission.EntityPermissions.Count == entityIds.Count())
                {
                    permissions.AddRange(permissionsPreFiltered.Where(p => p.ProfileId == group.Key));
                }
            }

            return Ok(permissions);
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("SaveUserPermissionByObject")]
        public async Task<IActionResult> SaveUserPermissionByObject([FromBody]UserPermissions obj)
        {
            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(obj);
            return Ok(obj);
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("SaveProfileTypePermissionByObject")]
        public async Task<IActionResult> SaveProfileTypePermissionByObject([FromBody]ProfileTypePermissions obj)
        {
            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(obj);
            return Ok(obj);
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("SaveUserPermissionsByObject")]
        public async Task<IActionResult> SaveUserPermissionsByObject([FromBody]List<UserPermissions> objs)
        {
            if (objs != null)
            {
                foreach (UserPermissions obj in objs)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(obj);
                }
                return Ok(objs);
            }
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("SaveProfileTypePermissionsByObject")]
        public async Task<IActionResult> SaveProfileTypePermissionsByObject([FromBody]ProfileTypePermissions[] objs)
        {
            if (objs != null)
            {
                foreach (ProfileTypePermissions obj in objs)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(obj);
                }
                return Ok(objs);
            }
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAllPermissionsByUser")]
        public async Task<IActionResult> GetAllPermissionsByUser()
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetAllPermissionsByUserAsync(await GetLoggedOnUser()));
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAllPermissionsByProfileType")]
        public async Task<IActionResult> GetAllPermissionsByProfileType()
        {
            List<ProfileTypePermissions> permissions = new List<ProfileTypePermissions>();
            foreach (MD.CMS.BusinessLogic.Core.DataAccess.Entities.ProfileType profile in (await GetLoggedOnUser()).ProfileTypes)
            {
                permissions.AddRange(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.PermissionsController.GetNewInstance().Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetAllPermissionsByProfileTypeAsync(profile));
            }
            return Ok(permissions);
        }
    }
}