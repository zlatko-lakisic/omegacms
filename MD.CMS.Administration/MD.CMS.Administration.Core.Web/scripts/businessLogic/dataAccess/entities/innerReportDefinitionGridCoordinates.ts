/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinitionGridCoordinates extends base.BaseEntity implements base.IBaseEntity<innerReportDefinitionGridCoordinates> {
        public x: number;
        public y: number;
        public width: number;
        public height: number;

        constructor(obj?: innerReportDefinitionGridCoordinates) {
            super(obj);
            this.x = 0;
            this.y = 0;
            this.width = 100;
            this.height = 50;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.x = this.getValue<number>(data, "x", 0);
            this.y = this.getValue<number>(data, "y", 0);
            this.width = this.getValue<number>(data, "width", 100);
            this.height = this.getValue<number>(data, "height", 50);
        }

        public clone(): innerReportDefinitionGridCoordinates {
            return new innerReportDefinitionGridCoordinates(this);
        }

    }
}