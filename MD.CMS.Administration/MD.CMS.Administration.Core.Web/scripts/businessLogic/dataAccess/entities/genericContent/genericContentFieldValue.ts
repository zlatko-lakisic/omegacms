/// <reference path="./genericContentField.ts" />
namespace mdBusinessLogic.dataAccess.entities.genericContent {
    export abstract class genericContentFieldValue extends genericContentField {
        public Value: string;

        constructor(obj?: genericContentFieldValue) {
            super(obj);
            this.Value = '';
            if (obj != undefined && obj != null) {
                this.construct(obj);
            } else {
                if ((this.Value === undefined || this.Value == null || this.Value == '') && (this.DefaultValue !== undefined && this.DefaultValue != null)) {
                    this.Value = this.DefaultValue;
                }
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.Value = this.getValue<string>(data, 'Value', '');

            if ((this.Value === undefined || this.Value == null || this.Value == '') && (this.DefaultValue !== undefined && this.DefaultValue != null)) {
                this.Value = this.DefaultValue;
            }
        }
    }
}
