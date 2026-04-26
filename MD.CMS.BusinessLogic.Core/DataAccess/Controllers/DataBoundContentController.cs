using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSorting;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using MD.Tools.Helpers.Core.Data;
using System.Threading.Tasks;
using MD.Tools.Helpers.Core.TypeConversion;
using MD.CMS.BusinessLogic.Core.Helpers.Calculations;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class DataBoundContentController<T> : ContentController<T, DataBoundContentController<T>>, Tools.BaseDataAccess.Core.Interfaces.IBaseControllerSettings
        where T : Content, new()
    {
        private bool _getOnlyPublished;

        public async Task<T> CreateAsync(DataRow row, ContentTypeDefinition<ContentTypeDefinitionFieldValue> dataBoundContentType = null)
        {
            T obj = await base.CreateAsync(row);
            if (obj != null)
            {
                obj.ContentType = dataBoundContentType;
                obj.ContentTypeDefinitionId = dataBoundContentType.Id;

                foreach (ContentTypeDefinitionFieldValue field in obj.ContentType.Fields)
                {
                    field.JsonField.enabled = false;
                    string fieldLookup = field.DataSourceField.Replace($"{field.DataSourceField.Split('.').First()}.", string.Empty);
                    field.Value = row.GetValue<string>(fieldLookup, string.Empty);
                    if (string.Compare(ContentEnum.ContentId.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.Id = row.GetValue<string>(fieldLookup);
                    }
                    if (string.Compare(ContentEnum.IsPublished.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.IsPublished = row.GetValue<bool>(fieldLookup);
                    }
                    if (string.Compare(ContentEnum.LCID.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.LCID = row.GetValue<int>(fieldLookup);
                    }
                    if (field.AttributeTypeDefinitionId == 10)
                    {
                        long dateValueNumber = field.Value.ToInt64(default);
                        if (!dateValueNumber.Equals(default))
                        {
                            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                            field.Value = start.AddMilliseconds(dateValueNumber).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            field.Value = row.GetValue<DateTime>(fieldLookup).ToString();
                        }
                    }
                    if (string.Compare(ContentEnum.DateCreated.GetStringValue(), field.Name, true).Equals(0))
                    {
                        string dateValue = row.GetValue<string>(fieldLookup);
                        long dateValueNumber = dateValue.ToInt64(default);
                        if (!dateValueNumber.Equals(default))
                        {
                            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                            obj.DateCreated = start.AddMilliseconds(dateValueNumber).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            obj.DateCreated = row.GetValue<DateTime>(fieldLookup).ToString();
                        }
                    }
                    if (string.Compare(ContentEnum.AuthorId.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.AuthorId = row.GetValue<string>(fieldLookup);
                    }
                    if (string.Compare(ContentEnum.FolderId.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.FolderId = row.GetValue<long>(fieldLookup);
                    }
                    if (string.Compare(ContentEnum.Title.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.Title = row.GetValue<string>(fieldLookup);
                    }
                    if (string.Compare(ContentEnum.Folderpath.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.Path = row.GetValue<string>(fieldLookup);
                    }
                    if (string.Compare(ContentEnum.Html.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.Html = row.GetValue<string>(fieldLookup);
                    }
                    if (string.Compare(ContentEnum.TaxonomyId.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.TaxonomyId = row.GetValue<long>(fieldLookup);
                    }
                    if (string.Compare(ContentEnum.ApprovalPending.GetStringValue(), field.Name, true).Equals(0))
                    {
                        obj.ApprovalPending = row.GetValue<bool>(fieldLookup);
                    }
                }

                List<Task> tasksToExecute = new List<Task>();
                foreach (ContentTypeDefinitionFieldValue field in obj.ContentType.Fields)
                {
                    if (field.AttributeTypeDefinitionId == 17)
                    {
                        tasksToExecute.Add(PostfixEvaluator.EvaluateAsync(UserMakingTheCall, obj.ContentType.Fields, field, field.DefaultValue));
                    }
                }
                Task.WaitAll(tasksToExecute.ToArray());

                LinkFields(obj);
            }
            return obj;
        }

        public async Task<T> GetByIdAsync(string id, long contentTypeId = default(long), IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> extraConditions = null)
        {
            await AuthenticateAndAuthorizeAsync();
            ContentTypeDefinition<ContentTypeDefinitionFieldValue> type = await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync<ContentTypeDefinitionFieldValue>(contentTypeId);

            DataTable table = new DataTable();
            DataSet set = new DataSet();
            IEnumerable<IGrouping<long, BaseDataBindableField>> dataBoundFields = type.Fields.Where(field => field.DataBound).GroupBy(field => field.DataSourceId);
            foreach (IGrouping<long, BaseDataBindableField> group in dataBoundFields)
            {
                ContentTypeDataSource dataSource = type.DataSources.First(ds => ds.Id.Equals(group.Key));
                DataBoundMethod method = new DataBoundMethod(dataSource.DbType, dataSource.ConnectionString, group.Select(field => field.DataSourceField));
                method.MethodType = MethodTypes.Read;
                method.Conditions.Add(new ContentTypeDefinitionFolderDataBoundCondition()
                {
                    Comparer = ComparerTypeEnum.Equals,
                    ContentTypeDefinitionId = type.Id,
                    LeftField = group.First(field => field.IsDataBoundPrimaryKey).DataSourceField,
                    Value = id
                });
                if (extraConditions != null && extraConditions.Any())
                {
                    method.Conditions.AddRange(extraConditions);
                }
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
                row: table.AsEnumerable().FirstOrDefault(),
                dataBoundContentType: type
                );
        }

        public override async Task<BasePaginationEntity<T>> GetByFolderIdWithPaginationAsync(long id, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", bool loadAuthor = false, int lcid = 0, string sort = "Title ASC", bool loadFields = false)
        {
            return await GetByFolderIdWithPaginationAsync(id, currentPageIndex, maxNumberOfRows, searchTerm, sort);
        }

        public async Task<BasePaginationEntity<T>> GetByFolderIdWithPaginationAsync(long id, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", string sort = "Title ASC")
        {
            return await GetByFolderWithPaginationAsync(await FolderController<T>.GetNewInstance().Caller(UserMakingTheCall).GetByIdAsync(id), currentPageIndex, maxNumberOfRows, searchTerm, sort);
        }

        public async Task<BasePaginationEntity<T>> GetByFolderWithPaginationAsync(Folder<T> folder, int currentPageIndex, int maxNumberOfRows, string searchTerm = "", string sort = "Title ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            if (string.IsNullOrEmpty(sort))
            {
                sort = "Title ASC";
            }
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

            ContentTypeDefinition<ContentTypeDefinitionFieldValue> type = (await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAsync<T, ContentTypeDefinitionFieldValue>(folder)).FirstOrDefault();

            return await Execute(new DataBoundContentRequestOptions()
            {
                FolderId = folder.Id,
                CurrentPageIndex = currentPageIndex,
                MaxNumberOfRows = maxNumberOfRows,
                SearchTerm = searchTerm,
                SortField = EnumExtensions.GetByStringValue<ContentEnum>(string.Join(' ', sort.Split(' ').Take(sort.Split(' ').Length - 1))),
                SortDirection = EnumExtensions.GetByStringValue<SortDirection>(sort.Split(' ').LastOrDefault()),
                ContentTypeId = type.Id,
                DataBoundConditions = await ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAndContentTypeDefinitionIdAsync(folder.Id, type.Id)
            });

            /*return await GetByDataBoundContentRequest(new DataBoundContentRequestOptions { 
                ContentTypeId = type.Id,
                FolderId = folder.Id,
                CurrentPageIndex = currentPageIndex,
                MaxNumberOfRows = maxNumberOfRows,
                SearchTerm = searchTerm,
                SortField = sort.Split(' ').First().GetEnumByStringValue<ContentEnum>(),
                SortDirection = sort.Split(' ').Last().GetEnumByStringValue<SortDirection>(),
                DataBoundConditions = await ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAndContentTypeDefinitionIdAsync(folder.Id, type.Id)
            });*/

            /*DataTable table = new DataTable();

            ContentTypeDefinition<ContentTypeDefinitionFieldValue> type = (await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAsync<T, ContentTypeDefinitionFieldValue>(folder)).FirstOrDefault();

            DataSet set = new DataSet();
            IEnumerable<IGrouping<long, ContentTypeDefinitionFieldValue>> dataBoundFields = type.Fields.Where(field => field.DataBound).GroupBy(field => field.DataSourceId);
            IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> conditions = await ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAndContentTypeDefinitionIdAsync(folder.Id, type.Id);

            foreach (IGrouping<long, ContentTypeDefinitionFieldValue> group in dataBoundFields)
            {
                ContentTypeDataSource dataSource = type.DataSources.First(ds => ds.Id.Equals(group.Key));
                DataBoundMethod method = new DataBoundMethod(dataSource.DbType, dataSource.ConnectionString, group.Select(field => field.DataSourceField));
                method.MethodType = MethodTypes.Read;
                conditions = conditions.Where(condition =>
                {
                    return type.Fields.Where(field => field.Id == condition.ContentTypeDefinitionFieldId).Any();
                });
                method.Conditions = conditions.Select(condition =>
                {
                    condition.LeftField = !string.IsNullOrEmpty(condition.LeftField) ? condition.LeftField : type.Fields.FirstOrDefault(field => field.Id == condition.ContentTypeDefinitionFieldId).DataSourceField;
                    return condition as MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.ContentTypeDefinitionFolderDataBoundCondition;
                }).ToList();
                ContentTypeDefinitionFolderDataBoundSorting defaultSort = null;
                try
                {
                    ContentTypeDefinitionFieldValue defaultSortingField = group.FirstOrDefault(field => string.Compare(field.Name, sort.Split(' ').First(), true).Equals(0));
                    if (defaultSortingField != null)
                    {
                        defaultSort = new ContentTypeDefinitionFolderDataBoundSorting()
                        {
                            LeftField = defaultSortingField.DataSourceField,
                            Sorter = (SortType)Enum.Parse(typeof(SortType), sort.Split(' ').Last())
                        };
                    }
                }
                finally
                {
                    if (defaultSort != null)
                    {
                        method.Sorts.Add(defaultSort);
                    }
                }
                method.PagingEnabled = true;
                method.PagingFrom = currentPageIndex;
                method.PagingSize = maxNumberOfRows;
                set.Tables.Add(await ExecuteMethodTableAsync(method));
            }

            return await ParseDataSetAsync(new DataBoundContentRequestOptions { 
                CurrentPageIndex = currentPageIndex,
                MaxNumberOfRows = maxNumberOfRows,
                ContentTypeId = type.Id
            }, set);*/

            /*table.Columns.Add(ContentEnum.ContentId.GetStringValue());
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

            int maxRowsFound = 0;
            foreach (DataTable dataTable in set.Tables)
            {
                if (dataTable.Rows.Count > maxRowsFound)
                {
                    maxRowsFound = dataTable.Rows.Count;
                }
            }

            for (int i = currentPageIndex * maxNumberOfRows; i < ((currentPageIndex + 1) * maxNumberOfRows); i++)
            {
                DataRow row = table.NewRow();
                Random contentId = new Random();
                row[ContentEnum.ContentId.GetStringValue()] = contentId.Next(1, int.MaxValue);
                row[ContentEnum.IsPublished.GetStringValue()] = true;
                bool rowExists = false;
                foreach (DataColumn column in table.Columns)
                {
                    DataRow rowToCopyFrom = null;
                    foreach (DataTable dataTable in set.Tables)
                    {
                        if (dataTable.Columns.Contains(column.ColumnName) && i < dataTable.Rows.Count)
                        {
                            rowToCopyFrom = dataTable.Rows[i];
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
            }
            foreach (DataRow row in table.Rows)
            {
                contents.Add(await CreateAsync(row, dataBoundContentType: type));
            }
            Entities.Base.BasePaginationEntity<T> basePaginationEntity = new Entities.Base.BasePaginationEntity<T>();
            basePaginationEntity.Items = contents;
            basePaginationEntity.TotalCount = maxRowsFound;
            return basePaginationEntity;*/
        }

        public async Task<string> SaveWithReturnIdAsync(T content)
        {
            await AuthenticateAndAuthorizeAsync();
            DateTime date = new DateTime(0001, 1, 1);
            if (content.DateCreated == date.ToString())
            {
                content.DateCreated = DateTime.UtcNow.ToString();
            }

            string contentId = content.Id;

            foreach (ContentTypeDataSource dataSource in content.ContentType.DataSources)
            {
                IEnumerable<ContentTypeDefinitionFieldValue> fields = content.ContentType.Fields.Where(field => field.DataSourceId.Equals(dataSource.Id));
            }

            if (content.ContentType != null && content.ContentType.Id != 0)
            {
                MethodTypes methodType = MethodTypes.Create;

                ContentTypeDefinitionFieldValue primaryKeyValue = content.ContentType.Fields.FirstOrDefault(field => field.DataBound && field.IsDataBoundPrimaryKey);
                if (primaryKeyValue != null && !string.IsNullOrEmpty(primaryKeyValue.Value))
                {
                    methodType = MethodTypes.Update;
                }


                IEnumerable<IGrouping<long, ContentTypeDefinitionFieldValue>> dataBoundFields = content.ContentType.Fields.Where(field => field.DataBound).GroupBy(field => field.DataSourceId);
                foreach (IGrouping<long, ContentTypeDefinitionFieldValue> group in dataBoundFields)
                {
                    ContentTypeDataSource dataSource = content.ContentType.DataSources.First(ds => ds.Id.Equals(group.Key));

                    using (DataBoundMethod method = new DataBoundMethod(dataSource.DbType, dataSource.ConnectionString, group.Select(field => field.DataSourceField)))
                    {
                        method.FieldProperties.AddRange(group.Select(field => new MethodFieldProperty(field.DataSourceField, field.IsDataBoundPrimaryKey) { Value = field.Value }));
                        method.MethodType = methodType;

                        DataRow result = await ExecuteMethodRowAsync(method);
                        if (result.Table.Columns.Contains(primaryKeyValue.DataSourceField))
                        {
                            contentId = result[primaryKeyValue.DataSourceField].ToString();
                        }
                    }

                }
            }
            return contentId;
        }

        public override async Task<T> SaveAsync(T content)
        {
            await AuthenticateAndAuthorizeAsync();
            string id = await SaveWithReturnIdAsync(content);
            T newContent = await GetByIdAsync(id, content.ContentType.Id);
            return newContent;
        }

        public override async Task<bool> DeleteAsync(T obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = true;
            bool succesForPermissions = false;


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
			}*/


            foreach (ContentTypeDataSource dataSource in obj.ContentType.DataSources)
            {
                using (DataBoundMethod method = new DataBoundMethod(dataSource.DbType, dataSource.ConnectionString))
                {
                    method.MethodType = MethodTypes.Delete;
                    method.Conditions = (await ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAndContentTypeDefinitionIdAsync(obj.FolderId, obj.ContentType.Id)).Select(condition =>
                    {
                        condition.LeftField = obj.ContentType.Fields.FirstOrDefault(field => field.Id == condition.ContentTypeDefinitionFieldId).DataSourceField;
                        return condition as MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.ContentTypeDefinitionFolderDataBoundCondition;
                    }).ToList();
                    success = success && await ExecuteMethodBooleanAsync(method);
                }
            }

            return success;
        }
    }
}
