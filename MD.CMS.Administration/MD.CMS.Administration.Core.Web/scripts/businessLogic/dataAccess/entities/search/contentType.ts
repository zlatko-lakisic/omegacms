/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="baseSearch.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export class contentType extends baseSearch implements base.IBaseEntity<contentType> {

        constructor(obj?: contentType) {
            super(obj);
        }

        public construct(data: any) {
            super.construct(data);
        }

        public clone(): contentType {
            return new contentType(this);
        }
    }
}