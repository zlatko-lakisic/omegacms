/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class attributeTypeDefinition extends base.BaseEntity implements base.IBaseEntity<attributeTypeDefinition> {
        public Name: string;
        public DefaultValue: string;
        public Type: number;
        public InputType: number;

        constructor(obj?: attributeTypeDefinition) {
            super(obj);
            this.Name = '';
            this.DefaultValue = '';
            this.Type = null;
            this.InputType = null;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.Name = this.getValue<string>(data, 'Name', '');
            this.DefaultValue = this.getValue<string>(data, 'DefaultValue', '');
            this.Type = this.getValue<number>(data, 'Type', 0);
            this.InputType = this.getValue<number>(data, 'InputType', 0);
        }

        public clone(): attributeTypeDefinition {
            return new attributeTypeDefinition(this);
        }
    }
}