/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/metaDataFieldValue.ts" />
/// <reference path="../entities/content.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class metaDataFieldValueController extends base.BaseController<metaDataFieldValueController, entities.metaDataFieldValue> {

        constructor() {
            super('MetaDataFieldValue/');
        }

        public getByContentId(id: number, onSuccess: (obj: Array<entities.metaDataFieldValue>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<metaDataFieldValueController, entities.metaDataFieldValue> = new base.AjaxMethodOptions<metaDataFieldValueController, entities.metaDataFieldValue>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByContentId', [id]);
            options.responseData = new entities.metaDataFieldValue();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldValueController, entities.metaDataFieldValue>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldValueController, entities.metaDataFieldValue>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByContent(content: entities.content, onSuccess: (obj: Array<entities.metaDataFieldValue>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<metaDataFieldValueController, entities.metaDataFieldValue> = new base.AjaxMethodOptions<metaDataFieldValueController, entities.metaDataFieldValue>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByContent');
            options.responseData = new entities.metaDataFieldValue();
            options.requestData = content;
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldValueController, entities.metaDataFieldValue>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldValueController, entities.metaDataFieldValue>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}