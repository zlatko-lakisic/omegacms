using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.StringExt;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MenuController : BaseController<MenuController>
    {
        public async Task<Menu> CreateAsync(DataRow row, bool fillParent = false, bool fillAllParents = false, bool fillContents = false)
        {
            Menu obj = base.Create<Menu, long>(row, MenuParamatersEnum.MenuId.GetStringValue());
            if (obj != null)
            {
                obj.LCID = row.GetValue<int>(MenuParamatersEnum.LCID.GetStringValue());
                obj.FolderId = row.GetValue<int>(MenuParamatersEnum.FolderId.GetStringValue());
                obj.ParentId = row.GetValue<long>(MenuParamatersEnum.ParentId.GetStringValue());
                obj.Name = row.GetValue<string>(MenuParamatersEnum.Name.GetStringValue());
                obj.Description = row.GetValue<string>(MenuParamatersEnum.Description.GetStringValue());
                obj.MenuPath = row.GetValue<string>(MenuParamatersEnum.MenuPath.GetStringValue());
                obj.LCID = row.GetValue<int>(MenuParamatersEnum.LCID.GetStringValue());
                obj.Options = row.GetValue<string>(MenuParamatersEnum.Options.GetStringValue());
                if (fillParent)
                {
                    if (obj.ParentId != 0 && obj.ParentId != null)
                    {
                        obj.Parent = await GetByIdAsync(obj.ParentId, fillAllParents);
                        obj.EntityPath = obj.Id.ToString();
                        if (obj.Parent != null)
                        {
                            obj.EntityPath = string.Format("{0}_{1}", obj.Parent.EntityPath, obj.Id);
                        }
                    }
                    else
                        obj.Parent = null;
                }
                obj.Contents = await ContentController<Content>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByMenuAsync(obj);
            }
            return obj;
        }

        public async Task<Menu> GetByIdAsync(long id, bool fillParent = false, bool fillContents = false)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.MenuId.GetIntValue()) { Value = id });
            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), true, fillContents: fillContents);
        }

        public async Task<Menu> GetMenuByPathAsync(string path = "", bool fillParent = false, bool fillAllParents = false, int lcid = 0)
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.GetByMenuPath.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.MenuPath.GetIntValue()) { Value = path });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = lcid });


            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), fillParent, fillAllParents, fillContents: true);
              //DataTable result = await ExecuteMethodTableAsync(method);
              //List<Menu> menus = new List<Menu>();
              //foreach (DataRow row in result.Rows)
              //{
              //    Menu obj = await CreateAsync(row, fillParent, fillAllParents, fillContents: true);
              //    menus.Add(obj);
              //}

              //return menus;
        }

        public async Task<List<Menu>> GetByContentIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Menu> folders = new List<Menu>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.GetByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ContentId.GetIntValue()) { Value = id });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            //return (from DataRow row in result.Rows select await CreateAsync(row)).ToList();

            foreach (DataRow row in result.Rows)
            {
                Menu obj = await CreateAsync(row, false);
                folders.Add(obj);
            }

            return folders;
        }

        public async Task<List<Menu>> GetByParentIdAsync(long id, int depth = 0, int lcid = 0)
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }

            List<Menu> folders = new List<Menu>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.GetByParentId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ParentId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = lcid });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Menu obj = await CreateAsync(row, false);
                    if (depth > 0)
                    {
                        obj.Children = await GetByParentIdAsync(obj.Id, depth - 1);
                    }
                    folders.Add(obj);
                }
            }
            return folders;
        }

        public async Task<List<Menu>> GetByContentAsync(Content content, int depth = int.MaxValue)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Menu> folders = new List<Menu>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.GetByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ParentId.GetIntValue()) { Value = content.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = content.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.DateTime.GetIntValue()) { Value = content.DateCreated });


            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in result.Rows)
            {
                Menu obj = await CreateAsync(row, false);
                if (depth > 0)
                {
                    obj.Children = await GetByParentIdAsync(obj.Id, depth - 1);
                }
                folders.Add(obj);
            }
            return folders;
        }

        public async Task<IEnumerable<Menu>> GetAllAsync(int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            List<Menu> folders = new List<Menu>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.GetAll.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = lcid });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in result.Rows)
            {
                Menu obj = await CreateAsync(row, false);
                folders.Add(obj);
            }
            return folders;
        }

        public async Task<EntityHierarchycalCollection<Menu>> GetHierarchyByParentIdAsync(long id, int depth = int.MaxValue)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Menu> folders = await GetByParentIdAsync(id, depth);
            EntityHierarchycalCollection<Menu> list = new EntityHierarchycalCollection<Menu>();
            list.AddRange(folders);
            return list;
        }

        public async Task<Menu> SaveAsync(Menu menu)
        {
            await AuthenticateAndAuthorizeAsync();
            Menu newMenu = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = menu.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.Name.GetIntValue()) { Value = menu.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.FolderId.GetIntValue()) { Value = menu.FolderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.Description.GetIntValue()) { Value = menu.Description.Safe() });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.Options.GetIntValue()) { Value = menu.Options });


                if (!menu.IsNew)
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.MenuId.GetIntValue()) { Value = menu.Id });
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.Insert.GetIntValue();
                }

                if (menu.Parent != null)
                {
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ParentId.GetIntValue()) { Value = menu.Parent.Id });
                }
                else
                {
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ParentId.GetIntValue()) { Value = menu.ParentId });
                }
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAllbyMenuIdAsync(menu);
                method.ClearCache = true;

                newMenu = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), true);
                // method.WaitForOnBeforeCompleted();

                if (menu.Contents != null && menu.Contents.Any())
                {
                    for (var i = 0; i < menu.Contents.Count; i++)
                    {
                        int order = i;
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(menu.Contents[i], newMenu, order);
                    }
                    //foreach (Content content in menu.Contents)
                    //{
                    //    MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.Instance.Save(content, newMenu);
                    //}
                }
                method.End();
                // method.WaitForOnAfterCompleted();
            }
            return newMenu;
        }

        public async Task<Menu> UpdateAsync(Menu menu, long order) 
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();

            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.Update.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = menu.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.Name.GetIntValue()) { Value = menu.Name });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ParentId.GetIntValue()) { Value = menu.ParentId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.Description.GetIntValue()) { Value = menu.Description.Safe() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.MenuId.GetIntValue()) { Value = menu.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.Order.GetIntValue()) { Value = order });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.Options.GetIntValue()) { Value = menu.Options});

            method.ClearCache = true;

            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<bool> DeleteAsync(Menu obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
             using (Method method = new Method())
             {
                 method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                 method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
                 method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.Delete.GetIntValue();
                 method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.MenuId.GetIntValue()) { Value = obj.Id });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                 //method.WaitForOnBeforeCompleted();
                 if (success)
                 {
                     success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAllbyMenuIdAsync(obj);
                     if (!success)
                     {
                         return false;
                     }
                     List<Menu> children = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MenuController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByParentIdAsync(obj.Id);
                     if (children != null && children.Any())
                     {
                         foreach (Menu child in children)
                         {
                             await DeleteAsync(child);
                         }
                     }

                 }
                 method.End();
                 //method.WaitForOnAfterCompleted();
             }
            return success;
        }
        public async Task<bool> DeleteParentIdAsync(Menu obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.DeleteByParentId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ParentId.GetIntValue()) { Value = obj.ParentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = obj.LCID });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();

            }
            return success;
        }

        public async Task<bool> AssignContentToMenuAsync(Menu menu, Content content)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
             using (Method method = new Method())
             {
                 method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                 method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
                 method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.Save.GetIntValue();

                 method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = menu.Id });
                 method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.ContentId.GetIntValue()) { Value = content.Id });
                 method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.DateCreated.GetIntValue()) { Value = content.DateCreated });
                 method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = content.LCID });

                 success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                 method.End();
                 //method.WaitForOnAfterCompleted();
             }
            return success;
        }

        public async Task<IEnumerable<Menu>> MenuSearchByNameAsync(string searchTerm, int currentPage, int pageSize, string orderColumn, bool reverseOrder)
        {
            await AuthenticateAndAuthorizeAsync();
            /*MySqlCommand command = GenerateStoredProcedure("Menus_SearchByName",
                    new MySqlParameter { ParameterName = "_searchTerm", DbType = DbType.String, Value = searchTerm },
                    new MySqlParameter { ParameterName = "_currentPage", DbType = DbType.Int32, Value = currentPage },
                    new MySqlParameter { ParameterName = "_pageSize", DbType = DbType.Int32, Value = pageSize },
                    new MySqlParameter { ParameterName = "_orderColumn", DbType = DbType.String, Value = orderColumn },
                    new MySqlParameter { ParameterName = "_reverseOrder", DbType = DbType.Boolean, Value = reverseOrder }
                );*/

            List<Menu> menu = new List<Menu>();
            /*DataTable results = ExecuteCommandTable(command);
            foreach (DataRow result in results.Rows)
            {
                menu.Add(await CreateAsync(result));
            }*/
            return menu;
        }

        /// <summary>
        /// Method for searching menus by search word found in menu name
        /// </summary>
        /// <param name="searchTerm">String to find in menu name</param>
        /// <param name="lcid">LCID for menu</param>
        /// <param name="parentId">Parent Menu</param>
        /// <param name="recursion">Search child menus also?</param>
        /// <returns>List of menus containing search query in their name</returns>
        public async Task<List<Menu>> MenusSearchAsync(string searchTerm, int lcid, long parentId, bool recursion)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Menu> menus = new List<Menu>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.MenusSearch.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ParentId.GetIntValue()) { Value = parentId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.Recursion.GetIntValue()) { Value = recursion });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in result.Rows)
            {
                Menu obj = await CreateAsync(row, false, false, true);
                menus.Add(obj);
            }
            return menus;
        }

        public async Task<Entities.Base.BasePaginationEntity<Menu>> GetByParentIdWithPaginationAsync(long id, long currentPageIndex, long maxNumberOfRows, string sortString, string searchTerm, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            List<Menu> menus = new List<Menu>();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.GetByParentIdWithPagination.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ParentId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.Sort.GetIntValue()) { Value = sortString });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                menus.Add(await CreateAsync(row, false, false, true));
            }
            Entities.Base.BasePaginationEntity<Menu> basePaginationEntity = new Entities.Base.BasePaginationEntity<Menu>();
            basePaginationEntity.Items = menus;
            if(table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalNumber");
            }
            return basePaginationEntity;
        }

        public async Task<int> GetByParentIdCountAsync(long menuId, int lcid, string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Menu;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods.GetByParentIdCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.ParentId.GetIntValue()) { Value = menuId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("MenuCount");
            return count;
        }


    }
}
