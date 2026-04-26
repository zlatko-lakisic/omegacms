using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MD.CMS.BusinessLogic.WebApi.Core.Exceptions
{
    public class MdCmsWebApiAuthorizationException : MdCmsBaseWebApiException
    {
        #region Methods
        public MdCmsWebApiAuthorizationException(string ipAddress, string requestAddress) : base(HttpStatusCode.Forbidden, "The API call was aborted because the user is not authorized to access this resource!")
        {
            this.Data.Add("IP Address", ipAddress);
            this.Data.Add("Request Address", requestAddress);
        }

        public MdCmsWebApiAuthorizationException(string ipAddress, MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session session, Entities entity, PermissionAccessTypeEnum permission) : base(HttpStatusCode.Forbidden, "The API call was aborted because the user is not authorized to access this resource!")
        {
            this.Data.Add("IP Address", ipAddress);
            this.Data.Add("User Session", session);
            this.Data.Add("Entity", entity);
            this.Data.Add("Permission Access Type", permission);
        }

        public MdCmsWebApiAuthorizationException(string ipAddress, MD.CMS.BusinessLogic.Core.DataAccess.Entities.Session session, Entities entity, PermissionAccessTypeEnum permission, Exception innerException) : base(HttpStatusCode.Forbidden, "The API call was aborted because the user is not authorized to access this resource!", innerException)
        {
            this.Data.Add("IP Address", ipAddress);
            this.Data.Add("User Session", session);
            this.Data.Add("Entity", entity);
            this.Data.Add("Permission Access Type", permission);
        }
        #endregion
    }
}
