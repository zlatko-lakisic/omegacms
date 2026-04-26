using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System;
using System.Globalization;
using MD.Tools.Helpers.Core.Data;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFieldValueController : BaseController<ContentTypeDefinitionFieldValueController>
    {
        public async Task<ContentTypeDefinitionFieldValue> CreateAsync(DataRow row)
        {
            ContentTypeDefinitionFieldValue obj = null;
            if (row != null)
            {
                obj = new ContentTypeDefinitionFieldValue(await ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(row.GetValue<long>(ContentTypeDefinitionFieldEnum.ContentTypeDefinitionFieldId.GetStringValue())));
                obj.ContentTypeDefinitionId = row.GetValue<long>(ContentTypeDefinitionFieldValueEnum.ContentTypeDefinitionId.GetStringValue());
                obj.ContentTypeDefinitionFieldId = row.GetValue<long>(ContentTypeDefinitionFieldValueEnum.ContentTypeDefinitionFieldId.GetStringValue());
                obj.ContentId = row.GetValue<string>(ContentTypeDefinitionFieldValueEnum.ContentId.GetStringValue());
                obj.LCID = row.GetValue<int>(ContentTypeDefinitionFieldValueEnum.LCID.GetStringValue());
                obj.DateCreated = DateTime.Parse(row.GetValue<string>(ContentTypeDefinitionFieldValueEnum.DateCreated.GetStringValue()), CultureInfo.InvariantCulture);
                obj.Value = row.GetValue<string>(ContentTypeDefinitionFieldValueEnum.Value.GetStringValue());
                obj.Name = row.GetValue<string>(ContentTypeDefinitionFieldValueEnum.Name.GetStringValue());
                obj.AttributeTypeDefinitionId = row.GetValue<long>(ContentTypeDefinitionFieldValueEnum.AttributeTypeDefinitionId.GetStringValue());
            }
            return obj;
        }

        public async Task<List<ContentTypeDefinitionFieldValue>> GetByContentAsync(Content obj)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Methods.GetByContent.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentId.GetIntValue()) { Value = obj.Id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = obj.ContentTypeDefinitionId});
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            ConcurrentQueue<ContentTypeDefinitionFieldValue> list = new ConcurrentQueue<ContentTypeDefinitionFieldValue>();
            await Task.WhenAll(table.AsEnumerable().Select(async row => {
                list.Enqueue(await ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row));
            }));
            return list.ToList();
        }
        public async Task<List<ContentTypeDefinitionFieldValue>> GetByContentIdAsync(string id, int lcid, string dateCreated)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Methods.GetByContentId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.DateCreated.GetIntValue()) { Value = dateCreated });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            ConcurrentQueue<ContentTypeDefinitionFieldValue> list = new ConcurrentQueue<ContentTypeDefinitionFieldValue>();
            await Task.WhenAll(table.AsEnumerable().Select(async row => {
                list.Enqueue(await ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row));
            }));
            return list.ToList();
        }

        public async Task<List<ContentTypeDefinitionFieldValue>> GetByValueAsync(string value, long contentTypeDefinitionId = default, long contentTypeDefinitionFieldId = default, ComparerTypeEnum comparer = ComparerTypeEnum.Equals, DataTransformEnum transform = DataTransformEnum.ToString)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Methods.GetByValue.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.Value.GetIntValue()) { Value = value });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.Comparer.GetIntValue()) { Value = comparer.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.DataTransform.GetIntValue()) { Value = transform.GetIntValue() });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeDefinitionId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentTypeDefinitionFieldId.GetIntValue()) { Value = contentTypeDefinitionFieldId });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            ConcurrentQueue<ContentTypeDefinitionFieldValue> list = new ConcurrentQueue<ContentTypeDefinitionFieldValue>();
            await Task.WhenAll(table.AsEnumerable().Select(async row => {
                list.Enqueue(await ContentTypeDefinitionFieldValueController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row));
            }));
            return list.ToList();
        }

        //TODO:Delete from plugins after final check
        //Checked for this method and found that is not used and dont make sense
        //public List<ContentTypeDefinitionFieldValue> GetByContentTyPeDefinition(ContentTypeDefinition obj)
        //{
        //    List<ContentTypeDefinitionFieldValue> list = new List<ContentTypeDefinitionFieldValue>();
        //    Method method = new Method();
        //    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
        //    method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue;
        //    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Methods.GetByContentTyPeDefinition.GetIntValue();
        //    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = obj.Id });
        //    DataTable table = ExecuteMethodTable(method, false);
        //    foreach (DataRow row in table.Rows)
        //    {
        //        ContentTypeDefinitionFieldValue fieldValue = Create(row);
        //        list.Add(fieldValue);
        //    }
        //    return list;
        //}

        public async Task<ContentTypeDefinitionFieldValue> SaveAsync(ContentTypeDefinitionFieldValue obj)
        {
            await AuthenticateAndAuthorizeAsync();
            ContentTypeDefinitionFieldValue contentTypeDefinitionField = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue;

                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = obj.ContentTypeDefinitionId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentId.GetIntValue()) { Value = obj.ContentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentTypeDefinitionFieldId.GetIntValue()) { Value = obj.ContentTypeDefinitionFieldId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.Value.GetIntValue()) { Value = obj.Value });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.Order.GetIntValue()) { Value = obj.Order });

                method.ClearCache = true;

                contentTypeDefinitionField = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
               // method.End();
               // method.WaitForOnAfterCompleted();
            }
            return contentTypeDefinitionField;
        }

        public async Task<ContentTypeDefinitionFieldValue> UpdateAsync(ContentTypeDefinitionFieldValue obj)
        {
            await AuthenticateAndAuthorizeAsync();
            ContentTypeDefinitionFieldValue contentTypeDefinitionField = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Methods.Update.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = obj.ContentTypeDefinitionId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentId.GetIntValue()) { Value = obj.ContentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentTypeDefinitionFieldId.GetIntValue()) { Value = obj.ContentTypeDefinitionFieldId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.Value.GetIntValue()) { Value = obj.Value });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.Order.GetIntValue()) { Value = obj.Order });

                contentTypeDefinitionField = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return contentTypeDefinitionField;
        }


        public async Task<ContentTypeDefinitionFieldValue> SelectAsync(ContentTypeDefinitionFieldValue obj)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionFieldValue;

            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Methods.Select.GetIntValue();

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentId.GetIntValue()) { Value = obj.ContentId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = obj.ContentTypeDefinitionId });

            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }


        public async Task<bool> DeleteAsync(ContentTypeDefinitionFieldValue obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.ContentId.GetIntValue()) { Value = obj.ContentId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.LCID.GetIntValue()) { Value = obj.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Parameters.DateCreated.GetIntValue()) { Value = obj.DateCreated });
                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);

                if (success)
                    obj = null;
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;

        }


    }
}
