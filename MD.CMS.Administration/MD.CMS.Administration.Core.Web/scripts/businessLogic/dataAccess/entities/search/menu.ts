/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="baseSearch.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export class menu extends baseSearch implements base.IBaseEntity<menu> {
        public Path: string;

        constructor(obj?: menu) {
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

        public clone(): menu {
            return new menu(this);
        }
    }
}