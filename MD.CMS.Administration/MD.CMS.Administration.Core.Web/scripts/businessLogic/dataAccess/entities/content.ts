/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />
/// <reference path="./metaDataFieldValue.ts" />
/// <reference path="./template.ts" />
/// <reference path="./taxonomy.ts" />
/// <reference path="./contentAlias.ts" />
/// <reference path="./contentTypeDefinition.ts" />
/// <reference path="./contentTypeDefinitionFieldValue.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class content extends base.BaseEntity implements base.IBaseEntity<content> {
        public LCID: number;
        public DateCreated: Date;
        public AuthorId: number;
        public FolderId: number;
        public Title: string;
        public Path: string;
        public Html: string;
        public Author: user;
        public ContentType: contentTypeDefinition<contentTypeDefinitionFieldValue>;
        public Taxonomy: Array<taxonomy>;
        public MetaDataFieldValues: Array<metaDataFieldValue>;
        public ContentAliases: Array<contentAlias>;
        public Template: template;
        public IsNew: boolean;
        public IsPublished: boolean;
        public IsDataBound: boolean;
        public UniqueId: string;
        public ContentTypeDefinitionId: number;

        constructor(obj?: content) {
            super(obj);
            this.LCID = 0;
            this.DateCreated = new Date();
            this.AuthorId = 0;
            this.FolderId = 0;
            this.Title = "";
            this.Path = "";
            this.Html = null;
            this.Author = null;
            this.ContentType = null;
            this.Taxonomy = new Array<taxonomy>();
            this.MetaDataFieldValues = new Array<any>();
            this.ContentAliases = new Array<contentAlias>();
            this.Template = null;
            this.IsNew = true;
            this.IsPublished = false;
            this.IsDataBound = false;
            this.UniqueId = "";
            this.ContentTypeDefinitionId = 0;

            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.LCID = this.getValue<number>(data, 'LCID', 0);
            this.DateCreated = this.getValue<Date>(data, 'DateCreated', null);
            this.AuthorId = this.getValue<number>(data, 'AuthorId', 0);
            this.FolderId = this.getValue<number>(data, 'FolderId', 0);
            this.Title = this.getValue<string>(data, 'Title', '');
            this.Path = this.getValue<string>(data, 'Path', '');
            this.Html = this.getValue<string>(data, 'Html', '');
            this.ContentTypeDefinitionId = this.getValue<number>(data, 'ContentTypeDefinitionId', 0);
            this.Author = this.getConstructEntityValue<user>(data, 'Author', new user());
            this.ContentType = this.getConstructEntityValue<contentTypeDefinition<contentTypeDefinitionFieldValue>>(data, 'ContentType', new contentTypeDefinition<contentTypeDefinitionFieldValue>(new contentTypeDefinitionFieldValue()));
            this.Taxonomy = this.getArrayConstructEntityValue<taxonomy>(data, 'Taxonomy', new Array<taxonomy>(), new taxonomy());
            this.MetaDataFieldValues = this.getArrayConstructEntityValue<metaDataFieldValue>(data, 'MetaDataFieldValues', new Array<metaDataFieldValue>(), new metaDataFieldValue());
            if (data.ContentAliases !== undefined && data.ContentAliases != null && data.ContentAliases.length > 0 && (typeof data.ContentAliases[0] === 'string' || data.ContentAliases[0] instanceof String)) {
                let thisObj = this;
                data.ContentAliases = data.ContentAliases.map(function (alias) {
                    let al: contentAlias = new contentAlias();
                    al.construct({
                        LCID: thisObj.LCID,
                        DateCreated: thisObj.DateCreated,
                        ContentId: thisObj.Id,
                        Alias: alias
                    });
                    return al;
                });
            }
            this.ContentAliases = this.getArrayConstructEntityValue<contentAlias>(data, 'ContentAliases', new Array<contentAlias>(), new contentAlias());
            this.Template = this.getConstructEntityValue<template>(data, 'Template', new template());
            this.IsNew = this.getValue<boolean>(data, 'IsNew', false);
            this.IsPublished = this.getValue<boolean>(data, 'IsPublished', false);
            this.IsDataBound = this.getValue<boolean>(data, 'IsDataBound', false);
            this.UniqueId = this.getValue<string>(data, 'UniqueId', '');
        }

        public clone(): content {
            return new content(this);
        }
    }
}
