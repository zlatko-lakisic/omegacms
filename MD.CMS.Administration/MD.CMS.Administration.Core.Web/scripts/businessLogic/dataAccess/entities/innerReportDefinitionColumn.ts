/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./innerReportDefinitionUniqueProperty.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinitionColumn extends innerReportDefinitionUniqueProperty implements base.IBaseEntity<innerReportDefinitionColumn> {
        public Type: number;
        public Value: string;

        constructor(obj?: innerReportDefinitionColumn) {
            super(obj);
            this.Type = 0;
            this.Value = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Type = this.getValue<number>(data, "Type", 0);
            this.Value = this.getValue<string>(data, "Value", '');
        }

        public clone(): innerReportDefinitionColumn {
            return new innerReportDefinitionColumn(this);
        }

    }
}