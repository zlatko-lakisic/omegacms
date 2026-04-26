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
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSorting;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options;
using MD.Tools.Helpers.Core.Collections;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class DataBoundContentController<T> : ContentController<T, DataBoundContentController<T>>, Tools.BaseDataAccess.Core.Interfaces.IBaseControllerSettings
        where T : Content, new()
    {
        public override Task<BasePaginationEntity<T>> Execute(IContentRequestOptions options)
        {
            if (!options.DataBound)
            {
                return base.Execute(options);
            }
            return Execute(new DataBoundContentRequestOptions(options));
        }

        public async Task<BasePaginationEntity<T>> Execute(IDataBoundContentRequestOptions options)
        {
            if (!options.DataBound)
            {
                return await base.Execute(options);
            }

            await AuthenticateAndAuthorizeAsync();
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ContentIds.Any())
            {
                ContentTypeDefinition<ContentTypeDefinitionFieldValue> type = null;
                if (!options.FolderId.Equals(default))
                {
                    Folder<T> folder = await FolderController<T>.GetNewInstance().Caller(UserMakingTheCall).GetByIdAsync(options.FolderId);
                    type = (await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAsync<T, ContentTypeDefinitionFieldValue>(folder)).FirstOrDefault();
                    IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> conditions = await ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAndContentTypeDefinitionIdAsync(options.FolderId, type.Id);

                    DataBoundContentRequestOptions opts = new DataBoundContentRequestOptions(options)
                    {
                        ContentTypeId = options.ContentTypeId
                    };

                    opts.DataBoundConditions.ToList().AddRange(options.ContentIds.Select(id =>
                    {
                        ContentTypeDefinitionFolderDataBoundCondition condition = conditions.FirstOrDefault(condition => condition.Type == Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.ContentTypeDefinitionFolderDataBoundCondition.ConditionType.PrimaryKey);

                        if (condition != null)
                        {
                            condition.Value = id;
                        }

                        return condition;
                    }));

                    return await GetData(opts, MethodTypes.ReadSingle);
                }

                type = await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync<ContentTypeDefinitionFieldValue>(options.ContentTypeId);
                IEnumerable<Folder<T>> folders = (await FolderController<T>.GetNewInstance().Caller(UserMakingTheCall).GetByParentIdAsync(0)).Where(folder => folder.ContentTypeDefinitions.Any(c => c.Id == type.Id));

                MdConcurrentOrderedQueue<BasePaginationEntity<T>> list = new MdConcurrentOrderedQueue<BasePaginationEntity<T>>();
                foreach(Folder<T> folder in folders)
                {
                    list.Enqueue(await GetData(new DataBoundContentRequestOptions(options)
                    {
                        ContentTypeId = options.ContentTypeId
                    }, MethodTypes.ReadSingle));
                }

                BasePaginationEntity<T> result = new BasePaginationEntity<T>();

                foreach(BasePaginationEntity<T> obj in list.ToList())
                {
                    result.Items.AddRange(obj.Items);
                    result.TotalCount += obj.TotalCount;
                }

                return result;
            }
            else if (!options.FolderId.Equals(default))
            {
                Folder<T> folder = await FolderController<T>.GetNewInstance().Caller(UserMakingTheCall).GetByIdAsync(options.FolderId);
                ContentTypeDefinition<ContentTypeDefinitionFieldValue> type = (await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAsync<T, ContentTypeDefinitionFieldValue>(folder)).FirstOrDefault();

                return await GetData(new DataBoundContentRequestOptions(options)
                {
                    ContentTypeId = type.Id,
                    DataBoundConditions = options.DataBoundConditions.Any() ? options.DataBoundConditions : await ContentTypeDefinitionFolderDataBoundConditionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAndContentTypeDefinitionIdAsync(options.FolderId, type.Id)
                }, MethodTypes.ReadMultiple);
            }

            return new BasePaginationEntity<T>();
        }

        private IEnumerable<IGrouping<long, ContentTypeDefinitionFieldValue>> GetDataBoundFieldGroups(ContentTypeDefinition<ContentTypeDefinitionFieldValue> type)
        {
            return type.Fields.Where(field => field.DataBound).GroupBy(field => field.DataSourceId);
        }

        private IEnumerable<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.ContentTypeDefinitionFolderDataBoundCondition> GetConditionsForDataBoundGroup(ContentTypeDefinition<ContentTypeDefinitionFieldValue> type, IDataBoundContentRequestOptions options)
        {
            IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> conditions = options.DataBoundConditions.Where(condition =>
            {
                return type.Fields.Where(field => field.Id == condition.ContentTypeDefinitionFieldId).Any();
            });
            return conditions.Select(condition =>
            {
                condition.LeftField = !string.IsNullOrEmpty(condition.LeftField) ? condition.LeftField : type.Fields.FirstOrDefault(field => field.Id == condition.ContentTypeDefinitionFieldId).DataSourceField;
                return condition as MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.ContentTypeDefinitionFolderDataBoundCondition;
            });
        }

        private DataBoundMethod GetDataBoundMethod(IGrouping<long, ContentTypeDefinitionFieldValue> group, ContentTypeDefinition<ContentTypeDefinitionFieldValue> type, IDataBoundContentRequestOptions options, MethodTypes methodType)
        {
            ContentTypeDataSource dataSource = type.DataSources.First(ds => ds.Id.Equals(group.Key));
            DataBoundMethod method = new DataBoundMethod(dataSource.DbType, dataSource.ConnectionString, group.Select(field => field.DataSourceField));
            method.MethodType = methodType;
            method.Conditions = GetConditionsForDataBoundGroup(type, options).ToList();
            ContentTypeDefinitionFolderDataBoundSorting defaultSort = null;
            try
            {
                ContentTypeDefinitionFieldValue defaultSortingField = group.FirstOrDefault(field => string.Compare(field.Name, options.SortField.GetStringValue(), true).Equals(0));
                if (defaultSortingField != null)
                {
                    defaultSort = new ContentTypeDefinitionFolderDataBoundSorting()
                    {
                        LeftField = defaultSortingField.DataSourceField,
                        Sorter = (SortType)Enum.Parse(typeof(SortType), options.SortDirection.GetStringValue())
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
            method.PagingFrom = options.CurrentPageIndex;
            method.PagingSize = options.MaxNumberOfRows;
            return method;
        }

        private async Task<DataSet> ExecuteMethods(IEnumerable<DataBoundMethod> methods)
        {
            DataSet set = new DataSet();
            foreach(DataBoundMethod method in methods)
            {
                set.Tables.Add(await ExecuteMethodTableAsync(method));
            }
            return set;
        }

        private async Task<BasePaginationEntity<T>> GetData(IDataBoundContentRequestOptions options, MethodTypes methodType)
        {
            ContentTypeDefinition<ContentTypeDefinitionFieldValue> type = await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync<ContentTypeDefinitionFieldValue>(options.ContentTypeId);
            List<T> contents = new List<T>();
            DataSet set = await ExecuteMethods(GetDataBoundFieldGroups(type).Select(group => GetDataBoundMethod(group, type, options, methodType)));

            DataTable table = new DataTable();
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

            int maxRowsFound = 0;
            foreach (DataTable dataTable in set.Tables)
            {
                if (dataTable.Rows.Count > maxRowsFound)
                {
                    maxRowsFound = dataTable.Rows.Count;
                }
            }

            for (int i = options.CurrentPageIndex * options.MaxNumberOfRows; i < ((options.CurrentPageIndex + 1) * options.MaxNumberOfRows); i++)
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
            BasePaginationEntity<T> basePaginationEntity = new BasePaginationEntity<T>();
            basePaginationEntity.Items = contents;
            basePaginationEntity.TotalCount = maxRowsFound;
            return basePaginationEntity;
        }

        /*private async Task<BasePaginationEntity<T>> GetByDataBoundContentRequest(IDataBoundContentRequestOptions options)
        {
            ContentTypeDefinition<ContentTypeDefinitionFieldValue> type = await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync<ContentTypeDefinitionFieldValue>(options.ContentTypeId);
            List<T> contents = new List<T>();

            DataSet set = new DataSet();
            IEnumerable<IGrouping<long, ContentTypeDefinitionFieldValue>> dataBoundFields = type.Fields.Where(field => field.DataBound).GroupBy(field => field.DataSourceId);
            IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> conditions = options.DataBoundConditions;

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
                    ContentTypeDefinitionFieldValue defaultSortingField = group.FirstOrDefault(field => string.Compare(field.Name, options.SortField.GetStringValue(), true).Equals(0));
                    if (defaultSortingField != null)
                    {
                        defaultSort = new ContentTypeDefinitionFolderDataBoundSorting()
                        {
                            LeftField = defaultSortingField.DataSourceField,
                            Sorter = (SortType)Enum.Parse(typeof(SortType), options.SortDirection.GetStringValue())
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
                method.PagingFrom = options.CurrentPageIndex;
                method.PagingSize = options.MaxNumberOfRows;
                set.Tables.Add(await ExecuteMethodTableAsync(method));
            }

            DataTable table = new DataTable();
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

            int maxRowsFound = 0;
            foreach (DataTable dataTable in set.Tables)
            {
                if (dataTable.Rows.Count > maxRowsFound)
                {
                    maxRowsFound = dataTable.Rows.Count;
                }
            }

            for (int i = options.CurrentPageIndex * options.MaxNumberOfRows; i < ((options.CurrentPageIndex + 1) * options.MaxNumberOfRows); i++)
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
            return basePaginationEntity;
        }*/
    }
}
