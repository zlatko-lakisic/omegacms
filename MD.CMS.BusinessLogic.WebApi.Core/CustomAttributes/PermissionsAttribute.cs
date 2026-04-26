using MD.CMS.BusinessLogic.WebApi.Core.Session;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers;
using Microsoft.AspNetCore.Http;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.WebApi.Core.Exceptions;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.Tools.Helpers.Core.TypeConversion;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes
{
    public class PermissionsAttribute : BaseAuthorizeAttribute
    {
        #region Attributes
        private Entities _entity;
        private PermissionAccessTypeEnum _permission;
        #endregion

        #region Methods
        public PermissionsAttribute(Entities entity, PermissionAccessTypeEnum permission)
        {
            _entity = entity;
            _permission = permission;
        }

        public override async Task OnAuthorizeAsync(AuthorizationFilterContext actionContext)
        {
            bool isAuthenticated = false;
            bool hasPermissions = false;

            string authenticationCode = actionContext.HttpContext.Request.Headers.GetValue(Properties.Settings.Default.AuthenticateHeaderName);
            if (!string.IsNullOrEmpty(authenticationCode))
            {
                MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session currentLoggedOnSession = await SessionTable.GetLoggedOnSessionAsync(authenticationCode);
                if (currentLoggedOnSession != null)
                {
                    isAuthenticated = true;
                    string rootId = BusinessLogic.Core.Properties.Settings.Default.RootId();
                    hasPermissions = await PermissionsController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(rootId).UserHasEntityPermissionAsync(_entity, currentLoggedOnSession.UserId, _permission);
                    if (!hasPermissions)
                    {
                        hasPermissions = (await Task.WhenAll((await UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(rootId).GetByIdAsync(currentLoggedOnSession.UserId)).ProfileTypes.Select(async (p) => await PermissionsController.GetNewInstance().ProfileTypeHasEntityPermissionAsync(_entity, p.Id, _permission)))).Any(res => res);
                    }

                    if (!hasPermissions)
                    {
                        throw new MdCmsWebApiAuthorizationException(actionContext.HttpContext.Connection.RemoteIpAddress.ToString(), currentLoggedOnSession, _entity, _permission);
                    }
                }
            }

            if(!isAuthenticated)
            {
                throw new MdCmsWebApiAuthenticationException(actionContext.HttpContext.Connection.RemoteIpAddress.ToString(), authenticationCode);
            }
        }
        #endregion
    }
}