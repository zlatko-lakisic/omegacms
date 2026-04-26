using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MediaContentMetaDataFieldValuesController : BaseController<MediaContentMetaDataFieldValuesController>
    {

        public async Task<MediaContentMetaDataFieldValues> CreateAsync(DataRow row)
        {
            MediaContentMetaDataFieldValues obj = null;
            MetaDataField field = await MetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row);
            if (field != null)
            {
                obj = new MediaContentMetaDataFieldValues(field);
                obj.MediaContentId = row.GetValue<long>("MediaContentId");
                obj.Value = row.GetValue<string>(MetaDataFieldValueEnum.Value.GetStringValue());
                obj.ListValue = row.GetValue<string>(MetaDataFieldEnum.ListValue.GetStringValue());
                obj.MetaDataFieldId = row.GetValue<long>(MetaDataFieldEnum.MetaDataFieldId.GetStringValue());
                // obj.DateCreated = row.GetValue<DateTime>(MetaDataFieldValueEnum.DateCreated.GetStringValue().ToString());
              
            }
            return obj;
        }

        public async Task<MediaContentMetaDataFieldValues> SaveAsync(MediaContentMetaDataFieldValues obj)
        {
            await AuthenticateAndAuthorizeAsync();
            DateTime date = new DateTime(0001, 1, 1);


            if (obj.DateCreated == date.ToString())
                obj.DateCreated = DateTime.UtcNow.ToString();
            MediaContentMetaDataFieldValues mediaContentMetaDataFieldValues = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContentMetaDataFieldValues;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Methods.Save.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Parameteres.MediacontentId.GetIntValue()) { Value = obj.MediaContentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Parameteres.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Parameteres.MetaDataFieldId.GetIntValue()) { Value = obj.MetaDataFieldId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Parameteres.Value.GetIntValue()) { Value = obj.Value });

                method.ClearCache = true;

                mediaContentMetaDataFieldValues = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return mediaContentMetaDataFieldValues;
            
        }


        public async Task<List<MediaContentMetaDataFieldValues>> GetByMediaContentAsync(MediaContent obj)
        {
            await AuthenticateAndAuthorizeAsync();
            List<MediaContentMetaDataFieldValues> list = new List<MediaContentMetaDataFieldValues>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContentMetaDataFieldValues;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Methods.GetByMediaContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Parameteres.Id.GetIntValue()) { Value = obj.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MetaDataFieldId_i" });

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            foreach (DataRow row in results.Rows)
            {
                list.Add(await CreateAsync(row));
            }
            return list;
        }

        public async Task<bool> DeleteByMediaContentAsync(MediaContent mediaContent)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContentMetaDataFieldValues;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Methods.DeleteByMediaContent.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Parameteres.MediacontentId.GetIntValue()) { Value = mediaContent.Id });

                method.ClearCache = true;


                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

    }
}
