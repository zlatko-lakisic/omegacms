/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class reportDirectory extends base.BaseEntity implements base.IBaseEntity<reportDirectory> {
        public Path: string;
        public Children: Array<reportDirectory>;

        constructor(obj?: reportDirectory) {
            super(obj);
            this.Path = '';
            this.Children = new Array<reportDirectory>();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Path = this.getValue<string>(data, "Path", '');
            this.Children = this.getArrayConstructEntityValue<reportDirectory>(data, "Children", new Array<reportDirectory>(), new reportDirectory());
        }

        public clone(): reportDirectory {
            return new reportDirectory(this);
        }

    }
}