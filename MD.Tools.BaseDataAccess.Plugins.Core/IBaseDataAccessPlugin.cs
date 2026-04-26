using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public interface IBaseDataAccessPlugin : IDisposable
    {
        #region Properties
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Content.Methods, IExtendedMethodProperty>> Content { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Menu.Methods, IExtendedMethodProperty>> Menu { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataField.Methods, IExtendedMethodProperty>> MetaDataField { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MetaDataFieldValue.Methods, IExtendedMethodProperty>> MetaDataFieldValue { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Permissions.Methods, IExtendedMethodProperty>> Permissions { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Profile.Methods, IExtendedMethodProperty>> Profile { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType.Methods, IExtendedMethodProperty>> ProfileType { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeField.Methods, IExtendedMethodProperty>> ProfileTypeField { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileTypeFieldValue.Methods, IExtendedMethodProperty>> ProfileTypeFieldValue { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Session.Methods, IExtendedMethodProperty>> Session { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy.Methods, IExtendedMethodProperty>> Taxonomy { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.User.Methods, IExtendedMethodProperty>> User { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MenuContent.Methods, IExtendedMethodProperty>> MenuContent { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder.Methods, IExtendedMethodProperty>> Folder { get; }
		IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinition.Methods, IExtendedMethodProperty>> ContentTypeDefinition { get; }
		IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSource.Methods, IExtendedMethodProperty>> ContentTypeDefinitionDataSource { get; }
		IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionDataSourceJoin.Methods, IExtendedMethodProperty>> ContentTypeDefinitionDataSourceJoin { get; }
		IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition.Methods, IExtendedMethodProperty>> ContentTypeDefinitionFolderDataBoundCondition { get; }
		IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFieldValue.Methods, IExtendedMethodProperty>> ContentTypeDefinitionFieldValue { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionField.Methods, IExtendedMethodProperty>> ContentTypeDefinitionField { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolder.Methods, IExtendedMethodProperty>> ContentTypeDefinitionFolder { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMediaContentMetaDataField.Methods, IExtendedMethodProperty>> FolderMediaContentMetaDataField { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.FolderMetaDataField.Methods, IExtendedMethodProperty>> FolderMetaDataField { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContent.Methods, IExtendedMethodProperty>> MediaContent { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.MediaContentMetaDataFieldValues.Methods, IExtendedMethodProperty>> MediaContentMetaDataFieldValues { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.AttributeTypeDefinition.Methods, IExtendedMethodProperty>> AttributeTypeDefinition { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias.Methods, IExtendedMethodProperty>> ContentAlias { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Culture.Methods, IExtendedMethodProperty>> Culture { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent.Methods, IExtendedMethodProperty>> TaxonomyContent { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Template.Methods, IExtendedMethodProperty>> Template { get; }
        IEntity<MD.Tools.BaseDataAccess.Plugins.Core.IEventHandlerMethod<MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSync.Methods, IExtendedMethodProperty>> ContentTypeDefinitionFolderDataBoundSync { get; }
        string PluginName { get; }
        string PluginSettings { get; set; }
        dynamic PluginSettingsJson { get; }
        Dictionary<Mapping.Entities, IEventHandlerEntity> EventHandlers { get; set; }
		string DatabaseType { get; }
		bool IsDataBoundFieldPlugin { get; }
        #endregion

        #region Methods
        DataSet ExecuteDataTableSet(Method message);
        DataTable ExecuteDataTable(Method message);
        DataRow ExecuteDataRow(Method message);
        bool ExecuteBool(Method message);
        void Execute(Method message);
        bool HasMethod(Method method);
		DataSet ExecuteDataTableSet(DataBoundMethod message);
		DataTable ExecuteDataTable(DataBoundMethod message);
		DataRow ExecuteDataRow(DataBoundMethod message);
		bool ExecuteBool(DataBoundMethod message);
		void Execute(DataBoundMethod message);
		dynamic GetDataStructure(DataBoundMethod method);


        Task<DataSet> ExecuteDataTableSetAsync(Method message);
        Task<DataTable> ExecuteDataTableAsync(Method message);
        Task<DataRow> ExecuteDataRowAsync(Method message);
        Task<bool> ExecuteBoolAsync(Method message);
        Task ExecuteAsync(Method message);
        Task<bool> HasMethodAsync(Method method);
        Task<DataSet> ExecuteDataTableSetAsync(DataBoundMethod message);
        Task<DataTable> ExecuteDataTableAsync(DataBoundMethod message);
        Task<DataRow> ExecuteDataRowAsync(DataBoundMethod message);
        Task<bool> ExecuteBoolAsync(DataBoundMethod message);
        Task ExecuteAsync(DataBoundMethod message);
        Task<dynamic> GetDataStructureAsync(DataBoundMethod method);
        IBaseDataAccessPlugin Clone();
        #endregion
    }
}
