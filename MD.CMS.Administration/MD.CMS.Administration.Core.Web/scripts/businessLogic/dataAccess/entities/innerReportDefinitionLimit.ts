/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinitionLimit extends base.BaseEntity implements base.IBaseEntity<innerReportDefinitionLimit> {
        public From: number;
        public To: number;

        constructor(obj?: innerReportDefinitionLimit) {
            super(obj);
            this.From = 0;
            this.To = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.From = this.getValue<number>(data, "From", 0);
            this.To = this.getValue<number>(data, "To", 0);
        }

        public clone(): innerReportDefinitionLimit {
            return new innerReportDefinitionLimit(this);
        }

    }
}