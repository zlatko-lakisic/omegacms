using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFolderController : BaseController<ContentTypeDefinitionFolderController>
    {
        public ContentTypeDefinitionFolder Create(DataRow row)
        {
            ContentTypeDefinitionFolder obj = base.Create<ContentTypeDefinitionFolder, long>(row, "FolderId");
            if (obj != null)
            {

                obj.ContentTypeDefinitionId = row.GetValue<long>("ContentTypeDefinitionId");
                obj.Title = row.GetValue<string>("Title");
            }
            return obj;
        }

        public async Task<ContentTypeDefinitionFolder> SaveAsync(long folderId, ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition)
        {
            await AuthenticateAndAuthorizeAsync();
            // var date = new DateTime(0001, 1, 1);


            //if (obj.DateCreated.Date == date)
            //    obj.DateCreated = DateTime.UtcNow;

            ContentTypeDefinitionFolder folder = new ContentTypeDefinitionFolder();
            
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.FolderId.GetIntValue()) { Value = folderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeDefinition.Id });

                method.ClearCache = true;

                folder = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();             
                //method.WaitForOnAfterCompleted();
            }
            return folder;
        }

        public async Task<ContentTypeDefinitionFolder> DeleteAsync(Folder<Content> obj, ContentTypeDefinition<ContentTypeDefinitionField> contentTypeDefinition)
        {
            await AuthenticateAndAuthorizeAsync();
            ContentTypeDefinitionFolder result = null;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.FolderId.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeDefinition.Id });

                method.ClearCache = true;

                result = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return result;
        }

        public async Task<ContentTypeDefinitionFolder> DeleteAllAsync(long folderId, long ContentTypeDefinitionId)
        {
            await AuthenticateAndAuthorizeAsync();
            ContentTypeDefinitionFolder result = null;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.FolderId.GetIntValue()) { Value = folderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.ContentTypeDefinitionId.GetIntValue()) { Value = ContentTypeDefinitionId });

                method.ClearCache = true;

                result = Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return result;
        }

        public async Task<List<ContentTypeDefinitionFolder>> GetByFolderAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ContentTypeDefinitionFolder> list = new List<ContentTypeDefinitionFolder>();

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Methods.GetByFolder.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.FolderId.GetIntValue()) { Value = id });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                ContentTypeDefinitionFolder folder = Create(row);
                list.Add(folder);
            }
            return list;
        }

        public async Task<bool> DeleteAllByFolderIdAsync(long folderId)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolder;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Methods.DeleteAllBFolderId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Parameteres.FolderId.GetIntValue()) { Value = folderId });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();             
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }
    }
}
