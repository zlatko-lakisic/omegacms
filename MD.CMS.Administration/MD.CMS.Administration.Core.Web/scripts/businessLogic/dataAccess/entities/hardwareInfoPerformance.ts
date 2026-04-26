/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./hardwareInfoDrive.ts" />
/// <reference path="./hardwareInfoNetworkInterface.ts" />
/// <reference path="./hardwareInfoProcess.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class hardwareInfoPerformance extends base.BaseEntity implements base.IBaseEntity<hardwareInfoPerformance> {
        public SampleDateTime: Date;
        public CpuUsage: number;
        public FreeMemoryMb: number;
        public TotalMemoryMb: number;
        public UsedMemoryMb: number;
        public Drives: Array<hardwareInfoDrive>;
        public NetworkInterfaces: Array<hardwareInfoNetworkInterface>;
        public Processes: Array<hardwareInfoProcess>;

        constructor(obj?: hardwareInfoPerformance) {
            super(obj);
            this.SampleDateTime = null;
            this.CpuUsage = 0;
            this.FreeMemoryMb = 0;
            this.TotalMemoryMb = 0;
            this.UsedMemoryMb = 0;
            this.Drives = new Array<hardwareInfoDrive>();
            this.NetworkInterfaces = new Array<hardwareInfoNetworkInterface>();
            this.Processes = new Array<hardwareInfoProcess>();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.SampleDateTime = this.getValue<Date>(data, "SampleDateTime", new Date());
            this.CpuUsage = this.getValue<number>(data, "CpuUsage", 0);
            this.FreeMemoryMb = this.getValue<number>(data, "FreeMemoryMb", 0);
            this.TotalMemoryMb = this.getValue<number>(data, "TotalMemoryMb", 0);
            this.UsedMemoryMb = this.TotalMemoryMb - this.FreeMemoryMb;
            this.Drives = this.getArrayConstructEntityValue<hardwareInfoDrive>(data, "Drives", new Array<hardwareInfoDrive>(), new hardwareInfoDrive());
            this.NetworkInterfaces = this.getArrayConstructEntityValue<hardwareInfoNetworkInterface>(data, "NetworkInterfaces", new Array<hardwareInfoNetworkInterface>(), new hardwareInfoNetworkInterface());
            this.Processes = this.getArrayConstructEntityValue<hardwareInfoProcess>(data, "Processes", new Array<hardwareInfoProcess>(), new hardwareInfoProcess());
        }

        public clone(): hardwareInfoPerformance {
            return new hardwareInfoPerformance(this);
        }

    }
}