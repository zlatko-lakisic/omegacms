/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="../base/baseEntity.ts" />
/// <reference path="../../../helpers.ts" />
/// <reference path="../menuContent.ts" />

namespace mdBusinessLogic.dataAccess.entities.generic {
    export interface iGenericKeyValuePair<T> {
        Key: string;
        Value: T;
    }

    export class genericKeyValuePair<T> implements base.IBaseEntity<genericKeyValuePair<T>>, iGenericKeyValuePair<T> {
        public Key: string;
        public Value: T;

        constructor(obj?: iGenericKeyValuePair<T>) {
            this.Key = '';
            this.Value = null;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any): void {
            this.Key = helpers.entityHelper.getValue<string>(data, "Key", '');
            this.Value = helpers.entityHelper.getValue<T>(data, "Value", null);
        }

        public clone(): genericKeyValuePair<T> {
            return new genericKeyValuePair<T>(this);
        }
    }
}
