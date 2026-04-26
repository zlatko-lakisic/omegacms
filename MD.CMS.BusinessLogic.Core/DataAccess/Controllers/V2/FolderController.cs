using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Data;
using System.Linq;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;
using System;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.Collections;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class FolderController<T> : BaseController<FolderController<T>>
        where T : Content, new()
    {
        public async Task<Folder<T>> CreateAsync(DataRow row, IFolderRequestOptions options)
        {
            Folder<T> obj = base.Create<Folder<T>, long>(row, MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.FolderId);
            if (obj != null)
            {

                obj.Name = row.GetValue<string>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.Name);
                obj.Description = row.GetValue<string>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.Description);
                obj.FolderPath = row.GetValue<string>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.FolderPath);
                obj.ParentId = row.GetValue<long>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.ParentId);
                obj.Inherit = row.GetValue<bool>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.Inherit);
                if (options.FillContentTypeDefinitions)
                {
                    obj.ContentTypeDefinitions = await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAsync<T, ContentTypeDefinitionField>(obj);
                }
                if (options.FillContents)
                {
                    options.ContentRequestOptions.FolderId = obj.Id;
                    if (options.ContentRequestOptions.MaxNumberOfRows.Equals(default))
                    {
                        options.ContentRequestOptions.MaxNumberOfRows = 10;
                    }

                    ContentTypeDefinition<ContentTypeDefinitionField> type = (await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAsync<T, ContentTypeDefinitionField>(obj)).FirstOrDefault();

                    options.ContentRequestOptions.DataBound = type != null && type.Fields.Any(field => field.DataBound);

                    Entities.Base.BasePaginationEntity<T> contents = await DataBoundContentController<T>.GetNewInstance().DefaultPlugin(UseDefaultPlugin).Caller(UserMakingTheCall).Execute(options.ContentRequestOptions);
                    obj.Contents = contents.Items;
                    obj.ContentsTotalCount = contents.TotalCount;
                }
                if (options.FillChildren)
                {
                    options.ChildFolderRequestOptions.ParentId = obj.Id;
                    if (options.ChildFolderRequestOptions.MaxNumberOfRows.Equals(default))
                    {
                        options.ChildFolderRequestOptions.MaxNumberOfRows = 10;
                    }
                    Entities.Base.BasePaginationEntity<Folder<T>> children = await FolderController<T>.GetNewInstance().DefaultPlugin(UseDefaultPlugin).Caller(UserMakingTheCall).Execute(options.ChildFolderRequestOptions);
                    obj.Children = children.Items;
                    obj.ChildrenTotalCount = children.TotalCount;
                }
                if (options.FillAllParents && !obj.ParentId.Equals(default))
                {
                    options.ParentFolderRequestOptions.FolderIds = new long[] { obj.ParentId }.ToList();
                    options.ParentFolderRequestOptions.FillAllParents = options.FillAllParents;
                    obj.Parent = (await FolderController<T>.GetNewInstance().DefaultPlugin(UseDefaultPlugin).Caller(UserMakingTheCall).Execute(options.ParentFolderRequestOptions)).Items.FirstOrDefault();

                    obj.EntityPath = obj.Id.ToString();

                    if (obj.Parent != null)
                    {
                        obj.EntityPath = string.Format("{0}_{1}", obj.Parent.EntityPath, obj.Id);
                    }
                }
                if (options.FillTemplates)
                {
                    obj.Templates = await TemplateController<T>.GetNewInstance().DefaultPlugin(UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAsync(obj);
                }
            }
            return obj;
        }

        public async Task<Entities.Base.BasePaginationEntity<Folder<T>>> Execute(IFolderRequestOptions options)
        {
            await AuthenticateAndAuthorizeAsync();
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            DataTable resultTable = new DataTable();
            MdConcurrentOrderedQueue<Folder<T>> resultList = new MdConcurrentOrderedQueue<Folder<T>>();

            using (Method method = new Method())
            {
                try
                {
                    if (options.FolderIds.Any())
                    {
                        if (options.FolderIds.Count() == 1)
                        {
                            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.GetById.GetIntValue();
                            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.FolderId.GetIntValue()) { Value = options.FolderIds.First() });
                            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.ReadSingle;
                        }
                    }
                    else if (options.Paths.Any())
                    {
                        if (options.Paths.Count() == 1)
                        {
                            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.GetFolderByPath.GetIntValue();
                            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.FolderPath.GetIntValue()) { Value = options.Paths.First() });
                            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.ReadSingle;
                        }
                    }
                    else if (options.ParentId != null && options.ParentId.HasValue)
                    {
                        method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.SelectByParentIdWithPagination.GetIntValue();
                        method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.ParentId.GetIntValue()) { Value = options.ParentId });
                    }

                    method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;

                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.SearchTerm.GetIntValue()) { Value = !string.IsNullOrEmpty(options.SearchTerm) ? options.SearchTerm : string.Empty });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = options.CurrentPageIndex });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = options.MaxNumberOfRows });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = $"{options.SortField.GetStringValue()} {options.SortDirection.GetStringValue()}" });

                    if (method.Id.Equals(default))
                    {
                        throw new Exception("Folder method not found!");
                    }

                    switch ((Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods)method.Id)
                    {
                        case Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.GetById:
                        case Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.GetFolderByPath:
                            resultTable = (await ExecuteMethodRowAsync(method, this.UseDefaultPlugin)).Table;
                            break;
                        default:
                            resultTable = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
                            break;
                    }

                    await Task.WhenAll(resultTable.AsEnumerable().Select(async row =>
                    {
                        resultList.Enqueue(await FolderController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row, options));
                    }));
                }
                catch (NotImplementedException exception)
                {
                    typeof(FolderController<T>).Log(exception);
                }
                catch (Exception exception)
                {
                    typeof(FolderController<T>).Log(exception);
                }
            }

            Entities.Base.BasePaginationEntity<Folder<T>> basePaginationEntity = new Entities.Base.BasePaginationEntity<Folder<T>>();
            basePaginationEntity.Items = resultList.ToList();
            if (resultTable.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = resultTable.Rows[0].GetValue<int>("TotalCount");
            }

            if(basePaginationEntity.TotalCount.Equals(default) && !basePaginationEntity.Items.Count.Equals(default))
            {
                basePaginationEntity.TotalCount = basePaginationEntity.Items.Count;
            }

            return basePaginationEntity;
        }
    }
}
