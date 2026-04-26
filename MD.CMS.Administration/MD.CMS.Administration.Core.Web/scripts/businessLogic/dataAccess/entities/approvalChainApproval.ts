/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./content.ts" />
/// <reference path="./user.ts" />
/// <reference path="./approvalChainStep.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class approvalChainApproval extends base.BaseEntity implements base.IBaseEntity<approvalChainApproval> {
        public ApprovalType: number;
        public ReviewDate: Date;
        public Content: content;
        public Comment: string;
        public User: user;
        public Step: approvalChainStep;

        constructor(obj?: approvalChainApproval) {
            super(obj);
            this.ApprovalType = 1;
            this.ReviewDate = null;
            this.Content = null;
            this.Comment = '';
            this.User = null;
            this.Step = null;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            super.construct(data);
            this.ApprovalType = this.getValue<number>(data, "ApprovalType", 1);
            this.ReviewDate = this.getValue<Date>(data, "ReviewDate", null);
            this.Content = this.getValue<content>(data, "Content", new content());
            this.Comment = this.getValue<string>(data, "Comment", '');
            this.User = this.getValue<user>(data, "User", new user());
            this.Step = this.getValue<approvalChainStep>(data, "Step", new approvalChainStep());
        }

        public clone(): approvalChainApproval {
            return new approvalChainApproval(this);
        }
    }
}