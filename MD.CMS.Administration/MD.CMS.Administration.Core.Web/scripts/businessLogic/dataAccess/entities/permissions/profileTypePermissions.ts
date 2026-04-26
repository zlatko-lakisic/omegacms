/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/baseEntity.ts" />
/// <reference path="../../../helpers.ts" />
/// <reference path="./permissionsBase.ts" />

namespace mdBusinessLogic.dataAccess.entities.permissions {
    export class profileTypePermissions extends permissionsBase implements base.IBaseEntity<profileTypePermissions> {
        public ProfileId: number;

        constructor(obj?: profileTypePermissions) {
            super(obj);
            this.ProfileId = 0;
            if (obj !== undefined && obj != null) {
                this.ProfileId = obj.ProfileId;
            }
        }

        construct(data: profileTypePermissions): void {
            this.ProfileId = data.ProfileId;
            this.EntityPermissions = data.EntityPermissions;
            this.ObjectPermissions = data.ObjectPermissions;
        }
        clone(): profileTypePermissions {
            return new profileTypePermissions(this);
        }
    }
}
