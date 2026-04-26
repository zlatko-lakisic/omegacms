using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Aliasing")]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content")]
    public class AliasingController : BaseLoggedOnWebApiController
    {

        [HttpGet]
        [Route("[action]/{id?}/{id2?}/{id3?}")]
        [ActionName("GetByAlias")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Aliasing GetByAlias")]
        public async Task<IActionResult> GetByAlias(string id = "", bool id2 = false, bool id3 = false)
        {
            bool fillFields = id2;
            bool fillMetaData = id3;
            string alias = !id.StartsWith("/") ? string.Format("/{0}", id) : id;

            Content content = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByAliasAsync(
                    alias,
                    fillFields: fillFields,
                    fillMetaDataFields: fillMetaData
                    );
            if (content != null)
            {
                return Ok(new AliasModel<Content>()
                {
                    AliasType = MD.CMS.BusinessLogic.WebApi.Core.Enums.AliasType.Content,
                    Id = content.Id.ToString(),
                    Template = content.Template != null ? content.Template.TemplateUrl : string.Empty,
                    Content = content
                });
            }

            return NotFound();
        }
    }
}
