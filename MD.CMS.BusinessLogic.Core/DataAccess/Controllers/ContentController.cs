using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.StringExt;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Google;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Google;
using MD.Tools.BaseDataAccess.Plugins.Core;
using Newtonsoft.Json;
using MD.CMS.BusinessLogic.Core.Helpers.Calculations;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;
using MD.Tools.Helpers.Core.Data;
using System.Collections.Concurrent;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public class ContentController<T> : ContentController<T, ContentController<T>>, Tools.BaseDataAccess.Core.Interfaces.IBaseControllerSettings
        where T : Content, new()
    {
        public ContentController<T> GetOnlyPublished(bool getOnlyPublished = false)
		{
			this._getOnlyPublished = getOnlyPublished;
            return this;
        }
    }

    public partial class ContentController<T, SingletonType> : BaseController<SingletonType>
        where T : Content, new()
        where SingletonType : class, Tools.BaseDataAccess.Core.Interfaces.IBaseControllerSettings, new()
	{
		protected bool _getOnlyPublished;

		protected void LinkFields(T content)
        {
			foreach (ContentTypeDefinitionFieldValue field in content.ContentType.Fields)
			{
				if (field.JsonField.linkToTitle)
				{
					content.Title = field.Value;
				}
			}
		}

		public virtual async Task<T> CreateAsync(DataRow row, bool loadAuthor = false, bool fillFields = false, bool fillMetaDataFields = false)
		{
			T obj = base.Create<T, string>(row, ContentEnum.ContentId.GetStringValue());
			if (obj != null)
            {
                obj.IsPublished = row.GetValue<bool>(ContentEnum.IsPublished.GetStringValue());
				obj.LCID = row.GetValue<int>(ContentEnum.LCID.GetStringValue());
				obj.DateCreated = row.GetValue<DateTime>(ContentEnum.DateCreated.GetStringValue()).ToString();
				obj.AuthorId = row.GetValue<string>(ContentEnum.AuthorId.GetStringValue());
				obj.FolderId = row.GetValue<long>(ContentEnum.FolderId.GetStringValue());
				obj.Title = row.GetValue<string>(ContentEnum.Title.GetStringValue());
                obj.Html = row.GetValue<string>(ContentEnum.Html.GetStringValue());
				obj.TaxonomyId = row.GetValue<long>(ContentEnum.TaxonomyId.GetStringValue());
				obj.ApprovalPending = row.GetValue<bool>(ContentEnum.ApprovalPending.GetStringValue());
				obj.ContentTypeDefinitionId = row.GetValue<long>(ContentEnum.ContentTypeDefinitionId.GetStringValue());

				ContentTypeDefinition<ContentTypeDefinitionFieldValue> contentTypeDefinition = await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync<ContentTypeDefinitionFieldValue>(obj.ContentTypeDefinitionId, fillFields: fillFields, transformExpression: true);
				if (contentTypeDefinition != null)
				{
					obj.ContentType = contentTypeDefinition;
				}

				await Task.WhenAll(new List<Task>() {
					Task.Run(async () =>
                    {
						Folder<Content> folder = (await FolderController<Content>.GetNewInstance().DefaultPlugin(UseDefaultPlugin).Caller(UserMakingTheCall).Execute(new FolderRequestOptions(){ 
							FolderIds = new long[]{ obj.FolderId }.ToList()
						})).Items.FirstOrDefault();

						if (folder != null)
						{
							obj.Path = folder.FolderPath;
						}
					}),
					Task.Run(async () =>
					{
						if (loadAuthor)
						{
							obj.Author = await UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(User.SystemUser()).GetByIdAsync(obj.AuthorId);
						}
					}),
					Task.Run(async () =>
					{
						if (fillFields)
						{
							if (obj.ContentType != null)
							{
								List<ContentTypeDefinitionFieldValue> valuesByContent = await ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByContentAsync(obj);

								List<Task> tasksToExecute = new List<Task>();
								foreach (ContentTypeDefinitionFieldValue field in obj.ContentType.Fields)
								{
									if (field.AttributeTypeDefinitionId == 17)
									{
										ContentTypeDefinitionFieldValue calculated = new ContentTypeDefinitionFieldValue(field);
										tasksToExecute.Add(PostfixEvaluator.EvaluateAsync(UserMakingTheCall, valuesByContent, calculated, calculated.DefaultValue));
										valuesByContent.Add(calculated);
									}
								}
								Task.WaitAll(tasksToExecute.ToArray());

								List<ContentTypeDefinitionFieldValue> additionalValues = new List<ContentTypeDefinitionFieldValue>();
								for (int i = 0, length = valuesByContent.Count; i < length; i++)
								{
									//16 - media content
									if (valuesByContent[i].AttributeTypeDefinitionId == 16)
									{
										int mediaContentId = valuesByContent[i].Value.ToInt();
										if (mediaContentId > 0)
										{
											MediaContent mediaContent = await MediaContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(mediaContentId);
											if (mediaContent != null)
											{
												string jsonValues = "{";
												valuesByContent[i].Value = mediaContentId + ";" + mediaContent.FullNameFile;
												if (mediaContent.MediaContentMetaDataFieldValues != null && mediaContent.MediaContentMetaDataFieldValues.Any())
												{
													foreach (MediaContentMetaDataFieldValues value in mediaContent.MediaContentMetaDataFieldValues)
													{
														jsonValues += "\"" + value.FriendlyName + "\":\"" + value.Value + "\",";
													}
												}

												jsonValues += "\"url\":\"" + mediaContent.FullNameFile + "\",";
												jsonValues += "\"alt\":\"" + mediaContent.Description + "\"";

												jsonValues += "}";
											}
										}
									}
								}

								var groups = valuesByContent.GroupBy(field => new { field.ContentTypeDefinitionFieldId });
								foreach (var group in groups)
								{
									List<string> values = new List<string>();
									foreach (ContentTypeDefinitionFieldValue fieldValue in group)
									{
										values.Add(fieldValue.Value);
										foreach (ContentTypeDefinitionFieldValue field in obj.ContentType.Fields)
										{
											if (fieldValue.ContentTypeDefinitionFieldId == field.Id || fieldValue.ContentTypeDefinitionFieldId > 500000)
											{
												field.Value = JsonConvert.SerializeObject(values);
											}
										}
									}
								}

								foreach (ContentTypeDefinitionFieldValue fieldValue in valuesByContent)
								{
									foreach (ContentTypeDefinitionFieldValue field in obj.ContentType.Fields)
									{
										if (fieldValue.ContentTypeDefinitionFieldId == field.ContentTypeDefinitionFieldId)
										{
											field.Value = fieldValue.Value;
											field.DateCreated = fieldValue.DateCreated;
											field.ContentId = fieldValue.ContentId;
											field.LCID = fieldValue.LCID;
										}
									}
								}

								LinkFields(obj);
							}
						}
					}),
					Task.Run(async () =>
					{
						if (fillMetaDataFields)
						{
							if (obj.MetaDataFieldValues == null)
							{
								obj.MetaDataFieldValues = new List<MetaDataFieldValue>();
								List<MetaDataField> metaDataFields = await MetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(obj.FolderId);
								List<MetaDataFieldValue> metaDataFieldValues = await MetaDataFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByContentAsync(obj);
								foreach (MetaDataField field in metaDataFields)
								{
									MetaDataFieldValue metaDataFieldValue = new MetaDataFieldValue(field);
									metaDataFieldValue.Id = field.Id;
									metaDataFieldValue.ContentId = obj.Id;
									metaDataFieldValue.LCID = obj.LCID;

									foreach (MetaDataFieldValue fieldValue in metaDataFieldValues)
									{
										if (field.Id == fieldValue.MetaDataFieldId)
										{
											metaDataFieldValue.Value = fieldValue.Value;
											metaDataFieldValue.DateCreated = fieldValue.DateCreated;
										}
									}
									obj.MetaDataFieldValues.Add(metaDataFieldValue);
								}
							}
						}
					}),
					Task.Run(async () =>
					{
						obj.ContentAliases = await ContentAliasController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetAllAliasesByContentAsync(obj);
					})
				});
			}
			return obj;
		}

		public virtual ContentController<T, SingletonType> GetOnlyPublished(bool getOnlyPublished = false)
		{
			this._getOnlyPublished = getOnlyPublished;
			return this;
		}

		public virtual async Task<bool> IsAuthorizedAsync(Content content, User user, Entities.Permissions.PermissionAccessTypeEnum permissionType)
		{
            return true;
            return await PermissionsController.GetNewInstance().Caller(User.SystemUser()).UserHasObjectPermissionAsync(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content, content.UniqueId, user.Id, permissionType);
		}

		[Obsolete("GetById is deprecated, please use GetByIdAsync instead.", true)]
		public virtual async Task<T> GetByIdAsync(string id, bool loadAuthor = false, int lcid = default(int), bool fillFields = true, bool fillMetaDataFields = true)
		{
			await AuthenticateAndAuthorizeAsync();
			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}

			return (await GetByIdAsync(new ContentOptions() {
				ContentIds = new string[]{ id }.ToList(),
				FillFields = fillFields,
				LoadAuthor = loadAuthor,
				FillMetaData = fillMetaDataFields,
				Lcid = lcid
			})).FirstOrDefault();
		}
		
		public virtual async Task<IEnumerable<T>> GetByIdAsync(ContentOptions options)
		{
			return (await Execute(new ContentRequestOptions()
			{
				ContentIds = options.ContentIds,
				FillFields = options.FillFields,
				FillMetaData = options.FillMetaData,
				Lcid = options.Lcid,
				LoadAuthor = options.LoadAuthor,
				OnlyPublished = _getOnlyPublished
			})).Items;
		}

		/*public virtual async Task<Entities.Base.BasePaginationEntity<T>> GetByFolderWithPaginationAsync(Folder<T> folder, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", bool loadAuthor = false, int lcid = default(int), string sort = "Title ASC", bool loadFields = false)
		{
			await AuthenticateAndAuthorizeAsync();
			if (string.IsNullOrEmpty(sort))
			{
				sort = "Title ASC";
			}

			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}
			if (searchTerm == null)
			{
				searchTerm = "";
			}
			searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
			DataTable table = new DataTable();

			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderIdWithPagination.GetIntValue();
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderIdWithPaginationOnlyPublished.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });



			//this grouping is used for solr only
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

			table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(table.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row, loadAuthor: true, fillFields: loadFields));
			}));

			Entities.Base.BasePaginationEntity<T> basePaginationEntity = new Entities.Base.BasePaginationEntity<T>();
			basePaginationEntity.Items = contents.ToList();
			if (table.Rows.Count > 0)
			{
				basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
			}
			return basePaginationEntity;
		}*/

		public virtual async Task<List<T>> GetByFolderIdAsync(long id, bool loadAuthor = false, int lcid = default(int), bool loadFields = false, bool loadMetaDataFields = false)
		{
			return (await Execute(new ContentRequestOptions()
			{
				Lcid = lcid,
				FolderId = id,
				OnlyPublished = _getOnlyPublished,
				MaxNumberOfRows = int.MaxValue
			})).Items;
		}

		public virtual Task<Entities.Base.BasePaginationEntity<T>> GetByFolderIdWithPaginationAsync(long id, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", bool loadAuthor = false, int lcid = default(int), string sort = "Title ASC", bool loadFields = false)
		{
            if (string.IsNullOrEmpty(sort))
            {
				sort = "Title ASC";
			}

			return Execute(new ContentRequestOptions()
			{
				FolderId = id,
				CurrentPageIndex = currentPageIndex,
				MaxNumberOfRows = maxNumberOfRows,
				SearchTerm = searchTerm,
				LoadAuthor = loadAuthor,
				Lcid = lcid,
				FillFields = loadFields,
				SortField = EnumExtensions.GetByStringValue<ContentEnum>(string.Join(' ', sort.Split(' ').Take(sort.Split(' ').Length - 1))),
				SortDirection = EnumExtensions.GetByStringValue<V2.Options.Interfaces.SortDirection>(sort.Split(' ').LastOrDefault())
			});
		}

		public virtual Task<Entities.Base.BasePaginationEntity<T>> GetByFolderWithPaginationAsync(Folder<T> folder, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", bool loadAuthor = false, int lcid = default(int), string sort = "Title ASC", bool loadFields = false)
		{
			return GetByFolderIdWithPaginationAsync(folder.Id, currentPageIndex, maxNumberOfRows, searchTerm, loadAuthor, lcid, sort, loadFields);




			/*await AuthenticateAndAuthorizeAsync();
			if (string.IsNullOrEmpty(sort))
			{
				sort = "Title ASC";
			}

			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}
			if (searchTerm == null)
			{
				searchTerm = "";
			}
			searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);
			DataTable table = new DataTable();

			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderIdWithPagination.GetIntValue();
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderIdWithPaginationOnlyPublished.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });



			//this grouping is used for solr only
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

			table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(table.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row, loadAuthor: true, fillFields: loadFields));
			}));

			Entities.Base.BasePaginationEntity<T> basePaginationEntity = new Entities.Base.BasePaginationEntity<T>();
			basePaginationEntity.Items = contents.ToList();
			if (table.Rows.Count > 0)
			{
				basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
			}
			return basePaginationEntity;*/
		}

		public virtual async Task<T> GetByAllAsync(T content, bool loadAuthor = false, bool fillFields = true, bool fillMetaDataFields = true)
		{
			await AuthenticateAndAuthorizeAsync();
			if (content.ContentType != null && content.ContentType.Fields.Any(field => field.DataBound && field.IsDataBoundPrimaryKey))
			{
				DataTable table = new DataTable();
				DataSet set = new DataSet();
				IEnumerable<IGrouping<long, ContentTypeDefinitionFieldValue>> dataBoundFields = content.ContentType.Fields.Where(field => field.DataBound).GroupBy(field => field.DataSourceId);
				foreach (IGrouping<long, ContentTypeDefinitionFieldValue> group in dataBoundFields)
				{
					ContentTypeDataSource dataSource = content.ContentType.DataSources.First(ds => ds.Id.Equals(group.Key));
					DataBoundMethod method = new DataBoundMethod(dataSource.DbType, dataSource.ConnectionString, group.Select(field => field.DataSourceField));
					method.Conditions.Add(new Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.ContentTypeDefinitionFolderDataBoundCondition()
					{
						Comparer = ComparerTypeEnum.Equals,
						ContentTypeDefinitionId = content.ContentType.Id,
						LeftField = group.First(field => field.IsDataBoundPrimaryKey).DataSourceField,
						Value = content.Id
					});
					set.Tables.Add(await ExecuteMethodTableAsync(method));
				}

				table.Columns.Add(ContentEnum.ContentId.GetStringValue());
				table.Columns.Add(ContentEnum.LCID.GetStringValue());
				table.Columns.Add(ContentEnum.IsPublished.GetStringValue());

				foreach (DataTable dataTable in set.Tables)
				{
					foreach (DataColumn column in dataTable.Columns)
					{
						if (!table.Columns.Contains(column.ColumnName))
						{
							table.Columns.Add(column.ColumnName);
						}
					}
				}

				DataRow row = table.NewRow();
				Random contentId = new Random();
				row[ContentEnum.ContentId.GetStringValue()] = contentId.Next(1, int.MaxValue);
				row[ContentEnum.LCID.GetStringValue()] = content.LCID;
				row[ContentEnum.IsPublished.GetStringValue()] = true;
				bool rowExists = false;
				foreach (DataColumn column in table.Columns)
				{
					DataRow rowToCopyFrom = null;
					foreach (DataTable dataTable in set.Tables)
					{
						if (dataTable.Columns.Contains(column.ColumnName) && dataTable.AsEnumerable().Any())
						{
							rowToCopyFrom = dataTable.Rows[0];
							break;
						}
					}
					if (rowToCopyFrom != null)
					{
						row[column.ColumnName] = rowToCopyFrom[column.ColumnName];
						rowExists = true;
					}
				}
				if (rowExists)
				{
					table.Rows.Add(row);
				}

				return await CreateAsync(
					row: table.AsEnumerable().FirstOrDefault()
					);
			}
			else
			{
				if (content.LCID.Equals(default(int)))
				{
					content.LCID = Settings.Default.DefaultLcid;
				}
				Method method = new Method();
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.SelectByAll.GetIntValue();
				if (_getOnlyPublished == true)
				{
					method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.SelectByAllOnlyPublished.GetIntValue();
				}
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentId.GetIntValue()) { Value = content.Id });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = content.LCID });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.DateCreated.GetIntValue()) { Value = content.DateCreated });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });

				return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), loadAuthor, fillFields, fillMetaDataFields);
			}
		}

		[Obsolete("SelectAllCount is not being used anymore!")]
		public virtual async Task<long> SelectAllCountAsync(int lcid = default(int))
		{
			await AuthenticateAndAuthorizeAsync();
			/*if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}
			return ExecuteCommandRow(
							GenerateStoredProcedure(ContentSPEnum.SelectAllCount.GetStringValue(),
														new MySqlParameter() { ParameterName = ContentParametersEnum.LCID.GetStringValue(), DbType = DbType.Int32, Value = lcid }
							)
						).GetValue<long>(ContentEnum.ContentCount.GetStringValue());*/
			return 0;
		}

		public virtual async Task<int> SelectByContentTypeDefinitionCountAsync(long id)
		{
			await AuthenticateAndAuthorizeAsync();
			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.SelectByContentTypeDefinitionCount.GetIntValue();
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.SelectByContentTypeDefinitionCountOnlyPublished.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });

			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

			DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
			int count = row.GetValue<int>("ContentCount");
			return count;
		}

		public virtual async Task<List<T>> GetAllVersionAsync(string id, int lcid = default(int))
		{
			await AuthenticateAndAuthorizeAsync();
			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}

			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.SelectAllVersion.GetIntValue();
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.SelectAllVersionOnlyPublished.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentId.GetIntValue()) { Value = id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });

			DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(results.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row, true, true, true));
			}));

			List<T> contentResults = contents.ToList();
			contentResults.Sort((x, y) => x.CompareTo(y));
			return contentResults;
		}

		public virtual async Task<List<T>> GetAllAsync(int lcid = default(int))
		{
			await AuthenticateAndAuthorizeAsync();
			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}

			Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			//there might be issue because mysql plugin doesn't have GetAllOnlyPublished method
			//and it was necesery to make both methods on solr plugin
			//this will be ok if flow goes naturally to solr plugin if mysql doesn't have some of the methods
			//otherwise check current plugin too in following condition
			//or make double methods on mysql plugins too and change procedures accordingly
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetAllOnlyPublished.GetIntValue();
			}
			else
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetAll.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });

			//new way for group by
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
			//end
			DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(results.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row, loadAuthor: false));
			}));

			return contents.ToList();
		}

		public virtual async Task<List<T>> SearchAsync(string searchTerm, int lcid = default(int))
		{
			await AuthenticateAndAuthorizeAsync();
			searchTerm = searchTerm.Replace("'", "''");

			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}

			return (await Execute(new ContentRequestOptions()
			{
				SearchTerm = searchTerm,
				Lcid = lcid,
				MaxNumberOfRows = int.MaxValue
			})).Items;

			/*Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.Search.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });

			DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(results.AsEnumerable().Select(async row => {
				contents.Enqueue(await CreateAsync(row, loadAuthor: true));
			}));

			return contents.ToList();*/
		}

		public virtual async Task<List<T>> TaxonomyContentGetContentByTaxonomyAsync(Taxonomy taxonomy, int lcid, bool fillFields = false, bool fillMetaDataFields = false)
		{
			await AuthenticateAndAuthorizeAsync();

			return (await Execute(new ContentRequestOptions()
			{
				TaxonomyId = taxonomy.Id,
				Lcid = lcid,
				OnlyPublished = _getOnlyPublished,
				MaxNumberOfRows = int.MaxValue,
				FillFields = fillFields,
				FillMetaData = fillMetaDataFields
			})).Items;
			/*Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByTaxonomyId.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomy.Id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

			DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(result.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(
					row,
					loadAuthor: false,
					fillFields: fillFields,
					fillMetaDataFields: fillMetaDataFields
					));
			}));

			return contents.ToList();*/
		}

		public virtual async Task<List<T>> MenuContentGetContentByMenuAsync(Menu menu)
		{
			await AuthenticateAndAuthorizeAsync();
			return (await Execute(new ContentRequestOptions()
			{
				MenuId = menu.Id,
				Lcid = menu.LCID,
				OnlyPublished = _getOnlyPublished,
				MaxNumberOfRows = int.MaxValue
			})).Items;
			/*Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByMenuId.GetIntValue();
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByMenuIdOnlyPublished.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.MenuId.GetIntValue()) { Value = menu.Id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });


			DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(result.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row, loadAuthor: false));
			}));

			return contents.ToList();*/
		}

		public virtual async Task<int> GetByFolderIdCountAsync(long folderId, int lcid, string searchTerm)
		{
			await AuthenticateAndAuthorizeAsync();

			return (await Execute(new ContentRequestOptions()
			{
				FolderId = folderId,
				Lcid = lcid,
				OnlyPublished = _getOnlyPublished,
				MaxNumberOfRows = 1,
				SearchTerm = searchTerm
			})).TotalCount;

			/*Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderIdCount.GetIntValue();
			//there might be issue because mysql plugin doesn't have GetByAliasOnlyPublished method
			//and it was necesery to make both methods on solr plugin
			//this will be ok if flow goes naturally to solr plugin if mysql doesn't have some of the methods
			//otherwise check current plugin too in following condition
			//or make double methods on mysql plugins too and change procedures accordingly
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderIdCountOnlyPublished.GetIntValue();
			}
			if (searchTerm == null)
			{
				searchTerm = "";
			}
			searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.FolderId.GetIntValue()) { Value = folderId });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
			DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
			int count = row.GetValue<int>("ContentByFolderCount");
			return count;*/
		}

		public virtual async Task<List<T>> ContentsGetByFolderIdAsync(long id, bool loadAuthor = false, int lcid = default(int))
		{
			await AuthenticateAndAuthorizeAsync();
			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}


			return (await Execute(new ContentRequestOptions()
			{
				FolderId = id,
				Lcid = lcid,
				OnlyPublished = _getOnlyPublished,
				MaxNumberOfRows = int.MaxValue,
				LoadAuthor = loadAuthor
			})).Items;

			/*Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderId.GetIntValue();
			//there might be issue because mysql plugin doesn't have GetByAliasOnlyPublished method
			//and it was necesery to make both methods on solr plugin
			//this will be ok if flow goes naturally to solr plugin if mysql doesn't have some of the methods
			//otherwise check current plugin too in following condition
			//or make double methods on mysql plugins too and change procedures accordingly
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderIdOnlyPublished.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.FolderId.GetIntValue()) { Value = id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });

			DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(results.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row, loadAuthor));
			}));

			return contents.ToList();*/
		}

		public virtual async Task<List<T>> GetBySearchTermAsync(string searchTerm, bool loadAuthor = false, int lcid = default(int))
		{
			await AuthenticateAndAuthorizeAsync();
			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}

			return (await Execute(new ContentRequestOptions()
			{
				SearchTerm = searchTerm,
				Lcid = lcid,
				OnlyPublished = _getOnlyPublished,
				MaxNumberOfRows = int.MaxValue,
				LoadAuthor = loadAuthor,
				FillMetaData = true
			})).Items;

			/*using (Method method = new Method())
			{
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetBySearchTerm.GetIntValue();
				if (_getOnlyPublished == true)
				{
					method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetBySearchTermOnlyPublished.GetIntValue();
				}
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });

				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

				DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

				ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
				await Task.WhenAll(results.AsEnumerable().Select(async row => {
					contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row, loadAuthor, fillFields: false, fillMetaDataFields: true));
				}));

				return contents.ToList();
			}*/
		}

		public virtual async Task<T> SaveAsync(T content)
		{
			await AuthenticateAndAuthorizeAsync();
			DateTime date = new DateTime(0001, 1, 1);
			if (content.DateCreated == date.ToString())
				content.DateCreated = DateTime.UtcNow.ToString();

			T newContent = null;
			using (Method method = new Method())
			{

				if (content.ContentType != null && content.ContentType.Fields != null && content.ContentType.Fields.Any())
				{
					foreach (ContentTypeDefinitionFieldValue field in content.ContentType.Fields)
					{
						if (field.AttributeTypeDefinitionId == 17)
						{
							Task.Run(async () => {
								await PostfixEvaluator.EvaluateAsync(UserMakingTheCall, content.ContentType.Fields, field, field.DefaultValue); }).Wait();
						}

						if (field.JsonField.linkToTitle)
						{
							content.Title = field.Value;
						}
					}
				}

				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
				IMethodProperty ctdIdParam = new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentTypeDefinitionId.GetIntValue());

				if (content.ContentType != null && content.ContentType.Id != 0)
				{
					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = content.ContentType.Id });
				}
				else
				{
					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = null });
				}

				if (!content.IsNew)
				{
					method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentId.GetIntValue()) { Value = content.Id });
				}
				else
				{
					method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentId.GetIntValue()) { Value = 0 });
				}

				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.Insert.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.FolderId.GetIntValue()) { Value = content.FolderId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = content.LCID });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.AuthorId.GetIntValue()) { Value = content.AuthorId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.Title.GetIntValue()) { Value = content.Title });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.Html.GetIntValue()) { Value = content.Html });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.DateCreated.GetIntValue()) { Value = DateTime.Now });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.IsPublished.GetIntValue()) { Value = content.IsPublished });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ApprovalPending.GetIntValue()) { Value = content.ApprovalPending });
				newContent = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

				//method.WaitForOnBeforeCompleted();

				if (newContent != null && !string.IsNullOrEmpty(newContent.Id) && newContent.Id != "0")
				{
					if (content.ContentType != null && content.ContentType.Fields != null && content.ContentType.Fields.Any())
					{
						foreach (ContentTypeDefinitionFieldValue field in content.ContentType.Fields)
						{
							if (field.Value != null)
							{
								string[] values = field.Value.Split('~');
								if (values.Length > 1)
								{
									for (int v = 0; v < values.Length - 1; v++)
									{
										field.ContentId = newContent.Id;
										field.LCID = newContent.LCID;
										field.DateCreated = Convert.ToDateTime(newContent.DateCreated);
										field.ContentTypeDefinitionFieldId = field.Id;
										field.Value = values[v];
										if (field.Value.Contains("*tilda*"))
										{
											field.Value = field.Value.Replace("*tilda*", "~");
										}
										await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(field);
									}
								}
								else
								{
									field.ContentId = newContent.Id;
									field.LCID = newContent.LCID;
									field.DateCreated = Convert.ToDateTime(newContent.DateCreated);
									field.ContentTypeDefinitionFieldId = field.Id;
									await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(field);
								}
							}
						}
					}

					if (content.MetaDataFieldValues != null && content.MetaDataFieldValues.Any())
					{
						foreach (MetaDataFieldValue field in content.MetaDataFieldValues)
						{
							if (field.Value != null)
							{
								field.DateCreated = newContent.DateCreated;
								field.ContentId = newContent.Id;
								field.LCID = newContent.LCID;
								await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MetaDataFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(field);
							}
						}
					}

					if (content.Template != null)
					{
						await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).AssignTemplateToContentAsync(content.Template, newContent);
					}
					if (content.Taxonomy != null)
					{
						for (var i = 0; i < content.Taxonomy.Count; i++)
						{
							int order = i;
							await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(newContent, content.Taxonomy[i], order);
						}
						//foreach (Taxonomy taxonomy in content.Taxonomy)
						//{
						//    MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.Instance.Save(newContent, taxonomy);
						//}
					}
					if (content.ContentAliases != null && content.ContentAliases.Any())
					{
						foreach (ContentAlias contentAlias in content.ContentAliases)
						{
							newContent.ContentAliases.Add(await ContentAliasController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(newContent, contentAlias.Alias));
						}
					}
				}
				method.End();
				//method.WaitForOnAfterCompleted();
			}

			newContent = (await GetByIdAsync(new ContentOptions()
			{
				ContentIds = new string[] { newContent.Id }.ToList(),
				FillFields = true,
				LoadAuthor = true,
				FillMetaData = true,
				Lcid = newContent.LCID
			})).FirstOrDefault();

			return newContent;
		}

		public virtual async Task<T> ApproveRejectAsync(T content)
		{
			await AuthenticateAndAuthorizeAsync();
			T c = null;
			using (Method method = new Method())
			{
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.ApproveReject.GetIntValue();
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentId.GetIntValue()) { Value = content.Id });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = content.LCID });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.DateCreated.GetIntValue()) { Value = content.DateCreated });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ApprovalPending.GetIntValue()) { Value = content.ApprovalPending });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.IsPublished.GetIntValue()) { Value = content.IsPublished });
				method.ClearCache = true;

				c = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
				method.End();
			}
			return c;
		}

		public virtual async Task<bool> DeleteByAllAsync(T content)
		{
			await AuthenticateAndAuthorizeAsync();
			bool success, succesForPermissions;

			/*using (Method method = new Method())
			{
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.RWDPermission;
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Methods.ContentUserPermissions_DeleteByContent.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.ContentId.GetIntValue()) { Value = content.Id });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.ContentDateCreated.GetIntValue()) { Value = content.DateCreated });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.ContentLCID.GetIntValue()) { Value = content.LCID });
				succesForPermissions = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
				method.End();
				//method.WaitForOnAfterCompleted();
			}*/



			using (Method method = new Method())
			{
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.DeleteByAll.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentId.GetIntValue()) { Value = content.Id });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = content.LCID });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.DateCreated.GetIntValue()) { Value = content.DateCreated });
				method.ClearCache = true;


				success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
				method.End();
				//method.WaitForOnAfterCompleted();
			}
			return success;
		}

		public virtual async Task<bool> DeleteAsync(T obj)
		{
			await AuthenticateAndAuthorizeAsync();
			bool success, succesForPermissions;

			/*using (Method method = new Method())
			{
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.RWDPermission;
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Methods.ContentUserPermissions_DeleteByContent.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.ContentId.GetIntValue()) { Value = obj.Id });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.ContentDateCreated.GetIntValue()) { Value = obj.DateCreated });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.ContentLCID.GetIntValue()) { Value = obj.LCID });
				succesForPermissions = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
				method.End();
				//method.WaitForOnAfterCompleted();
			}*/



			using (Method method = new Method())
			{
				method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
				method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.Delete.GetIntValue();
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentId.GetIntValue()) { Value = obj.Id });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
				method.ClearCache = true;

				success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
				method.End();
				//method.WaitForOnAfterCompleted();
			}
			return success;
		}

		public virtual async Task<T> TranslateAsync(T source, Culture targetCulture)
		{
			await AuthenticateAndAuthorizeAsync();
			Culture sourceCulture = await CultureController.GetNewInstance().Caller(User.SystemUser()).GetByLCIDAsync(source.LCID, true);
			T result = source;

			GoogleTranslationEntity translatedText = GoogleController.GetNewInstance().TranslateText(sourceCulture, targetCulture, result.Title, result.Html);
			if (translatedText.data.translations.Count == 2)
			{
				result.Title = translatedText.data.translations[0].translatedText;
				result.Html = translatedText.data.translations[1].translatedText;
			}
			if (result.ContentType != null && result.ContentType.Fields != null)
			{
				translatedText = GoogleController.GetNewInstance().TranslateText(sourceCulture, targetCulture, result.ContentType.Fields.Select(f => f.Value).ToArray());
				if (result.ContentType.Fields.Count == translatedText.data.translations.Count)
				{
					for (int i = 0; i < result.ContentType.Fields.Count; i++)
					{
						result.ContentType.Fields[i].Value = translatedText.data.translations[i].translatedText;
					}
				}
			}
			if (result.MetaDataFieldValues != null)
			{
				translatedText = GoogleController.GetNewInstance().TranslateText(sourceCulture, targetCulture, result.ContentType.Fields.Select(f => f.Value).ToArray());
				if (result.ContentType.Fields.Count == translatedText.data.translations.Count)
				{
					for (int i = 0; i < result.MetaDataFieldValues.Count; i++)
					{
						result.MetaDataFieldValues[i].Value = translatedText.data.translations[i].translatedText;
					}
				}
			}
			return result;
		}

		public virtual async Task<List<T>> GetByTaxonomyIdAsync(long id, int lcid = default(int))
		{
			await AuthenticateAndAuthorizeAsync();
			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}

			return (await Execute(new ContentRequestOptions()
			{
				TaxonomyId = id,
				Lcid = lcid,
				OnlyPublished = _getOnlyPublished,
				MaxNumberOfRows = int.MaxValue,
				FillFields = true
			})).Items;
			
			/*Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByTaxonomyId.GetIntValue();
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByTaxonomyIdOnlyPublished.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.TaxonomyId.GetIntValue()) { Value = id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
			DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(results.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row, loadAuthor: true, fillFields: true));
			}));

			return contents.ToList();*/
		}

		public virtual async Task<List<T>> GetByTaxonomyAsync(Taxonomy obj)
		{
			await AuthenticateAndAuthorizeAsync();

			return (await Execute(new ContentRequestOptions()
			{
				TaxonomyId = obj.Id,
				Lcid = obj.LCID,
				OnlyPublished = _getOnlyPublished,
				MaxNumberOfRows = int.MaxValue
			})).Items;

			/*Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByTaxonomyId.GetIntValue();
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByTaxonomyIdOnlyPublished.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.TaxonomyId.GetIntValue()) { Value = obj.Id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "GroupId_s" });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
			DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(results.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row));
			}));

			return contents.ToList();*/
		}

		public virtual Task<List<T>> GetByMenuAsync(Menu obj)
		{
			//await AuthenticateAndAuthorizeAsync();

			return MenuContentGetContentByMenuAsync(obj);
			
			/*Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByMenuIdAndLcid.GetIntValue();
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByMenuIdOnlyPublished.GetIntValue();
			}
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.MenuId.GetIntValue()) { Value = obj.Id });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });

			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

			DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

			ConcurrentQueue<T> contents = new ConcurrentQueue<T>();
			await Task.WhenAll(results.AsEnumerable().Select(async row => {
				contents.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row));
			}));

			return contents.ToList();*/
		}

		public virtual async Task<Content> GetByAliasAsync(string alias, bool loadAuthor = false, int lcid = default(int), bool fillFields = true, bool fillMetaDataFields = false, bool useDefaultPlugin = false)
		{
			await AuthenticateAndAuthorizeAsync();
			if (lcid.Equals(default(int)))
			{
				lcid = Settings.Default.DefaultLcid;
			}

			alias = alias.Length > 1 && alias.StartsWith("/") ? alias.Substring(1) : alias;

			return (await Execute(new ContentRequestOptions()
			{
				Alias = alias,
				Lcid = lcid,
				OnlyPublished = _getOnlyPublished,
				MaxNumberOfRows = int.MaxValue,
				FillFields = fillFields,
				FillMetaData = fillMetaDataFields,
				LoadAuthor = loadAuthor
			})).Items.FirstOrDefault();

			/*Method method = new Method();
			method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;

			//there might be issue because mysql plugin doesn't have GetByAliasOnlyPublished method
			//and it was necesery to make both methods on solr plugin
			//this will be ok if flow goes naturally to solr plugin if mysql doesn't have some of the methods
			//otherwise check current plugin too in following condition
			//or make double methods on mysql plugins too and change procedures accordingly
			if (_getOnlyPublished == true)
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByAliasOnlyPublished.GetIntValue();
			}
			else
			{
				method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByAlias.GetIntValue();
			}

			method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByAlias.GetIntValue();
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.Alias.GetIntValue()) { Value = alias.Length > 1 && alias.StartsWith("/") ? alias.Substring(1) : alias });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = lcid });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = _getOnlyPublished });
			//new way for group by
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
			method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
			//end
			return await CreateAsync(
				row: await ExecuteMethodRowAsync(method, this.UseDefaultPlugin),
				loadAuthor: loadAuthor,
				fillFields: fillFields,
				fillMetaDataFields: fillMetaDataFields
				);*/
		}
	}
}
