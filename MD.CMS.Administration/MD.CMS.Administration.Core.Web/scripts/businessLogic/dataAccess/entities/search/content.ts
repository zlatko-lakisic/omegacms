/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="baseSearch.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export class content extends baseSearch implements base.IBaseEntity<content> {
        public Path: string;
        public DateCreated: Date;
        public FolderId: number;

        constructor(obj?: content) {
            super(obj);
            this.Path = '';
            this.DateCreated = null;
            this.FolderId = 0;
            if (obj !== undefined && obj != null) {
                this.Path = obj.Path;
                this.DateCreated = obj.DateCreated;
                this.FolderId = obj.FolderId;
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.Path = mdBusinessLogic.helpers.entityHelper.getValue<string>(data, 'Path', '');
            this.DateCreated = mdBusinessLogic.helpers.entityHelper.getValue<Date>(data, 'DateCreated', null);
            this.FolderId = mdBusinessLogic.helpers.entityHelper.getValue<number>(data, 'FolderId', 0);
        }

        public clone(): content {
            return new content(this);
        }
    }
}