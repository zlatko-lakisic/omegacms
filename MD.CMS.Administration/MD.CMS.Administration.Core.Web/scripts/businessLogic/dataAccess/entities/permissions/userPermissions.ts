/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/baseEntity.ts" />
/// <reference path="./permissionsBase.ts" />
/// <reference path="../../../helpers.ts" />

namespace mdBusinessLogic.dataAccess.entities.permissions {
    export class userPermissions extends permissionsBase implements base.IBaseEntity<userPermissions> {
        public UserId: number;

        constructor(obj?: userPermissions) {
            super(obj);
            this.UserId = 0;
            if (obj !== undefined && obj != null) {
                this.UserId = obj.UserId;
            }
        }

        construct(data: userPermissions): void {
            this.UserId = data.UserId;
            this.EntityPermissions = data.EntityPermissions;
            this.ObjectPermissions = data.ObjectPermissions;
        }

        clone(): userPermissions {
            return new userPermissions(this);
        }
    }
}
