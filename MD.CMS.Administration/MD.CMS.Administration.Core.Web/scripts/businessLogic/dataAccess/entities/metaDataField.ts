/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./attributeTypeDefinition.ts" />
/// <reference path="./genericContent/baseField.ts" />
/// <reference path="./genericContent/genericContentFieldValue.ts" />

namespace mdBusinessLogic {
    export namespace dataAccess {
        export namespace entities {
            export class metaDataField extends entities.genericContent.genericContentField implements base.IBaseEntity<metaDataField>{

                constructor(obj?: metaDataField) {
                    super(obj);
                }

                public construct(data: any) {
                    super.construct(data);
                }

                public clone(): metaDataField {
                    return new metaDataField(this);
                }
            }
        }
    }
}
