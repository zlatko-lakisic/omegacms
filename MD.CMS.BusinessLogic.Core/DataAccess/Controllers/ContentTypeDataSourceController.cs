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
    public partial class ContentTypeDataSourceController : BaseController<ContentTypeDataSourceController>
    {
        public ContentTypeDataSource Create(DataRow row) {
			ContentTypeDataSource obj = base.Create<ContentTypeDataSource, long>(row, ContentTypeDataSourceEnum.DataSourceId.GetStringValue());
			if (obj != null)
			{
				obj.ConnectionString = row.GetValue<string>(ContentTypeDataSourceEnum.ConnectionString.GetStringValue());
				obj.ContentTypeDefinitionId = row.GetValue<long>(ContentTypeDataSourceEnum.ContentTypeDefinitionId.GetStringValue());
				obj.DbType = row.GetValue<string>(ContentTypeDataSourceEnum.DatabaseType.GetStringValue());
			}
			return obj;
		}

        public async Task<ContentTypeDataSource> GetByIdAsync(long dataSourceId)
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Parameters.DataSourceId.GetIntValue()) { Value = dataSourceId });
            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<IEnumerable<ContentTypeDataSource>> GetByContentTypeDefinitionIdAsync(long contentTypeDefinitionId)
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Methods.GetByContentTypeDefinitionId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeDefinitionId });
            return (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).AsEnumerable().Select(row => Create(row));
		}
		public async Task<ContentTypeDataSource> SaveAsync(ContentTypeDataSource contentTypeDataSource)
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource;
			if(contentTypeDataSource.Id > default(long))
			{
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Methods.Update.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Parameters.DataSourceId.GetIntValue()) { Value = contentTypeDataSource.Id });
			} 
			else
			{
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Methods.Insert.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeDataSource.ContentTypeDefinitionId });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Parameters.ConnectionString.GetIntValue()) { Value = contentTypeDataSource.ConnectionString });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Parameters.DatabaseType.GetIntValue()) { Value = contentTypeDataSource.DbType });
			method.ClearCache = true;

			return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
		}

        public async Task<bool> DeleteAsync(ContentTypeDataSource contentTypeDataSource)
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionDataSource;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Methods.Delete.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Parameters.DataSourceId.GetIntValue()) { Value = contentTypeDataSource.Id });
			method.ClearCache = true;

			return await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
		}
		public async Task<dynamic> GetDataStructureAsync(string type, string connectionString, string field = "")
		{
			await AuthenticateAndAuthorizeAsync();
			return await GetDataStructureAsync(new DataBoundMethod(type, connectionString, new[] { field }));
		}

		public async Task<IEnumerable<string>> GetAllDatabaseTypesAsync()
		{
			await AuthenticateAndAuthorizeAsync();
			return GetDataBoundPluginsAsync().Result.Select(plugin => plugin.DatabaseType);
		}
	}
}
