/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class culture extends base.BaseEntity implements base.IBaseEntity<culture> {
        public LCID: number;
        public Name: string;
        public Code: string;
        public IsoCode: string;
        public IsApproved: boolean;

        constructor(obj?: culture) {
            super(obj);
            this.LCID = 0;
            this.Name = '';
            this.Code = '';
            this.IsoCode = '';
            this.IsApproved = false
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.LCID = this.getValue<number>(data, "LCID", 0);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Code = this.getValue<string>(data, "Code", '');
            this.IsoCode = this.getValue<string>(data, "IsoCode", '');
            this.IsApproved = this.getValue<boolean>(data, "IsApproved", false);
        }

        public clone(): culture {
            return new culture(this);
        }

    }
}