/// <reference path="socketModel.ts" />

namespace mdBusinessLogic.dataAccess.entities.comm {
    export class awsSocketModel extends socketModel implements base.IBaseEntity<awsSocketModel> {
        public requestId: string;

        constructor(obj?: awsSocketModel) {
            super(obj);
            this.requestId = '';
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            this.requestId = helpers.entityHelper.getValue<string>(data, "requestId", null);
        }

        public clone(): awsSocketModel {
            return new awsSocketModel(this);
        }
    }

    export enum executionScheduleType {
        Manual = 0,
        Recurring = 1
    }
}
