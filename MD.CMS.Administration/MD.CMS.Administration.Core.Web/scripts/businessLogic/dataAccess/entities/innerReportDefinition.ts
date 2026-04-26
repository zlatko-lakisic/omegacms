/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./innerReportDefinitionEntity.ts" />
/// <reference path="./innerReportDefinitionJoin.ts" />
/// <reference path="./innerReportDefinitionColumn.ts" />
/// <reference path="./innerReportDefinitionFilter.ts" />
/// <reference path="./innerReportDefinitionGroup.ts" />
/// <reference path="./innerReportDefinitionLimit.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinition extends base.BaseEntity implements base.IBaseEntity<innerReportDefinition> {
        public Entities: Array<innerReportDefinitionEntity>;
        public Joins: Array<innerReportDefinitionJoin>;
        public Columns: Array<innerReportDefinitionColumn>;
        public Filters: Array<innerReportDefinitionFilter>;
        public Groupings: Array<innerReportDefinitionGroup>;
        public Limit: innerReportDefinitionLimit;

        constructor(obj?: innerReportDefinition) {
            super(obj);
            this.Entities = new Array<innerReportDefinitionEntity>();
            this.Joins = new Array<innerReportDefinitionJoin>();
            this.Columns = new Array<innerReportDefinitionColumn>();
            this.Filters = new Array<innerReportDefinitionFilter>();
            this.Groupings = new Array<innerReportDefinitionGroup>();
            this.Limit = new innerReportDefinitionLimit();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Entities = this.getArrayConstructEntityValue<innerReportDefinitionEntity>(data, "Entities", new Array<innerReportDefinitionEntity>(), new innerReportDefinitionEntity());
            this.Joins = this.getArrayConstructEntityValue<innerReportDefinitionJoin>(data, "Joins", new Array<innerReportDefinitionJoin>(), new innerReportDefinitionJoin());
            this.Columns = this.getArrayConstructEntityValue<innerReportDefinitionColumn>(data, "Columns", new Array<innerReportDefinitionColumn>(), new innerReportDefinitionColumn());
            this.Filters = this.getArrayConstructEntityValue<innerReportDefinitionFilter>(data, "Filters", new Array<innerReportDefinitionFilter>(), new innerReportDefinitionFilter());
            this.Groupings = this.getArrayConstructEntityValue<innerReportDefinitionGroup>(data, "Groupings", new Array<innerReportDefinitionGroup>(), new innerReportDefinitionGroup());
            this.Limit = this.getConstructEntityValue<innerReportDefinitionLimit>(data, "Limit", new innerReportDefinitionLimit());
        }

        public clone(): innerReportDefinition {
            return new innerReportDefinition(this);
        }

    }
}