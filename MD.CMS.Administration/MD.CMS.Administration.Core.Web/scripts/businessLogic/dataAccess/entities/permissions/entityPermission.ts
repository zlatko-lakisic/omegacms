/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/baseEntity.ts" />
/// <reference path="../../entities.ts" />
/// <reference path="../../../helpers.ts" />
/// <reference path="./permissionAccessTypeEnum.ts" />

namespace mdBusinessLogic.dataAccess.entities.permissions {
    export class entityPermission {
        public Object: entitiesEnum;
        public Entity: entitiesEnum;
        public AccessTypes: Array<permissionAccessTypeEnum>;

        constructor() {
            this.AccessTypes = new Array<permissionAccessTypeEnum>();
        }
    }
}
