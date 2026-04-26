using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetByIdAndUserId")]
        [ApiExplorerSettings(GroupName = "Message")]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Message, PermissionAccessTypeEnum.Read)]
        public async Task<IActionResult> GetByIdAndUserId(long id)
        {           
            Message message = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAndUserIdAsync(id, await GetLoggedOnUser());
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }


        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Message> allMessages = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync();
            return Ok(allMessages);
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetByMessageFolder")]
        public async Task<IActionResult> GetByMessageFolder(int id)
        {
            int messageFolderId = id;
            MessageFolder folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(messageFolderId, await GetLoggedOnUser());
            if (folder == null)
            {
                return NotFound();
            }
            List<Message> messagesByFolder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMessageFolderAsync(folder);
            return Ok(messagesByFolder);
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetByMessageFolderAndUserWithPagination")]
        public async Task<IActionResult> GetByMessageFolderAndUserWithPagination([FromQuery]int folderId, [FromQuery] int currentPageIndex, [FromQuery] int maxNumberOfRows, [FromQuery] string searchTerm)
        {
            MessageFolder folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(folderId, await GetLoggedOnUser());
            if (folder == null)
            {
                return NotFound();
            }
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMessageFolderAndUserAsync(folder, await GetLoggedOnUser(), currentPageIndex, maxNumberOfRows, searchTerm));
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetUnreadByUser")]
        public async Task<IActionResult> GetUnreadByUser()
        {
            List<Message> unreadMessages = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetUnreadByUserAsync(await GetLoggedOnUser());
            return Ok(unreadMessages);
        }

        [HttpGet]
        [Route("[action]/{searchTerm}/{currentPageIndex}/{maxNumberOfRows}")]
        [ActionName("Search")]
        public async Task<IActionResult> Search(string searchTerm, int currentPageIndex, int maxNumberOfRows)
        {
            List<Message> searchResults = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(await GetLoggedOnUser(), searchTerm, currentPageIndex, maxNumberOfRows);
            return Ok(searchResults);
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("SearchCount")]
        public async Task<IActionResult> SearchCount(string id)
        {
            string searchTerm = id;
            int count = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchCountAsync(await GetLoggedOnUser(), searchTerm);
            return Ok(count);
        }
        

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetByParent")]
        public async Task<IActionResult> GetByParent(long id)
        {
            Message parent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAndUserIdAsync(id, await GetLoggedOnUser());
            if (parent == null)
            {
                return NotFound();
            }
            List<Message> messagesByParent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByParentAsync(parent);
            return Ok(messagesByParent);
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetByUserId")]
        public async Task<IActionResult> GetByUser()
        {
            List<Message> messagesByUser = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByUserIdAsync(await GetLoggedOnUser());
            return Ok(messagesByUser);
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        [ActionName("GetByMainThread")]
        public async Task<IActionResult> GetByMainThread(long id)
        {
            long mainThread = id;
            List<Message> messagesByThread = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByMainThreadAsync(mainThread);
            return Ok(messagesByThread);
        }

        [HttpPost]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByIdAndAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAll", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAllSystemFolders", typeof(MessageFolder))]
        [ActionName("Save")]
        public async Task<IActionResult> Post([FromBody]Message messageToSend)
        {
            try
            {
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(messageToSend);
                return Ok(new SimpleResponse("Message sent"));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("MessageRead")]
        public async Task<IActionResult> MessageRead([FromBody]Message message)
        {
            Message updatedMessage = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SetIsReadAsync(message, await GetLoggedOnUser());
            return Ok(updatedMessage);
        }

        [HttpPost]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByIdAndAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAll", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAllSystemFolders", typeof(MessageFolder))]
        [ActionName("Replace")]
        public async Task<IActionResult> Replace([FromBody]Message messageToReplace)
        {
            Message replaced = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).ReplaceAsync(messageToReplace, await GetLoggedOnUser());
            return Ok(replaced);
        }

        [HttpDelete]
        [Route("[action]/{id?}")]
        [OmegaInvalidateCache("GetById", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByIdAndAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAll", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAllSystemFolders", typeof(MessageFolder))]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            bool deleted = false;
            Message messageToDelete = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAndUserIdAsync(id, await GetLoggedOnUser());
            if (messageToDelete == null)
            {
                return NotFound();
            }
            try
            {
                deleted = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(messageToDelete, await GetLoggedOnUser());

                return Ok("Deleted");
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e);
            }
        }
      
        [HttpPost]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByIdAndAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAll", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAllSystemFolders", typeof(MessageFolder))]
        [ActionName("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody]GenericJsonSingleObject<Message> obj)
        {
            foreach (Message message in obj.ValueArray)
            {
                Delete(message.Id);
            }
            return Ok(obj.ValueArray);
        }

        [HttpPost]
        [Route("[action]")]
        [OmegaInvalidateCache("GetById", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByIdAndAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAll", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetByAuthorId", typeof(MessageFolder))]
        [OmegaInvalidateCache("GetAllSystemFolders", typeof(MessageFolder))]
        [ActionName("ReplaceMultiple")]
        public async Task<IActionResult> ReplaceMultiple([FromBody] GenericJsonSingleObject<Message> obj)
        {
            foreach (Message message in obj.ValueArray)
            {
                Replace(message);
            }
            return Ok(obj.ValueArray);
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAllChats")]
        public async Task<IActionResult> GetAllChats()
        {
            MessageFolder folder = (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageFolderController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(await GetLoggedOnUser())).FirstOrDefault(folder => string.Compare(folder.Name, "Chat", StringComparison.InvariantCultureIgnoreCase).Equals(0));
            if(folder == null)
            {
                return Ok(new MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<Message>());
            }

            return await GetByMessageFolderAndUserWithPagination(folder.Id, 0, int.MaxValue, string.Empty);
        }
    }
}