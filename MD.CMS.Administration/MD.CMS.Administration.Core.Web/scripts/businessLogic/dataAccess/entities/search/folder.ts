/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="baseSearch.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export class folder extends baseSearch implements base.IBaseEntity<folder> {
        public Path: string;

        constructor(obj?: folder) {
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

        public clone(): folder {
            return new folder(this);
        }
    }
}