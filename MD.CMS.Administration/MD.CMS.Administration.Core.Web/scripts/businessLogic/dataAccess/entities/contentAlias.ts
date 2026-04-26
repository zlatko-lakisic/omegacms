/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/BaseEntity.ts" />
namespace mdBusinessLogic.dataAccess.entities {
    export class contentAlias extends base.BaseEntity implements base.IBaseEntity<contentAlias> {
        public LCID: number;
        public DateCreated: Date;
        public ContentId: number;
        public Alias: string;

        constructor(obj?: contentAlias) {
            super(obj);
            this.LCID = 0;
            this.DateCreated = new Date();
            this.ContentId = 0;
            this.Alias = '';
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.LCID = this.getValue<number>(data, 'LCID', 0);
            this.DateCreated = this.getValue<Date>(data, 'DateCreated', null);
            this.ContentId = this.getValue<number>(data, 'ContentId', 0);
            this.Alias = this.getValue<string>(data, 'Alias', '');
        }

        public clone(): contentAlias {
            return new contentAlias(this);
        }
    }
}