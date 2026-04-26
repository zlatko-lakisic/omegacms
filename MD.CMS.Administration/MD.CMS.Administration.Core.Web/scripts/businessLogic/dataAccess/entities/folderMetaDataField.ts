/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class folderMetaDataField extends base.BaseEntity implements base.IBaseEntity<folderMetaDataField> {
        public FolderId: number;
        public MetaDataFieldId: number;
        public IsRequired: boolean;
        public Checked: boolean;
        public Name: string;

        constructor(obj?: folderMetaDataField) {
            super(obj);
            this.FolderId = 0;
            this.MetaDataFieldId = 0;
            this.IsRequired = false;
            this.Checked = false;
            this.Name = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.FolderId = this.getValue<number>(data, "FolderId", 0);
            this.MetaDataFieldId = this.getValue<number>(data, "MetaDataFieldId", 0);
            this.IsRequired = this.getValue<boolean>(data, "IsRequired", false);
            this.Checked = this.getValue<boolean>(data, "Checked", false);
            this.Name = this.getValue<string>(data, "Name", '');
        }

        public clone(): folderMetaDataField {
            return new folderMetaDataField(this);
        }

    }
}