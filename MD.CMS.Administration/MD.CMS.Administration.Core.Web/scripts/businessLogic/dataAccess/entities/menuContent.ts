/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class menuContent extends base.BaseEntity implements base.IBaseEntity<menuContent> {
        public LCID: number;
        public DateCreated: Date;
        public MenuId: number;
        public Title: string;
        public MenuContentPath: string;

        constructor(obj?: menuContent) {
            super(obj);
            this.LCID = 0;
            this.DateCreated = new Date();
            this.MenuId = 0;
            this.Title = '';
            this.MenuContentPath = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.LCID = this.getValue<number>(data, "LCID", 0);
            this.DateCreated = this.getValue<Date>(data, "DateCreated", new Date());
            this.MenuId = this.getValue<number>(data, "MenuId", 0);
            this.Title = this.getValue<string>(data, "Title", '');
            this.MenuContentPath = this.getValue<string>(data, "MenuContentPath", '');
        }

        public clone(): menuContent {
            return new menuContent(this);
        }

    }
}