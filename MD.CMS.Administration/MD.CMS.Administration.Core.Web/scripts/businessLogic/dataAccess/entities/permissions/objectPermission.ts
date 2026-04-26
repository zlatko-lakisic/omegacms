/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/baseEntity.ts" />
/// <reference path="../../entities.ts" />
/// <reference path="../../../helpers.ts" />
/// <reference path="./permissionAccessTypeEnum.ts" />

namespace mdBusinessLogic.dataAccess.entities.permissions {
    export class objectPermission {
        public AccessTypes: Array<permissionAccessTypeEnum>;
        public Object: entitiesEnum;
        public ObjectId: string;

        constructor() {
            this.AccessTypes = new Array<permissionAccessTypeEnum>();
        }
    }
}
