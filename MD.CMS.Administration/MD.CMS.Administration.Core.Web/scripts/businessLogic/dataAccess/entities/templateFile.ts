/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class templateFile extends base.BaseEntity implements base.IBaseEntity<templateFile> {
        public Path: string;
        public Name: string;

        constructor(obj?: templateFile) {
            super(obj);
            this.Path = '';
            this.Name = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Path = this.getValue<string>(data, "Path", '');
            this.Name = this.getValue<string>(data, "Name", '');
        }

        public clone(): templateFile {
            return new templateFile(this);
        }

    }
}