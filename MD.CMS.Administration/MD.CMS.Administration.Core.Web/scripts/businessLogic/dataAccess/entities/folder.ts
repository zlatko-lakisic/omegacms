/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./../../helpers.ts" />
/// <reference path="./content.ts" />
/// <reference path="./mediaContent.ts" />
/// <reference path="./template.ts" />
/// <reference path="./profileType.ts" />
/// <reference path="./folderMetaDataField.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class folder<T extends base.BaseEntity & base.IBaseEntity<T>> extends base.BaseEntity implements base.IBaseEntity<folder<T>> {
        public ParentId: number;
        public Name: string;
        public Description: string;
        public Parent: folder<T>;
        public Children: Array<folder<T>>;
        public Contents: Array<content>;
        public FolderPath: string;
        public MetaDataFields: Array<folderMetaDataField>;
        public MediaContent: Array<mediaContent>;
        public ProfileTypePermissions: Array<profileType>;
        public NotAuthorizedUsers: Array<user>;
        public FolderMediaContentMetaDataField: Array<folderMediaContentMetaDataField>;
        public ContentTypeDefinitionFolder: Array<contentTypeDefinitionFolder>;
        public ContentTypeDefinitions: Array<contentTypeDefinition<contentTypeDefinitionField>>;
        public ContentTypeDefinitionId: number;
        public Templates: Array<template>;
        public Inherit: boolean;
        public IsNew: boolean;
        public ParentArray: Array<folder<T>>;
        public ChildrenTotalCount: number;
        public ContentsTotalCount: number;
        public MediaContentTotalCount: number;
        public IsHidden: boolean;

        constructor(obj?: folder<T>) {
            super(obj);
            this.ParentId = 0;
            this.Name = '';
            this.Description = '';
            this.Parent = null;
            this.Children = new Array<folder<T>>();
            this.Contents = new Array<content>();
            this.FolderPath = '';
            this.MetaDataFields = new Array<folderMetaDataField>();
            this.MediaContent = new Array<mediaContent>();
            this.ProfileTypePermissions = new Array<profileType>();
            this.NotAuthorizedUsers = new Array<user>();
            this.FolderMediaContentMetaDataField = new Array<folderMediaContentMetaDataField>();
            this.ContentTypeDefinitionFolder = new Array<contentTypeDefinitionFolder>();
            this.ContentTypeDefinitions = new Array<contentTypeDefinition<contentTypeDefinitionField>>();
            this.ContentTypeDefinitionId = 0;
            this.Templates = new Array<template>();
            this.Inherit = true;
            this.IsNew = true;
            this.ParentArray = new Array<folder<T>>();
            this.ChildrenTotalCount = 0;
            this.ContentsTotalCount = 0;
            this.MediaContentTotalCount = 0;
            this.IsHidden = false;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.ParentId = this.getValue<number>(data, "ParentId", 0);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Description = this.getValue<string>(data, "Description", '');
            this.Parent = this.getConstructEntityValue<folder<T>>(data, "Parent", new folder<T>());
            this.Children = this.getArrayConstructEntityValue<folder<T>>(data, "Children", new Array<folder<T>>(), new folder<T>());
            this.Contents = this.getArrayConstructEntityValue<content>(data, "Contents", new Array<content>(), new content());
            this.FolderPath = this.getValue<string>(data, "FolderPath", '');
            this.MetaDataFields = this.getArrayConstructEntityValue<folderMetaDataField>(data, "MetaDataFields", new Array<folderMetaDataField>(), new folderMetaDataField());
            this.MediaContent = this.getArrayConstructEntityValue<mediaContent>(data, "MediaContent", new Array<mediaContent>(), new mediaContent());
            this.ProfileTypePermissions = this.getArrayConstructEntityValue<profileType>(data, "ProfileTypePermissions", new Array<profileType>(), new profileType());
            this.NotAuthorizedUsers = this.getArrayConstructEntityValue<user>(data, "NotAuthorizedUsers", new Array<user>(), new user());
            this.FolderMediaContentMetaDataField = this.getArrayConstructEntityValue<folderMediaContentMetaDataField>(data, "FolderMediaContentMetaDataField", new Array<folderMediaContentMetaDataField>(), new folderMediaContentMetaDataField());
            this.ContentTypeDefinitionFolder = this.getArrayConstructEntityValue<contentTypeDefinitionFolder>(data, "ContentTypeDefinitionFolder", new Array<contentTypeDefinitionFolder>(), new contentTypeDefinitionFolder());
            this.ContentTypeDefinitions = this.getArrayConstructEntityValue<contentTypeDefinition<contentTypeDefinitionField>>(data, "ContentTypeDefinitions", new Array<contentTypeDefinition<contentTypeDefinitionField>>(), new contentTypeDefinition<contentTypeDefinitionField>());
            this.ContentTypeDefinitionId = this.getValue<number>(data, "ContentTypeDefinitionId", 0);
            this.Templates = this.getArrayConstructEntityValue<template>(data, "Templates", new Array<template>(), new template());
            this.Inherit = this.getValue<boolean>(data, "Inherit", true);
            this.IsNew = this.getValue<boolean>(data, "IsNew", true);
            this.ParentArray = helpers.loadParentArray(this, 'Name', 'FolderPath')
            this.ChildrenTotalCount = this.getValue<number>(data, "ChildrenTotalCount", 0);
            this.ContentsTotalCount = this.getValue<number>(data, "ContentsTotalCount", 0);
            this.MediaContentTotalCount = this.getValue<number>(data, "MediaContentTotalCount", 0);
            this.IsHidden = this.getValue<boolean>(data, "IsHidden", false);
        }

        public clone(): folder<T> {
            return new folder<T>(this);
        }
    }
}
