/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class approvalChainStepAction extends base.BaseEntity implements base.IBaseEntity<approvalChainStepAction> {
        public StepId: number;
        public UserId: number;
        public Action: number;
        public Type: number;
        public RedirectTo: number;

        constructor(obj?: approvalChainStepAction) {
            super(obj);
            this.StepId = 0;
            this.UserId = 0;
            this.Action = 0;
            this.Type = 0;
            this.RedirectTo = 0;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.StepId = this.getValue<number>(data, "StepId", 0);
            this.UserId = this.getValue<number>(data, "UserId", 0);
            this.Action = this.getValue<number>(data, "Action", 0);
            this.Type = this.getValue<number>(data, "Type", 0);
            this.RedirectTo = this.getValue<number>(data, "RedirectTo", 0);
        }

        public clone(): approvalChainStepAction {
            return new approvalChainStepAction(this);
        }
    }
}