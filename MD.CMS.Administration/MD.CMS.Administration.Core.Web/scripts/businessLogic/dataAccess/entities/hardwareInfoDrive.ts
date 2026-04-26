/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class hardwareInfoDrive extends base.BaseEntity implements base.IBaseEntity<hardwareInfoDrive> {
        public Label: string;
        public TotalSizeMb: number;
        public AvaliableSizeMb: number;
        public UsedSizeMb: number;
        public TotalSizeGb: number;
        public AvaliableSizeGb: number;
        public UsedSizeGb: number;
        public Format: string;

        constructor(obj?: hardwareInfoDrive) {
            super(obj);
            this.Label = '';
            this.TotalSizeMb = 0;
            this.AvaliableSizeMb = 0;
            this.UsedSizeMb = 0;
            this.TotalSizeGb = 0;
            this.AvaliableSizeGb = 0;
            this.UsedSizeGb = 0;
            this.Format = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Label = this.getValue<string>(data, "Label", '');
            this.TotalSizeMb = this.getValue<number>(data, "TotalSizeMb", 0);
            this.AvaliableSizeMb = this.getValue<number>(data, "AvaliableSizeMb", 0);
            this.UsedSizeMb = this.getValue<number>(data, "UsedSizeMb", 0);
            this.TotalSizeGb = this.getValue<number>(data, "TotalSizeGb", 0);
            this.AvaliableSizeGb = this.getValue<number>(data, "AvaliableSizeGb", 0);
            this.UsedSizeGb = this.getValue<number>(data, "UsedSizeGb", 0);
            this.Format = this.getValue<string>(data, "Format", '');
        }

        public clone(): hardwareInfoDrive {
            return new hardwareInfoDrive(this);
        }

    }
}