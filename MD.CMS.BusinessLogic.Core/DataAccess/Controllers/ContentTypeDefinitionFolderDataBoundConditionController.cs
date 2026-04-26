using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core.Data;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFolderDataBoundConditionController : BaseController<ContentTypeDefinitionFolderDataBoundConditionController>
    {
        public async Task<ContentTypeDefinitionFolderDataBoundCondition> Create(DataRow row)
        {
			ContentTypeDefinitionFolderDataBoundCondition obj = new ContentTypeDefinitionFolderDataBoundCondition();
            if (obj != null)
			{
				obj.ContentTypeDefinitionFieldId = row.GetValue<long>(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Data.Columns.ContentTypeDefinitionFieldId);
				obj.ContentTypeDefinitionId = row.GetValue<long>(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Data.Columns.ContentTypeDefinitionId);
				obj.FolderId = row.GetValue<long>(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Data.Columns.FolderId);
				obj.Value = row.GetValue<string>(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Data.Columns.Value);
				obj.Comparer = (ComparerTypeEnum)row.GetValue<int>(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Data.Columns.Comparer);
				ContentTypeDefinitionField field = await ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(obj.ContentTypeDefinitionFieldId);
				if(field != null)
				{
					obj.LeftField = field.Name;
				}
			}
            return obj;
		}

		public async Task<ContentTypeDefinitionFolderDataBoundCondition> SaveAsync(ContentTypeDefinitionFolderDataBoundCondition contentTypeDefinition)
		{
			await AuthenticateAndAuthorizeAsync();
			using (Method method = new Method())
			{
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundCondition;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Methods.Insert.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.FolderId.GetIntValue()) { Value = contentTypeDefinition.FolderId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeDefinition.ContentTypeDefinitionId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.ContentTypeDefinitionFieldId.GetIntValue()) { Value = contentTypeDefinition.ContentTypeDefinitionFieldId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.Value.GetIntValue()) { Value = contentTypeDefinition.Value });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.Comparer.GetIntValue()) { Value = contentTypeDefinition.Comparer });

				method.ClearCache = true;

				contentTypeDefinition = await Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
				method.End();
			}
			return contentTypeDefinition;
		}

		public async Task<IEnumerable<ContentTypeDefinitionFolderDataBoundCondition>> GetByFolderAndContentTypeDefinitionIdAsync(long folderId, long contentTypeId)
		{
			await AuthenticateAndAuthorizeAsync();
			List<ContentTypeDefinitionFolderDataBoundCondition> result = new List<ContentTypeDefinitionFolderDataBoundCondition>();
			using (Method method = new Method())
			{
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundCondition;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Methods.GetByFolderAndContentTypeDefinitionId.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.FolderId.GetIntValue()) { Value = folderId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeId });

				result.AddRange(await Task.WhenAll((await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).AsEnumerable().Select(async row => await Create(row))));

				method.End();
			}
			return result;
		}

		public async Task<bool> DeleteAllAsync(long folderId, long contentTypeId)
		{
			await AuthenticateAndAuthorizeAsync();
			bool result = false;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundCondition;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Methods.DeleteByFolderAndContentTypeDefinitionId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.FolderId.GetIntValue()) { Value = folderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeId });

				method.ClearCache = true;

				result = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
            }
            return result;
		}

		public async Task<bool> DeleteAsync(long folderId, long contentTypeId, long fieldId)
		{
			await AuthenticateAndAuthorizeAsync();
			bool result = false;
			using (Method method = new Method())
			{
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundCondition;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Methods.DeleteByFolderAndContentTypeDefinitionIdAndContentTypeDefinitionFieldId.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.FolderId.GetIntValue()) { Value = folderId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Parameters.FolderId.GetIntValue()) { Value = fieldId });

				method.ClearCache = true;

				result = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
				method.End();
			}
			return result;
		}
    }
}
