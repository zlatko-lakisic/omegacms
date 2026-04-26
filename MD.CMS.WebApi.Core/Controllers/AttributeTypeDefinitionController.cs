using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "AttributeTypeDefinition")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content Type")]
    public class AttributeTypeDefinitionController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.AttributeTypeDefinition, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "AttributeTypeDefinition GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            AttributeTypeDefinition attributeTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.AttributeTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);

            if (attributeTypeDefinition == null)
                return NotFound();

            return Ok(attributeTypeDefinition);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.AttributeTypeDefinition, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetByInputTypeId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "AttributeTypeDefinition GetByInputTypeId")]
        public async Task<IActionResult> GetByInputTypeId(long id)
        {
            AttributeTypeDefinition attributeTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.AttributeTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByInputTypeIdAsync(id);

            if (attributeTypeDefinition == null)
                return NotFound();

            return Ok(attributeTypeDefinition);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.AttributeTypeDefinition, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "AttributeTypeDefinition GetAll")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<AttributeTypeDefinition> attributeTypeDefinitions = new List<AttributeTypeDefinition>();

            attributeTypeDefinitions = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.AttributeTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync();
            if (attributeTypeDefinitions == null)
                return NotFound();

            return Ok(attributeTypeDefinitions);
        }
    }
}
