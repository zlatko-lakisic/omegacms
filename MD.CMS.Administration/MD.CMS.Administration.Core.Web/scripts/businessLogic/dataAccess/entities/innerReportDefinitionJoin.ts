/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./innerReportDefinitionJoinInner.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinitionJoin extends base.BaseEntity implements base.IBaseEntity<innerReportDefinitionJoin> {
        public Left: innerReportDefinitionJoinInner;
        public Right: innerReportDefinitionJoinInner;
        public Type: number;

        constructor(obj?: innerReportDefinitionJoin) {
            super(obj);
            this.Left = new innerReportDefinitionJoinInner();
            this.Right = new innerReportDefinitionJoinInner();
            this.Type = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Left = this.getConstructEntityValue<innerReportDefinitionJoinInner>(data, "Left", new innerReportDefinitionJoinInner());
            this.Right = this.getConstructEntityValue<innerReportDefinitionJoinInner>(data, "Right", new innerReportDefinitionJoinInner());
            this.Type = this.getValue<number>(data, "Type", 0);
        }

        public clone(): innerReportDefinitionJoin {
            return new innerReportDefinitionJoin(this);
        }

    }
}