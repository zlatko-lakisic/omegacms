/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class reportSchedulerAction extends base.BaseEntity implements base.IBaseEntity<reportSchedulerAction> {
        public SchedulerId: number;
        public Name: string;
        public AuthorId: number;
        public DateCreated: Date;
        public DateEdited: Date;
        public ActionType: number;
        public Options: string;
        public IsActive: boolean;

        constructor(obj?: reportSchedulerAction) {
            super(obj);
            this.SchedulerId = 0;
            this.Name = '';
            this.AuthorId = 0;
            this.DateCreated = new Date();
            this.DateEdited = null;
            this.ActionType = 0;
            this.Options = '';
            this.IsActive = false;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.SchedulerId = this.getValue<number>(data, "SchedulerId", 0);
            this.Name = this.getValue<string>(data, "Name", '');
            this.AuthorId = this.getValue<number>(data, "AuthorId", 0);
            this.DateCreated = this.getValue<Date>(data, "DateCreated", new Date());
            this.DateEdited = this.getValue<Date>(data, "DateEdited", null);
            this.ActionType = this.getValue<number>(data, "ActionType", 0);
            this.Options = this.getValue<string>(data, "Options", '');
            this.IsActive = this.getValue<boolean>(data, "IsActive", false);
        }

        public clone(): reportSchedulerAction {
            return new reportSchedulerAction(this);
        }

    }
}