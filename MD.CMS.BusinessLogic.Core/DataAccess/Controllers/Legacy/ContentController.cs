using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentController<T, SingletonType> : BaseController<SingletonType>
        where T : Content, new()
        where SingletonType : class, Tools.BaseDataAccess.Core.Interfaces.IBaseControllerSettings, new()
	{
		[Obsolete("Deprecated", true)]
		public virtual bool IsAuthorized(Content content, User user, Entities.Permissions.PermissionAccessTypeEnum permissionType)
		{
			return IsAuthorizedAsync(content, user, permissionType).Result;
		}

		[Obsolete("GetById is deprecated, please use GetByIdAsync instead.")]
		public virtual T GetById(string id, bool loadAuthor = false, int lcid = default(int), bool fillFields = true, bool fillMetaDataFields = true)
		{
			return GetByIdAsync(id, loadAuthor, lcid, fillFields, fillMetaDataFields).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual T GetByAll(T content, bool loadAuthor = false, bool fillFields = true, bool fillMetaDataFields = true)
		{
			return GetByAllAsync(content, loadAuthor, fillFields, fillMetaDataFields).Result;
		}

		[Obsolete("SelectAllCount is not being used anymore!")]
		public virtual long SelectAllCount(int lcid = default(int))
		{
			return SelectAllCountAsync(lcid).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual int SelectByContentTypeDefinitionCount(long id)
		{
			return SelectByContentTypeDefinitionCountAsync(id).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> GetAllVersion(string id, int lcid = default(int))
		{
			return GetAllVersionAsync(id, lcid).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> GetAll(int lcid = default(int))
		{
			return GetAllAsync(lcid).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> Search(string searchTerm, int lcid = default(int))
		{
			return SearchAsync(searchTerm, lcid).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> TaxonomyContentGetContentByTaxonomy(Taxonomy taxonomy, int lcid, bool fillFields = false, bool fillMetaDataFields = false)
		{
			return TaxonomyContentGetContentByTaxonomyAsync(taxonomy, lcid, fillFields, fillMetaDataFields).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> MenuContentGetContentByMenu(Menu menu)
		{
			return MenuContentGetContentByMenuAsync(menu).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> GetByFolderId(long id, bool loadAuthor = false, int lcid = default(int), bool loadFields = false, bool loadMetaDataFields = false)
		{
			return GetByFolderIdAsync(id, loadAuthor, lcid, loadFields, loadMetaDataFields).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual Entities.Base.BasePaginationEntity<T> GetByFolderIdWithPagination(long id, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", bool loadAuthor = false, int lcid = default(int), string sort = "Title ASC", bool loadFields = false)
		{
			return GetByFolderIdWithPaginationAsync(id, currentPageIndex, maxNumberOfRows, searchTerm, loadAuthor, lcid, sort, loadFields).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual Entities.Base.BasePaginationEntity<T> GetByFolderWithPagination(Folder<T> folder, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", bool loadAuthor = false, int lcid = default(int), string sort = "Title ASC", bool loadFields = false)
		{
			return GetByFolderWithPaginationAsync(folder, currentPageIndex, maxNumberOfRows, searchTerm, loadAuthor, lcid, sort, loadFields).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual int GetByFolderIdCount(long folderId, int lcid, string searchTerm)
		{
			return GetByFolderIdCountAsync(folderId, lcid, searchTerm).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> ContentsGetByFolderId(long id, bool loadAuthor = false, int lcid = default(int))
		{
			return ContentsGetByFolderIdAsync(id, loadAuthor, lcid).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> GetBySearchTerm(string searchTerm, bool loadAuthor = false, int lcid = default(int))
		{
			return GetBySearchTermAsync(searchTerm, loadAuthor, lcid).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual T Save(T content)
        {
			return SaveAsync(content).Result;
        }

		[Obsolete("Deprecated", true)]
		public virtual T ApproveReject(T content)
		{
			return ApproveRejectAsync(content).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual bool DeleteByAll(T content)
		{
			return DeleteByAllAsync(content).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual bool Delete(T obj)
		{
			return DeleteAsync(obj).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual T Translate(T source, Culture targetCulture)
		{
			return TranslateAsync(source, targetCulture).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> GetByTaxonomyId(long id, int lcid = default(int))
		{
			return GetByTaxonomyIdAsync(id, lcid).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> GetByTaxonomy(Taxonomy obj)
		{
			return GetByTaxonomyAsync(obj).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual List<T> GetByMenu(Menu obj)
		{
			return GetByMenuAsync(obj).Result;
		}

		[Obsolete("Deprecated", true)]
		public virtual Content GetByAlias(string alias, bool loadAuthor = false, int lcid = default(int), bool fillFields = true, bool fillMetaDataFields = false, bool useDefaultPlugin = false)
		{
			return GetByAliasAsync(alias, loadAuthor, lcid, fillFields, fillMetaDataFields, useDefaultPlugin).Result;
		}
	}
}
