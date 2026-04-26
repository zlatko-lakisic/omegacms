/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./attributeTypeDefinition.ts" />
/// <reference path="./fieldValidation.ts" />
/// <reference path="./genericContent/genericContentField.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class contentTypeDefinitionField extends entities.genericContent.genericContentField implements base.IBaseEntity<contentTypeDefinitionField> {
        public ContentTypeDefinitionId: number;

        constructor(obj?: contentTypeDefinitionField) {
            super(obj);
            this.ContentTypeDefinitionId = 0;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.ContentTypeDefinitionId = this.getValue<number>(data, 'ContentTypeDefinitionId', 0);
        }

        public clone(): contentTypeDefinitionField {
            return new contentTypeDefinitionField(this);
        }
    }
}
