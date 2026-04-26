/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./../../helpers.ts" />
/// <reference path="./content.ts" />
/// <reference path="./mediaContent.ts" />
/// <reference path="./template.ts" />
/// <reference path="./profileType.ts" />
/// <reference path="./folderMetaDataField.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class actionSchedule<T> extends base.BaseEntity implements base.IBaseEntity<actionSchedule<T>> {
        public ActionType: T;
        public ExecutionType: executionScheduleType;
        public ExecutionSecondsFrequency: number;
        public ExecutionStart: Date;
        public ExecutionEnd: Date;
        public Enabled: boolean;

        constructor(obj?: actionSchedule<T>) {
            super(obj);
            this.ActionType = null;
            this.ExecutionType = executionScheduleType.Manual;
            this.ExecutionSecondsFrequency = 0;
            this.ExecutionStart = null;
            this.ExecutionEnd = null;
            this.Enabled = false;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.ActionType = this.getValue<T>(data, "ActionType", null);
            this.ExecutionType = this.getValue<executionScheduleType>(data, "ExecutionType", 0);
            this.ExecutionSecondsFrequency = this.getValue<number>(data, "ExecutionSecondsFrequency", 0);
            this.ExecutionStart = this.getValue<Date>(data, "ExecutionStart", null);
            this.ExecutionEnd = this.getValue<Date>(data, "ExecutionEnd", null);
            this.Enabled = this.getValue<boolean>(data, "Enabled", false);
        }

        public clone(): actionSchedule<T> {
            return new actionSchedule<T>(this);
        }
    }

    export enum executionScheduleType {
        Manual = 0,
        Recurring = 1
    }
}
