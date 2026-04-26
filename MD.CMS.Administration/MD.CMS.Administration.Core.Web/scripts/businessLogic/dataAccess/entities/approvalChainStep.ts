/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./approvalChainStepAction.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class approvalChainStep extends base.BaseEntity implements base.IBaseEntity<approvalChainStep> {
        public ApprovalChainId: number;
        public ComboOperator: number;
        public Order: number;
        public UserIds: Array<primitiveType<number>>;
        public Actions: Array<approvalChainStepAction>;

        constructor(obj?: approvalChainStep) {
            super(obj);
            this.ApprovalChainId = 0;
            this.ComboOperator = 0;
            this.Order = 0;
            this.UserIds = new Array<primitiveType<number>>();
            this.Actions = new Array<approvalChainStepAction>();
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.ApprovalChainId = this.getValue<number>(data, "ApprovalChainId", 0);
            this.ComboOperator = this.getValue<number>(data, "ComboOperator", 0);
            this.Order = this.getValue<number>(data, "Order", 0);
            this.UserIds = this.getArrayConstructEntityValue<primitiveType<number>>(data, "UserIds", new Array<primitiveType<number>>(), new primitiveType());
            this.Actions = this.getArrayConstructEntityValue<approvalChainStepAction>(data, "Actions", new Array<approvalChainStepAction>(), new approvalChainStepAction());
        }

        public clone(): approvalChainStep {
            return new approvalChainStep(this);
        }
    }
}