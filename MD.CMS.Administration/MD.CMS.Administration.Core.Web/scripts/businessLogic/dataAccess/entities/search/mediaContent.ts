/// <reference path="../base/iBaseEntity.ts" />
/// <reference path="baseSearch.ts" />
/// <reference path="../../../helpers/entityHelper.ts" />
namespace mdBusinessLogic.dataAccess.entities.search {
    export class mediaContent extends baseSearch implements base.IBaseEntity<mediaContent> {
        public Path: string;
        public DateCreated: Date;
        public FolderId: number;
        public FileType: mdBusinessLogic.dataAccess.entities.mediaContentInputType
        public FileName: string;

        constructor(obj?: mediaContent) {
            super(obj);
            this.Path = '';
            this.DateCreated = null;
            this.FolderId = 0;
            this.FileType = null;
            this.FileName = '';
            if (obj !== undefined && obj != null) {
                this.Path = obj.Path;
                this.DateCreated = obj.DateCreated;
                this.FolderId = obj.FolderId;
                this.FileType = obj.FileType;
                this.FileName = obj.FileName;
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.Path = mdBusinessLogic.helpers.entityHelper.getValue<string>(data, 'Path', '');
            this.DateCreated = mdBusinessLogic.helpers.entityHelper.getValue<Date>(data, 'DateCreated', null);
            this.FolderId = mdBusinessLogic.helpers.entityHelper.getValue<number>(data, 'FolderId', 0);
            this.FileType = mdBusinessLogic.helpers.entityHelper.getValue<number>(data, 'FileType', 0);
            this.FileName = mdBusinessLogic.helpers.entityHelper.getValue<string>(data, 'FileName', '');
        }

        public clone(): mediaContent {
            return new mediaContent(this);
        }
    }
}