using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{

    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Search")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Search")]
    public class SearchController : BaseLoggedOnWebApiController
    {

        /// <summary>
        /// Searches for folders, taxonomies, contents, content types, users, profile types and media contents inside of one culture
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> FullText(string id)
        {          
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().Caller(await GetLoggedOnUser()).SearchCmsAsync(id));
        }

    }
}