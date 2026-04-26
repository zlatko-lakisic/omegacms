/// <reference path="../attributeTypeDefinition.ts" />
namespace mdBusinessLogic.dataAccess.entities.genericContent {
    export abstract class baseField extends base.BaseEntity {
        public AttributeTypeDefinitionId: number;
        public Name: string;
        public DefaultValue: string;
        public AttributeTypeDefinition: attributeTypeDefinition;
        public Delimiter: string;
        public ListValue: string;
        public FriendlyName: string;
        public IsRequired: boolean;
        public UniqueId: string;
        public IsReadOnly: boolean;

        constructor(obj?: baseField) {
            super(obj);
            this.AttributeTypeDefinitionId = 0;
            this.AttributeTypeDefinition = null;
            this.Name = '';
            this.IsRequired = false;
            this.DefaultValue = '';
            this.Delimiter = '';
            this.ListValue = '';
            this.FriendlyName = '';
            this.UniqueId = '';
            this.IsReadOnly = false;

            if (obj != undefined && obj != null) {
                this.construct(obj);
            } else {
                if (this.ListValue == '[]') {
                    this.ListValue = '';
                }
                if (this.DefaultValue == '[]') {
                    this.DefaultValue = '';
                }

                if ((this.DefaultValue === undefined || this.DefaultValue == null || this.DefaultValue == '') && !(this.ListValue === undefined || this.ListValue == null || this.ListValue == '')) {
                    this.DefaultValue = this.getListValueAsArray()[0] || '';
                }
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.AttributeTypeDefinitionId = this.getValue<number>(data, 'AttributeTypeDefinitionId', 0);
            this.AttributeTypeDefinition = this.getConstructEntityValue<attributeTypeDefinition>(data, 'AttributeTypeDefinition', new attributeTypeDefinition());
            this.Name = this.getValue<string>(data, 'Name', '');
            this.IsRequired = this.getValue<boolean>(data, 'IsRequired', false);
            this.DefaultValue = this.getValue<string>(data, 'DefaultValue', '');
            this.FriendlyName = this.getValue<string>(data, 'FriendlyName', '');
            this.UniqueId = this.getValue<string>(data, 'UniqueId', '');
            this.Delimiter = this.getValue<string>(data, 'Delimiter', '');
            this.ListValue = this.getValue<string>(data, 'ListValue', '');
            this.IsReadOnly = this.getValue<boolean>(data, 'IsReadOnly', false);

            if (this.ListValue == '[]') {
                this.ListValue = '';
            }
            if (this.DefaultValue == '[]') {
                this.DefaultValue = '';
            }

            if ((this.DefaultValue === undefined || this.DefaultValue == null || this.DefaultValue == '') && !(this.ListValue === undefined || this.ListValue == null || this.ListValue == '')) {
                this.DefaultValue = this.getListValueAsArray()[0] || '';
            }
        }

        public getListValueAsArray(): Array<string> {
            if (this.ListValue === undefined || this.ListValue == null) {
                this.ListValue = '';
            }
            if (this.Delimiter === undefined || this.Delimiter == null) {
                this.Delimiter = '';
            }
            return this.ListValue.split(this.Delimiter);
        }
    }
}
