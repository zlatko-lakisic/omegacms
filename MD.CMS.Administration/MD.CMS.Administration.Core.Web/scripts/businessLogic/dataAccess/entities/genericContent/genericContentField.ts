namespace mdBusinessLogic.dataAccess.entities.genericContent {
    export abstract class genericContentField extends baseField {
        public Description: string;
        public SafeName: string;
        public Order: number;
        public Options: string;
        public JsonField: genericContentFieldJsonField;
        public OptionsJson: any;
        public DataBound: boolean;
        public DataSourceId: number;
        public DataSourceField: string;
        public DataBoundReadOnly: boolean;
        public IsDataBoundPrimaryKey: boolean;

        constructor(obj?: genericContentField) {
            super(obj);
            this.Description = ''
            this.SafeName = '';
            this.Order = 0;
            this.Options = '';
            this.JsonField = new genericContentFieldJsonField();
            this.OptionsJson = {};
            this.DataBound = false;
            this.DataSourceId = 0;
            this.DataSourceField = '';
            this.DataBoundReadOnly = false;
            this.IsDataBoundPrimaryKey = false;
            if (obj !== undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            super.construct(data);
            this.Description = this.getValue<string>(data, 'Description', '');
            this.SafeName = this.getValue<string>(data, 'SafeName', '');
            this.Order = this.getValue<number>(data, 'Order', 0);
            this.Options = this.getValue<string>(data, 'Options', '');
            this.JsonField = this.getConstructValue<genericContentFieldJsonField>(data, 'JsonField', new genericContentFieldJsonField());
            this.OptionsJson = this.getValue<any>(data, 'OptionsJson', {});
            this.DataBound = this.getValue<boolean>(data, 'DataBound', false);
            this.DataSourceId = this.getValue<number>(data, 'DataSourceId', 0);
            this.DataSourceField = this.getValue<string>(data, 'DataSourceField', '');
            this.DataBoundReadOnly = this.getValue<boolean>(data, 'DataBoundReadOnly', false);
            this.IsDataBoundPrimaryKey = this.getValue<boolean>(data, 'IsDataBoundPrimaryKey', false);
        }

        public setOptions(optionsJson: any): void {
            this.Options = JSON.stringify(optionsJson);
        }
    }
}
