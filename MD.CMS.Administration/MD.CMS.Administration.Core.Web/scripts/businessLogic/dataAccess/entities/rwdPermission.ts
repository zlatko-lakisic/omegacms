/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class rwdPermission extends base.BaseEntity implements base.IBaseEntity<rwdPermission> {
        public Read: boolean;
        public Write: boolean;
        public Delete: boolean;
        public Target: rwdPermissionTargetEnum;
        public TargetPrimaryKey: string;

        constructor(obj?: rwdPermission) {
            super(obj);
            this.Read = false;
            this.Write = false;
            this.Delete = false;
            this.Target = rwdPermissionTargetEnum.None;
            this.TargetPrimaryKey = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Read = this.getValue<boolean>(data, "Read", false);
            this.Write = this.getValue<boolean>(data, "Write", false);
            this.Delete = this.getValue<boolean>(data, "Delete", false);
            this.Target = this.getValue<rwdPermissionTargetEnum>(data, "Target", rwdPermissionTargetEnum.None);
            this.TargetPrimaryKey = this.getValue<string>(data, "TargetPrimaryKey", '');
        }

        public clone(): rwdPermission {
            return new rwdPermission(this);
        }

    }

    export enum rwdPermissionTargetEnum {
        None = 0,
        Folder = 1,
        Content = 2,
        MediaContent = 3
    }
}
