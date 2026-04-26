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
    public partial class MetaDataFieldController : BaseController<MetaDataFieldController>
    {
        public async Task<MetaDataField> CreateAsync(DataRow row)
        {
            MetaDataField obj = base.Create<MetaDataField, long>(row, MetaDataFieldEnum.MetaDataFieldId.GetStringValue());
            if (obj != null)
            {
                obj.AttributeTypeDefinitionId = row.GetValue<long>(MetaDataFieldEnum.AttributeTypeDefinitionId.GetStringValue());
                obj.Name = row.GetValue<string>(MetaDataFieldEnum.Name.GetStringValue());
                obj.AttributeTypeDefinition = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.AttributeTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(obj.AttributeTypeDefinitionId);
                obj.ListValue = row.GetValue<string>(MetaDataFieldEnum.ListValue.GetStringValue());
                obj.Delimiter = row.GetValue<string>(MetaDataFieldEnum.Delimiter.GetStringValue());
                obj.DefaultValue = row.GetValue<string>(MetaDataFieldEnum.DefaultValue.GetStringValue());
                //obj.MetaDataFieldId = row.GetValue<long>(MetaDataFieldEnum.MetaDataFieldId.GetStringValue());

                // obj.IsRequired = row.GetValue<int>(MetaDataFieldEnum.IsRequired.GetStringValue());
            }
            return obj;
        }

        public async Task<MetaDataField> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.Id.GetIntValue()) { Value = id });

            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<List<MetaDataField>> GetByFolderAsync<T>(Folder<T> folder) where T : Content, new()
        {
            return await GetByFolderIdAsync(folder.Id);
        }

        public async Task<List<MetaDataField>> GetByFolderIdAsync(long folderId)
        {
            await AuthenticateAndAuthorizeAsync();
            List<MetaDataField> metaDataFields = new List<MetaDataField>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.GetByFolder.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.FolderId.GetIntValue()) { Value = folderId });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            foreach (DataRow row in results.Rows)
            {
                metaDataFields.Add(await CreateAsync(row));
            }
            return metaDataFields;
        }

        public async Task<List<MetaDataField>> GetAllAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<MetaDataField> list = new List<MetaDataField>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.GetAll.GetIntValue();
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                list.Add(await CreateAsync(row));
            }
            return list;
        }

        public async Task<bool> DeleteAsync(MetaDataField obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.Id.GetIntValue()) { Value = obj.Id });

                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }

            return success;
        }

        public async Task<MetaDataField> SaveAsync(MetaDataField obj)
        {
            await AuthenticateAndAuthorizeAsync();
            MetaDataField metaDataField = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.AttributeTypeDefinitionId.GetIntValue()) { Value = obj.AttributeTypeDefinitionId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.Name.GetIntValue()) { Value = obj.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.ListValue.GetIntValue()) { Value = obj.ListValue });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.Delimiter.GetIntValue()) { Value = obj.Delimiter });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.DefaultValue.GetIntValue()) { Value = obj.DefaultValue });

                if (obj.IsNew)
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.Insert.GetIntValue();
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.Id.GetIntValue()) { Value = obj.Id });
                }
                method.ClearCache = true;

                metaDataField = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return metaDataField;
        }

        public async Task<Entities.Base.BasePaginationEntity<MetaDataField>> GetAllWithPaginationAsync(int currentPageIndex, int maxNumberOfRows, string searchTerm, string searchColumn, string sort = "Name ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            List<MetaDataField> metaData = new List<MetaDataField>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.SelectAllWithPagination.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.Sort.GetIntValue()) { Value = sort });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                MetaDataField metadata = await CreateAsync(row);
                metaData.Add(metadata);
            }
            Entities.Base.BasePaginationEntity<MetaDataField> basePaginationEntity = new Entities.Base.BasePaginationEntity<MetaDataField>();
            basePaginationEntity.Items = metaData;
            basePaginationEntity.TotalCount = table.Rows.Count > 0 ? table.Rows[0].GetValue<int>("TotalCount") : 0;
            return basePaginationEntity;
        }

        public async Task<int> SelectAllCountAsync(string searchTerm, string searchColumn)
        {
            await AuthenticateAndAuthorizeAsync();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.SelectAllCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("MetaDataCount");
            return count;
        }

        public async Task<IEnumerable<MetaDataField>> MetaDataMediaContentGetByFolderIdAsync<T>(Folder<T> folder) where T : Content, new()
        {
            await AuthenticateAndAuthorizeAsync();
            List<MetaDataField> metaDataFields = new List<MetaDataField>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.MetaDataMediaContentGetByFolderId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.FolderId.GetIntValue()) { Value = folder.Id });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            foreach (DataRow row in results.Rows)
            {
                metaDataFields.Add(await CreateAsync(row));
            }
            return metaDataFields;
        }

        public async Task<List<MetaDataField>> SearchAsync(string searchTerm, string searchColumn)
        {
            await AuthenticateAndAuthorizeAsync();
            List<MetaDataField> searchResults = new List<MetaDataField>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MetaDataField;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods.Search.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                searchResults.Add(await CreateAsync(row));
            }
            return searchResults;

        }
    }
}
