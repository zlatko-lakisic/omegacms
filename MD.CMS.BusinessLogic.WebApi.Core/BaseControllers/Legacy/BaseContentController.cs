using MD.CMS.BusinessLogic.WebApi.Core.ActionResults;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.CMS.BusinessLogic.WebApi.Core.CustomAttributes;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using MD.CMS.BusinessLogic.Core.DataAccess;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Caching.Attributes;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.ApprovalChain;
using System.Text;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.Options;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.WebApi.Core.Exceptions;
using System;

namespace MD.CMS.BusinessLogic.WebApi.Core.BaseControllers
{
    public abstract partial class BaseContentController<T> : BaseLoggedOnWebApiController
        where T : Content, new()
    {
        #region Action Methods
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<T> Ok(T result)
        {
            return OkGenericResult(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<T> OkGenericResult(T result)
        {
            return new ContentActionResult<T>(System.Net.HttpStatusCode.OK, this, result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<string> Ok(string result)
        {
            return OkStringResult(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<string> OkStringResult(string result)
        {
            return new ContentActionResult<string>(System.Net.HttpStatusCode.OK, this, result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<IEnumerable<T>> Ok(IEnumerable<T> result)
        {
            return OkEnumerableResult(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<IEnumerable<T>> OkEnumerableResult(IEnumerable<T> result)
        {
            return new ContentActionResult<IEnumerable<T>>(System.Net.HttpStatusCode.OK, this, result);
        }
        #endregion

        #region Data Access Methods
        /// <summary>
        /// Data Access method to get a content by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public IEnumerable<T> DataAccess_GetByIds(string[] ids, bool fillFields = true, bool isDataBound = false, long contentTypeDefinitionId = default(long))
        {
            return DataAccess_GetByIdsAsync(ids, fillFields, isDataBound, contentTypeDefinitionId).Result;
        }

        // new method which accepts all parameters and return one content
        //id=contentidid2=lcid
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public T DataAccess_GetByAll(T obj)
        {
            return DataAccess_GetByAllAsync(obj).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public IEnumerable<T> DataAccess_GetAll()
        {
            return DataAccess_GetAllAsync().Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public IEnumerable<T> DataAccess_Search(string searchTerm)
        {
            return DataAccess_SearchAsync(searchTerm).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public IEnumerable<T> DataAccess_GetAllVersion(string id)
        {
            return DataAccess_GetAllVersionAsync(id).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public T DataAccess_Post(T content)
        {
            return DataAccess_PostAsync(content).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public T DataAccess_ApproveReject(T content)
        {
            return DataAccess_ApproveRejectAsync(content).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public T DataAccess_DeleteByAll(string id)
        {
            return DataAccess_DeleteByAllAsync(id).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public T DataAccess_Delete(string id, bool fillFields = true)
        {
            return DataAccess_DeleteAsync(id, fillFields).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public IEnumerable<T> DataAccess_GetByFolderId(long folderId,
            bool loadAuthor = false,
            int lcid = default(int),
            bool loadFields = false,
            bool loadMetaDataFields = false)
        {
            return DataAccess_GetByFolderIdAsync(folderId, loadAuthor, lcid, loadFields, loadMetaDataFields).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public IEnumerable<T> DataAccess_GetBySearchTerm(string id)
        {
            return DataAccess_GetBySearchTermAsync(id).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public T DataAccess_Translate(string id, int id2)
        {
            return DataAccess_TranslateAsync(id, id2).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<T> DataAccess_PaginationGetByFolderId(long folderId, int lcid, int pageIndex, int pageSize, string searchTerm, long contentTypeDefinitionId = default(long), string sort ="Title ASC")
        {
            return DataAccess_PaginationGetByFolderIdAsync(folderId, lcid, pageIndex, pageSize, searchTerm, contentTypeDefinitionId, sort).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public int DataAccess_SelectByContentTypeDefinitionCount(long id)
        {
            return DataAccess_SelectByContentTypeDefinitionCountAsync(id).Result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [Obsolete]
        public int DataAccess_GetByFolderIdCount(long id, string searchTerm)
        {
            return DataAccess_GetByFolderIdCountAsync(id, searchTerm).Result;
        }
        #endregion

        #region ContentActionResult Methods
        
        protected ContentActionResult<T> GetByAll_ActionResult(T content)
        {
            content.NotFound = () =>
            {
                ErrorNotFound();
            };
            if (content == null)
                return NotFound();


            return Ok(content);
        }

        protected Task<ContentActionResult<T>> GetById(T content)
        {
            return Task.Run(() => {
                if (content == null)
                {
                    return NotFound();
                }
                content.NotFound = () =>
                {
                    ErrorNotFound();
                };
                return Ok(content);
            });
        }

        protected ContentActionResult<IEnumerable<T>> GetById(IEnumerable<T> contents)
        {
            return Ok(contents);
        }

        protected ContentActionResult<IEnumerable<T>> GetAll(IEnumerable<T> content)
        {
            if (content == null)
            {
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "The requested content was not found!");
            }
            return new ContentActionResult<IEnumerable<T>>(System.Net.HttpStatusCode.OK, this, content);
        }

        #endregion
    }
}