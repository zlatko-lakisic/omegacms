using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class FolderMetaDataFieldController : BaseController<FolderMetaDataFieldController>
    {

        public FolderMetaDataField Create(DataRow row)
        {
            FolderMetaDataField obj = base.Create<FolderMetaDataField, long>(row, MetaDataFieldEnum.MetaDataFieldId.GetStringValue());
            if (obj != null)
            {
                obj.FolderId = row.GetValue<long>("FolderId");
                obj.MetaDataFieldId = row.GetValue<long>("MetaDataFieldId");
                obj.IsRequired = row.GetValue<bool>("IsRequired");
                obj.Name = row.GetValue<string>("Name");
            }
            return obj;
        }

        public async Task<FolderMetaDataField> FolderMetaDataFieldGetByIdsAsync(long folderId, long metaDataFieldId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Methods.FolderMetaDataFieldGetByIds.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Paremeteres.FolderId.GetIntValue()) { Value = folderId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Paremeteres.MetaDataFieldId.GetIntValue()) { Value = metaDataFieldId });
            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<List<FolderMetaDataField>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<FolderMetaDataField> list = new List<FolderMetaDataField>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Methods.GetAll.GetIntValue();
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                list.Add(Create(row));
            }
            return list;
        }


        public async Task<List<FolderMetaDataField>> GetOnlyUsedAsync(long folderId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Methods.GetOnlyUsed.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Paremeteres.FolderId.GetIntValue()) { Value = folderId });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            return (from DataRow row in results.Rows select Create(row)).ToList();
        }



        public async Task<List<FolderMetaDataField>> GetUsedMetaDataFieldsByFolderAsync(long folderId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Methods.GetUsedMetaDataFieldsByFolder.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Paremeteres.FolderId.GetIntValue()) { Value = folderId });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            List<FolderMetaDataField> usedFields = (from DataRow row in results.Rows select Create(row)).ToList();
            List<FolderMetaDataField> allMetaDataFields = await FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetAllAsync();
            List<FolderMetaDataField> toReturn = new List<FolderMetaDataField>();

            foreach (FolderMetaDataField metaDataField in allMetaDataFields)
            {
                metaDataField.MetaDataFieldId = metaDataField.Id;
                foreach (FolderMetaDataField checkedField in usedFields)
                {                   
                    if (metaDataField.Id == checkedField.Id)
                    {
                        metaDataField.Checked = true;
                    }
                    if (checkedField.MetaDataFieldId == metaDataField.MetaDataFieldId && checkedField.IsRequired == true)
                    {
                        metaDataField.IsRequired = true;
                    }
                }
            }
            return allMetaDataFields;
        }

        public async Task<bool> DeleteByFolderIdAsync(long folderId)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Methods.DeleteByFolderId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Paremeteres.FolderId.GetIntValue()) { Value = folderId });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();                
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<bool> AssignMetaDataFieldToFolderAsync(long folderId, FolderMetaDataField folderMetaDataField)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Methods.AssignMetaDataFieldToFolder.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Paremeteres.FolderId.GetIntValue()) { Value = folderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Paremeteres.MetaDataFieldId.GetIntValue()) { Value = folderMetaDataField.MetaDataFieldId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Paremeteres.IsRequired.GetIntValue()) { Value = folderMetaDataField.IsRequired });

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();             
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<List<FolderMetaDataField>> GetByFolderIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            List<FolderMetaDataField> folders = new List<FolderMetaDataField>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.FolderMetaDataField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Methods.GetByFolderId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Paremeteres.FolderId.GetIntValue()) { Value = id });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                FolderMetaDataField obj = Create(row);
                folders.Add(obj);
            }

            return folders;
        }
    }

}
