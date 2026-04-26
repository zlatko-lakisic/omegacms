/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />
/// <reference path="./reportSchedulerAction.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class reportScheduler extends base.BaseEntity implements base.IBaseEntity<reportScheduler> {
        public Name: string;
        public AuthorId: number;
        public DateCreated: Date;
        public IsRecurring: boolean;
        public Interval: number;
        public Start: Date;
        public End: Date;
        public ReportId: number;
        public IsActive: boolean;
        public Actions: Array<reportSchedulerAction>;
        public Author: user;

        constructor(obj?: reportScheduler) {
            super(obj);
            this.Name = '';
            this.AuthorId = 0;
            this.DateCreated = new Date();
            this.IsRecurring = false;
            this.Interval = 0;
            this.Start = null;
            this.End = null;
            this.ReportId = 0;
            this.IsActive = false;
            this.Actions = new Array<reportSchedulerAction>();
            this.Author = new user();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Name = this.getValue<string>(data, "Name", '');
            this.AuthorId = this.getValue<number>(data, "AuthorId", 0);
            this.DateCreated = this.getValue<Date>(data, "DateCreated", new Date());
            this.IsRecurring = this.getValue<boolean>(data, "IsRecurring", false);
            this.Interval = this.getValue<number>(data, "Interval", 0);
            this.Start = this.getValue<Date>(data, "Start", null);
            this.End = this.getValue<Date>(data, "End", null);
            this.ReportId = this.getValue<number>(data, "ReportId", 0);
            this.IsActive = this.getValue<boolean>(data, "IsActive", false);
            this.Actions = this.getArrayConstructEntityValue<reportSchedulerAction>(data, "Actions", new Array<reportSchedulerAction>(), new reportSchedulerAction());
            this.Author = this.getConstructEntityValue<user>(data, "Author", new user());
        }

        public clone(): reportScheduler {
            return new reportScheduler(this);
        }

    }
}