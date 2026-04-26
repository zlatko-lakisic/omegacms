/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="netException.ts" />
/// <reference path="../../../helpers.ts" />

namespace mdBusinessLogic.dataAccess.entities.exceptions {
    export class errorDetails implements base.IBaseEntity<errorDetails> {
        public StatusCode: number;
        public Message: string;
        public InnerException: netException;

        constructor(obj?: errorDetails) {
            this.StatusCode = 0;
            this.Message = '';
            this.InnerException = null;
            if (obj !== undefined && obj != null) {
                this.StatusCode = obj.StatusCode;
                this.Message = obj.Message;
                this.InnerException = obj.InnerException;
            }
        }

        public construct(data: any): void {
            this.StatusCode = helpers.entityHelper.getValue<number>(data, "StatusCode", 0);
            this.Message = helpers.entityHelper.getValue<string>(data, "Message", '');
            this.InnerException = helpers.entityHelper.getConstructValue<netException>(data, "InnerException", null);
        }

        public clone(): errorDetails {
            return new errorDetails(this);
        }
    }
}
