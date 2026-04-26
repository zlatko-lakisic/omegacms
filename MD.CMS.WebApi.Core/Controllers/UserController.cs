using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using MD.CMS.BusinessLogic.WebApi.Core.Session;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using MD.Tools.Helpers.Core.Exceptions;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.BaseDataAccess.PluginMethods.Core.DataAccess;
using System.Threading.Tasks;
using System.Linq;

namespace MD.CMS.WebApi.Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : BaseLoggedOnWebApiController
    {
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id}")]
        [ActionName("GetAuthData")]
        [ApiExplorerSettings(GroupName = "User")]
        public async Task<IActionResult> GetAuthData(string id)
        {
            AuthData authData = new AuthData();
            User user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, true);
            if (user == null)
                return NotFound();

            authData.AuthenticationProviderName = user.AuthenticationProvider;
            authData.Values.Add("username", user.Username);

            return Ok(authData);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetById")]
        [ApiExplorerSettings(GroupName = "User")]
        public async Task<IActionResult> GetById(string id)
        {
            User user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id, true);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync());
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("PaginationGetAll")]
        public async Task<IActionResult> PaginationGetAll([FromQuery] int currentPageIndex, [FromQuery] int maxNumberOfRows, [FromQuery] string searchTerm, [FromQuery] string sort = null)
        {
            if (string.IsNullOrEmpty(sort))
            {
                sort = "Username ASC";
            }
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllWithPaginationAsync(currentPageIndex, maxNumberOfRows, searchTerm, sort));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAllCount")]
        public async Task<IActionResult> GetAllCount([FromQuery] string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
            int count = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SelectAllCountAsync(searchTerm);
            return Ok(count);
        }

        [HttpPost]
        [Route("[action]")]
        //[Permissions(PerrmissionsEnum.UserControllerLogin)]
        public async Task<IActionResult> LoginAuthData([FromBody] AuthData data)
        {
            try
            {
                User user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetByAuthDataAsync(data);

                if (user != null)
                {
                    LoggedOnUser loggedOnUser = new LoggedOnUser(user);
                    await SessionTable.RemoveUserByUserIdAsync(loggedOnUser.Id);
                    Session session = await SessionTable.AddUserAsync(loggedOnUser.Username, loggedOnUser.Id, loggedOnUser.Token, data.AuthenticationProviderName);
                    loggedOnUser.SessionId = session.SessionId;
                    user.Token = session.Authdata;
                    user.DateRefreshToken = session.DateAdded.ToString("yyyy-MM-dd HH:mm:ss");
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(user).UpdateTokenAsync(user);
                    return Ok(loggedOnUser);
                }
            }
            catch (MDEntityUnauthorizedException error)
            {
                typeof(UserController).Log(error);
                throw;
            }
            catch (Exception error)
            {
                typeof(UserController).Log(error);
                throw;
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("[action]")]
        [Obsolete("Obsolete action, please switch to LoginAuthData")]
        //[Permissions(PerrmissionsEnum.UserControllerLogin)]
        public async Task<IActionResult> Login([FromBody]User userLoggingOn)
        {
            return await Login (userLoggingOn, 5);
        }

        [Obsolete]
        private async Task<IActionResult> Login(User userLoggingOn, int tries)
        {
            try
            {
                /*User user = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetByUsernameAndPassword(userLoggingOn.Username, userLoggingOn.Password);

                if (user != null)
                {
                    LoggedOnUser loggedOnUser = new LoggedOnUser(user);
                    SessionTable.RemoveUser(user.Id);
                    Session session = SessionTable.AddUser(userLoggingOn.Username, user.Id, userLoggingOn.Token);
                    loggedOnUser.SessionId = session.SessionId;
                    user.Token = session.Authdata;
                    user.DateRefreshToken = session.DateAdded.ToString("yyyy-MM-dd HH:mm:ss");
                    MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).UpdateToken(user);
                    return Ok(loggedOnUser);
                }*/
            }
            catch(MDEntityUnauthorizedException error)
            {
                typeof(UserController).Log(error);
                throw;
            }
            catch (Exception error)
            {
                if (tries > 0)
                {
                    return await Login(userLoggingOn, tries - 1);
                }
                else
                {
                    typeof(UserController).Log(error);
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> GetByToken(string id)
        {
            Session session = await SessionTable.GetLoggedOnSessionAsync(HttpUtility.UrlDecode(id));
            User obj = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(session.UserId, true, false);
            if (obj == null || obj.Id.Equals(default(long)))
            {
                return Ok(new GenericResponse<int>(GenericResponseStatusText.Ok) { Value = 404 });
            }
            obj.DateRefreshToken = session.DateAdded.ToLongDateString();
            return Ok(obj);

        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("Logout")]
        //[Permissions(PerrmissionsEnum.UserControllerLogout)]
        public async Task<IActionResult> Logout([FromBody]LoggedOnUser userLoggingOut = null)
        {
            if (userLoggingOut != null)
            {
                await SessionTable.RemoveUserByUserIdAsync(userLoggingOut.Id);
            }
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        [ActionName("ResetAccount")]
        public async Task<IActionResult> ResetAccount([FromBody]User userLoggingOn)
        {
            User resetUser = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).ResetPasswordAsync(userLoggingOn);

            bool success = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.EmailController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(resetUser).MailSend(resetUser.Username, "Password reset", string.Format("{0}/reset/{1}", Request.Headers.GetValue("Referer"), resetUser.Token));
            if (success)
            {
                return Ok(new GenericResponse<string>(GenericResponseStatusText.Ok) { Value = "Ok" });
            }
            else
            {
                return Ok(new GenericResponse<string>(GenericResponseStatusText.Fail) { Value = "Fail" });
            }

        }

        [HttpGet]
        [Route("[action]/{id?}/{id2?}/{id3?}")]
        [ActionName("PasswordReset")]
        public async Task<IActionResult> PasswordReset(string id, string id2, string id3)
        {
            string token = id;
            string username = id2;
            string password = id3;

            User user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetIdByTokenAsync(token);
            if(user != null && string.CompareOrdinal(username, user.Username).Equals(0))
            {
                //user.Password = password;
                user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateUserByTokenAsync(user);
                user.Token = string.Empty;
                user = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateTokenAsync(user);
                return Ok(user);
            }
            return Ok(new GenericResponse<string>(GenericResponseStatusText.Fail) { Value = "404" });
        }

        [HttpPost]
        [Route("[action]")]
        //[Permissions(PerrmissionsEnum.UserControllerPost)]
        [ActionName("UpdateUser")]
        public IActionResult UpdateUser([FromBody]User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            /*if (user.Password == null && user.Id == 0)
                return BadRequest();

            if (user.Password == null && user.Id != 0)
                user.Password = "";*/


            /*if (user.Password == null)
                return BadRequest();

            if (!string.IsNullOrEmpty(user.Password))
            {
                User addedUser = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateUserByToken(user);

                if (addedUser == null)
                {
                    return Ok(new GenericResponse<string>(GenericResponseStatusText.Fail) { Value = "Fail" });
                }
                else
                {
                    return Ok(addedUser);
                }
            }*/

            return Ok(user);
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            /*if (user.Password == null && user.Id == 0)
                return BadRequest();

            if (user.Password == null && user.Id != 0)
                user.Password = "";

            if (user.Password == null)
                return BadRequest();*/

            if (!await Startup.LicenseValid(HttpContext))
            {
                throw new Tools.Licensing.LicensingException(Tools.Licensing.LicensingException.LicensingExceptionErrorType.LicenseInvalid);
            }


            User newUser = new User();
            if (user.IsNew)
            {
                newUser = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(user);
                return Ok(newUser);
            }
            else
            {
                /*if (string.IsNullOrEmpty(user.Password))
                {
                    if (user.OldPassword == null)
                    {
                        user.OldPassword = String.Empty;
                    }
                    updatedUser = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Update(user);
                    return Ok(updatedUser);
                }*/

                /*User userWithEnteredPassword = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByUsernameAndPassword(user.Username, user.OldPassword);
                if (userWithEnteredPassword == null)
                {
                    throw new HttpException((int)HttpStatusCode.NotAcceptable, String.Format("Incorrect password"));
                }
                else
                {
                    updatedUser = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Update(user);
                }*/
                return Ok(user);
            }
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("UpdateAuthData")]
        public async Task<IActionResult> UpdateAuthData([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).UpdateAsync(user));
        }

        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.User, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            if (user == null)
                throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("User does not exist ", user.Username));

            if (user != null)
            {
                bool success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAllAsync(user.Id);
                if (!success)
                {
                    throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("some error", user.Username));
                }

                success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(user);
                if (!success)
                {
                    throw new HttpException((int)HttpStatusCode.InternalServerError, String.Format("{0} user is not deleted. Please try again.", user.Username));
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("Search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            List<User> searchResults = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm);
            return Ok(searchResults);
        }
    }
}
