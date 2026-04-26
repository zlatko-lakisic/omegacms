/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./metaDataField.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class mediaContentMetaDataFeldValues extends metaDataField implements base.IBaseEntity<mediaContentMetaDataFeldValues> {
        public MediaContentId: number;
        public DateCreated: Date;
        public Value: string;
        public MetaDataFieldId: number;

        constructor(obj?: mediaContentMetaDataFeldValues) {
            super(obj);
            this.MediaContentId = 0;
            this.DateCreated = new Date();
            this.Value = '';
            this.MetaDataFieldId = 0;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.MediaContentId = this.getValue<number>(data, "MediaContentId", 0);
            this.DateCreated = this.getValue<Date>(data, "DateCreated", new Date());
            this.Value = this.getValue<string>(data, "Value", '');
            this.MetaDataFieldId = this.getValue<number>(data, "MetaDataFieldId", 0);
        }

        public clone(): mediaContentMetaDataFeldValues {
            return new mediaContentMetaDataFeldValues(this);
        }

    }
}