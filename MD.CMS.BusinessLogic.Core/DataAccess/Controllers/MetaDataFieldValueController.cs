using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Collections.Generic;
using System.Data;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MetaDataFieldValueController : BaseController<MetaDataFieldValueController>
    {
        public async Task<MetaDataFieldValue> CreateAsync(DataRow row)
        {
            MetaDataFieldValue obj = null;
            MetaDataField field = await MetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row);
            if (field != null)
            {
                obj = new MetaDataFieldValue(field);
                obj.ContentId = row.GetValue<string>(MetaDataFieldValueEnum.ContentId.GetStringValue());
                obj.LCID = row.GetValue<int>(MetaDataFieldValueEnum.LCID.GetStringValue());
                obj.DateCreated = row.GetValue<string>(MetaDataFieldValueEnum.DateCreated.GetStringValue());
                obj.Value = row.GetValue<string>(MetaDataFieldValueEnum.Value.GetStringValue());
                obj.ListValue = row.GetValue<string>(MetaDataFieldEnum.ListValue.GetStringValue());
                obj.MetaDataFieldId = row.GetValue<long>(MetaDataFieldEnum.MetaDataFieldId.GetStringValue());

            }
            return obj;
        }

        public async Task<List<MetaDataFieldValue>> GetByContentAsync(Content obj)
        {
            await AuthenticateAndAuthorizeAsync();
            List<MetaDataFieldValue> list = new List<MetaDataFieldValue>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataFieldValue;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Methods.GetByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.ContentId.GetIntValue()) { Value = obj.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MetaDataFieldId_i" });

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                list.Add(await CreateAsync(row));
            }
            return list;
        }

        public async Task<MetaDataFieldValue> SaveAsync(MetaDataFieldValue obj)
        {
            await AuthenticateAndAuthorizeAsync();
            MetaDataFieldValue metaDatFieldValue = null;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataFieldValue;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.ContentId.GetIntValue()) { Value = obj.ContentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.MetaDataFieldId.GetIntValue()) { Value = obj.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.Value.GetIntValue()) { Value = obj.Value });
                method.ClearCache = true;

                metaDatFieldValue = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
               // method.End();
               // method.WaitForOnAfterCompleted();
            }
            return metaDatFieldValue;
        }

        public async Task<MetaDataFieldValue> UpdateAsync(MetaDataFieldValue obj)
        {
            await AuthenticateAndAuthorizeAsync();
            MetaDataFieldValue metaDatFieldValue = null;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataFieldValue;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Methods.Update.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.ContentId.GetIntValue()) { Value = obj.ContentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.MetaDataFieldId.GetIntValue()) { Value = obj.MetaDataFieldId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.Value.GetIntValue()) { Value = obj.Value });
                method.ClearCache = true;

                metaDatFieldValue = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return metaDatFieldValue;
        }

        public async Task<bool> DeleteByContentAsync(MetaDataFieldValue obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataFieldValue;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Methods.DeleteByContent.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.ContentId.GetIntValue()) { Value = obj.ContentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();

            }

            return success;
        }
    }
}
