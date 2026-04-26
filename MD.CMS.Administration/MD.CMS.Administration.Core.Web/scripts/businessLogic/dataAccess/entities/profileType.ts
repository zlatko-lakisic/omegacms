/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />
/// <reference path="./profileTypeFieldValue.ts" />
/// <reference path="./rwdPermission.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class profileType extends base.BaseEntity implements base.IBaseEntity<profileType> {
        public Name: string;
        public Icon: string;
        public Description: string;
        public PermissionXmlText: string;
        public Fields: Array<profileTypeFieldValue>;
        public IsAssigned: boolean;
        public RWDPermissions: Array<rwdPermission>;

        constructor(obj?: profileType) {
            super(obj);
            this.Name = '';
            this.Icon = '';
            this.Description = '';
            this.PermissionXmlText = '';
            this.Fields = new Array<profileTypeFieldValue>();
            this.IsAssigned = false;
            this.RWDPermissions = new Array<rwdPermission>();
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        construct(data: any): void {
            super.construct(data);
            this.Name = this.getValue<string>(data, "Name", '');
            this.Icon = this.getValue<string>(data, "Icon", '');
            this.Description = this.getValue<string>(data, "Description", '');
            this.PermissionXmlText = this.getValue<string>(data, "PermissionXmlText", '');
            this.Fields = this.getArrayConstructEntityValue<profileTypeFieldValue>(data, "Fields", new Array<profileTypeFieldValue>(), new profileTypeFieldValue());
            this.IsAssigned = this.getValue<boolean>(data, "IsAssigned", false);
            this.RWDPermissions = this.getArrayConstructEntityValue<rwdPermission>(data, "RWDPermissions", new Array<rwdPermission>(), new rwdPermission());
        }

        public clone(): profileType {
            return new profileType(this);
        }

        public setFieldValue(value: string, fieldName: string): void {
            if (this.Fields != null) {
                for (var i in this.Fields) {
                    if (this.Fields[i].Name == fieldName) {
                        this.Fields[i]['Value'] = value;
                        break;
                    }
                }
            }
        }

        public getFieldValue(fieldName: string): string {
            if (this.Fields != null) {
                for (var i in this.Fields) {
                    if (this.Fields[i].Name == fieldName && this.Fields[i]['Value'] !== undefined) {
                        return this.Fields[i]['Value'];
                    }
                }
            }
            return null;
        }

        public getField(fieldName: string): profileTypeFieldValue {
            if (this.Fields != null) {
                for (var i in this.Fields) {
                    if (this.Fields[i].Name == fieldName) {
                        return this.Fields[i];
                    }
                }
            }
            return null;
        }
    }
}
