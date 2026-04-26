/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./genericContent/genericContentFieldValue.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class contentTypeDefinitionFieldValue extends entities.genericContent.genericContentFieldValue implements base.IBaseEntity<contentTypeDefinitionFieldValue>{
        public ContentId: string;
        public LCID: number;
        public DateCreated: Date;
        public ContentTypeDefinitionFieldId: number;
        public ContentTypeDefinitionId: number;

        constructor(obj?: contentTypeDefinitionFieldValue) {
            super(obj);
            this.ContentId = '0';
            this.LCID = 0;
            this.DateCreated = new Date();
            this.ContentTypeDefinitionFieldId = 0;
            this.ContentTypeDefinitionId = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.ContentId = this.getValue<string>(data, 'ContentId', '0');
            this.LCID = this.getValue<number>(data, 'LCID', 0);
            this.DateCreated = this.getValue<Date>(data, 'DateCreated', new Date());
            this.ContentTypeDefinitionFieldId = this.getValue<number>(data, 'ContentTypeDefinitionFieldId', 0);
            this.ContentTypeDefinitionId = this.getValue<number>(data, 'ContentTypeDefinitionId', 0);
        }

        public clone(): contentTypeDefinitionFieldValue {
            return new contentTypeDefinitionFieldValue(this);
        }
    }
}
