/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class contentTypeDefinitionFolder extends base.BaseEntity implements base.IBaseEntity<contentTypeDefinitionFolder> {
        public FolderId: number;
        public ContentTypeDefinitionId: number;
        public Title: string;

        constructor(obj?: contentTypeDefinitionFolder) {
            super(obj);
            this.FolderId = 0;
            this.ContentTypeDefinitionId = 0;
            this.Title = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.FolderId = this.getValue<number>(data, "FolderId", 0);
            this.ContentTypeDefinitionId = this.getValue<number>(data, "ContentTypeDefinitionId", 0);
            this.Title = this.getValue<string>(data, "Title", '');
        }

        public clone(): contentTypeDefinitionFolder {
            return new contentTypeDefinitionFolder(this);
        }
    }
}