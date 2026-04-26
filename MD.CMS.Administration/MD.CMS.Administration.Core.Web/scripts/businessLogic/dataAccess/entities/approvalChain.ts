/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class approvalChain extends base.BaseEntity implements base.IBaseEntity<approvalChain> {
        public FolderId: number;
        public IsActive: boolean;
        public Steps: any[];
        public ChainId: number;

        constructor(obj?: approvalChain) {
            super(obj);
            this.FolderId = 0;
            this.IsActive = false;
            this.Steps = [];
            this.ChainId = 0;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.FolderId = this.getValue<number>(data, "FolderId", 0);
            this.IsActive = this.getValue<boolean>(data, "IsActive", false);
            this.Steps = this.getValue<any[]>(data, "Steps", []);
            this.ChainId = this.getValue<number>(data, "ChainId", 0);
        }

        public clone(): approvalChain {
            return new approvalChain(this);
        }
    }
}