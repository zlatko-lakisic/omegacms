/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./metaDataField.ts" />
/// <reference path="./genericContent/genericContentFieldValue.ts" />
namespace mdBusinessLogic {
    export namespace dataAccess {
        export namespace entities {
            export class metaDataFieldValue extends entities.genericContent.genericContentFieldValue implements base.IBaseEntity<metaDataFieldValue>{
                public ContentId: number;
                public LCID: number;
                public DateCreated: Date;
                public MetaDataFieldId: number;

                constructor(obj?: metaDataFieldValue) {
                    super(obj);
                    this.ContentId = 0;
                    this.LCID = 0;
                    this.DateCreated = new Date();
                    this.MetaDataFieldId = 0;
                    if (obj != undefined && obj != null) {
                        this.construct(obj);
                    }
                }

                public construct(data: any) {
                    super.construct(data);
                    this.ContentId = this.getValue<number>(data, 'ContentId', 0);
                    this.LCID = this.getValue<number>(data, 'LCID', 0);
                    this.DateCreated = this.getValue<Date>(data, 'DateCreated', new Date());
                    this.MetaDataFieldId = this.getValue<number>(data, 'MetaDataFieldId', 0);
                }

                public clone(): metaDataFieldValue {
                    return new metaDataFieldValue(this);
                }
            }
        }
    }
}
