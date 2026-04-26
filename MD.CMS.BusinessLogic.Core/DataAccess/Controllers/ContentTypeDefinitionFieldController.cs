using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ContentTypeDefinitionFieldController : BaseController<ContentTypeDefinitionFieldController>
    {
        public async Task<ContentTypeDefinitionField> CreateAsync(DataRow row, bool transformExpression = true)
        {
            ContentTypeDefinitionField obj = base.Create<ContentTypeDefinitionField, long>(row, ContentTypeDefinitionFieldEnum.ContentTypeDefinitionFieldId.GetStringValue());
            if (obj != null)
            {
                obj.Id = row.GetValue<long>(ContentTypeDefinitionFieldEnum.ContentTypeDefinitionFieldId.GetStringValue());
                obj.ContentTypeDefinitionId = row.GetValue<long>(ContentTypeDefinitionFieldEnum.ContentTypeDefinitionId.GetStringValue());
                obj.AttributeTypeDefinitionId = row.GetValue<long>(ContentTypeDefinitionFieldEnum.AttributeTypeDefinitionId.GetStringValue());
                obj.AttributeTypeDefinition = await AttributeTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(obj.AttributeTypeDefinitionId);
                obj.Name = row.GetValue<string>(ContentTypeDefinitionFieldEnum.Name.GetStringValue());
                obj.Description = row.GetValue<string>(ContentTypeDefinitionFieldEnum.Description.GetStringValue());
                obj.DefaultValue = row.GetValue<string>(ContentTypeDefinitionFieldEnum.DefaultValue.GetStringValue());
                obj.Order = row.GetValue<int>(ContentTypeDefinitionFieldEnum.Order.GetStringValue());
                obj.Options = row.GetValue<string>(ContentTypeDefinitionFieldEnum.Options.GetStringValue());
                obj.ListValue = row.GetValue<string>(ContentTypeDefinitionFieldEnum.ListValue.GetStringValue());
                obj.Delimiter = row.GetValue<string>(ContentTypeDefinitionFieldEnum.Delimiter.GetStringValue());

                //transform infix expression to postfix expression (because it's much easier to work with postfix)
                //this should not happened only if content type definition edit is called
                if (obj.AttributeTypeDefinitionId == 18 && !String.IsNullOrEmpty(obj.DefaultValue) && transformExpression == true)
                {
                    /*PostfixMaker maker = new PostfixMaker();
                    string postfixExpression = maker.MakePostfixFromInfix(obj.DefaultValue);
                    obj.DefaultValue = postfixExpression;*/
                    obj.DefaultValue = obj.DefaultValue.Trim().Trim(',');
                }

				obj.DataBound = row.GetValue<bool>(ContentTypeDefinitionFieldEnum.DataBound.GetStringValue());
				obj.DataSourceId = row.GetValue<long>(ContentTypeDefinitionFieldEnum.DataSourceId.GetStringValue());
				obj.DataSourceField = row.GetValue<string>(ContentTypeDefinitionFieldEnum.DataSourceField.GetStringValue());
                obj.DataBoundReadOnly = row.GetValue<bool>(ContentTypeDefinitionFieldEnum.DataBoundReadOnly.GetStringValue());
                obj.IsDataBoundPrimaryKey = row.GetValue<bool>(ContentTypeDefinitionFieldEnum.IsDataBoundPrimaryKey.GetStringValue());
            }
            return obj;
        }

        public async Task<ContentTypeDefinitionField> GetByIdAsync(long id, bool transformExpression = true)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.ContentTypeDefinitionFieldId.GetIntValue()) { Value = id });
            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), transformExpression);
        }

        public async Task<List<ContentTypeDefinitionField>> GetByContentTypeDefinitionIdAsync(long contentTypeDefinitionId, bool transformExpression = true)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Methods.GetByContentTypeDefinition.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = contentTypeDefinitionId });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            ConcurrentQueue<ContentTypeDefinitionField> list = new ConcurrentQueue<ContentTypeDefinitionField>();
            await Task.WhenAll(table.AsEnumerable().Select(async row => {
                list.Enqueue(await ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row));
            }));
            return list.ToList();
        }



        public async Task<ContentTypeDefinitionField> SaveAsync(ContentTypeDefinitionField obj)
        {
            await AuthenticateAndAuthorizeAsync();
            ContentTypeDefinitionField newObj = null;          

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionField;
                obj.Serialize();
                if (obj.IsNew)
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Methods.Insert.GetIntValue();
				}
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.Id.GetIntValue()) { Value = obj.Id });

				}
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.ContentTypeDefinitionId.GetIntValue()) { Value = obj.ContentTypeDefinitionId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.AttributeTypeDefinitionId.GetIntValue()) { Value = obj.AttributeTypeDefinitionId });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.Name.GetIntValue()) { Value = obj.Name });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.Description.GetIntValue()) { Value = obj.Description });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.DefaultValue.GetIntValue()) { Value = obj.DefaultValue });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.Order.GetIntValue()) { Value = obj.Order });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.Options.GetIntValue()) { Value = obj.Options });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.ListValue.GetIntValue()) { Value = obj.ListValue });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.Delimiter.GetIntValue()) { Value = obj.Delimiter });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.DataBound.GetIntValue()) { Value = obj.DataBound });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.DataSourceId.GetIntValue()) { Value = obj.DataSourceId.Equals(default(long)) ? null : obj.DataSourceId.ToString() });
				method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.DataSourceField.GetIntValue()) { Value = obj.DataSourceField });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.DataBoundReadOnly.GetIntValue()) { Value = obj.DataBoundReadOnly });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.IsDataBoundPrimaryKey.GetIntValue()) { Value = obj.IsDataBoundPrimaryKey });

                newObj = await CreateAsync(await ExecuteMethodRowAsync(method));

                method.End();
                //method.WaitForOnAfterCompleted();
                
            }
            return newObj;
        }

        public async Task<bool> DeleteAsync(ContentTypeDefinitionField obj)
        {
            return await DeleteAsync(obj.Id);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ContentTypeDefinitionField;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Parameters.ContentTypeDefinitionFieldId.GetIntValue()) { Value = id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
            }
            return success;
        }
    }
}
