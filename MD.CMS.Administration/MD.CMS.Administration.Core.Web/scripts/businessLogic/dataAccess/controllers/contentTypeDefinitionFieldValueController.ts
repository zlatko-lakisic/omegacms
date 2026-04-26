/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentTypeDefinitionFieldValue.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class contentTypeDefinitionFieldValueController extends base.BaseController<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue> {

        constructor() {
            super('ContentTypeDefinitionFieldValue/');
        }

        public getByContent(id: number, onSuccess: (obj: Array<entities.contentTypeDefinitionFieldValue>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue> = new base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByContent', [id]);
            options.responseData = new entities.contentTypeDefinitionFieldValue();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByContentId(id: number, onSuccess: (obj: Array<entities.contentTypeDefinitionFieldValue>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue> = new base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByContentId', [id]);
            options.responseData = new entities.contentTypeDefinitionFieldValue();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByValue(value: string, contentTypeDefinitionId: number = 0, contentTypeDefinitionFieldId: number = 0, comparer: helpers.data.comparerTypeEnum = helpers.data.comparerTypeEnum.equals, transform: helpers.data.dataTransformEnum = helpers.data.dataTransformEnum.toString, onSuccess: (obj: Array<entities.contentTypeDefinitionFieldValue>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue> = new base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByValue', [
                value,
                contentTypeDefinitionId,
                contentTypeDefinitionFieldId,
                comparer,
                transform
            ]);
            options.responseData = new entities.contentTypeDefinitionFieldValue();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(fieldValue: entities.contentTypeDefinitionFieldValue, onSuccess: (obj: entities.contentTypeDefinitionFieldValue) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue> = new base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.contentTypeDefinitionFieldValue();
            options.requestData = fieldValue;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFieldValueController, entities.contentTypeDefinitionFieldValue>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}