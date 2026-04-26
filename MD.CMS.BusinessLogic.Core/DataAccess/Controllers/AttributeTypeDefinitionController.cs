using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.Tools.BaseDataAccess.Plugins.Core;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class AttributeTypeDefinitionController : BaseController<AttributeTypeDefinitionController>
    {
        public AttributeTypeDefinition Create(DataRow row)
        {
            AttributeTypeDefinition obj = base.Create<AttributeTypeDefinition, long>(row, AttributeTypeDefinitionEnum.AttributeTypeDefinitionId.GetStringValue());
            if (obj != null)
            {
                obj.DefaultValue = row.GetValue<string>(AttributeTypeDefinitionEnum.DefaultValue.GetStringValue(), "");
                obj.InputType = (AttributeTypeDefinition.EnumInputType)row.GetValue<int>(AttributeTypeDefinitionEnum.InputType.GetStringValue());
                obj.Type = (AttributeTypeDefinition.EnumType)row.GetValue<int>(AttributeTypeDefinitionEnum.Type.GetStringValue());
                obj.Name = row.GetValue<string>(AttributeTypeDefinitionEnum.Name.GetStringValue());                     
            }         
            return obj;
        }

        public async Task<AttributeTypeDefinition> GetByInputTypeIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.AttributeTypeDefinition;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Methods.GetByInputType.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Parameters.InputType.GetIntValue()) { Value = id });
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;

            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<AttributeTypeDefinition> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.AttributeTypeDefinition;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Parameters.AttributeTypeDefinitionId.GetIntValue()) { Value = id });
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;

            return Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));          
        }

        public async Task<List<AttributeTypeDefinition>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();

            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.AttributeTypeDefinition;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Methods.GetAll.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Parameters.AttributeTypeDefinitionId.GetIntValue()) { Value = DBNull.Value });
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            ConcurrentQueue<AttributeTypeDefinition> list = new ConcurrentQueue<AttributeTypeDefinition>();
            await Task.WhenAll(results.AsEnumerable().Select(async row => {
                list.Enqueue(Create(row));
            }));

            return list.ToList();
        }
    }
}
