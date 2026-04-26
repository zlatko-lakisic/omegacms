/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./innerReportDefinitionProperty.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinitionUniqueProperty extends base.BaseEntity implements base.IBaseEntity<innerReportDefinitionUniqueProperty> {
        public UniqueId: string;
        public Property: innerReportDefinitionProperty;

        constructor(obj?: innerReportDefinitionUniqueProperty) {
            super(obj);
            this.UniqueId = '';
            this.Property = new innerReportDefinitionProperty();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.UniqueId = this.getValue<string>(data, "UniqueId", '');
            this.Property = this.getConstructEntityValue<innerReportDefinitionProperty>(data, "Property", new innerReportDefinitionProperty());
        }

        public clone(): innerReportDefinitionUniqueProperty {
            return new innerReportDefinitionUniqueProperty(this);
        }

    }
}