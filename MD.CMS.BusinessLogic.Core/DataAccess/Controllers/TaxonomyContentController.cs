using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Globalization;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class TaxonomyContentController : BaseController<TaxonomyContentController>
    {
        public TaxonomyContent Create(DataRow row)
        {
            TaxonomyContent obj = base.Create<TaxonomyContent, long>(row, TaxonomyContentParamatersEnum.ContentId.GetStringValue());
            if (obj != null)
            {
                obj.LCID = row.GetValue<int>(TaxonomyContentParamatersEnum.LCID.GetStringValue());
                obj.DateCreated = row.GetValue<DateTime>(TaxonomyContentParamatersEnum.DateCreated.GetStringValue()).ToString(CultureInfo.InvariantCulture);
                obj.TaxonomyId = row.GetValue<long>(TaxonomyContentParamatersEnum.TaxonomyId.GetStringValue());
                obj.Title = row.GetValue<string>(TaxonomyContentParamatersEnum.Title.GetStringValue());
                obj.Alias = row.GetValue<string>("Alias");
                obj.Path = row.GetValue<string>(TaxonomyContentParamatersEnum.folderpath.GetStringValue());
                obj.Type = row.GetValue<string>(TaxonomyContentParamatersEnum.type.GetStringValue());
            }
            return obj;
        }

        public async Task<TaxonomyContent> SaveAsync(Content obj, Taxonomy taxonomy,int order)
        {
            await AuthenticateAndAuthorizeAsync();
            TaxonomyContent result = null;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.ContentId.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomy.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.Order.GetIntValue()) { Value = order});

                method.ClearCache = true;

                result = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
           //     method.End();
           //     method.WaitForOnAfterCompleted();
            }
            return result;
        }


        public async Task<TaxonomyContent> UpdateAsync(Content obj, Taxonomy taxonomy, int order)
        {
            await AuthenticateAndAuthorizeAsync();
            TaxonomyContent result = null;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.Update.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.ContentId.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomy.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.Order.GetIntValue()) { Value = order });

                method.ClearCache = true;

                result = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                //     method.End();
                //     method.WaitForOnAfterCompleted();
            }
            return result;
        }



        public async Task<bool> DeleteTaxonomyAsync(Content obj, Taxonomy taxonomy)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;

            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.ContentId.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomy.Id });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();

            }
            return success;

        }



        public async Task SaveTaxonomyContentAsync(Taxonomy taxonomy, Content content)
        {

            /*ExecuteCommand(GenerateStoredProcedure(TaxonomyContentSPEnum.Insert.GetStringValue(),
                                                new MySqlParameter() { ParameterName = TaxonomyContentEnum.ContentId.GetStringValue(), DbType = DbType.Int64, Value = content.Id },
                                                new MySqlParameter() { ParameterName = TaxonomyContentEnum.LCID.GetStringValue(), DbType = DbType.String, Value = content.LCID },
                                                new MySqlParameter() { ParameterName = TaxonomyContentEnum.DateCreated.GetStringValue(), DbType = DbType.DateTime, Value = content.DateCreated },
                                                new MySqlParameter() { ParameterName = TaxonomyContentEnum.TaxonomyId.GetStringValue(), DbType = DbType.Int64, Value = taxonomy.Id }));*/
        }



        public async Task<List<TaxonomyContent>> GetByTaxonomyIdAsync(long id, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            List<TaxonomyContent> contents = new List<TaxonomyContent>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.GetByTaxonomyId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.TaxonomyId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    TaxonomyContent obj = Create(row);
                    contents.Add(obj);
                }
            }
            return contents;
        }

        public async Task<int> GetByTaxonomyIdCountAsync(long taxonomyId, string searchTerm, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.GetByTaxonomyIdCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomyId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("TaxonomyContentCount");
            return count;
        }

        public async Task<Entities.Base.BasePaginationEntity<TaxonomyContent>> GetByTaxonomyIdWithPaginationAsync(long id, long currentPageIndex, long maxNumberOfRows, string searchTerm, int lcid = default(int), string sort = "Order ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            if (searchTerm == null)
            {
                searchTerm = "";
            }

            List<TaxonomyContent> taxonomyContents = new List<TaxonomyContent>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.GetByTaxonomyIdWithPagination.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.TaxonomyId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            if (table.Rows.Count != 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    TaxonomyContent taxonomyContent = Create(row);
                    taxonomyContents.Add(taxonomyContent);
                }
            }
            Entities.Base.BasePaginationEntity<TaxonomyContent> basePaginationEntity = new Entities.Base.BasePaginationEntity<TaxonomyContent>();
            basePaginationEntity.Items = taxonomyContents;
            if (table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntity;
        }

        public async Task<List<TaxonomyContent>> SearchAsync(string searchTerm, long parentId, int lcid)
        {
            await AuthenticateAndAuthorizeAsync();
            searchTerm = searchTerm.Replace("'", "''");


            List<TaxonomyContent> taxonomyContents = new List<TaxonomyContent>();

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.Search.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.TaxonomyId.GetIntValue()) { Value = parentId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.LCID.GetIntValue()) { Value = lcid });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            foreach (DataRow row in results.Rows)
            {
                taxonomyContents.Add(Create(row));
            }
            return taxonomyContents;
        }

        public async Task<bool> DeleteAsync(TaxonomyContent taxonomyContent)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success=false;
              using (Method method = new Method())
              {
                  method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                  method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
                  method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.Delete.GetIntValue();
                  method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.ContentId.GetIntValue()) { Value = taxonomyContent.Id });
                  method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.LCID.GetIntValue()) { Value = taxonomyContent.LCID });
                  method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.DateCreated.GetIntValue()) { Value = taxonomyContent.DateCreated });
                  method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomyContent.TaxonomyId });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                  method.End();
                  //method.WaitForOnAfterCompleted();
              }
        

            return success;
        }

        public async Task<bool> DeleteAllByTaxonomyIdAsync(Taxonomy taxonomy)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success=false;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.DeleteAllByTaxonomyId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomy.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<bool> DeleteContentTaxonomyAsync(int id, int id2)
        {
            /*bool success = (ExecuteCommand(GenerateStoredProcedure("Taxonomycontent_Delete",
                 new MySqlParameter() { ParameterName = "_ContentId", DbType = DbType.Int64, Value = id },
                 new MySqlParameter() { ParameterName = "_taxonomyId", DbType = DbType.Int64, Value = id2 })));
            return success;*/
            return true;
        }

        public async Task<List<TaxonomyContent>> GetByContentAsync(Content obj)
        {
            await AuthenticateAndAuthorizeAsync();
            List<TaxonomyContent> folders = new List<TaxonomyContent>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.TaxonomyContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods.GetByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.Id.GetIntValue()) { Value = obj.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                TaxonomyContent tax = Create(row);
                folders.Add(tax);
            }

            return folders;
        }
    }
}
