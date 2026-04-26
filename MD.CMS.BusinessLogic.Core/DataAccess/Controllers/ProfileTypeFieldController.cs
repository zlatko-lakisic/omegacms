using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System.Xml;
using System;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.Helpers.Calculations;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ProfileTypeFieldController : BaseController<ProfileTypeFieldController>
    {
        public async Task<ProfileTypeField> CreateAsync(DataRow row, bool transformExpression = true)
        {
            ProfileTypeField obj = base.Create<ProfileTypeField, long>(row, "ProfileTypeFieldId");
            if (obj != null)
            {

                obj.Id = row.GetValue<long>("ProfileTypeFieldId");
                obj.ProfileTypeId = row.GetValue<long>("ProfileTypeId");
                obj.AttributeTypeDefinitionId = row.GetValue<long>("AttributeTypeDefinitionId");
                if (obj.AttributeTypeDefinitionId != default(int))
                {
                    AttributeTypeDefinition attributeTypeDefinition = await AttributeTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(obj.AttributeTypeDefinitionId);
                    obj.AttributeTypeDefinition = attributeTypeDefinition;
                }
                obj.Name = row.GetValue<string>("Name");
                obj.DefaultValue = row.GetValue<string>("DefaultValue");
                obj.Description = row.GetValue<string>("Description");
                obj.ListValue = row.GetValue<string>("ListValue");
                obj.Order = row.GetValue<int>("Order");
                obj.Delimiter = row.GetValue<string>("Delimiter");
                obj.Options =row.GetValue<string>("Options");
                //transform infix expression to postfix expression (because it's much easier to work with postfix)
                //this should not happened only if content type definition edit is called
                if (obj.AttributeTypeDefinitionId == 18 && !String.IsNullOrEmpty(obj.DefaultValue) && transformExpression == true)
                {
                    PostfixMaker maker = new PostfixMaker();
                    string postfixExpression = maker.MakePostfixFromInfix(obj.DefaultValue);
                    obj.DefaultValue = postfixExpression;
                }
            }
            return obj;
        }

        public async Task<ProfileTypeField> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Id.GetIntValue()) { Value = id });
            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }


        public async Task<List<ProfileTypeField>> GetByProfileTypeAsync(ProfileType obj, bool transformExpression = true)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ProfileTypeField> fields = new List<ProfileTypeField>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods.GetByProfileType.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.ProfileTypeId.GetIntValue()) { Value = obj.Id });


            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                fields.Add(await CreateAsync(row,transformExpression:transformExpression));
            }
            return fields;
        }


        public async Task<ProfileTypeField> SaveAsync(ProfileTypeField obj)
        {
            await AuthenticateAndAuthorizeAsync();
            ProfileTypeField profileTypeField = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods.GetByProfileType.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.ProfileTypeId.GetIntValue()) { Value = obj.ProfileTypeId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.AttributeTypeDefinitionId.GetIntValue()) { Value = obj.AttributeTypeDefinitionId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Name.GetIntValue()) { Value = obj.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Description.GetIntValue()) { Value = obj.Description });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.DefaultValue.GetIntValue()) { Value = obj.DefaultValue });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.ListValue.GetIntValue()) { Value = obj.ListValue });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Delimiter.GetIntValue()) { Value = obj.Delimiter });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Order.GetIntValue()) { Value = obj.Order });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Options.GetIntValue()) { Value = obj.Options });


                if (obj.IsNew)
                {
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods.Insert.GetIntValue();
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                }
                else
                {
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods.Update.GetIntValue();
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Id.GetIntValue()) { Value = obj.Id });
                }
                method.ClearCache = true;

                profileTypeField = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return profileTypeField;
        }

        public async Task<bool> DeleteAsync(ProfileTypeField obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Id.GetIntValue()) { Value = obj.Id });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();

            }

            return success;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.Id.GetIntValue()) { Value = id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<bool> DeleteAllByProfileTypeIdAsync(long profileTypeId)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success = false;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ProfileTypeField;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods.DeleteAllByProfileTypeId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Parameters.ProfileTypeId.GetIntValue()) { Value = profileTypeId });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();

            }

            return success;
        }
    }
}
