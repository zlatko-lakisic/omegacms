using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentAliasController : BaseController<ContentAliasController>
    {

        public ContentAlias Create(DataRow row)
        {
            ContentAlias obj = base.Create<ContentAlias, long>(row, ContentAliasEnum.ContentId.GetStringValue());
            if (obj != null)
            {
                obj.ContentId = row.GetValue<long>(ContentAliasEnum.ContentId.GetStringValue());
                obj.LCID = row.GetValue<int>(ContentAliasEnum.LCID.GetStringValue());
                obj.DateCreated = row.GetValue<string>(ContentAliasEnum.DateCreated.GetStringValue());

                obj.Alias = row.GetValue<string>(ContentAliasEnum.Alias.GetStringValue());
            }
            return obj;
        }

        public async Task<List<ContentAlias>> GetAllAsync(int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods.GetAll.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            ConcurrentQueue<ContentAlias> list = new ConcurrentQueue<ContentAlias>();
            await Task.WhenAll(table.AsEnumerable().Select(async row => {
                list.Enqueue(Create(row));
            }));

            return list.ToList();
        }

        public async Task<ContentAlias> GetByIdAsync(long id, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            List<ContentAlias> contents = new List<ContentAlias>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.Id.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "ContentId_i" });

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

        }

        public async Task<List<ContentAlias>> GetByContentAsync(long id, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            List<ContentAlias> contents = new List<ContentAlias>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods.GetByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.Id.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.LCID.GetIntValue()) { Value = lcid });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            ConcurrentQueue<ContentAlias> list = new ConcurrentQueue<ContentAlias>();
            await Task.WhenAll(table.AsEnumerable().Select(async row => {
                list.Enqueue(Create(row));
            }));

            return list.ToList();
        }

        public async Task<bool> DeleteAsync(ContentAlias obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.Id.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.LCID.GetIntValue()) { Value = obj.LCID });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                if (success)
                    obj = null; 
            }
            return success;
        }

        public async Task<bool> DeleteByContentAsync(Content obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.Id.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.LCID.GetIntValue()) { Value = obj.LCID });
                method.ClearCache = true;
                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<ContentAlias> GetByContentIdAsync(long id, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            List<ContentAlias> contents = new List<ContentAlias>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods.GetByContentId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.Id.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.LCID.GetIntValue()) { Value = lcid });

            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<string> GetAliasByContentAsync(Content content)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods.GetAliasByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.ContentId.GetIntValue()) { Value = content.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.LCID.GetIntValue()) { Value = content.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.DateCreated.GetIntValue()) { Value = content.DateCreated });
            ContentAlias contentAlias = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            if (contentAlias != null)
                 return contentAlias.Alias;

            return string.Empty;
        }

        public async Task<ContentAlias> SaveAsync(Content content, string alias)
        {
            await AuthenticateAndAuthorizeAsync();
            ContentAlias newContentAlias = null;

            using (Method method = new Method())
            {
                
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods.Insert.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.ContentId.GetIntValue()) { Value = content.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.LCID.GetIntValue()) { Value = content.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.DateCreated.GetIntValue()) { Value = content.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.Alias.GetIntValue()) { Value = alias });

                newContentAlias = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
            return newContentAlias;
        }

        public async Task<List<ContentAlias>> GetAllAliasesByContentAsync(Content content)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentAlias;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods.SelectAllAliasesByContent.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.ContentId.GetIntValue()) { Value = content.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.LCID.GetIntValue()) { Value = content.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Parameteres.DateCreated.GetIntValue()) { Value = content.DateCreated });
                DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

                ConcurrentQueue<ContentAlias> list = new ConcurrentQueue<ContentAlias>();
                await Task.WhenAll(table.AsEnumerable().Select(async row => {
                    list.Enqueue(Create(row));
                }));

                return list.ToList();
            }
        }
    }
}