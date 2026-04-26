/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="baseSearch.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export class profileType extends baseSearch implements base.IBaseEntity<profileType> {

        constructor(obj?: profileType) {
            super(obj);
        }

        public construct(data: any) {
            super.construct(data);
        }

        public clone(): profileType {
            return new profileType(this);
        }
    }
}