/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../../../helpers.ts" />

namespace mdBusinessLogic.dataAccess.entities.exceptions {
    export class netException implements base.IBaseEntity<netException> {
        public StackTrace: string;
        public Source: string;
        public Message: string;
        public InnerException: netException;
        public HResult: number;
        public Data: any;

        constructor(obj?: netException) {
            this.StackTrace = '';
            this.Source = '';
            this.Message = null;
            this.InnerException = null;
            this.HResult = 0;
            this.Data = null;
            if (obj !== undefined && obj != null) {
                this.StackTrace = obj.StackTrace;
                this.Source = obj.Source;
                this.Message = obj.Message;
                this.InnerException = obj.InnerException;
                this.HResult = obj.HResult;
                this.Data = obj.Data;
            }
        }

        public construct(data: any): void {
            this.StackTrace = helpers.entityHelper.getValue<string>(data, "StackTrace", '');
            this.Source = helpers.entityHelper.getValue<string>(data, "Source", '');
            this.Message = helpers.entityHelper.getValue<string>(data, "Message", '');
            this.InnerException = helpers.entityHelper.getConstructValue<netException>(data, "InnerException", null);
            this.HResult = helpers.entityHelper.getValue<number>(data, "HResult", 0);
            this.Data = helpers.entityHelper.getValue<any>(data, "Data", null);
        }

        public clone(): netException {
            return new netException(this);
        }
    }
}
