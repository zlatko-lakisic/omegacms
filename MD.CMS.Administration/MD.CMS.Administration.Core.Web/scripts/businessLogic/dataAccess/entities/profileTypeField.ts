/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./attributeTypeDefinition.ts" />
/// <reference path="./attributeTypeDefinition.ts" />
/// <reference path="./fieldValidation.ts" />
/// <reference path="./profileTypeFieldJsonField.ts" />
/// <reference path="./genericContent/genericContentField.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class profileTypeField extends entities.genericContent.genericContentField implements base.IBaseEntity<profileTypeField> {
        public ProfileTypeId: number;

        constructor(obj?: profileTypeField) {
            super(obj);
            this.ProfileTypeId = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.ProfileTypeId = this.getValue<number>(data, "ProfileTypeId", 0);
        }

        public clone(): profileTypeField {
            return new profileTypeField(this);
        }

    }
}
