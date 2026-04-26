using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using System.Globalization;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MediaContentController : BaseController<MediaContentController>
    {
        public async Task<MediaContent> CreateAsync(DataRow row, bool full = false)
        {
            MediaContent obj = new MediaContent();
            if (row != null)
            {
                obj.Id = row.GetValue<int>(MediaContentEnum.MediaContentId.GetStringValue());
                obj.LCID = row.GetValue<int>(MediaContentEnum.LCID.GetStringValue());
                obj.FolderId = row.GetValue<long>(MediaContentEnum.FolderId.GetStringValue());
                obj.FileType = row.GetValue<int>(MediaContentEnum.FileType.GetStringValue());
                obj.Size = row.GetValue<string>(MediaContentEnum.Size.GetStringValue());
                obj.Path = row.GetValue<string>(MediaContentEnum.Path.GetStringValue());
                obj.Name = row.GetValue<string>(MediaContentEnum.Name.GetStringValue());
                obj.Description = row.GetValue<string>(MediaContentEnum.Description.GetStringValue());
                obj.IsDeleted = row.GetValue<bool>("IsDeleted");
                obj.PreviewUrl = row.GetValue<string>(MediaContentEnum.FullNameFile.GetStringValue());
                obj.FullNameFile = row.GetValue<string>(MediaContentEnum.FullNameFile.GetStringValue());
				obj.InputType = (MediaContent.EnumInputType)row.GetValue<int>(MediaContentEnum.FileType.GetStringValue());
                if (full)
                {
                    if (obj.MediaContentMetaDataFieldValues == null)
                    {
                        Folder<Content> folder = await FolderController<Content>.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByIdAsync(obj.FolderId);
                        obj.MediaContentMetaDataFieldValues = new List<MediaContentMetaDataFieldValues>();
                        IEnumerable<MetaDataField> metaDataFields = await MetaDataFieldController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).MetaDataMediaContentGetByFolderIdAsync(folder);
                        List<MediaContentMetaDataFieldValues> metaDataFieldValues = await MediaContentMetaDataFieldValuesController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetByMediaContentAsync(obj);
                        foreach (MetaDataField field in metaDataFields)
                        {
                            MediaContentMetaDataFieldValues metaDataFieldValue = new MediaContentMetaDataFieldValues(field);
                            metaDataFieldValue.Id = field.Id;
                            metaDataFieldValue.MediaContentId = obj.Id;
                            metaDataFieldValue.MetaDataFieldId = field.Id;
                            metaDataFieldValue.DateCreated = obj.DateCreated;

                            foreach (MediaContentMetaDataFieldValues fieldValue in metaDataFieldValues)
                            {
                                if (field.Id == fieldValue.MetaDataFieldId)
                                {
                                    metaDataFieldValue.Value = fieldValue.Value;
                                    metaDataFieldValue.DateCreated = fieldValue.DateCreated;
                                }
                            }
                            obj.MediaContentMetaDataFieldValues.Add(metaDataFieldValue);
                        }
                    }
                }
                obj.DateCreated = row.GetValue<DateTime>("DateCreated").ToString(CultureInfo.InvariantCulture);
            }
            return obj;
        }

        public async Task<MediaContent> GetByIdAsync(long id, int lcid = default(int), bool fillMetaDataFields = false)
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = DataAccessSettings.SelectedLcid;      
            }

            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.GetById.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.MediaContentId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MediaContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), fillMetaDataFields);

        }

        public async Task<long> SelectAllCountAsync(int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.SelectAllCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MediaContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("MediaContentCount");
            return count;

            //return ExecuteCommandRow(
            //                GenerateStoredProcedure(MediaContentSPEnum.SelectAllCount.GetStringValue(),
            //                                            new MySqlParameter() { ParameterName = MediaContentParametersEnum.LCID.GetStringValue(), DbType = DbType.Int32, Value = lcid }
            //                )
            //            ).GetValue<long>(MediaContentEnum.MediaContentId.GetStringValue());
        }

        public async Task<List<MediaContent>> GetByFolderIdAsync(long id, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            List<MediaContent> contents = new List<MediaContent>();


            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.GetByFolderId.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FolderId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MediaContentId_i" });

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                contents.Add(await CreateAsync(row));
            }
            return contents;
        }

        public async Task<List<MediaContent>> GetByFileTypeAsync(long id, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            List<MediaContent> contents = new List<MediaContent>();


            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.GetByFileType.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FileType.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MediaContentId_i" });

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                contents.Add(await CreateAsync(row, true));
            }
            return contents;
        }

        public async Task<List<MediaContent>> SearchByFileTypeAsync(string searchTerm, int fileType, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            List<MediaContent> contents = new List<MediaContent>();


            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.SearchByFileType.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FileType.GetIntValue()) { Value = fileType });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MediaContentId_i" });

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                contents.Add(await CreateAsync(row, true));
            }
            return contents;
        }

        public async Task<List<MediaContent>> SearchAsync(string searchTerm, int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            List<MediaContent> searchResults = new List<MediaContent>();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.Search.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in results.Rows)
            {
                searchResults.Add(await CreateAsync(row));
            }
            return searchResults;
        }

        public async Task<bool> DeleteAsync(MediaContent mediaContent)
        {
            await AuthenticateAndAuthorizeAsync();
            bool success, succesForPermissions;

            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.RWDPermission;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Methods.MediaCntUserPerm_DeletePermissionByMediaCnt.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentId.GetIntValue()) { Value = mediaContent.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentDateCreaded.GetIntValue()) { Value = mediaContent.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentLCID.GetIntValue()) { Value = mediaContent.LCID });
                method.ClearCache = true;

                succesForPermissions = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }


            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.RWDPermission;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Methods.MediaCntProfileTypePerms_DeleteByMediaCnt.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentId.GetIntValue()) { Value = mediaContent.Id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentDateCreaded.GetIntValue()) { Value = mediaContent.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentLCID.GetIntValue()) { Value = mediaContent.LCID });
                succesForPermissions = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                method.End();
                //method.WaitForOnAfterCompleted();
            }


            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Delete;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.Delete.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.MediaContentId.GetIntValue()) { Value = mediaContent.Id });
                //method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.DateCreated.GetIntValue()) { Value = mediaContent.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = mediaContent.LCID });
                success = await ExecuteMethodBooleanAsync(method, this.UseDefaultPlugin);
                //method.WaitForOnBeforeCompleted();

                List<MediaContentMetaDataFieldValues> mediaContentMetaDataFieldValues = await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentMetaDataFieldValuesController.GetNewInstance().Caller(UserMakingTheCall).GetByMediaContentAsync(mediaContent);
                if (mediaContentMetaDataFieldValues != null && mediaContentMetaDataFieldValues.Any())
                {
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentMetaDataFieldValuesController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).DeleteByMediaContentAsync(mediaContent);
                }
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return success;
        }

        public async Task<List<MediaContent>> GetAllAsync(int lcid = default(int))
        {
            await AuthenticateAndAuthorizeAsync();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            List<MediaContent> folders = new List<MediaContent>();

            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.GetAll.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MediaContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataTable results = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);

            foreach (DataRow row in results.Rows)
            {
                MediaContent obj = await CreateAsync(row, false);
                folders.Add(obj);
            }
            return folders;
        }

        public async Task<MediaContent> SaveAsync(MediaContent mediaContent)
        {
            await AuthenticateAndAuthorizeAsync();
            DateTime date = new DateTime(0001, 1, 1);


            if (mediaContent.DateCreated == date.ToString())
            {
                mediaContent.DateCreated = DateTime.UtcNow.ToString();
            }

            MediaContentController con = new MediaContentController();
            MediaContent newMediaContent = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;


                if (mediaContent.Id != 0)
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.MediaContentId.GetIntValue()) { Value = mediaContent.Id });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.DateCreated.GetIntValue()) { Value = DateTime.Now });

                }
                else
                {
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Create;
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.MediaContentId.GetIntValue()) { Value = 0 });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.DateCreated.GetIntValue()) { Value = DateTime.Now });

                }
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.Insert.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = mediaContent.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FolderId.GetIntValue()) { Value = mediaContent.FolderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FileType.GetIntValue()) { Value = mediaContent.FileType });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Size.GetIntValue()) { Value = mediaContent.Size });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Path.GetIntValue()) { Value = mediaContent.Path });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Name.GetIntValue()) { Value = mediaContent.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Description.GetIntValue()) { Value = mediaContent.Description });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.PreviewUrl.GetIntValue()) { Value = mediaContent.PreviewUrl });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FullNameFile.GetIntValue()) { Value = mediaContent.FullNameFile });

                method.ClearCache = true;

                newMediaContent = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                //method.WaitForOnBeforeCompleted();

                if (newMediaContent != null && newMediaContent.Id != default(int))
                {

                    if (mediaContent.MediaContentMetaDataFieldValues != null && mediaContent.MediaContentMetaDataFieldValues.Any())
                    {
                        foreach (MediaContentMetaDataFieldValues field in mediaContent.MediaContentMetaDataFieldValues)
                        {
                            if (field.Value != null)
                            {
                                field.MediaContentId = newMediaContent.Id;
                                field.DateCreated = newMediaContent.DateCreated;

                                await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.MediaContentMetaDataFieldValuesController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).SaveAsync(field);
                            }
                        }
                    }



                }
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return newMediaContent;
        }

        public async Task<bool> IsAuthorizedAsync(MediaContent mediaContent, User user, RWDPermissionType permissionType)
        {

            /*bool profileTypeIsAuth = false;
            RWDPermission result = null;
            if (mediaContent != null && user != null)
            {
                using (Method method = new Method())
                {
                    #region ProcedureCall
                    method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                    method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.RWDPermission;
                    method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Methods.GetMediaContentUserPermissionByMediaContentAndUser.GetIntValue();
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentId.GetIntValue()) { Value = mediaContent.Id });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentLCID.GetIntValue()) { Value = mediaContent.LCID });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentDateCreaded.GetIntValue()) { Value = mediaContent.DateCreated });
                    method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.UserId.GetIntValue()) { Value = user.Id });

                    result = RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), Enumerations.RWDPermissionTarget.MediaContent);
                    method.End();                  
                    #endregion
                }
                if (result == null)
                {
                    //provjera profile types
                    foreach (var x in user.ProfileTypes)
                    {
                        using (Method method = new Method())
                        {
                            #region ProcedureCall
                            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
                            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.RWDPermission;
                            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Methods.GetMediaContentProfileTypePermissionByMediaContentAndProfileType.GetIntValue();
                            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentId.GetIntValue()) { Value = mediaContent.Id });
                            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentLCID.GetIntValue()) { Value = mediaContent.LCID });
                            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.MediaContentDateCreaded.GetIntValue()) { Value = mediaContent.DateCreated });
                            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions.Parameters.ProfileTypeId.GetIntValue()) { Value = x.Id });
                            result = RWDPermissionController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).Create(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin), Enumerations.RWDPermissionTarget.MediaContent);
                            method.End();                          
                            #endregion

                        }
                        if (result == null)
                            return true;
                        else
                        {
                            //nasao u bazi barm jedan profileType sto mu ne smije dozvoliti onaj default return true
                            profileTypeIsAuth = true;
                            #region SwitchCase
                            switch (permissionType)
                            {
                                case RWDPermissionType.Read:
                                    if (result.Read == true)
                                        return true;
                                    else
                                        break;
                                case RWDPermissionType.Write:
                                    if (result.Write == true)
                                        return true;
                                    else
                                        break;
                                case RWDPermissionType.Delete:
                                    if (result.Delete == true)
                                        return true;
                                    else
                                        break;
                            }
                            #endregion
                        }
                    }
                    if (profileTypeIsAuth == false)
                        return true;
                    else
                        return false;
                }
                else
                {
                    #region SwitchCase
                    //result nije null kod usera
                    switch (permissionType)
                    {
                        case RWDPermissionType.Read:
                            if (result.Read == true)
                                return true;
                            else
                                return false;
                        case RWDPermissionType.Write:
                            if (result.Write == true)
                                return true;
                            else
                                return false;
                        case RWDPermissionType.Delete:
                            if (result.Delete == true)
                                return true;
                            else
                                return false;
                    }
                    #endregion
                }
            }
            else
            {
                //prazan media content ili user
                return false;
            }*/

            //defaultni
            return true;
        }

        public async Task<MediaContent> UpdatePathAsync(string path, long id)
        {
            await AuthenticateAndAuthorizeAsync();
            MediaContentController con = new MediaContentController();
            MediaContent newMedia = new MediaContent();
            newMedia = await GetByIdAsync(id);
            MediaContent updateMediaContent = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.UpdatePath.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Path.GetIntValue()) { Value = path });

                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.MediaContentId.GetIntValue()) { Value = id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = newMedia.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.DateCreated.GetIntValue()) { Value = newMedia.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = newMedia.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FolderId.GetIntValue()) { Value = newMedia.FolderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FileType.GetIntValue()) { Value = newMedia.FileType });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Size.GetIntValue()) { Value = newMedia.Size });

                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Name.GetIntValue()) { Value = newMedia.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Description.GetIntValue()) { Value = newMedia.Description });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.PreviewUrl.GetIntValue()) { Value = newMedia.PreviewUrl });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FullNameFile.GetIntValue()) { Value = newMedia.FullNameFile });

                method.ClearCache = true;

                updateMediaContent = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return updateMediaContent;

        }

        public async Task<MediaContent> UpdateFullNameAsync(string fullname, long id)
        {
            await AuthenticateAndAuthorizeAsync();
            MediaContentController con = new MediaContentController();
            MediaContent newMedia = new MediaContent();
            newMedia = await GetByIdAsync(id);
            MediaContent updateMediaContent = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.UpdateFullName.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FullNameFile.GetIntValue()) { Value = fullname });

                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.MediaContentId.GetIntValue()) { Value = id });

                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.DateCreated.GetIntValue()) { Value = newMedia.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = newMedia.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FolderId.GetIntValue()) { Value = newMedia.FolderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FileType.GetIntValue()) { Value = newMedia.FileType });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Size.GetIntValue()) { Value = newMedia.Size });

                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Name.GetIntValue()) { Value = newMedia.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Description.GetIntValue()) { Value = newMedia.Description });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.PreviewUrl.GetIntValue()) { Value = newMedia.PreviewUrl });
                //method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Path.GetIntValue()) { Value = newMedia.Path });

                method.ClearCache = true;

                updateMediaContent = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return updateMediaContent;

        }

        public async Task<MediaContent> UpdatePreviewUrlAsync(string path, long id)
        {
            await AuthenticateAndAuthorizeAsync();
            MediaContentController con = new MediaContentController();
            MediaContent updateMediaContent = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.UpdatePreviewUrl.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FullNameFile.GetIntValue()) { Value = path });

                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.MediaContentId.GetIntValue()) { Value = id });
                method.ClearCache = true;

                updateMediaContent = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
                method.End();
                //method.WaitForOnAfterCompleted();
            }
            return updateMediaContent;
        }

        public async Task<MediaContent> GetBaseInformationAsync(long id)
        {
            await AuthenticateAndAuthorizeAsync();
            MediaContentController con = new MediaContentController();
            Method method = new Method();
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.GetBaseInformation.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.MediaContentId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MediaContentId_i" });

            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });


            return await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
        }

        public async Task<MediaContent> UpdatePreviewUrlAsync(long id, string pathyoutube)
        {
            await AuthenticateAndAuthorizeAsync();
            MediaContentController con = new MediaContentController();
            MediaContent newMedia = new MediaContent();
            newMedia = await GetByIdAsync(id);
            MediaContent updateMediaContent = null;
            using (Method method = new Method())
            {
                method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
                method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Update;
                method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.UpdatePreviewUrl.GetIntValue();
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.MediaContentId.GetIntValue()) { Value = id });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.PreviewUrl.GetIntValue()) { Value = pathyoutube });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.DateCreated.GetIntValue()) { Value = newMedia.DateCreated });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = newMedia.LCID });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FolderId.GetIntValue()) { Value = newMedia.FolderId });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FileType.GetIntValue()) { Value = newMedia.FileType });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Size.GetIntValue()) { Value = newMedia.Size });

                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Name.GetIntValue()) { Value = newMedia.Name });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Description.GetIntValue()) { Value = newMedia.Description });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Path.GetIntValue()) { Value = newMedia.Path });
                method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FullNameFile.GetIntValue()) { Value = newMedia.FullNameFile });

                method.ClearCache = true;

                updateMediaContent = await CreateAsync(await ExecuteMethodRowAsync(method, this.UseDefaultPlugin));
            }
            return updateMediaContent;

        }

        public async Task<Entities.Base.BasePaginationEntity<MediaContent>> GetByFolderIdWithPaginationAsync(long id, long currentPageIndex, long maxNumberOfRows, string searchTerm = "", int lcid = default(int), string sort = "Name ASC")
        {
            await AuthenticateAndAuthorizeAsync();
            List<MediaContent> mediacontents = new List<MediaContent>();
            if (lcid.Equals(default(int)))
            {
                lcid = Settings.Default.DefaultLcid;
            }
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.GetByFolderIdWithPagination.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FolderId.GetIntValue()) { Value = id });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.CurrentPageIndex.GetIntValue()) { Value = currentPageIndex });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Pagination.Parameters.MaxNumberOfRows.GetIntValue()) { Value = maxNumberOfRows });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.Sort.GetIntValue()) { Value = sort });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MediaContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataTable table = await ExecuteMethodTableAsync(method, this.UseDefaultPlugin);
            foreach (DataRow row in table.Rows)
            {
                mediacontents.Add(await CreateAsync(row));
            }
            Entities.Base.BasePaginationEntity<MediaContent> basePaginationEntity = new Entities.Base.BasePaginationEntity<MediaContent>();
            basePaginationEntity.Items = mediacontents;
            if(table.Rows.Count > 0)
            {
                basePaginationEntity.TotalCount = table.Rows[0].GetValue<int>("TotalCount");
            }
            return basePaginationEntity;
        }

        public async Task<int> GetByFolderIdCountAsync(long folderId, int lcid, string searchTerm)
        {
            await AuthenticateAndAuthorizeAsync();
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            searchTerm = System.Web.HttpUtility.UrlDecode(searchTerm);

            Method method = new Method();
            method.MethodType = Tools.BaseDataAccess.Plugins.Core.Mapping.MethodTypes.Read;
            method.Entity = Tools.BaseDataAccess.Plugins.Core.Mapping.Entities.MediaContent;
            method.Id = Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods.GetByFolderIdCount.GetIntValue();
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.FolderId.GetIntValue()) { Value = folderId });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.LCID.GetIntValue()) { Value = lcid });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Parameters.SearchTerm.GetIntValue()) { Value = searchTerm });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupField.GetIntValue()) { Value = "MediaContentId_i" });
            method.Properties.Add(new MethodProperty(Tools.BaseDataAccess.Plugins.Core.Mapping.Grouping.Parameters.GroupSort.GetIntValue()) { Value = "DateCreated_s desc" });

            DataRow row = await ExecuteMethodRowAsync(method, this.UseDefaultPlugin);
            int count = row.GetValue<int>("MediaContentByFolderCount");
            return count;
        }

    }
}
