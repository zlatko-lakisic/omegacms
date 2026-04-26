/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/profileTypeField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class profileTypeFieldController extends base.BaseController<profileTypeFieldController, entities.profileTypeField> {

        constructor() {
            super('ProfileTypeField/');
        }

        public getById(id: number, onSuccess: (obj: entities.profileTypeField) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField> = new base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.profileTypeField();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByProfileType(id: number, onSuccess: (obj: Array<entities.profileTypeField>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField> = new base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByProfileType', [id]);
            options.responseData = new entities.profileTypeField();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(profileTypeField: entities.profileTypeField, onSuccess: (obj: entities.profileTypeField) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField> = new base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.profileTypeField();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.profileTypeField) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField> = new base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.profileTypeField();
            options.onSuccess = (options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<profileTypeFieldController, entities.profileTypeField>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }
    }
}