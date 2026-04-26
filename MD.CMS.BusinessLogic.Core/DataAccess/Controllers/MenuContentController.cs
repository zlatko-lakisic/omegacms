using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MenuContentController : BaseController<MenuContentController>
    {
        public MenuContent Create(DataRow row)
        {
            MenuContent obj = base.Create<MenuContent, long>(row, MenuContentParamatersEnum.ContentId.GetStringValue());
            if (obj != null)
            {
                obj.LCID = row.GetValue<int>(MenuContentParamatersEnum.LCID.GetStringValue());
                obj.DateCreated = row.GetValue<DateTime>(MenuContentParamatersEnum.DateCreated.GetStringValue()).ToString();
                obj.MenuId = row.GetValue<long>(MenuContentParamatersEnum.MenuId.GetStringValue());
                obj.Title = row.GetValue<string>(MenuContentParamatersEnum.Title.GetStringValue());
                obj.MenuContentPath = row.GetValue<string>(MenuContentParamatersEnum.folderpath.GetStringValue());
            }
            return obj;
        }

        public async Task<MenuContent> SaveAsync(Content obj, Menu menu,int order)
        {
            await AuthenticateAndAuthorizeAsync();
            MenuContent result = null;

            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.Save.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.ContentId.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = menu.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.Order.GetIntValue()) { Value = order });

                method.ClearCache = true;

                result = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return result;
        }

        public async Task<MenuContent> UpdateAsync(MenuContent obj, Menu menu, int order)
        {
            await AuthenticateAndAuthorizeAsync();
            MenuContent result = null;

            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.Update.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.ContentId.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = menu.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.Order.GetIntValue()) { Value = order });

                method.ClearCache = true;

                result = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return result;
        }

        public async Task<MenuContent> DeleteMenuAsync(Content obj, Menu menu)
        {
            await AuthenticateAndAuthorizeAsync();
            MenuContent result = null;

            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.DeleteMenu.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.ContentId.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = menu.Id });

                method.ClearCache = true;

                result = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return result;
        }

        public async Task SaveMenuContentAsync(Menu menu, Content content)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.Save.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.ContentId.GetIntValue()) { Value = content.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = content.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.DateCreated.GetIntValue()) { Value = content.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = menu.Id });

                method.ClearCache = true;

                await ExecuteMethodVoidAsync(method, this.UseDefaultPlugin);
            }
        }

        public async Task<List<MenuContent>> GetByMenuIdAsync(long id, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            List<MenuContent> contents = new List<MenuContent>();

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.GetByMenuId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });


            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    MenuContent obj = Create(row);
                    contents.Add(obj);
                }
            }

            return contents;
        }

        public async Task<bool> DeleteAsync(MenuContent menuContent)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.DeleteMenu.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.ContentId.GetIntValue()) { Value = menuContent.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = menuContent.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.DateCreated.GetIntValue()) { Value = menuContent.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = menuContent.MenuId });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();

            }
            return success;
        }

        public async Task<MenuContent> DeleteMenu1Async(string contentId, int lcid, string dateCreated, Menu newMenu)
        {
            await AuthenticateAndAuthorizeAsync();
            MenuContent result = null;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.DeleteMenu.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.ContentId.GetIntValue()) { Value = contentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = lcid });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.DateCreated.GetIntValue()) { Value = dateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = newMenu.Id });

                method.ClearCache = true;

                result = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return result;
        }

        public async Task<bool> DeleteAllbyMenuIdAsync(Menu menu)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.DeleteContentByMenuId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = menu.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<List<MenuContent>> GetByContentAsync(Content obj)
        {
            await AuthenticateAndAuthorizeAsync();
            List<MenuContent> folders = new List<MenuContent>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.GetByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.Id.GetIntValue()) { Value = obj.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = obj.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.DateCreated.GetIntValue()) { Value = obj.DateCreated });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                MenuContent menu = Create(row);
                folders.Add(menu);
            }

            return folders;
        }

        public async Task<int> GetByMenuIdCountAsync(long menuId, string searchTerm, int lcid = default(int))
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
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.GetByMenuIdCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = menuId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("MenuContentCount");
            return count;
        }

        /// <summary>
        /// Search for menu contents by given keyword
        /// </summary>
        /// <param name="searchTerm">Word to search for in menu content title</param>
        /// <param name="menuId">Menu where search is done</param>
        /// <param name="lcid">Content language</param>
        /// <returns></returns>
        public async Task<List<MenuContent>> MenuContentsSearchAsync(string searchTerm, int lcid, long menuId)
        {
            await AuthenticateAndAuthorizeAsync();
            List<MenuContent> menuContents = new List<MenuContent>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.MenuContentsSearch.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = menuId });

            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in result.Rows)
            {
                MenuContent obj = Create(row);
                menuContents.Add(obj);
            }
            return menuContents;
        }

        public async Task<Entities.Base.BasePaginationEntity<MenuContent>> GetByMenuIdWithPaginationAsync(long id, long currentPageIndex, long maxNumberOfRows, string searchTerm, int lcid = default(int), string sort = "Order ASC")
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
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

            List<MenuContent> menuContents = new List<MenuContent>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MenuContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods.GetByMenuIdWithPagination.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.MenuId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Parameteres.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "Order_i asc" });
            
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            if (table.Rows.Count != 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    MenuContent menuContent = Create(row);
                    menuContents.Add(menuContent);
                }
            }
            Entities.Base.BasePaginationEntity<MenuContent> basePaginationEntity = new Entities.Base.BasePaginationEntity<MenuContent>();
            basePaginationEntity.Items = menuContents;
            if(table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntity;
        }
    }
}
