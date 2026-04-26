using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionController : BaseController<ContentTypeDefinitionController>
	{
		[Obsolete("Deprecated", true)]
		public ContentTypeDefinition<T> GetById<T>(long id, bool fillFields = true, bool transformExpression = true)
            where T : Entities.GenericContent.GenericContentField
		{
			return GetByIdAsync<T>(id, fillFields, transformExpression).Result;
		}

		[Obsolete("Deprecated", true)]
		public List<ContentTypeDefinition<F>> GetByFolder<T, F>(Folder<T> obj)
			where T : Content, new()
            where F : Entities.GenericContent.GenericContentField
		{
			return GetByFolderAsync<T, F>(obj).Result;
		}

		[Obsolete("Deprecated", true)]
		public List<ContentTypeDefinition<T>> GetByParentId<T>(long id)
            where T : Entities.GenericContent.GenericContentField

		{
			return GetByParentIdAsync<T>(id).Result;
		}

		[Obsolete("Deprecated", true)]
		public List<ContentTypeDefinition<T>> GetAll<T>()
            where T : Entities.GenericContent.GenericContentField
		{
			return GetAllAsync<T>().Result;
		}

		[Obsolete("Deprecated", true)]
		public ContentTypeDefinition<T> Save<T>(ContentTypeDefinition<T> contentTypeDefinition)
            where T : Entities.GenericContent.GenericContentField
		{
			return SaveAsync(contentTypeDefinition).Result;
		}

		[Obsolete("Deprecated", true)]
		public bool Delete<T>(ContentTypeDefinition<T> obj)
            where T : Entities.GenericContent.GenericContentField
		{
			return DeleteAsync(obj).Result;
		}

		[Obsolete("Deprecated", true)]
		public Entities.Base.BasePaginationEntity<ContentTypeDefinition<T>> GetAllWithPagination<T>(int currentPageIndex, int maxNumberOfRows, string searchTerm, string searchColumn, string sort = "Name ASC")
            where T : Entities.GenericContent.GenericContentField
		{
			return GetAllWithPaginationAsync<T>(currentPageIndex, maxNumberOfRows, searchTerm, searchColumn, sort).Result;
		}

		[Obsolete("Deprecated", true)]
		public int SelectAllCount(string searchTerm, string searchColumn)
		{
			return SelectAllCountAsync(searchTerm, searchColumn).Result;
		}

		[Obsolete("Deprecated", true)]
		public List<ContentTypeDefinition<T>> Search<T>(string searchTerm, string searchColumn)
            where T : Entities.GenericContent.GenericContentField
		{
			return SearchAsync<T>(searchTerm, searchColumn).Result;
		}
	}
}
