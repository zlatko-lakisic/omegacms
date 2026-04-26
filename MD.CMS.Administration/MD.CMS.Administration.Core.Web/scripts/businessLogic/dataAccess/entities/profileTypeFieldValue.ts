/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./profileTypeField.ts" />
/// <reference path="./genericContent/genericContentFieldValue.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class profileTypeFieldValue extends entities.genericContent.genericContentFieldValue implements base.IBaseEntity<profileTypeFieldValue> {
        public ProfileTypeFieldId: number;
        public ProfileTypeId: number;
        public UserId: number;

        constructor(obj?: profileTypeFieldValue) {
            super(obj);
            this.ProfileTypeFieldId = 0;
            this.ProfileTypeId = 0;
            this.UserId = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.ProfileTypeFieldId = this.getValue<number>(data, "ProfileTypeFieldId", 0);
            this.ProfileTypeId = this.getValue<number>(data, "ProfileTypeId", 0);
            this.UserId = this.getValue<number>(data, "UserId", 0);
        }

        public clone(): profileTypeFieldValue {
            return new profileTypeFieldValue(this);
        }

    }
}
