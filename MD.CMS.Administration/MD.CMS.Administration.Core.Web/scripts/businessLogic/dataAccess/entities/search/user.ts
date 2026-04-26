/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="baseSearch.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export class user extends baseSearch implements base.IBaseEntity<user> {

        constructor(obj?: user) {
            super(obj);
        }

        public construct(data: any) {
            super.construct(data);
        }

        public clone(): user {
            return new user(this);
        }
    }
}