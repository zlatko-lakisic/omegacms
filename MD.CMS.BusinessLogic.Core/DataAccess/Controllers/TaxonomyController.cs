using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.StringExt;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class TaxonomyController : BaseController<TaxonomyController>
    {
        public async Task<Taxonomy> CreateAsync(DataRow row, bool fillParent = false, bool fillAllParents = false)
        {
            Taxonomy obj = base.Create<Taxonomy, long>(row, TaxonomyParamatersEnum.TaxonomyId.GetStringValue());
            if (obj != null)
            {
                obj.LCID = row.GetValue<int>(TaxonomyParamatersEnum.LCID.GetStringValue());
                obj.FolderId = row.GetValue<int>(TaxonomyParamatersEnum.FolderId.GetStringValue());
                obj.ParentId = row.GetValue<long>(TaxonomyParamatersEnum.ParentId.GetStringValue());
                obj.Name = row.GetValue<string>(TaxonomyParamatersEnum.Name.GetStringValue());
                obj.Description = row.GetValue<string>(TaxonomyParamatersEnum.Description.GetStringValue());
                obj.TaxonomyPath = row.GetValue<string>(TaxonomyParamatersEnum.TaxonomyPath.GetStringValue());
                obj.LCID = row.GetValue<int>(TaxonomyParamatersEnum.LCID.GetStringValue());
                obj.Order = row.GetValue<int>(TaxonomyParamatersEnum.Order.GetStringValue());
                if (fillParent)
                {
                    obj.Parent = await GetByIdAsync(obj.ParentId, fillAllParents);
                    obj.EntityPath = obj.Id.ToString();
                    if (obj.Parent != null)
                    {
                        obj.EntityPath = string.Format("{0}_{1}", obj.Parent.EntityPath, obj.Id);
                    }
                }
                
               // obj.Contents = ContentController<Content>.Instance.GetByTaxonomy(obj);
            }
            return obj;
        }

        public async Task<Taxonomy> GetByIdAsync(long id, bool fillParent = false)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.TaxonomyId.GetIntValue()) { Value = id });
            //return await CreateAsync(await ExecuteMethodRowAsync(method));
            return await CreateAsync(
                     await ExecuteMethodRowAsync(method, this.UseDefaultPlugin),
                     !id.Equals(default(long))
                 );

        }

        public async Task<Taxonomy> GetTaxonomyByPathAsync(string path = "", bool fillParent = false, bool fillAllParents = false, int lcid = 0)
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.GetTaxonomyByPath.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.TaxonomyPath.GetIntValue()) { Value = path });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = lcid });
            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), fillParent, fillAllParents);
        }

        public async Task<List<Taxonomy>> GetByContentIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Taxonomy> folders = new List<Taxonomy>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.GetByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ContentId.GetIntValue()) { Value = id });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                Taxonomy obj = await CreateAsync(row, false);
                folders.Add(obj);
            }

            return folders;
        }

        public async Task<List<Taxonomy>> GetByParentIdAsync(long id, int depth = 0, int lcid = 0, bool loadContents = false)
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }

            List<Taxonomy> taxonomies = new List<Taxonomy>();

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.GetByParentId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ParentId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = lcid });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Taxonomy obj = await CreateAsync(row, false);
                    if (depth > 0)
                    {
                        obj.Children = await GetByParentIdAsync(obj.Id, depth - 1, loadContents: loadContents);
                        if (loadContents)
                        {
                            obj.Contents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(UseDefaultPlugin).Caller(UserMakingTheCall).GetByTaxonomyIdAsync(obj.Id, lcid);
                        }
                    }
                    taxonomies.Add(obj);
                }
            }
            return taxonomies;
        }

        public async Task<Entities.Base.BasePaginationEntity<Taxonomy>> GetByParentIdWithPaginationAsync(long id, long pageIndex, long pageSize, string searchTerm, int depth = int.MaxValue, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }

            List<Taxonomy> taxonomies = new List<Taxonomy>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.GetByParentIdWithPagination.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ParentId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = pageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = pageSize });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "TaxonomyId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "Order_i asc" });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            if (table.Rows.Count != 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    Taxonomy obj = await CreateAsync(row, false);
                    if (depth > 0)
                    {
                        obj.Children = await GetByParentIdAsync(obj.Id, depth - 1);
                    }
                    taxonomies.Add(obj);
                }
            }
            Entities.Base.BasePaginationEntity<Taxonomy> basePaginationEntity = new Entities.Base.BasePaginationEntity<Taxonomy>();
            basePaginationEntity.Items = taxonomies;
            if (table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntity;
        }

        public async Task<List<Taxonomy>> SearchAsync(string searchTerm, long parentId, bool recursive)
        {
            await AuthenticateAndAuthorizeAsync();
            searchTerm = searchTerm.Replace("'", "''");


            List<Taxonomy> taxonomies = new List<Taxonomy>();

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.Search.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ParentId.GetIntValue()) { Value = parentId });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            foreach (DataRow row in results.Rows)
            {
                taxonomies.Add(await CreateAsync(row, false));
            }

            if (recursive)
            {
                List<Taxonomy> children = await GetByParentIdAsync(parentId);
                foreach (Taxonomy child in children)
                {
                    taxonomies = taxonomies.Concat(await SearchAsync(searchTerm, child.Id, true)).ToList();
                }
            }

            return taxonomies;
        }

        public async Task<int> GetByParentIdCountAsync(long parentId, int lcid, string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.GetByParentIdCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ParentId.GetIntValue()) { Value = parentId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("TaxonomyCount");
            return count;
        }

        public async Task<List<Taxonomy>> TaxonomyContentGetTaxonomyByContentAsync(Content content)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Taxonomy> contents = new List<Taxonomy>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.TaxonomyContentGetTaxonomyByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ContentId.GetIntValue()) { Value = content.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = content.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.DateCreated.GetIntValue()) { Value = content.DateCreated });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Taxonomy obj = await CreateAsync(row, false);
                    contents.Add(obj);
                }
            }
            return contents;
        }

        public async Task<List<Taxonomy>> GetByContentAsync(Content content, int depth = int.MaxValue)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Taxonomy> folders = new List<Taxonomy>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.GetByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ContentId.GetIntValue()) { Value = content.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = content.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.DateCreated.GetIntValue()) { Value = content.DateCreated });


            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in result.Rows)
            {
                Taxonomy obj = await CreateAsync(row, false);
                if (depth > 0)
                {
                    obj.Children = await GetByParentIdAsync(obj.Id, depth - 1);
                }
                folders.Add(obj);
            }
            return folders;
        }

        public async Task<List<Taxonomy>> GetAllAsync(int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }

            List<Taxonomy> folders = new List<Taxonomy>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.GetAll.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = lcid });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            foreach (DataRow row in table.Rows)
            {
                Taxonomy obj = await CreateAsync(row, false);
                folders.Add(obj);
            }
            return folders;
        }


        public async Task<EntityHierarchycalCollection<Taxonomy>> GetHierarchyByParentIdAsync(long id, int depth = int.MaxValue, bool loadContents = false)
        {
            List<Taxonomy> folders = await GetByParentIdAsync(id, depth, loadContents: loadContents);
            EntityHierarchycalCollection<Taxonomy> list = new EntityHierarchycalCollection<Taxonomy>();
            list.AddRange(folders);
            return list;
        }

        public async Task<Taxonomy> SaveAsync(Taxonomy taxonomy)
        {
            await AuthenticateAndAuthorizeAsync();
            Taxonomy newTaxonomy = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = taxonomy.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.Name.GetIntValue()) { Value = taxonomy.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.FolderId.GetIntValue()) { Value = taxonomy.FolderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.Description.GetIntValue()) { Value = taxonomy.Description.Safe() });

                if (!taxonomy.IsNew)
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomy.Id });
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.Insert.GetIntValue();
                }

                if (taxonomy.Parent != null && taxonomy.Parent.Id != 0)
                {
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ParentId.GetIntValue()) { Value = taxonomy.Parent.Id });
                }
                else if (taxonomy.ParentId != 0)
                {
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ParentId.GetIntValue()) { Value = taxonomy.ParentId });
                }
                else
                {
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ParentId.GetIntValue()) { Value = taxonomy.ParentId });
                }
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAllByTaxonomyIdAsync(taxonomy);

                newTaxonomy = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), true);
                //method.WaitForOnBeforeCompleted();


                if (taxonomy.Contents != null && taxonomy.Contents.Any())
                {
                    for (var i = 0; i < taxonomy.Contents.Count; i++)
                    {
                        int order = i;
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(taxonomy.Contents[i], newTaxonomy, order);
                    }
                }
                method.End();
            }
            return newTaxonomy;
        }

        public async Task<Taxonomy> UpdateAsync(Taxonomy taxonomy, long order)
        {
            await AuthenticateAndAuthorizeAsync();

            Method method = new Method();

            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.Update.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = taxonomy.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.Name.GetIntValue()) { Value = taxonomy.Name });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ParentId.GetIntValue()) { Value = taxonomy.ParentId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.Description.GetIntValue()) { Value = taxonomy.Description.Safe() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomy.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.Order.GetIntValue()) { Value = order });

            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<bool> DeleteAsync(Taxonomy obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {

                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.TaxonomyId.GetIntValue()) { Value = obj.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                //method.WaitForOnBeforeCompleted();

                if (success)
                {

                    success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAllByTaxonomyIdAsync(obj);
                    if (!success)
                    {
                        return false;
                    }
                    List<Taxonomy> children = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TaxonomyController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByParentIdAsync(obj.Id);
                    if (children != null && children.Any())
                    {
                        foreach (Taxonomy child in children)
                        {
                            await DeleteAsync(child);
                        }
                    }
                    return success;
                }
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<bool> AssignContentToTaxonomyAsync(Taxonomy taxonomy, Content content)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Taxonomy;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods.AssignContentToTaxonomy.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.TaxonomyId.GetIntValue()) { Value = taxonomy.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.ContentId.GetIntValue()) { Value = content.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.DateCreated.GetIntValue()) { Value = content.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.LCID.GetIntValue()) { Value = content.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Parameters.Order.GetIntValue()) { Value = 0 });

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task GetAssignedContentItemsAsync(Taxonomy taxonomy)
        {

        }


        public async Task<IEnumerable<Taxonomy>> TaxonomySearchByNameAsync(string searchTerm, int currentPage, int pageSize, string orderColumn, bool reverseOrder)
        {

            /*MySqlCommand command = GenerateStoredProcedure("TaxonomiewSearchByName",
                    new MySqlParameter { ParameterName = "_searchTerm", DbType = DbType.String, Value = searchTerm },
                    new MySqlParameter { ParameterName = "_currentPage", DbType = DbType.Int32, Value = currentPage },
                    new MySqlParameter { ParameterName = "_pageSize", DbType = DbType.Int32, Value = pageSize },
                    new MySqlParameter { ParameterName = "_orderColumn", DbType = DbType.String, Value = orderColumn },
                    new MySqlParameter { ParameterName = "_reverseOrder", DbType = DbType.Boolean, Value = reverseOrder }
                );*/

            List<Taxonomy> taxnomy = new List<Taxonomy>();
            /*DataTable results = ExecuteCommandTable(command);
            foreach (DataRow result in results.Rows)
            {
                taxnomy.Add(await CreateAsync(result, false));
            }*/
            return taxnomy;

        }


    }
}
