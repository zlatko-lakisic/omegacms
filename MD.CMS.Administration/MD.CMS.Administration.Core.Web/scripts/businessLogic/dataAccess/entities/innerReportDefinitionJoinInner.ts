/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./innerReportDefinitionEntity.ts" />
/// <reference path="./innerReportDefinitionProperty.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinitionJoinInner extends base.BaseEntity implements base.IBaseEntity<innerReportDefinitionJoinInner> {
        public Entity: innerReportDefinitionEntity;
        public Property: innerReportDefinitionProperty;

        constructor(obj?: innerReportDefinitionJoinInner) {
            super(obj);
            this.Entity = new innerReportDefinitionEntity();
            this.Property = new innerReportDefinitionProperty();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Entity = this.getConstructEntityValue<innerReportDefinitionEntity>(data, "Entity", new innerReportDefinitionEntity());
            this.Property = this.getConstructEntityValue<innerReportDefinitionProperty>(data, "Property", new innerReportDefinitionProperty());
        }

        public clone(): innerReportDefinitionJoinInner {
            return new innerReportDefinitionJoinInner(this);
        }

    }
}