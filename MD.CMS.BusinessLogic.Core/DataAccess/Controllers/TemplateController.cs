using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class TemplateController<T> : BaseController<TemplateController<T>> where T : Content
    {
        public Template Create(DataRow row)
        {
            Template obj = base.Create<Template, long>(row, TemplateEnum.TemplateId.GetStringValue());
            if (obj != null)
            {
                obj.Name = row.GetValue<string>(TemplateEnum.Name.GetStringValue());
                obj.Description = row.GetValue<string>(TemplateEnum.Description.GetStringValue());
                obj.TemplateUrl = row.GetValue<string>(TemplateEnum.TemplateUrl.GetStringValue());
            }
            return obj;
        }

        public async Task<List<Template>> GetAllAsync(string sort = "Name ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            List<Template> templates = new List<Template>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.GetAll.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.Sort.GetIntValue()) { Value = sort });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                Template template = Create(row);
                templates.Add(template);
            }
            return templates;
        }

        public async Task<long> GetAllCountAsync(string searchTerm, string searchColumn)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.GetAllCount.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            long count = row.GetValue<long>("TemplateCount");
            return count;
        }

        public async Task<Entities.Base.BasePaginationEntity<Template>> GetAllWithPaginationAsync(string sort, long pageIndex, long pageSize, string searchTerm, string searchColumn)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Template> templates = new List<Template>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.GetAllWithPagination.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = pageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = pageSize });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                Template template = Create(row);
                templates.Add(template);
            }
            Entities.Base.BasePaginationEntity<Template> basePaginationEntity = new Entities.Base.BasePaginationEntity<Template>();
            basePaginationEntity.Items = templates;
            if(table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntity;
        }

        public async Task<List<Template>> GetByFolderAsync<T>(Folder<T> folder) where T : Content
        {
            await AuthenticateAndAuthorizeAsync();
            List<Template> templates = new List<Template>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.GetByFolder.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                Template template = Create(row);
                templates.Add(template);
            }
            return templates;
        }
        public async Task<List<Template>> GetByParentIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Template> templates = new List<Template>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.GetByFolder.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.FolderId.GetIntValue()) { Value = id });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                Template template = Create(row);
                templates.Add(template);
            }
            return templates;
        }

        public async Task<Template> GetByContentAsync(Content content)
        {
            await AuthenticateAndAuthorizeAsync();
            Template template = new Template();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.GetByContent.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.ContentId.GetIntValue()) { Value = content.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.LCID.GetIntValue()) { Value = content.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.DateCreated.GetIntValue()) { Value = content.DateCreated });
            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<Template> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.GetById.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.TemplateId.GetIntValue()) { Value = id });
            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<Template> SaveAsync(Template template)
        {
            await AuthenticateAndAuthorizeAsync();
            Template newTemplate = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.Name.GetIntValue()) { Value = template.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.Description.GetIntValue()) { Value = template.Description });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.TemplateUrl.GetIntValue()) { Value = template.TemplateUrl });
                method.ClearCache = true;

                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                newTemplate = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));               
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return newTemplate;
        }

        public async Task<Template> UpdateAsync(Template template)
        {
            await AuthenticateAndAuthorizeAsync();
            Template newTemplate = null;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.Update.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.TemplateId.GetIntValue()) { Value = template.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.Name.GetIntValue()) { Value = template.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.Description.GetIntValue()) { Value = template.Description });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.TemplateUrl.GetIntValue()) { Value = template.TemplateUrl });

                method.ClearCache = true;

                newTemplate = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                //method.WaitForOnBeforeCompleted();
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return newTemplate;
        }

        public async Task<bool> DeleteAsync(Template template)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.Delete.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.TemplateId.GetIntValue()) { Value = template.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                if (success)
                    template = null;
                method.End();
                //method.WaitForOnAfterCompleted();
            }

            return success;
        }

        public async Task<bool> AssignTemplateToFolderAsync(Template template, Folder<T> folder)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.ConnectWithFolder.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.TemplateId.GetIntValue()) { Value = template.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();              
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<bool> DeleteConnectionWithFolderAsync(Template template, Folder<T> folder)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.DeleteConnectionWithFolder.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.TemplateId.GetIntValue()) { Value = template.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<bool> DeleteByFolderAsync(Folder<T> folder)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.DeleteByFolder.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();               
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<bool> AssignTemplateToContentAsync(Template template, Content content)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.ConnectWithContent.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.TemplateId.GetIntValue()) { Value = template.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.ContentId.GetIntValue()) { Value = content.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.LCID.GetIntValue()) { Value = content.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.DateCreated.GetIntValue()) { Value = content.DateCreated });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
             //   method.End();
              //  method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<List<Template>> SearchAsync(string searchTerm, string searchColumn)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Template> searchResults = new List<Template>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Template;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods.Search.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                searchResults.Add(Create(row));
            }
            return searchResults;
        }
    }
}
