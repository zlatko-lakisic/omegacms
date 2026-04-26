using MD.CMS.WebApi.Core.BusinessLogic.BaseControllers;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using Microsoft.AspNetCore.Mvc;

namespace MD.CMS.WebApi.Core.Controllers
{
    [TokenAuth]
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Content")]
    public class ContentController : BaseContentController<MD.CMS.BusinessLogic.Core.DataAccess.Entities.Content>
    {
    }
}
