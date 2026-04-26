/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities.models {
    export class initModel implements base.IBaseEntity<initModel> {
        public Initiated: boolean;

        constructor(obj?: initModel) {
            this.Initiated = false;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            this.Initiated = helpers.entityHelper.getValue<boolean>(data, "Initiated", false);
        }

        public clone(): initModel {
            return new initModel(this);
        }
    }
}