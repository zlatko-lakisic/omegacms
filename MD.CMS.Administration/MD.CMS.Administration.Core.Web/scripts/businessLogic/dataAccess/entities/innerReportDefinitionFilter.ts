/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./innerReportDefinitionUniqueProperty.ts" />
/// <reference path="./innerReportDefinitionEntity.ts" />
/// <reference path="./innerReportDefinitionProperty.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinitionFilter extends innerReportDefinitionUniqueProperty implements base.IBaseEntity<innerReportDefinitionFilter> {
        public Type: number;
        public Value: string;
        public Entity: innerReportDefinitionEntity;
        public Property: innerReportDefinitionProperty;
        public IsDynamic: boolean;

        constructor(obj?: innerReportDefinitionFilter) {
            super(obj);
            this.Type = 0;
            this.Value = '';
            this.Entity = new innerReportDefinitionEntity();
            this.Property = new innerReportDefinitionProperty();
            this.IsDynamic = false;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Type = this.getValue<number>(data, "Type", 0);
            this.Entity = this.getConstructEntityValue<innerReportDefinitionEntity>(data, "Entity", new innerReportDefinitionEntity());
            this.Value = this.getValue<string>(data, "Value", '');
            this.Property = this.getConstructEntityValue<innerReportDefinitionProperty>(data, "Property", new innerReportDefinitionProperty());
            this.IsDynamic = this.getValue<boolean>(data, "IsDynamic", false);
        }

        public clone(): innerReportDefinitionFilter {
            return new innerReportDefinitionFilter(this);
        }

    }
}