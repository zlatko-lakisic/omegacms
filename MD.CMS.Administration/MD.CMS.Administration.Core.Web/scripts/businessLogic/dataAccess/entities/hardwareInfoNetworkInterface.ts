/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class hardwareInfoNetworkInterface extends base.BaseEntity implements base.IBaseEntity<hardwareInfoNetworkInterface> {
        public Name: string;
        public Description: string;
        public SentMb: number;
        public ReceivedMb: number;
        public SentGb: number;
        public ReceivedGb: number;
        public NetworkUtilization: number;

        constructor(obj?: hardwareInfoNetworkInterface) {
            super(obj);
            this.Name = '';
            this.Description = '';
            this.SentMb = 0;
            this.ReceivedMb = 0;
            this.SentGb = 0;
            this.ReceivedGb = 0;
            this.NetworkUtilization = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Description = this.getValue<string>(data, "Description", '');
            this.SentMb = this.getValue<number>(data, "SentMb", 0);
            this.ReceivedMb = this.getValue<number>(data, "ReceivedMb", 0);
            this.SentGb = this.getValue<number>(data, "SentGb", 0);
            this.ReceivedGb = this.getValue<number>(data, "ReceivedGb", 0);
            this.NetworkUtilization = this.getValue<number>(data, "NetworkUtilization", 0);
        }

        public clone(): hardwareInfoNetworkInterface {
            return new hardwareInfoNetworkInterface(this);
        }

    }
}