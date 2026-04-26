using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFolderDataBoundSyncController : BaseController<ContentTypeDefinitionFolderDataBoundSyncController>
    {
        public ContentTypeDefinitionFolderDataBoundSync Create(DataRow row) {
            ContentTypeDefinitionFolderDataBoundSync obj = null;
			if (row != null)
			{
                obj = new ContentTypeDefinitionFolderDataBoundSync();
				obj.FolderId = row.GetValue<long>(ContentTypeDefinitionFolderDataBoundSyncParamatersEnum.FolderId.GetStringValue());
				obj.ContentTypeDefinitionId = row.GetValue<long>(ContentTypeDefinitionFolderDataBoundSyncParamatersEnum.ContentTypeDefinitionId.GetStringValue());
                obj.StartTime = row.GetValue<DateTime>(ContentTypeDefinitionFolderDataBoundSyncParamatersEnum.StartDate.GetStringValue(), DateTime.Now);
                obj.EndTime = row.GetValue<DateTime?>(ContentTypeDefinitionFolderDataBoundSyncParamatersEnum.EndDate.GetStringValue(), null);
                obj.Frequency = TimeSpan.FromSeconds(row.GetValue<int>(ContentTypeDefinitionFolderDataBoundSyncParamatersEnum.Frequency.GetStringValue(), TimeSpan.MaxValue.Seconds));
                obj.Enabled = row.GetValue<bool>(ContentTypeDefinitionFolderDataBoundSyncParamatersEnum.Enabled.GetStringValue(), false);
                obj.SyncType = (ContentTypeDefinitionFolderDataBoundSync.ContentTypeDefinitionFolderDataBoundSyncType)row.GetValue<int>(ContentTypeDefinitionFolderDataBoundSyncParamatersEnum.SyncType.GetStringValue(), 0);
                obj.DeltaFieldId = row.GetValue<long>(ContentTypeDefinitionFolderDataBoundSyncParamatersEnum.DeltaFieldId.GetStringValue(), 0);
                obj.DeltaFieldId = obj.DeltaFieldId.Equals(0) ? null : obj.DeltaFieldId;
            }
			return obj;
		}

        public async Task<ContentTypeDefinitionFolderDataBoundSync> GetByFolderAndContentTypeDefinitionIdAsync(long folderId, long contentTypeDefinitionId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundSync;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Methods.GetByFolderAndContentTypeDefinitionId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.FolderId.GetIntValue()) { Value = folderId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeDefinitionId });
            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<IEnumerable<ContentTypeDefinitionFolderDataBoundSync>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundSync;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Methods.GetAll.GetIntValue();
            return (await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).AsEnumerable().Select(row => Create(row));
		}

		public async Task<ContentTypeDefinitionFolderDataBoundSync> SaveAsync(ContentTypeDefinitionFolderDataBoundSync obj)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
			method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundSync;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Methods.Save.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.FolderId.GetIntValue()) { Value = obj.FolderId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = obj.ContentTypeDefinitionId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.StartTime.GetIntValue()) { Value = obj.StartTime });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.EndTime.GetIntValue()) { Value = obj.EndTime });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.Frequency.GetIntValue()) { Value = obj.Frequency.TotalSeconds });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.Enabled.GetIntValue()) { Value = obj.Enabled });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.SyncType.GetIntValue()) { Value = obj.SyncType });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.DeltaFieldId.GetIntValue()) { Value = obj.DeltaFieldId });
            method.ClearCache = true;

            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
		}

        public async Task<bool> DeleteAsync(long folderId, long contentTypeDefinitionId)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFolderDataBoundSync;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Methods.DeleteByFolderAndContentTypeDefinitionId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.FolderId.GetIntValue()) { Value = folderId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeDefinitionId });
            method.ClearCache = true;

            return await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
		}
	}
}
