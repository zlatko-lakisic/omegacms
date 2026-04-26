using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class ReportDefinitionController : BaseController<ReportDefinitionController>
    {
        private async Task<ReportDefinition> CreateAsync(DataRow row)
        {
            ReportDefinition obj = Create<ReportDefinition, long>(row, Data.Columns.ReportDefinitionId, Data.Columns.IsDeleted);

            if (obj != null)
            {
                obj.Name = row.GetValue<string>(Data.Columns.Name);
                obj.Json = row.GetValue<string>(Data.Columns.ReportDefinitionJson);
                obj.Sql = row.GetValue<string>(Data.Columns.ReportDefinitionSql);
                obj.AuthorId = row.GetValue<string>(Data.Columns.AuthorId);
                obj.DateCreated = row.GetValue<DateTime>(Data.Columns.DateCreated);
                obj.DateModified = row.GetValue<DateTime>(Data.Columns.DateUpdated);
                if (!obj.AuthorId.Equals(default(long)))
                {
                    obj.Author = await UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(obj.AuthorId, true);
                }

                if (obj.Definition != null) {
                    List<Entity> entities = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ReportDefinitionController.GetNewInstance().DefaultPlugin(UseDefaultPlugin).Caller(UserMakingTheCall).GetAllEntitiesAsync();

                    obj.Definition.Entities = obj.Definition.Entities.Where(e => entities.Any(entity => string.Compare(entity.Name, e.Name, true).Equals(0))).ToList();

                    foreach (Entity entity in obj.Definition.Entities)
                    {
                        Entity original = entities.FirstOrDefault(e => e.Name.StartsWith(entity.Name));
                        if(original != null)
                        {
                            entity.Fields = entity.Fields.Where(f => original.Fields.Any(field => string.Compare(field.Name, f.Name, true).Equals(0))).ToList();
                            entity.Fields.AddRange(original.Fields.Where(f => !entity.Fields.Any(field => string.Compare(field.Name, f.Name, true).Equals(0))));

                            entity.ExtendedFields = entity.ExtendedFields.Where(f => original.ExtendedFields.Any(field => string.Compare(field.Name, f.Name, true).Equals(0))).ToList();
                            entity.ExtendedFields.AddRange(original.ExtendedFields.Where(f => !entity.ExtendedFields.Any(field => string.Compare(field.Name, f.Name, true).Equals(0))));
                        }
                    }

                    obj.Definition.Joins = obj.Definition.Joins.Where(j => entities.Any(e =>
                        (e.Name.StartsWith(j.Left.Entity.Name) && e.Fields.Any(f => string.Compare(f.Name, j.Left.Property.Name).Equals(0))) ||
                        (e.Name.StartsWith(j.Right.Entity.Name) && e.Fields.Any(f => string.Compare(f.Name, j.Right.Property.Name).Equals(0)))
                    )).ToList();

                    obj.Definition.Filters = obj.Definition.Filters.Where(filter => entities.Any(e =>
                        e.Name.StartsWith(filter.Entity.Name) && e.Fields.Any(f => string.Compare(f.Name, filter.Property.Name).Equals(0))
                    )).ToList();
                }
            }

            return obj;
        }

        public async Task<ReportDefinition> SaveAsync(ReportDefinition definition)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {

                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportDefinition;
                if (definition.Id.Equals(default(long)))
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Methods.Insert.GetIntValue();
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.ReportdefinitionId.GetIntValue()) { Value = definition.Id });
                }
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.AuthorId.GetIntValue()) { Value = definition.AuthorId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.Name.GetIntValue()) { Value = definition.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.ReportDefinitionJson.GetIntValue()) { Value = definition.Json });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.ReportDefinitionSql.GetIntValue()) { Value = definition.Sql });
                method.ClearCache = true;

                definition = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
            return definition;
        }

        public async Task<List<ReportDefinition>> GetAllAsync(string sort = "Name ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportDefinition> definitions = new List<ReportDefinition>();
            using (Method method = new Method())
            {

                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportDefinition;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Methods.GetAll.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });

                definitions = (await Task.WhenAll((await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(async row => await CreateAsync(row)))).ToList();

            } 
            return definitions;
        }

        public async Task<Entities.Base.BasePaginationEntity<ReportDefinition>> GetAllWithPaginationAsync(long pageIndex, long pageSize, string searchTerm, string searchColumn, string sort)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportDefinition> definitions = new List<ReportDefinition>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportDefinition;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Methods.GetAllWithPagination.GetIntValue();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = pageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = pageSize });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Sorting.Parameters.SortField.GetIntValue()) { Value = sort });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach(DataRow row in table.Rows)
            {
                definitions.Add(await CreateAsync(row));
            }
            Entities.Base.BasePaginationEntity<ReportDefinition> basePaginationEntity = new Entities.Base.BasePaginationEntity<ReportDefinition>();
            basePaginationEntity.Items = definitions;
            if(table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntity;
        }

        public async Task<long> GetAllCountAsync(string searchTerm, string searchColumn)
        {
            await AuthenticateAndAuthorizeAsync();
            long count = 0;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportDefinition;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Methods.GetAllCount.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });

                DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
                count = row.GetValue<long>("ReportDefinitionsCount");
            }
            return count;
        }

        public async Task<ReportDefinition> GetByIdAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportDefinition;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Methods.GetById.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.ReportdefinitionId.GetIntValue()) { Value = id });
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));

            }
        }

        public async Task<bool> DeleteAsync(ReportDefinition obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportDefinition;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.ReportdefinitionId.GetIntValue()) { Value = obj.Id });
                method.ClearCache = true;

                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);

                if (success)
                    obj = null;
                method.End();
            }
            return success;
        }

        public async Task<List<ReportDefinition>> SearchAsync(string searchTerm, string searchColumn)
        {
            await AuthenticateAndAuthorizeAsync();
            List<ReportDefinition> definitions = new List<ReportDefinition>();
            using (Method method = new Method())
            {

                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.ReportDefinition;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Methods.Search.GetIntValue();
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.Parameters.SearchColumn.GetIntValue()) { Value = searchColumn });

                definitions = (await Task.WhenAll((await ExecuteMethodTableAsync(method, this.UseDefaultPlugin)).Rows.Cast<DataRow>().Select(async row => await CreateAsync(row)))).ToList();
            }
            return definitions;
        }

        public async Task<List<Entity>> GetAllEntitiesAsync()
        {
            List<Entity> entities = new List<Entity>();

            Entity entityContent = new Entity()
            {
                Coordinates = new Entity.GridCoordinates(),
                Name = "Content",
                Type = MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.Enumerations.EntityType.Content,
                Id = 0
            };
            entityContent.AddBasicFields<Content>();
            entities.Add(entityContent);

            IEnumerable<ContentTypeDefinition<ContentTypeDefinitionField>> customTypes = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetAllAsync<ContentTypeDefinitionField>();
            IEnumerable<Entity> customTypeEntities = customTypes.Select(def =>
            {
                Entity entity = new Entity()
                {
                    Coordinates = new Entity.GridCoordinates(),
                    Name = def.Name,
                    Type = MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.Enumerations.EntityType.Content,
                    Id = def.Id,
                    Icon = def.Icon
                };
                entity.AddBasicFields<Content>();
                return entity;
            });

            foreach(Entity en in customTypeEntities)
            {
                IEnumerable<ContentTypeDefinitionField> fields = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByContentTypeDefinitionIdAsync(en.Id);
                en.ExtendedFields.AddRange(fields.Select(field => new Property(field.Name, (int)field.AttributeTypeDefinition.Type)));
                entities.Add(en);
            }

            IEnumerable<ProfileType> customProfiles = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetAllAsync();
            IEnumerable<Entity> customProfilesEntities = customProfiles.Select(def =>
            {
                Entity entity = new Entity()
                {
                    Coordinates = new Entity.GridCoordinates(),
                    Name = def.Name,
                    Type = MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.Enumerations.EntityType.User,
                    Id = def.Id
                };
                entity.AddBasicFields<User>();
                return entity;
            });

            foreach (Entity en in customProfilesEntities)
            {
                IEnumerable<ProfileTypeField> fields = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ProfileTypeFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByProfileTypeAsync(new ProfileType() { Id = en.Id });
                en.ExtendedFields.AddRange(fields.Select(field => new Property(field.Name, (int)field.AttributeTypeDefinition.Type)));
                entities.Add(en);
            }

            Entity entityMediaContent = new Entity()
            {
                Coordinates = new Entity.GridCoordinates(),
                Name = "Media Content",
                Type = MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.Enumerations.EntityType.MediaContent,
                Id = 0
            };
            entityMediaContent.AddBasicFields<MediaContent>();
            entities.Add(entityMediaContent);

            Entity entityTaxonomy = new Entity()
            {
                Coordinates = new Entity.GridCoordinates(),
                Name = "Taxonomy",
                Type = MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.Enumerations.EntityType.Taxonomy,
                Id = 0
            };
            entityTaxonomy.AddBasicFields<Taxonomy>();
            entities.Add(entityTaxonomy);

            Entity entityFolder = new Entity()
            {
                Coordinates = new Entity.GridCoordinates(),
                Name = "Folder",
                Type = MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.Enumerations.EntityType.Folder,
                Id = 0
            };
            entityFolder.AddBasicFields<Folder<Content>>();
            entities.Add(entityFolder);

            return entities;
        }
    }
}
