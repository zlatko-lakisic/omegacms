/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./innerReportDefinitionGridCoordinates.ts" />
/// <reference path="./innerReportDefinitionProperty.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class innerReportDefinitionEntity extends base.BaseEntity implements base.IBaseEntity<innerReportDefinitionEntity> {
        public Type: number;
        public Name: string;
        public Coordinates: innerReportDefinitionGridCoordinates;
        public UniqueId: string;
        public Fields: Array<innerReportDefinitionProperty>;
        public BaseFields: Array<innerReportDefinitionProperty>;
        public ExtendedFields: Array<innerReportDefinitionProperty>;
        public Icon: string;

        constructor(obj?: innerReportDefinitionEntity) {
            super(obj);
            this.Type = 0;
            this.Name = '';
            this.Icon = '';
            this.Coordinates = new innerReportDefinitionGridCoordinates();
            this.UniqueId = '';
            this.Fields = new Array<innerReportDefinitionProperty>();
            this.BaseFields = new Array<innerReportDefinitionProperty>();
            this.ExtendedFields = new Array<innerReportDefinitionProperty>();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Type = this.getValue<number>(data, "Type", 0);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Icon = this.getValue<string>(data, "Icon", '');
            this.Coordinates = this.getConstructEntityValue<innerReportDefinitionGridCoordinates>(data, "Coordinates", new innerReportDefinitionGridCoordinates());
            this.UniqueId = this.getValue<string>(data, "UniqueId", '');
            this.Fields = this.getArrayConstructEntityValue<innerReportDefinitionProperty>(data, "Fields", new Array<innerReportDefinitionProperty>(), new innerReportDefinitionProperty());
            this.BaseFields = this.getArrayConstructEntityValue<innerReportDefinitionProperty>(data, "BaseFields", new Array<innerReportDefinitionProperty>(), new innerReportDefinitionProperty());
            this.ExtendedFields = this.getArrayConstructEntityValue<innerReportDefinitionProperty>(data, "ExtendedFields", new Array<innerReportDefinitionProperty>(), new innerReportDefinitionProperty());
        }

        public clone(): innerReportDefinitionEntity {
            return new innerReportDefinitionEntity(this);
        }

        public getTypeString(): string {
            return innerReportDefinitionEntityTypes[this.Type];
        }
    }

    export enum innerReportDefinitionEntityTypes {
        Content = 1,
        User = 2,
        Taxonomy = 3,
        MediaContent = 4,
        Folder = 5
    }
}