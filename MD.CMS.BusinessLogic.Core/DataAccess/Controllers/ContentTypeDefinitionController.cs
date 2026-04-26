using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System.Linq;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent;
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionController : BaseController<ContentTypeDefinitionController>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <param name="fillFields"></param>
		/// <param name="transformExpression">If true expression in expression field is going to be in postfix form (desirable in content form), otherwise it's going to be in its natural form (desirable in content type form)</param>
		/// <returns></returns>
		public async Task<ContentTypeDefinition<T>> CreateAsync<T>(DataRow row, bool fillFields = true, bool transformExpression = true)
            where T : Entities.GenericContent.GenericContentField
        {
            Type fieldType = typeof(T);
            ContentTypeDefinition<T> obj = base.Create<ContentTypeDefinition<T>, long>(row, ContentTypeDefinitionParamatersEnum.ContentTypeDefinitionId.GetStringValue());
			if (obj != null)
            {
                obj.Id = row.GetValue<long>(ContentTypeDefinitionParamatersEnum.ContentTypeDefinitionId.GetStringValue());
                obj.Name = row.GetValue<string>(ContentTypeDefinitionParamatersEnum.Name.GetStringValue());
				obj.Description = row.GetValue<string>(ContentTypeDefinitionParamatersEnum.Description.GetStringValue());
				obj.Options = row.GetValue<string>(ContentTypeDefinitionParamatersEnum.Options.GetStringValue());
				obj.Fields = new List<T>();
				obj.IsEditable = row.GetValue<bool>("IsEditable");
				obj.Icon = row.GetValue<string>("Icon");
				if (fillFields)
				{
					List<ContentTypeDefinitionField> fields = await ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByContentTypeDefinitionIdAsync(obj.Id, transformExpression: transformExpression);
					List<T> fieldsByContentType = fields.Select(f => (fieldType == typeof(ContentTypeDefinitionFieldValue)) ? new ContentTypeDefinitionFieldValue(f) as T : f as T).ToList();
					if (fieldsByContentType != null && fieldsByContentType.Any())
					{
						obj.Fields.AddRange(fieldsByContentType);
					}
				}
				obj.DataSources = (await ContentTypeDataSourceController.GetNewInstance().Caller(UserMakingTheCall).GetByContentTypeDefinitionIdAsync(obj.Id)).ToList();
				obj.Joins = new List<ContentTypeDataSourceJoin>();
				await Task.WhenAll(obj.DataSources.Select(async (dataSource) => {
					obj.Joins.AddRange(await ContentTypeDataSourceJoinController.GetNewInstance().Caller(UserMakingTheCall).GetByIdAsync(dataSource.Id));
				}));
			}
			return obj;
		}


		public async Task<ContentTypeDefinition<T>> GetByIdAsync<T>(long id, bool fillFields = true, bool transformExpression = true)
            where T : Entities.GenericContent.GenericContentField
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Methods.GetById.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = id });
			return await CreateAsync<T>(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), fillFields: fillFields, transformExpression: transformExpression);
		}


		public async Task<List<ContentTypeDefinition<F>>> GetByFolderAsync<T, F>(Folder<T> obj)
			where T : Content, new()
            where F : Entities.GenericContent.GenericContentField
		{
			await AuthenticateAndAuthorizeAsync();

			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Methods.GetByFolder.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.FolderId.GetIntValue()) { Value = obj.Id });

			DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<ContentTypeDefinition<F>> list = new ConcurrentQueue<ContentTypeDefinition<F>>();
			await Task.WhenAll(table.AsEnumerable().Select(async row => {
				list.Enqueue(await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync<F>(row));
			}));
			return list.ToList();

		}
		public async Task<List<ContentTypeDefinition<T>>> GetByParentIdAsync<T>(long id)
            where T : Entities.GenericContent.GenericContentField

		{
			await AuthenticateAndAuthorizeAsync();

			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Methods.GetByFolder.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.FolderId.GetIntValue()) { Value = id });

			DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
			
			ConcurrentQueue<ContentTypeDefinition<T>> list = new ConcurrentQueue<ContentTypeDefinition<T>>();
			await Task.WhenAll(table.AsEnumerable().Select(async row => {
				list.Enqueue(await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync<T>(row));
			}));
			return list.ToList();
		}


		public async Task<List<ContentTypeDefinition<T>>> GetAllAsync<T>()
            where T : Entities.GenericContent.GenericContentField
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Methods.GetAll.GetIntValue();
			DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<ContentTypeDefinition<T>> list = new ConcurrentQueue<ContentTypeDefinition<T>>();
			await Task.WhenAll(table.AsEnumerable().Select(async row => {
				list.Enqueue(await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync<T>(row));
			}));
			return list.ToList();
		}


		public async Task<ContentTypeDefinition<T>> SaveAsync<T>(ContentTypeDefinition<T> contentTypeDefinition)
            where T : Entities.GenericContent.GenericContentField
		{
			await AuthenticateAndAuthorizeAsync();
			ContentTypeDefinition<T> newContentTypeDefinition = null;
			using (Method method = new Method())
			{

				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition;

				if (contentTypeDefinition.Id > 0)
				{
					method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
					method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Methods.Update.GetIntValue();
					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.Id.GetIntValue()) { Value = contentTypeDefinition.Id });
				}
				else
				{
					method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
					method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Methods.Insert.GetIntValue();

				}
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.Name.GetIntValue()) { Value = contentTypeDefinition.Name });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.Description.GetIntValue()) { Value = contentTypeDefinition.Description });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.Options.GetIntValue()) { Value = contentTypeDefinition.Options });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.IsEditable.GetIntValue()) { Value = true });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.Icon.GetIntValue()) { Value = contentTypeDefinition.Icon });
				method.ClearCache = true;


				newContentTypeDefinition = await CreateAsync<T>(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

				if (contentTypeDefinition.DataSources != null)
				{
					newContentTypeDefinition.DataSources.Clear();
					foreach (ContentTypeDataSource dataSource in contentTypeDefinition.DataSources)
					{
						dataSource.ContentTypeDefinitionId = newContentTypeDefinition.Id;
						newContentTypeDefinition.DataSources.Add(await ContentTypeDataSourceController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(dataSource));
					}
				}

				List<ContentTypeDefinitionField> newFields = new List<ContentTypeDefinitionField>();
				List<ContentTypeDefinitionField> exFields = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByContentTypeDefinitionIdAsync(contentTypeDefinition.Id);


				if (newContentTypeDefinition != null && contentTypeDefinition.Fields != null && contentTypeDefinition.Fields.Any())
				{
					int index = 1;
					foreach (T genericTypeField in contentTypeDefinition.Fields)
					{
						ContentTypeDefinitionField field = new ContentTypeDefinitionField()
						{
							Id = genericTypeField.Id,
							ContentTypeDefinitionId = newContentTypeDefinition.Id,
							AttributeTypeDefinitionId = genericTypeField.AttributeTypeDefinitionId,
							Name = genericTypeField.Name,
							Description = genericTypeField.Description,
							DefaultValue = genericTypeField.DefaultValue,
							Order = genericTypeField.Order,
							ListValue = genericTypeField.ListValue,
							Delimiter = genericTypeField.Delimiter,
							DataBound = genericTypeField.DataBound,
							DataSourceId = genericTypeField.DataSourceId,
							DataSourceField = genericTypeField.DataSourceField,
                            DataBoundReadOnly = genericTypeField.DataBoundReadOnly,
                            IsDataBoundPrimaryKey = genericTypeField.IsDataBoundPrimaryKey
                        };

						field.Options = genericTypeField.Options;

						for(int i = 0; i < contentTypeDefinition.DataSources.Count; i++)
						{
							if (contentTypeDefinition.DataSources[i].Id == field.DataSourceId)
							{
								field.DataSourceId = newContentTypeDefinition.DataSources[i].Id;
								break;
							}
						}

						field.ContentTypeDefinitionId = newContentTypeDefinition.Id;
						ContentTypeDefinitionField savedField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(field);
						newFields.Add(field);
						index++;
					}
				}

				if (newFields != null)
				{
					foreach (ContentTypeDefinitionField exField in exFields)
					{
						bool found = false;
						foreach (ContentTypeDefinitionField newField in newFields)
						{
							if (exField.Id == newField.Id)
							{
								found = true;
							}
						}
						if (!found)
						{
							await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAsync(exField);
							newContentTypeDefinition.Fields.RemoveAll(f => f.Id == exField.Id);
						}
					}
				}

				if (contentTypeDefinition.DataSources != null)
				{
					for(int i = 0; i < contentTypeDefinition.DataSources.Count; i++)
                    {
						contentTypeDefinition.DataSources[i] = await ContentTypeDataSourceController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(contentTypeDefinition.DataSources[i]);
					}
				}

				if (contentTypeDefinition.Joins != null)
				{
					foreach (ContentTypeDataSourceJoin dataSourceJoin in contentTypeDefinition.Joins)
					{
						await ContentTypeDataSourceJoinController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAsync(dataSourceJoin);
					}

					if (newContentTypeDefinition.DataSources != null && contentTypeDefinition.DataSources.Count == newContentTypeDefinition.DataSources.Count)
					{
						foreach (ContentTypeDataSourceJoin dataSourceJoin in contentTypeDefinition.Joins)
						{
							if (dataSourceJoin.RightDataSourceId < 0)
							{
								for (int i = 0; i < contentTypeDefinition.DataSources.Count; i++)
								{
									if (dataSourceJoin.RightDataSourceId == contentTypeDefinition.DataSources[i].Id)
									{
										dataSourceJoin.RightDataSourceId = newContentTypeDefinition.DataSources[i].Id;
									}
								}
							}
						}
					}

					foreach (ContentTypeDataSourceJoin dataSourceJoin in contentTypeDefinition.Joins)
					{
						if (dataSourceJoin.LeftFieldId < 0)
						{
							for (int i = 0; i < contentTypeDefinition.Fields.Count; i++)
							{
								if (dataSourceJoin.LeftFieldId == contentTypeDefinition.Fields[i].Id)
								{
									dataSourceJoin.LeftFieldId = newContentTypeDefinition.Fields[i].Id;
								}
							}
						}

						if (dataSourceJoin.RightFieldId < 0)
						{
							for (int i = 0; i < contentTypeDefinition.Fields.Count; i++)
							{
								if (dataSourceJoin.RightFieldId == contentTypeDefinition.Fields[i].Id)
								{
									dataSourceJoin.RightFieldId = newContentTypeDefinition.Fields[i].Id;
								}
							}
						}
						newContentTypeDefinition.Joins.Add(dataSourceJoin);
					}
				}

				foreach (ContentTypeDataSourceJoin dataSourceJoin in newContentTypeDefinition.Joins)
				{
					await ContentTypeDataSourceJoinController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(dataSourceJoin);
				}
			}

			return newContentTypeDefinition;
		}

		public async Task<bool> DeleteAsync<T>(ContentTypeDefinition<T> obj)
            where T : Entities.GenericContent.GenericContentField
		{
			await AuthenticateAndAuthorizeAsync();
			bool success;

			using (Method method = new Method())
			{
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Methods.Delete.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.Id.GetIntValue()) { Value = obj.Id });
				method.ClearCache = true;

				success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
			}
			return success;
		}

		public async Task<Entities.Base.BasePaginationEntity<ContentTypeDefinition<T>>> GetAllWithPaginationAsync<T>(int currentPageIndex, int maxNumberOfRows, string searhTerm, string searchColumn, string sort = "Name ASC")
            where T : Entities.GenericContent.GenericContentField
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition;
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Methods.SelectAllWithPagination.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.SearchTerm.GetIntValue()) { Value = searhTerm });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
			DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<ContentTypeDefinition<T>> list = new ConcurrentQueue<ContentTypeDefinition<T>>();
			await Task.WhenAll(table.AsEnumerable().Select(async row => {
				list.Enqueue(await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync<T>(row));
			}));

			Entities.Base.BasePaginationEntity<ContentTypeDefinition<T>> basePaginationEntity = new Entities.Base.BasePaginationEntity<ContentTypeDefinition<T>>();
			basePaginationEntity.Items = list.ToList();
			if (table.Rows.Count > 0)
			{
				basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
			}
			return basePaginationEntity;
		}

		public async Task<int> SelectAllCountAsync(string searchTerm, string searchColumn)
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Methods.SelectAllCount.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
			DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
			int count = row.GetValue<int>("ContentTypeDefinitionsCount");
			return count;
		}

		public async Task<List<ContentTypeDefinition<T>>> SearchAsync<T>(string searchTerm, string searchColumn)
            where T : Entities.GenericContent.GenericContentField
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinition;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Methods.Search.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
			DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<ContentTypeDefinition<T>> list = new ConcurrentQueue<ContentTypeDefinition<T>>();
			await Task.WhenAll(table.AsEnumerable().Select(async row => {
				list.Enqueue(await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync<T>(row));
			}));
			return list.ToList();

		}
	}
}
