using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageFolderController : BaseLoggedOnWebApiController
    {

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [ApiExplorerSettings(GroupName = "Message")]
        public async Task<IActionResult> GetById(int id)
        {
            MessageFolder messageFolder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, await GetLoggedOnUser());
            if (messageFolder == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(messageFolder);
            }
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetByIdAndAuthorId")]
        public async Task<IActionResult> GetByIdAndAuthorId(int id)
        {
            MessageFolder result = new MessageFolder();
            if (id <= Enum.GetNames(typeof(SystemMessageFolder)).Length)
            {
                result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, await GetLoggedOnUser());
            }
            else
            {
                result = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAndAuthorIdAsync(id, await GetLoggedOnUser());
            }

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<MessageFolder> folders = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(await GetLoggedOnUser());
            return Ok(folders);
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAllSystemFolders")]
        public async Task<IActionResult> GetAllSystemFolders()
        {
            List<MessageFolder> folders = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllSystemFoldersAsync(await GetLoggedOnUser());
            return Ok(folders);
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetByAuthorId")]
        public async Task<IActionResult> GetByAuthorId()
        {           
            List<MessageFolder> folders = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByAuthorIdAsync(await GetLoggedOnUser());
            return Ok(folders);
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("Save")]
        public async Task<IActionResult> Save([FromBody]MessageFolder messageFolder)
        {
            MessageFolder savedMessageFolder = new MessageFolder();
            try
            {
                savedMessageFolder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(messageFolder, await GetLoggedOnUser());
            }
            catch (UnauthorizedAccessException e)
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, e.Message);
            }
            return Ok(savedMessageFolder);
        }

        [HttpDelete]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = false;
            MessageFolder messageFolderToDelete = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, await GetLoggedOnUser());
            if (messageFolderToDelete == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    success = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(messageFolderToDelete);
                }
                catch (UnauthorizedAccessException e)
                {
                    throw new HttpException((int)HttpStatusCode.Forbidden, e.Message);
                }
            }
            if (success)
            {
                return Ok("Deleted");
            }
            else
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}