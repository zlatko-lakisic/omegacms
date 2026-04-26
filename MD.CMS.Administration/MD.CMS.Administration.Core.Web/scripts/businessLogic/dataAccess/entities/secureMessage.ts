/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class secureMessage extends base.BaseEntity implements base.IBaseEntity<secureMessage> {
        public EndPoint: string;
        public Message: string;
        public IsEncripted: boolean;

        constructor(obj?: secureMessage) {
            super(obj);
            this.EndPoint = '';
            this.Message = '';
            this.IsEncripted = false;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.EndPoint = this.getValue<string>(data, "EndPoint", '');
            this.Message = this.getValue<string>(data, "Message", '');
            this.IsEncripted = this.getValue<boolean>(data, "IsEncripted", false);
        }

        public clone(): secureMessage {
            return new secureMessage(this);
        }

    }
}