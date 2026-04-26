using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Threading.Tasks;
using MD.Tools.Helpers.Core.Collections;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Logging;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentController<T, SingletonType> : BaseController<SingletonType>
        where T : Content, new()
        where SingletonType : class, Tools.BaseDataAccess.Core.Interfaces.IBaseControllerSettings, new()
	{
        public virtual async Task<Entities.Base.BasePaginationEntity<T>> Execute(IContentRequestOptions options)
		{
			await AuthenticateAndAuthorizeAsync();
			if (options is null)
			{
				throw new ArgumentNullException(nameof(options));
			}
			DataTable resultTable = new DataTable();
			MdConcurrentOrderedQueue<T> resultList = new MdConcurrentOrderedQueue<T>();

			using (Method method = new Method())
			{
				try
				{
					method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.ReadMultiple;
					method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Content;
					if (options.ContentIds.Any())
					{
						method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByIds.GetIntValue();
						if (options.OnlyPublished)
						{
							method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByIdsOnlyPublished.GetIntValue();
						}
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.ContentIds.GetIntValue()) { Value = MethodProperty.ArrayToValue(options.ContentIds.ToArray()) });
					}
					else if (!options.FolderId.Equals(default))
					{
						method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderIdWithPagination.GetIntValue();
						if (options.OnlyPublished)
						{
							//method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByFolderIdWithPaginationOnlyPublished.GetIntValue();
						}
						method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.ReadList;
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.FolderId.GetIntValue()) { Value = options.FolderId });
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
					}
					else if (!string.IsNullOrEmpty(options.SearchTerm) && !string.IsNullOrWhiteSpace(options.SearchTerm))
					{
						method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.Search.GetIntValue();
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
					}
					else if (!options.TaxonomyId.Equals(default))
					{
						method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByTaxonomyId.GetIntValue();
						if (options.OnlyPublished)
						{
							method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByTaxonomyIdOnlyPublished.GetIntValue();
						}
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.TaxonomyId.GetIntValue()) { Value = options.TaxonomyId });
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
					}
					else if (!options.MenuId.Equals(default))
					{
						method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByMenuIdAndLcid.GetIntValue();
						if (options.OnlyPublished)
						{
							method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByMenuIdAndLcidOnlyPublished.GetIntValue();
						}
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.MenuId.GetIntValue()) { Value = options.MenuId });
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
					}
					else if (!string.IsNullOrEmpty(options.Alias) && !string.IsNullOrWhiteSpace(options.Alias))
					{
						method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByAlias.GetIntValue();
						if (options.OnlyPublished)
						{
							method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByAliasOnlyPublished.GetIntValue();
						}
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
						method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
					}

					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.SearchTerm.GetIntValue()) { Value = !string.IsNullOrEmpty(options.SearchTerm) ? options.SearchTerm : string.Empty });
					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = options.CurrentPageIndex });
					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = options.MaxNumberOfRows });
					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = $"{options.SortField.GetStringValue()} {options.SortDirection.GetStringValue()}" });

					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.LCID.GetIntValue()) { Value = options.Lcid });
					method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Parameters.OnlyPublished.GetIntValue()) { Value = options.OnlyPublished });

					if (method.Id.Equals(default))
					{
						throw new Exception("Content method not found!");
					}


					switch ((Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods)method.Id)
					{
						case Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods.GetByAlias:
							resultTable = (await ExecuteMethodRowAsync(method, this.UseDefaultPlugin)).Table;
							break;
						default:
							resultTable = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
							break;
					}

					await Task.WhenAll(resultTable.AsEnumerable().Select(async row =>
					{
						resultList.Enqueue(await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row,
							loadAuthor: options.LoadAuthor,
							fillFields: options.FillFields,
							fillMetaDataFields: options.FillMetaData));
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

			Entities.Base.BasePaginationEntity<T> basePaginationEntity = new Entities.Base.BasePaginationEntity<T>();
			basePaginationEntity.Items = resultList.ToList();
			if (resultTable.Rows.Count > 0)
			{
				basePaginationEntity.TotalCount = resultTable.Rows[0].GetValue<int>("TotalCount");
			}

			if (basePaginationEntity.TotalCount.Equals(default) && !basePaginationEntity.Items.Count.Equals(default))
			{
				basePaginationEntity.TotalCount = basePaginationEntity.Items.Count;
			}
			return basePaginationEntity;
		}
	}
}
