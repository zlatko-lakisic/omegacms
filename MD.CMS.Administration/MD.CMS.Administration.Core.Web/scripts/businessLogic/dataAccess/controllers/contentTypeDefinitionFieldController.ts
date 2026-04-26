/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentTypeDefinitionField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class contentTypeDefinitionFieldController extends base.BaseController<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField> {

        constructor() {
            super('ContentTypeDefinitionField/');
        }

        public getById(id: number, onSuccess: (obj: entities.contentTypeDefinitionField) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField> = new base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.contentTypeDefinitionField();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByContentTypeDefinition(id: number, onSuccess: (obj: Array<entities.contentTypeDefinitionField>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField> = new base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByContentTypeDefinition', [id]);
            options.responseData = new entities.contentTypeDefinitionField();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(field: entities.contentTypeDefinitionField, onSuccess: (obj: entities.contentTypeDefinitionField) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField> = new base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.contentTypeDefinitionField();
            options.requestData = field;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.contentTypeDefinitionField) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField> = new base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.contentTypeDefinitionField();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldController, entities.contentTypeDefinitionField>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }
    }
}