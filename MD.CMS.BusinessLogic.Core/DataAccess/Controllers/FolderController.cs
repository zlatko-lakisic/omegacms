using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class FolderController<T> : BaseController<FolderController<T>>
        where T : Content, new()
    {
        public async Task<Folder<T>> CreateAsync(DataRow row, bool fillParent, bool fillAllParents = false, bool fillContentTypeDefinitions = true)
        {
            Folder<T> obj = base.Create<Folder<T>, long>(row, MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.FolderId);
            if (obj != null)
            {

                obj.Name = row.GetValue<string>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.Name);
                obj.Description = row.GetValue<string>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.Description);
                obj.FolderPath = row.GetValue<string>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.FolderPath);
                obj.ParentId = row.GetValue<long>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.ParentId);
                obj.Inherit = row.GetValue<bool>(MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Data.Columns.Inherit);
                if (fillContentTypeDefinitions)
                {
                    obj.ContentTypeDefinitions = await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAsync<T, ContentTypeDefinitionField>(obj);
                }
                if (fillParent && !obj.ParentId.Equals(default))
                {
                    obj.Parent = await FolderController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(obj.ParentId, fillAllParents);
                    obj.EntityPath = obj.Id.ToString();
                    if (obj.Parent != null)
                    {
                        obj.EntityPath = string.Format("{0}_{1}", obj.Parent.EntityPath, obj.Id);
                    }
                }
            }
            return obj;
        }

        public async Task<Folder<T>> GetByIdAsync(long id, bool fillParent = false)
        {
            await AuthenticateAndAuthorizeAsync();

            return (await Execute(new FolderRequestOptions() { 
                FolderIds = new long[] { id }.ToList(),
                FillParent = fillParent
            })).Items.FirstOrDefault();

            /*Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.FolderId.GetIntValue()) { Value = id });
            return await CreateAsync(
                        await ExecuteMethodRowAsync(method, this.UseDefaultPlugin),
                        !id.Equals(default(long))
                    );*/
        }

        public async Task<List<Folder<T>>> GetRootsAsync()
        {
            await AuthenticateAndAuthorizeAsync();
            List<Folder<T>> roots = new List<Folder<T>>();

            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.GetRoots.GetIntValue();


            DataTable result = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            foreach (DataRow row in result.Rows)
            {
                Folder<T> obj = await CreateAsync(row, new FolderRequestOptions() { FillParent = false });
                roots.Add(obj);
            }
            return roots;
        }

        public async Task<Folder<T>> GetFolderByPathAsync(string path = "", bool fillParent = false, bool fillAllParents = false)
        {
            await AuthenticateAndAuthorizeAsync();

            return (await Execute(new FolderRequestOptions()
            {
                Paths = new string[] { path }.ToList(),
                FillParent = fillParent,
                FillAllParents = fillAllParents
            })).Items.FirstOrDefault();

            /*Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.GetFolderByPath.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.FolderPath.GetIntValue()) { Value = path });

            return await CreateAsync(
                await ExecuteMethodRowAsync(method, this.UseDefaultPlugin),
                       fillParent,
                       fillAllParents
                   );*/
        }

        public async Task<List<Folder<T>>> GetByParentIdAsync(long id, int depth = int.MaxValue, bool fillContents = false)
        {
            await AuthenticateAndAuthorizeAsync();

            Entities.Base.BasePaginationEntity<Folder<T>> result = await Execute(new FolderRequestOptions()
            {
                ParentId = id,
                Depth = depth,
                FillContents = fillContents,
                MaxNumberOfRows = int.MaxValue
            });

            await Task.WhenAll(result.Items.Select(item => {
                return Task.Run(async () =>
                {
                    if (depth > 0)
                    {
                        item.Children = await FolderController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByParentIdAsync(item.Id, depth - 1, fillContents).ConfigureAwait(true);
                    }
                });
            }));

            return result.Items;

            /*using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.GetByParentId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.ParentId.GetIntValue()) { Value = id });
                DataTable result = await ExecuteMethodTableAsync(method, UseDefaultPlugin);

                ConcurrentQueue<Folder<T>> folders = new ConcurrentQueue<Folder<T>>();

                await Task.WhenAll(result.AsEnumerable().Select(async row => {

                    Folder<T> obj = await CreateAsync(row, false);
                    await Task.WhenAll(new List<Task> {
                        Task.Run(async () => {
                            if (fillContents)
                            {
                                obj.Contents = await ContentController<T>.GetNewInstance().DefaultPlugin(UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(obj.Id);
                            }
                        }),
                        Task.Run(async () => {
                            if (depth > 0)
                            {
                                obj.Children = await FolderController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByParentIdAsync(obj.Id, depth - 1, fillContents).ConfigureAwait(true);
                            }
                        })
                    });
                    folders.Enqueue(obj);
                }));
                return folders.ToList();
            }*/
        }

        public async Task<Entities.Base.BasePaginationEntity<Folder<T>>> GetByParentIdWithPaginationAsync(long id, int currentPageIndex, int maxNumberOfRows, string searchTerm, int depth = int.MaxValue, bool fillContents = false)
        {
            await AuthenticateAndAuthorizeAsync();

            return await Execute(new FolderRequestOptions()
            {
                ParentId = id,
                CurrentPageIndex = currentPageIndex,
                MaxNumberOfRows = maxNumberOfRows,
                SearchTerm = searchTerm,
                Depth = depth,
                FillContents = fillContents
            });

            /*if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

            ConcurrentQueue<Folder<T>> folders = new ConcurrentQueue<Folder<T>>();
            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.SelectByParentIdWithPagination.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.ParentId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            await Task.WhenAll(table.AsEnumerable().Select(async row => {
                Folder<T> folder = await CreateAsync(row, false, fillContentTypeDefinitions: false);
                if (fillContents)
                {

                    ContentTypeDefinition<ContentTypeDefinitionField> type = (await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderAsync<T, ContentTypeDefinitionField>(folder)).FirstOrDefault();
                    if (type != null && type.Fields.Any(field => field.DataBound))
                    {
                        folder.Contents = await DataBoundContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(folder.Id);
                    }
                    else
                    {
                        folder.Contents = await ContentController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(folder.Id);
                    }
                }
                if (depth > 0)
                {
                    folder.Children = (await FolderController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByParentIdAsync(folder.Id, depth - 1, fillContents)).ToList();
                }
                folders.Enqueue(folder);
            }));
            Entities.Base.BasePaginationEntity<Folder<T>> basePaginationEntity = new Entities.Base.BasePaginationEntity<Folder<T>>();
            basePaginationEntity.Items = folders.ToList();
            if (table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntity;*/
        }

        public async Task<List<Folder<T>>> SearchAsync(string searchTerm, long parentId, bool recursive)
        {
            await AuthenticateAndAuthorizeAsync();

            return (await Execute(new FolderRequestOptions()
            {
                SearchTerm = searchTerm,
                ParentId = parentId,
                FillAllParents = recursive
            })).Items;

            /*searchTerm = searchTerm.Replace("'", "''");

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.Search.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.ParentId.GetIntValue()) { Value = parentId });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            ConcurrentQueue<Folder<T>> list = new ConcurrentQueue<Folder<T>>();
            await Task.WhenAll(new List<Task>(){
                Task.Run(async () =>
                {
                    await Task.WhenAll(results.AsEnumerable().Select(async row => {
                        list.Enqueue(await FolderController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).CreateAsync(row,true,false,true));
                    }));
                }),
                Task.Run(async () =>
                {
                    if (recursive) {
                        await Task.WhenAll((await GetByParentIdAsync(parentId)).Select(async child => {
                            await Task.WhenAll((await FolderController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SearchAsync(searchTerm, child.Id, true)).Select(async result => {
                            list.Enqueue(result);
                            }));
                        }));
                    }
                })
            });

            return list.ToList();*/
        }

        public async Task<int> GetByParentIdCountAsync(long parentId, string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();

            return (await SearchAsync(searchTerm, parentId, false)).Count;

            /*if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.SelectByParentIdCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.ParentId.GetIntValue()) { Value = parentId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("FolderCount");
            return count;*/
        }

        public async Task<EntityHierarchycalCollection<Folder<T>>> GetHierarchyByParentIdAsync(long id, int depth = int.MaxValue)
        {
            await AuthenticateAndAuthorizeAsync();

            return new EntityHierarchycalCollection<Folder<T>>(await GetByParentIdAsync(id, depth));

            /*IEnumerable<Folder<T>> folders = await GetByParentIdAsync(id, depth).ConfigureAwait(false);
            EntityHierarchycalCollection<Folder<T>> list = new EntityHierarchycalCollection<Folder<T>>();
            list.AddRange(folders);
            return list;*/
        }

        private ConcurrentQueue<Folder<T>> childWithInherit = new ConcurrentQueue<Folder<T>>();
        public async Task GetChildsAsync(long ParentId, Folder<T> folder)
        {
            await AuthenticateAndAuthorizeAsync();
            await Task.WhenAll((await GetByParentIdAsync(ParentId, 0)).Select(async child => {
                if (child.Inherit == true)
                {
                    childWithInherit.Enqueue(child);
                    await InsertForChildrensAsync(folder, child);
                    await GetChildsAsync(child.Id, folder);
                }
            }));
        }

        public async Task InsertForChildrensAsync(Folder<T> folder, Folder<T> inheritedChildren)
        {
            await AuthenticateAndAuthorizeAsync();
            Folder<Content> contentFolder = new Folder<Content>();
            contentFolder.Id = inheritedChildren.Id;
            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByFolderIdAsync(inheritedChildren.Id);
            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByFolderIdAsync(inheritedChildren.Id);
            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAllByFolderIdAsync(inheritedChildren.Id);
            await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByFolderAsync(inheritedChildren);
            //MD.CMS.BusinessLogic.Core.DataAccess.Controllers.RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteProfilePermissionByFolderId(inheritedChildren.Id);
            //MD.CMS.BusinessLogic.Core.DataAccess.Controllers.RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteUserPermissionByFolderId(inheritedChildren.Id);

            await Task.WhenAll(new List<Task> {
                Task.WhenAll(folder.MetaDataFields.Select(async item => await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).AssignMetaDataFieldToFolderAsync(inheritedChildren.Id, item))),
                Task.WhenAll(folder.FolderMediaContentMetaDataField.Select(async item => await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).AssignMetaDataFieldToFolderAsync(inheritedChildren.Id, item))),
                Task.WhenAll(folder.ContentTypeDefinitions.Select(async contenttypedefinition => await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(inheritedChildren.Id, contenttypedefinition))),
                Task.WhenAll(folder.Templates.Select(async template => await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).AssignTemplateToFolderAsync(template, inheritedChildren)))
            });
        }

        public async Task<Folder<T>> SaveAsync(Folder<T> folder)
        {
            await AuthenticateAndAuthorizeAsync();
            List<Folder<T>> childsInherited = new List<Folder<T>>();
            Folder<T> newFolder = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
                if (folder.Parent != null)
                {
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.ParentId.GetIntValue()) { Value = folder.Parent.Id });
                }

                if (!folder.IsNew)
                {
                    await GetChildsAsync(folder.Id, folder);
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.Update.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.Name.GetIntValue()) { Value = folder.Name });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.Description.GetIntValue()) { Value = folder.Description });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.ParentId.GetIntValue()) { Value = folder.ParentId });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.Inherit.GetIntValue()) { Value = folder.Inherit });
                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.Insert.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.Name.GetIntValue()) { Value = folder.Name });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.Description.GetIntValue()) { Value = folder.Description });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.ParentId.GetIntValue()) { Value = folder.ParentId });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.Inherit.GetIntValue()) { Value = folder.Inherit });

                }
                method.ClearCache = true;


                newFolder = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), new FolderRequestOptions() { 
                    FillParent = false
                });
                    //await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), false);
                //method.WaitForOnBeforeCompleted();

                if (folder.Inherit == true && !folder.IsNew)
                {
                    folder.MetaDataFields = await FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(folder.ParentId);
                    folder.FolderMediaContentMetaDataField = await FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(folder.ParentId);
                    folder.ContentTypeDefinitions = await ContentTypeDefinitionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByParentIdAsync<ContentTypeDefinitionField>(folder.ParentId);
                    folder.Templates = await TemplateController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByParentIdAsync(folder.ParentId);
                    //RWDPermission rwdPermission = new RWDPermission();
                    //rwdPermission.TargetPrimaryKey = folder.ParentId + "";
                    //rwdPermission.Target = RWDPermissionTarget.Folder;
                    //folder.ProfilePermissions = ProfileTypeController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetAll(rwdPermission);
                    //folder.NotAuthorizedUsers = UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetOnlyNotAuthorizedUsersByFolder(rwdPermission);
                    await GetChildsAsync(folder.Id, folder);

                }

                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByFolderIdAsync(newFolder.Id);
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByFolderIdAsync(newFolder.Id);
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAllByFolderIdAsync(newFolder.Id);
                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByFolderAsync(newFolder);
                //MD.CMS.BusinessLogic.Core.DataAccess.Controllers.RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteProfilePermissionByFolderId(newFolder.Id);
                //MD.CMS.BusinessLogic.Core.DataAccess.Controllers.RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteUserPermissionByFolderId(newFolder.Id);

                foreach (FolderMetaDataField item in folder.MetaDataFields)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).AssignMetaDataFieldToFolderAsync(newFolder.Id, item);
                }

                foreach (FolderMediaContentMetaDataField item in folder.FolderMediaContentMetaDataField)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).AssignMetaDataFieldToFolderAsync(newFolder.Id, item);
                }

                foreach (ContentTypeDefinition<ContentTypeDefinitionField> contenttypedefinition in folder.ContentTypeDefinitions)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(newFolder.Id, contenttypedefinition);
                }

                foreach (Template template in folder.Templates)
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.TemplateController<T>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).AssignTemplateToFolderAsync(template, newFolder);
                }
                Folder<Content> contentFolder = new Folder<Content>();
                contentFolder.Id = newFolder.Id;
                //MD.CMS.BusinessLogic.Core.DataAccess.Controllers.RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveFolderProfileTypePermissions(folder.ProfilePermissions, contentFolder);
                //MD.CMS.BusinessLogic.Core.DataAccess.Controllers.RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveFolderUserPermissions(folder.NotAuthorizedUsers, contentFolder);

                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return newFolder;
        }

        public async Task<bool> DeleteAsync(Folder<T> folder)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success, succesForPermissions;

            /*using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.RWDPermission;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Methods.FolderUserPermissions_DeleteByFolder.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
                succesForPermissions = ExecuteMethodBoolean(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }*/


            using (Method method = new Method())
            {

                success = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentTypeDefinitionFolderController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAllByFolderIdAsync(folder.Id);
                if (!success)
                {
                    return false;
                }
                IEnumerable<Folder<Content>> children = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByParentIdAsync(folder.Id);
                if (children != null && children.Any())
                {
                    foreach (Folder<Content> child in children)
                    {
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderController<Content>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByParentIdAsync(child);
                    }
                }
                List<Content> contents = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(folder.Id);
                if (contents != null && contents.Any())
                {
                    foreach (Content content in contents)
                    {
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.ContentController<Content>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAsync(content);
                    }
                }

                List<FolderMediaContentMetaDataField> folderMediaContentMetaDatafield = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(folder.Id);

                if (folderMediaContentMetaDatafield != null && folderMediaContentMetaDatafield.Any())
                {
                    foreach (FolderMediaContentMetaDataField child in folderMediaContentMetaDatafield)
                    {
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMediaContentMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByFolderIdAsync(folder.Id);
                    }
                }

                List<FolderMetaDataField> foldeMetaDataField = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(folder.Id);
                if (foldeMetaDataField != null && foldeMetaDataField.Any())
                {
                    foreach (FolderMetaDataField child in foldeMetaDataField)
                    {
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.FolderMetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByFolderIdAsync(folder.Id);
                    }
                }

                List<MediaContent> mediaContent = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByFolderIdAsync(folder.Id);
                if (mediaContent != null && mediaContent.Any())
                {
                    foreach (MediaContent child in mediaContent)
                    {
                        await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteAsync(child);
                    }
                }
                //method.WaitForOnBeforeCompleted();



                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.Id.GetIntValue()) { Value = folder.Id });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);

                method.End();
                //method.WaitForOnAfterCompleted();

            }
            return success;
        }

        public async Task<bool> DeleteByParentIdAsync(Folder<T> obj)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success;
            using (Method method = new Method())
            {

                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.Folder;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods.DeleteByParentId.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Parameters.ParentId.GetIntValue()) { Value = obj.ParentId });
                method.ClearCache = true;

                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);

                if (success)
                    obj = null;
                method.End();
                //method.WaitForOnAfterCompleted();
            }

            return success;
        }

        public async Task<bool> IsAuthorizedAsync(User user, Folder<Content> folder, RWDPermissionType permission)
        {
            bool isAuthorizedprofile = false;
            //RWDPermission result = null;
            if (user != null && folder != null)
            {
                using (Method method = new Method())
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                    method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.RWDPermission;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Methods.GetFolderUserPermissionByFolderAndUser.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.UserId.GetIntValue()) { Value = user.Id });

                    //result = RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).Create(ExecuteMethodRow(method, this.UseDefaultPlugin), Enumerations.RWDPermissionTarget.Folder);
                    method.End();
                }

                /*if (result == null)
                {
                    foreach (var x in user.ProfileTypes)
                    {
                        using (Method method = new Method())
                        {
                            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.RWDPermission;
                            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Methods.GetFolderProfileTypePermissionByFolderAndProfileType.GetIntValue();
                            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.FolderId.GetIntValue()) { Value = folder.Id });
                            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.ProfileTypeId.GetIntValue()) { Value = x.Id });

                            result = RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).Create(ExecuteMethodRow(method, this.UseDefaultPlugin), Enumerations.RWDPermissionTarget.Folder);
                            method.End();
                            //method.WaitForOnAfterCompleted();
                        }
                        if (result == null)
                            return true;
                        else
                        {
                            isAuthorizedprofile = true;
                            switch (permission)
                            {
                                case RWDPermissionType.Read: if (result.Read == true)
                                    {
                                        return true;
                                    }
                                    break;
                                case RWDPermissionType.Write: if (result.Write == true)
                                    {
                                        return true;
                                    }
                                    break;
                                case RWDPermissionType.Delete: if (result.Delete == true)
                                    {
                                        return true;
                                    }
                                    break;
                            }
                        }
                    }
                    if (isAuthorizedprofile == false)
                        return true;
                    return false;
                }
                else
                {
                    switch (permission)
                    {
                        case RWDPermissionType.Read: if (result.Read == true)
                            {
                                return true;
                            }
                            else { return false; }
                        case RWDPermissionType.Write: if (result.Write == true)
                            {
                                return true;
                            }
                            else
                                return false;
                        case RWDPermissionType.Delete: if (result.Delete == true)
                            {
                                return true;
                            }
                            else
                                return false;
                    }
                }*/
                return true;
            }
            //user and folder is null
            else
            {
                return false;
            }
        }
    }
}
