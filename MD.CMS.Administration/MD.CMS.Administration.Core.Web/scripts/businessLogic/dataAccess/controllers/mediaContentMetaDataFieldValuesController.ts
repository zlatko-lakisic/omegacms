/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/mediaContentMetaDataFeldValues.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class mediaContentMetaDataFeldValuesController extends base.BaseController<mediaContentMetaDataFeldValuesController, entities.mediaContentMetaDataFeldValues> {

        constructor() {
            super('MediaContentMetaDataFieldValues/');
        }

        public getByMediaContentId(id: number, onSuccess: (obj: entities.mediaContentMetaDataFeldValues) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<mediaContentMetaDataFeldValuesController, entities.mediaContentMetaDataFeldValues> = new base.AjaxMethodOptions<mediaContentMetaDataFeldValuesController, entities.mediaContentMetaDataFeldValues>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByMediaContentId', [id]);
            options.responseData = new entities.mediaContentMetaDataFeldValues();
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentMetaDataFeldValuesController, entities.mediaContentMetaDataFeldValues>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentMetaDataFeldValuesController, entities.mediaContentMetaDataFeldValues>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}