/// <reference path="./genericKeyValuePair.ts" />

namespace mdBusinessLogic.dataAccess.entities.generic {
    export class keyValuePair extends genericKeyValuePair<string> implements base.IBaseEntity<keyValuePair> {

        constructor(obj?: iGenericKeyValuePair<string>) {
            super(obj);
        }

        public clone(): keyValuePair {
            return new keyValuePair(this);
        }
    }
}
