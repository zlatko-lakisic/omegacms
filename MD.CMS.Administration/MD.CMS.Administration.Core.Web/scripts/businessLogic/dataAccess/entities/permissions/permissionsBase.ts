/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/baseEntity.ts" />
/// <reference path="../../../helpers.ts" />
/// <reference path="./entityPermission.ts" />
/// <reference path="./objectPermission.ts" />

namespace mdBusinessLogic.dataAccess.entities.permissions {
    export class permissionsBase extends base.BaseEntity {
        public EntityPermissions: Array<entityPermission>;
        public ObjectPermissions: Array<objectPermission>;

        constructor(obj?: permissionsBase) {
            super();
            this.EntityPermissions = new Array<entityPermission>();
            this.ObjectPermissions = new Array<objectPermission>();
            if (obj !== undefined && obj != null) {
                this.EntityPermissions = obj.EntityPermissions;
                this.ObjectPermissions = obj.ObjectPermissions;
            }
        }
    }
}
