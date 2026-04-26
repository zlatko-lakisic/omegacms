using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDataSourceJoinController : BaseController<ContentTypeDataSourceJoinController>
    {
        public ContentTypeDataSourceJoin Create(DataRow row) {
			ContentTypeDataSourceJoin obj = null;
			if (row != null)
			{
				obj.LeftFieldId = row.GetValue<long>(ContentTypeDataSourceJoinEnum.RightDataSourceId.GetStringValue());
				obj.LeftRightDataSourceJoinType = row.GetValue<string>(ContentTypeDataSourceJoinEnum.LeftRightDataSourceJoinType.GetStringValue());
				obj.RightFieldId = row.GetValue<long>(ContentTypeDataSourceJoinEnum.RightFieldId.GetStringValue());
				obj.LeftFieldId = row.GetValue<long>(ContentTypeDataSourceJoinEnum.LeftFieldId.GetStringValue());
			}
			return obj;
		}

        public async Task<IEnumerable<ContentTypeDataSourceJoin>> GetByIdAsync(long rightDataSourceId)
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSourceJoin;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Parameters.RightDataSourceId.GetIntValue()) { Value = rightDataSourceId });
            return (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).AsEnumerable().Select(row => Create(row));
        }

		public async Task<ContentTypeDataSourceJoin> SaveAsync(ContentTypeDataSourceJoin contentTypeDataSourceJoin)
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSourceJoin;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Methods.Insert.GetIntValue();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Parameters.LeftFieldId.GetIntValue()) { Value = contentTypeDataSourceJoin.LeftFieldId });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Parameters.LeftRightDataSourceJoinType.GetIntValue()) { Value = contentTypeDataSourceJoin.LeftRightDataSourceJoinType });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Parameters.RightDataSourceId.GetIntValue()) { Value = contentTypeDataSourceJoin.RightDataSourceId });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Parameters.RightFieldId.GetIntValue()) { Value = contentTypeDataSourceJoin.RightFieldId });
			method.ClearCache = true;

			return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
		}

        public async Task<bool> DeleteAsync(ContentTypeDataSourceJoin ContentTypeDefinitionDataSourceJoin)
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSourceJoin;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Methods.Delete.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Parameters.RightDataSourceId.GetIntValue()) { Value = ContentTypeDefinitionDataSourceJoin.RightDataSourceId });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Parameters.LeftFieldId.GetIntValue()) { Value = ContentTypeDefinitionDataSourceJoin.LeftFieldId });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Parameters.RightFieldId.GetIntValue()) { Value = ContentTypeDefinitionDataSourceJoin.RightFieldId });
			method.ClearCache = true;

			return await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
		}
	}
}
