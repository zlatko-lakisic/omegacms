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
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options;
using MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.Options;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.WebApi.Core.Exceptions;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base;

namespace MD.CMS.BusinessLogic.WebApi.Core.BaseControllers
{
    [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Controller, OutputCacheName = "Content")]
    public abstract partial class BaseContentController<T> : BaseLoggedOnWebApiController
        where T : Content, new()
    {
        private bool _getOnlyPublished;

        #region Action Methods
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<T> OkAsync(T result)
        {
            return OkGenericResultAsync(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<T> OkGenericResultAsync(T result)
        {
            return new ContentActionResult<T>(System.Net.HttpStatusCode.OK, this, result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<string> OkAsync(string result)
        {
            return OkStringResultAsync(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<string> OkStringResultAsync(string result)
        {
            return new ContentActionResult<string>(System.Net.HttpStatusCode.OK, this, result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<IEnumerable<T>> OkAsync(IEnumerable<T> result)
        {
            return OkEnumerableResultAsync(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public ContentActionResult<IEnumerable<T>> OkEnumerableResultAsync(IEnumerable<T> result)
        {
            return new ContentActionResult<IEnumerable<T>>(System.Net.HttpStatusCode.OK, this, result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public new ContentActionResult<T> NotFound()
        {
            base.NotFound();
            return null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public void ErrorNotFound()
        {
            base.NotFound();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public void Error(string error)
        {
            throw new HttpException((int)System.Net.HttpStatusCode.InternalServerError, error);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public void ErrorForbbiden(string error)
        {
            throw new MdCmsWebApiAuthorizationException(HttpContext.Connection.RemoteIpAddress.ToString(), string.Format("{0}://{1}{2}{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, HttpContext.Request.Path, HttpContext.Request.QueryString));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public void ErrorNotSaved()
        {
            Error("The content was not saved!");
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
        public async Task<T> DataAccess_GetByIdAsync(string id, bool fillFields = true, bool isDataBound = false, long contentTypeDefinitionId = default(long))
        {
            string loadAuthorString = HttpContext.Request.Headers.GetValue("loadAuthor");
            bool loadAuthor = false;
            int lcid = DataAccessSettings.SelectedLcid;
            if (!string.IsNullOrEmpty(loadAuthorString))
            {
                loadAuthor = loadAuthorString.ToBoolean(true);
            }

            T content = (await DataAccess_ExecuteAsync(new BusinessLogic.Core.DataAccess.Controllers.V2.Options.DataBoundContentRequestOptions()
            {
                ContentIds = new List<string> { id },
                Lcid = lcid,
                FillFields = fillFields,
                LoadAuthor = loadAuthor,
                DataBound = isDataBound
            })).Items.FirstOrDefault();

            if (content != null)
            {
                ////provjeriti da li ima uradjen dio za user permisije na contentu
                if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(content, await GetLoggedOnUser(), PermissionAccessTypeEnum.Read))
                {
                    return content;
                }
                else
                {
                    ErrorForbbiden("403");
                }
            }

            return content;
        }
        /// <summary>
        /// Data Access method to get a content by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<IEnumerable<T>> DataAccess_GetByIdsAsync(string[] ids, bool fillFields = true, bool isDataBound = false, long contentTypeDefinitionId = default(long))
        {
            string loadAuthorString = HttpContext.Request.Headers.GetValue("loadAuthor");
            bool loadAuthor = false;
            int lcid = DataAccessSettings.SelectedLcid;
            if (!string.IsNullOrEmpty(loadAuthorString))
            {
                loadAuthor = loadAuthorString.ToBoolean(true);
            }

            return (await DataAccess_ExecuteAsync(new BusinessLogic.Core.DataAccess.Controllers.V2.Options.DataBoundContentRequestOptions()
            {
                ContentIds = ids.ToList(),
                FillFields = fillFields,
                Lcid = lcid,
                LoadAuthor = loadAuthor
            })).Items;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<IEnumerable<T>> DataAccess_GetByRequestAsync(IDataBoundContentRequestOptions request)
        {
            return (await DataAccess_ExecuteAsync(request)).Items;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<BasePaginationEntity<T>> DataAccess_ExecuteAsync(IDataBoundContentRequestOptions request)
        {
            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.DataBoundContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Execute(request);
        }

        // new method which accepts all parameters and return one content
        //id=contentidid2=lcid
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<T> DataAccess_GetByAllAsync(T obj)
        {
            T content = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByAllAsync(obj);
            if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(content, await GetLoggedOnUser(), PermissionAccessTypeEnum.Read))
            {
                return content;
            }
            else
            {
                ErrorForbbiden("403");
            }

            return content;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<IEnumerable<T>> DataAccess_GetAllAsync()
        {
            int lcid = DataAccessSettings.SelectedLcid;
            IEnumerable<T> content = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllAsync(lcid);
            return content;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<IEnumerable<T>> DataAccess_SearchAsync(string searchTerm)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            IEnumerable<T> content = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SearchAsync(searchTerm, lcid);
            return content;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<IEnumerable<T>> DataAccess_GetAllVersionAsync(string id)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            IEnumerable<T> content = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetAllVersionAsync(id, lcid);
            return content;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<T> DataAccess_PostAsync(T content)
        {
            content.AuthorId = (await GetLoggedOnUser()).Id;
            bool isAuthorized = false;
            bool waitForApproval = false;
            ApprovalChain approvalChain = null;
            Folder<Content> folder = new Folder<Content>();
            if (content.FolderId != default(long))
            {
                folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(content.FolderId);
                if (folder != null)
                {
                    isAuthorized = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(await GetLoggedOnUser(), folder, RWDPermissionType.Write);
                    approvalChain = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.ApprovalChainController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdAsync(content.FolderId);
                    if (approvalChain != null)
                    {
                        waitForApproval = approvalChain.IsActive;
                    }
                }
            }
            if (!content.IsNew)
            {
                isAuthorized = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(content, await GetLoggedOnUser(), PermissionAccessTypeEnum.Write);
            }


            if (content != null)
            {
                if (isAuthorized)
                {
                    if (waitForApproval)
                    {
                        content.ApprovalPending = true;
                        content.IsPublished = false;
                    }
                    else
                    {
                        content.ApprovalPending = false;
                    }
                    if (content.IsDataBound)
                    {
                        content = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.DataBoundContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(content);
                    }
                    else
                    {
                        content = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(content);
                    }
                    if (waitForApproval)
                    {
                        List<Step> steps = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ApprovalChain.StepController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByApprovalChainIdAsync(approvalChain.Id);

                        if (steps != null && steps.Count > 1)
                        {
                            Step nextStep = steps[1];
                            Message message = new Message();
                            StringBuilder contentParams = new StringBuilder();
                            contentParams.Append("{ \"Id\": ");
                            contentParams.Append(content.Id);
                            contentParams.Append(", \"LCID\" : ");
                            contentParams.Append(content.LCID);
                            contentParams.Append(", \"DateCreated\": \"");
                            contentParams.Append(content.DateCreated);
                            contentParams.Append("\", \"FolderId\" : ");
                            contentParams.Append(content.FolderId);
                            contentParams.Append(", \"stepId\": ");
                            contentParams.Append(nextStep.Id);
                            if (!content.IsNew)
                            {
                                contentParams.Append(", \"Edit\": 1"); // notify user about new version of existing content
                            }
                            contentParams.Append("}");
                            message.Subject = "Approval pending for " + content.Title;
                            if (message.Subject.Length > 45)
                            {
                                message.Subject = message.Subject.Substring(0, 40);
                                message.Subject += "...";
                            }
                            message.MessageContent = contentParams.ToString();
                            foreach (string userId in nextStep.UserIds)
                            {
                                message.ToUserId = userId;
                                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MessageController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SaveAsync(message, SystemMessageFolder.Approvals);
                            }
                        }
                    }
                    return content;
                }
                else
                {
                    ErrorForbbiden("403");
                }
            }
            else
            {
                ErrorNotSaved();
            }

            return content;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<T> DataAccess_ApproveRejectAsync(T content)
        {
            content.AuthorId = (await GetLoggedOnUser()).Id;
            User Author = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(content.AuthorId);
            Folder<Content> folder = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(content.FolderId);
            bool isAuthorized = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(content, await GetLoggedOnUser(), PermissionAccessTypeEnum.Write);

            if (isAuthorized)
            {
                if (content.ApprovalPending)
                {
                    content.IsPublished = true;
                }
                else
                {
                    content.IsPublished = false;
                }
                return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).ApproveRejectAsync(content);
            }
            ErrorForbbiden("403");
            return null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<T> DataAccess_DeleteByAllAsync(string id)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            // T content = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByAll(obj);
            T content = (await DataAccess_ExecuteAsync(new BusinessLogic.Core.DataAccess.Controllers.V2.Options.DataBoundContentRequestOptions()
            {
                ContentIds = new List<string> { id },
                Lcid = lcid
            })).Items.FirstOrDefault();

            if (content == null)
            {
                ErrorNotFound();
            }
            else
            {
                if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(content, await GetLoggedOnUser(), PermissionAccessTypeEnum.Delete))
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteByAllAsync(content);
                }
                else
                {
                    ErrorForbbiden("403");
                }
            }
            return content;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<T> DataAccess_DeleteAsync(string id, bool fillFields = true)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            T content = (await DataAccess_ExecuteAsync(new BusinessLogic.Core.DataAccess.Controllers.V2.Options.DataBoundContentRequestOptions()
            {
                ContentIds = new List<string> { id },
                Lcid = lcid
            })).Items.FirstOrDefault();
            
            if (content == null)
            {
                ErrorNotFound();
            }
            else
            {
                if (await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).IsAuthorizedAsync(content, await GetLoggedOnUser(), PermissionAccessTypeEnum.Delete))
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).DeleteAsync(content);
                }
                else
                {
                    ErrorForbbiden("403");
                }
            }
            return content;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<IEnumerable<T>> DataAccess_GetByFolderIdAsync(long folderId,
            bool loadAuthor = false,
            int lcid = default(int),
            bool loadFields = false,
            bool loadMetaDataFields = false)
        {
            if (lcid == default(int))
            {
                lcid = DataAccessSettings.SelectedLcid;
            }
            IEnumerable<T> contents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdAsync(folderId, loadAuthor, lcid, loadFields, loadMetaDataFields);
            return contents;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<IEnumerable<T>> DataAccess_GetBySearchTermAsync(string id)
        {
            string loadAuthorString = HttpContext.Request.Headers.GetValue("loadAuthor");
            bool loadAuthor = false;
            if (!string.IsNullOrEmpty(loadAuthorString))
            {
                loadAuthor = loadAuthorString.ToBoolean(false);
            }
            int lcid = DataAccessSettings.SelectedLcid;
            IEnumerable<T> contents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetBySearchTermAsync(id, loadAuthor, lcid);
            return contents;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<T> DataAccess_TranslateAsync(string id, int id2)
        {
            T content = (await DataAccess_ExecuteAsync(new BusinessLogic.Core.DataAccess.Controllers.V2.Options.DataBoundContentRequestOptions()
            {
                ContentIds = new List<string> { id }
            })).Items.FirstOrDefault();
            
            if (content == null)
            {
                NotFound();
            }

            T newContent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).TranslateAsync(content, await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByLCIDAsync(id2, true));

            return content;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<T>> DataAccess_PaginationGetByFolderIdAsync(long folderId, int lcid, int pageIndex, int pageSize, string searchTerm, long contentTypeDefinitionId = default(long), string sort = "Title ASC")
        {
            BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<T> paginationEntity = null;
            ContentTypeDefinition<ContentTypeDefinitionFieldValue> type = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync<ContentTypeDefinitionFieldValue>(contentTypeDefinitionId);
            if (type != null && type.Fields.Any(field => field.DataBound))
            {
                paginationEntity = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.DataBoundContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetOnlyPublished(_getOnlyPublished).GetByFolderIdWithPaginationAsync(folderId, pageIndex, pageSize, searchTerm, lcid: lcid, sort: sort);
            }
            else
            {
                paginationEntity = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetOnlyPublished(_getOnlyPublished).GetByFolderIdWithPaginationAsync(folderId, pageIndex, pageSize, searchTerm, lcid: lcid, sort: sort);
            }
            return paginationEntity;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<int> DataAccess_SelectByContentTypeDefinitionCountAsync(long id)
        {
            int contentCount = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SelectByContentTypeDefinitionCountAsync(id);
            return contentCount;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<int> DataAccess_GetByFolderIdCountAsync(long id, string searchTerm)
        {
            int lcid = DataAccessSettings.SelectedLcid;
            int contentCount = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByFolderIdCountAsync(id, lcid, searchTerm);
            return contentCount;
        }
        #endregion

        #region ContentActionResult Methods

        protected ContentActionResult<T> GetByAll_ActionResultAsync(T content)
        {
            content.NotFound = () =>
            {
                ErrorNotFound();
            };

            if (content == null)
            {
                return NotFound();
            }
            return OkAsync(content);
        }

        protected ContentActionResult<T> GetByIdAsync(T content)
        {
            if (content == null)
            {
                return NotFound();
            }
            content.NotFound = () =>
            {
                ErrorNotFound();
            };
            return OkAsync(content);
        }

        protected ContentActionResult<IEnumerable<T>> GetByIdAsync(IEnumerable<T> contents)
        {
            return OkAsync(contents);
        }

        protected ContentActionResult<IEnumerable<T>> GetAllAsync(IEnumerable<T> content)
        {

            if (content == null)
            {
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "The requested content was not found!");
            }
            return new ContentActionResult<IEnumerable<T>>(System.Net.HttpStatusCode.OK, this, content);
        }

        #endregion

        #region Web Methods
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content Get")]
        public virtual async Task<BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<T>> Get([FromQuery] MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.ContentRequestOptions options)
        {
            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.DataBoundContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).Execute(options);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Lcid]
        [Route("[action]/{id?}/{id2?}/{id3?}/{id4?}")]
        [ActionName("GetById")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content GetById")]
        public virtual async Task<ContentActionResult<T>> GetById(string id, bool id2 = true, bool id3 = false, long id4 = default(long))
        {
            return OkGenericResultAsync(await DataAccess_GetByIdAsync(id, id2, id3, id4));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Lcid]
        [Route("[action]/{idList?}/{id2?}/{id3?}/{id4?}")]
        [ActionName("GetByIds")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content GetByIds")]
        public virtual async Task<ContentActionResult<IEnumerable<T>>> GetByIds(string idList, bool id2 = true, bool id3 = false, long id4 = default(long))
        {
            return GetByIdAsync(await DataAccess_GetByIdsAsync(idList.Split(';'), id2, id3, id4));
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Lcid]
        [Route("[action]")]
        [ActionName("GetByRequest")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content GetByRequest")]
        public virtual Task<BasePaginationEntity<T>> GetByRequest([FromBody] BusinessLogic.Core.DataAccess.Controllers.V2.Options.DataBoundContentRequestOptions request)
        {
            return DataAccess_ExecuteAsync(request);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("SelectAllCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content SelectAllCount")]
        public virtual async Task<long> SelectAllCount(int id = default(int))
        {
            return await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).SelectAllCountAsync(id);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetAll")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content GetAll")]
        public virtual async Task<ContentActionResult<IEnumerable<T>>> GetAllAsync()
        {
            IEnumerable<T> content = await DataAccess_GetAllAsync();
            return OkAsync(content);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetAllVersion")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content GetAllVersion")]
        public virtual async Task<ContentActionResult<IEnumerable<T>>> GetAllVersion(string id)
        {
            IEnumerable<T> content = await DataAccess_GetAllVersionAsync(id);
            return Ok(content);
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetByAll")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content GetByAll")]
        public virtual async Task<ContentActionResult<T>> GetByAll([FromBody]T content)
        {
            return GetByAll_ActionResultAsync(await DataAccess_GetByAllAsync(content));
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}/{id3?}")]
        [ActionName("TaxonomyContentGetContentByTaxonomy")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content TaxonomyContentGetContentByTaxonomy")]
        public virtual async Task<ContentActionResult<IEnumerable<T>>> TaxonomyContentGetContentByTaxonomy(long id, bool id2 = false, bool id3 = false)
        {
            bool fillFields = id2;
            bool fillMetaDataFields = id3;
            Taxonomy taxonomy = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            IEnumerable<T> content = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).TaxonomyContentGetContentByTaxonomyAsync(
                taxonomy,
                DataAccessSettings.SelectedLcid,
                fillFields: fillFields,
                fillMetaDataFields: fillMetaDataFields
                );

            if (content == null)
                NotFound();

            return Ok(content);
        }
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("MenuContentGetContentByMenu")]
        [Lcid]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content MenuContentGetContentByMenu")]
        public virtual async Task<ContentActionResult<IEnumerable<T>>> MenuContentGetContentByMenu(long id)
        {
            Menu menu = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByIdAsync(id);
            IEnumerable<T> content = await MD .CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).MenuContentGetContentByMenuAsync(menu);

            if (content == null)
                NotFound();

            return Ok(content);
        }
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}/{id2?}/{id3?}/{id4?}/{id5?}")]
        [ActionName("GetByFolderId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content GetByFolderId")]
        public virtual async Task<ContentActionResult<IEnumerable<T>>> GetByFolderId(long id, bool id2 = false, int id3 = default(int), bool id4 = false, bool id5 = false)
        {
            IEnumerable<T> content = await DataAccess_GetByFolderIdAsync(id, id2, id3, id4, id5);
            return Ok(content);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("PaginationGetByFolderId")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content PaginationGetByFolderId")]
        public virtual async Task<IActionResult> PaginationGetByFolderId([FromQuery] long folderId, [FromQuery] int lcid, [FromQuery] int currentPageIndex, [FromQuery] int maxNumberOfRows, [FromQuery] string searchTerm, [FromQuery] long contentTypeDefinitionId = default(long), [FromQuery] string sort = "Title ASC")
        {
            BusinessLogic.Core.DataAccess.Entities.Base.BasePaginationEntity<T> paginationEntity = await DataAccess_PaginationGetByFolderIdAsync(folderId, lcid, currentPageIndex, maxNumberOfRows, searchTerm, contentTypeDefinitionId, sort);
            return Ok(paginationEntity);
        }


        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("GetBySearchTerm")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content GetBySearchTerm")]
        public virtual async Task<ContentActionResult<IEnumerable<T>>> GetBySearchTerm(string id)
        {
            string searchTerm = id;
            IEnumerable<T> content = await DataAccess_GetBySearchTermAsync(id);
            return Ok(content);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("Search")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content Search")]
        public virtual async Task<ContentActionResult<IEnumerable<T>>> Search([FromQuery] string searchTerm)
        {
            return Ok(await DataAccess_SearchAsync(searchTerm));
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("Translate")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content Translate")]
        public virtual async Task<ContentActionResult<T>> Translate([FromBody] T content, [FromQuery] int id)
        {
            if (content == null)
            {
                ErrorNotFound();
            }

            T newContent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<T>.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).TranslateAsync(content, await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.CultureController.GetNewInstance().DefaultPlugin(IsAdministration).Caller(await GetLoggedOnUser()).GetByLCIDAsync(id, true));

            return Ok(newContent);
        }

        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("SelectByContentTypeDefinitionCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content SelectByContentTypeDefinitionCount")]
        public virtual async Task<int> SelectByContentTypeDefinitionCount(long id)
        {
            int contentCount = await DataAccess_SelectByContentTypeDefinitionCountAsync(id);
            return contentCount;
        }

        //id = folderId, id2 = lcid
        [HttpGet]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]")]
        [ActionName("GetByFolderIdCount")]
        [OmegaOutputCache(OutputCacheType = OmegaOutputCacheAttribute.CacheType.Method, OutputCacheName = "Content GetByFolderIdCount")]
        public virtual async Task<int> GetByFolderIdCount([FromQuery] long folderId, [FromQuery] string searchTerm)
        {
            int contentCount = await DataAccess_GetByFolderIdCountAsync(folderId, searchTerm);
            return contentCount;
        }

        [HttpPost]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Write)]
        [Route("[action]")]
        [ActionName("Save")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("SelectAllCount")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllVersion")]
        [OmegaInvalidateCache("GetByAll")]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy")]
        [OmegaInvalidateCache("MenuContentGetContentByMenu")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("PaginationGetByFolderId")]
        [OmegaInvalidateCache("GetBySearchTerm")]
        [OmegaInvalidateCache("Translate")]
        [OmegaInvalidateCache("SelectByContentTypeDefinitionCount")]
        [OmegaInvalidateCache("GetByFolderIdCount")]
        [OmegaInvalidateCacheBy(OutputCacheName = "Content Save")]
        public virtual async Task<ContentActionResult<T>> Post([FromBody]T content)
        {
            content.AuthorId = (await GetLoggedOnUser()).Id;
            T newContent = await DataAccess_PostAsync(content);
            return Ok(newContent);
        }

        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Delete)]
        [Route("[action]/{id?}")]
        [ActionName("Delete")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("SelectAllCount")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllVersion")]
        [OmegaInvalidateCache("GetByAll")]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy")]
        [OmegaInvalidateCache("MenuContentGetContentByMenu")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("PaginationGetByFolderId")]
        [OmegaInvalidateCache("GetBySearchTerm")]
        [OmegaInvalidateCache("Translate")]
        [OmegaInvalidateCache("SelectByContentTypeDefinitionCount")]
        [OmegaInvalidateCache("GetByFolderIdCount")]
        [OmegaInvalidateCacheBy(OutputCacheName = "Content Delete")]
        public virtual async Task<ContentActionResult<T>> Delete(string id)
        {
            return GetByIdAsync(await DataAccess_DeleteAsync(id));
        }


        //id + content.Id, // id2 = dateCreated
        [HttpDelete]
        [Permissions(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, PermissionAccessTypeEnum.Read)]
        [Route("[action]/{id?}")]
        [ActionName("DeleteByAll")]
        [OmegaInvalidateCache("GetById")]
        [OmegaInvalidateCache("SelectAllCount")]
        [OmegaInvalidateCache("GetAll")]
        [OmegaInvalidateCache("GetAllVersion")]
        [OmegaInvalidateCache("GetByAll")]
        [OmegaInvalidateCache("TaxonomyContentGetContentByTaxonomy")]
        [OmegaInvalidateCache("MenuContentGetContentByMenu")]
        [OmegaInvalidateCache("GetByFolderId")]
        [OmegaInvalidateCache("PaginationGetByFolderId")]
        [OmegaInvalidateCache("GetBySearchTerm")]
        [OmegaInvalidateCache("Translate")]
        [OmegaInvalidateCache("SelectByContentTypeDefinitionCount")]
        [OmegaInvalidateCache("GetByFolderIdCount")]
        [OmegaInvalidateCacheBy(OutputCacheName = "Content DeleteByAll")]
        public virtual async Task<ContentActionResult<T>> DeleteByAll(string id)
        {
            return GetByIdAsync(await DataAccess_DeleteByAllAsync(id));
        }

        #endregion
    }
}