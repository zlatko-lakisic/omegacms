/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class hardwareInfoProcess extends base.BaseEntity implements base.IBaseEntity<hardwareInfoProcess> {
        public Name: string;
        public User: string;
        public ProcessorUsage: number;
        public MemoryUsageMb: number;

        constructor(obj?: hardwareInfoProcess) {
            super(obj);
            this.Name = '';
            this.User = '';
            this.ProcessorUsage = 0;
            this.MemoryUsageMb = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Name = this.getValue<string>(data, "Name", '');
            this.User = this.getValue<string>(data, "User", '');
            this.ProcessorUsage = this.getValue<number>(data, "ProcessorUsage", 0);
            this.MemoryUsageMb = this.getValue<number>(data, "MemoryUsageMb", 0);
        }

        public clone(): hardwareInfoProcess {
            return new hardwareInfoProcess(this);
        }

    }
}