/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinitionProperty extends base.BaseEntity implements base.IBaseEntity<innerReportDefinitionProperty> {
        public Type: number;
        public Name: string;
        public Enabled: boolean;

        constructor(obj?: innerReportDefinitionProperty) {
            super(obj);
            this.Type = 0;
            this.Name = '';
            this.Enabled = false;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Type = this.getValue<number>(data, "Type", 0);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Enabled = this.getValue<boolean>(data, "Enabled", false);
        }

        public clone(): innerReportDefinitionProperty {
            return new innerReportDefinitionProperty(this);
        }

    }
}