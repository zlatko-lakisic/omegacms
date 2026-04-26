/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../../../helpers.ts" />

namespace mdBusinessLogic.dataAccess.entities.comm {
    export class socketModel implements base.IBaseEntity<socketModel> {
        public message: string;
        public connectionId: string;

        constructor(obj?: socketModel) {
            this.message = '';
            this.connectionId = '';
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            this.message = helpers.entityHelper.getValue<string>(data, "message", null);
            this.connectionId = helpers.entityHelper.getValue<string>(data, "connectionId", null);
        }

        public clone(): socketModel {
            return new socketModel(this);
        }
    }
}
