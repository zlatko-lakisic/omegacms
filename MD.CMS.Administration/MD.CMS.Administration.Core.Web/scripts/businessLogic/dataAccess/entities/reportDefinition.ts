/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./user.ts" />
/// <reference path="./innerReportDefinition.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class reportDefinition extends base.BaseEntity implements base.IBaseEntity<reportDefinition> {
        public Name: string;
        public Definition: innerReportDefinition;
        public Sql: string;
        public AuthorId: number;
        public Author: user;
        public Json: string;
        public DateCreated: Date;
        public DateModified: Date;

        constructor(obj?: reportDefinition) {
            super(obj);
            this.Name = '';
            this.Definition = new innerReportDefinition();
            this.Sql = '';
            this.AuthorId = 0;
            this.Author = new user();
            this.Json = '';
            this.DateCreated = new Date();
            this.DateModified = new Date();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Definition = this.getConstructEntityValue<innerReportDefinition>(data, "Definition", new innerReportDefinition());
            this.Sql = this.getValue<string>(data, "Sql", '');
            this.AuthorId = this.getValue<number>(data, "AuthorId", 0);
            this.Author = this.getConstructEntityValue<user>(data, "Author", new user());
            this.Json = this.getValue<string>(data, "Json", '');
            this.DateCreated = this.getValue<Date>(data, "DateCreated", new Date());
            this.DateModified = this.getValue<Date>(data, "DateModified", new Date());
        }

        public clone(): reportDefinition {
            return new reportDefinition(this);
        }

    }
}