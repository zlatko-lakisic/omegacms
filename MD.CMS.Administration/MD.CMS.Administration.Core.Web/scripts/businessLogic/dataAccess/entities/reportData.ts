/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class reportData extends base.BaseEntity implements base.IBaseEntity<reportData> {
        public ReportId: number;
        public DateCreated: Date;
        public Data: any;

        constructor(obj?: reportData) {
            super(obj);
            this.DateCreated = new Date();
            this.Data = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.ReportId = this.getValue<number>(data, "ReportId", 0);
            this.DateCreated = this.getValue<Date>(data, "DateCreated", new Date());
            this.Data = this.getValue<any>(data, "Data", null);
        }

        public clone(): reportData {
            return new reportData(this);
        }

    }
}