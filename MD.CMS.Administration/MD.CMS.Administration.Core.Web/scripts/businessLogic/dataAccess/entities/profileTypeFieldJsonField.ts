/// <reference path="./base/iBaseEntity.ts" />
/// <reference path="./base/baseEntity.ts" />

namespace mdBusinessLogic.dataAccess.entities {
    export class profileTypeFieldJsonField implements base.IBaseEntity<profileTypeFieldJsonField> {
        public validation: fieldValidation;
        public helpText: string;
        public access: string;
        public cssClass: string;
        public toggle: string;
        public hidden: boolean;
        public enabled: boolean;

        constructor(obj?: profileTypeFieldJsonField) {
            this.validation = new fieldValidation();
            this.helpText = '';
            this.access = '';
            this.cssClass = '';
            this.toggle = '';
            this.hidden = false;
            this.enabled = true;
            if (obj != undefined && obj != null) {
                this.construct(obj);
            }
        }

        public construct(data: any) {
            this.validation = helpers.entityHelper.getConstructValue<fieldValidation>(data, 'validation', new fieldValidation());
            this.helpText = helpers.entityHelper.getValue<string>(data, 'helpText', '');
            this.access = helpers.entityHelper.getValue<string>(data, 'access', '');
            this.cssClass = helpers.entityHelper.getValue<string>(data, 'cssClass', '');
            this.toggle = helpers.entityHelper.getValue<string>(data, 'toggle', '');
            this.hidden = helpers.entityHelper.getValue<boolean>(data, 'hidden', false);
            this.enabled = helpers.entityHelper.getValue<boolean>(data, 'enabled', true);
        }

        public clone(): profileTypeFieldJsonField {
            return new profileTypeFieldJsonField(this);
        }
    }
}
