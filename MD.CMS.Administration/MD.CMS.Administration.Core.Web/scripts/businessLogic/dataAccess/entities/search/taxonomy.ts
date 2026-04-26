/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="baseSearch.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export class taxonomy extends baseSearch implements base.IBaseEntity<taxonomy> {
        public Path: string;

        constructor(obj?: taxonomy) {
            super(obj);
            this.Path = '';
            if (obj !== undefined && obj != null) {
                this.Path = obj.Path;
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.Path = mdBusinessLogic.helpers.entityHelper.getValue<string>(data, 'Path', '');
        }

        public clone(): taxonomy {
            return new taxonomy(this);
        }
    }
}