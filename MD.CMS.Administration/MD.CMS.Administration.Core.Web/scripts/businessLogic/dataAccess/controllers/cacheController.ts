/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/cacheResponse.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class cacheController extends base.BaseController<cacheController, entities.cacheResponse> {

        constructor() {
            super('Cache/');
        }

        public getAllDataCache(onSuccess: (obj: Array<entities.cacheResponse>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<cacheController, entities.cacheResponse> = new base.AjaxMethodOptions<cacheController, entities.cacheResponse>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetDataCache');
            options.responseData = new entities.cacheResponse();
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<cacheController, entities.cacheResponse>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<cacheController, entities.cacheResponse>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public invalidateDataCache(provider: string, cacheKey: string, onSuccess: (obj: entities.cacheResponse) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<cacheController, entities.cacheResponse> = new base.AjaxMethodOptions<cacheController, entities.cacheResponse>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('InvalidateDataCache', [provider, encodeURI(cacheKey)]);
            options.responseData = new entities.cacheResponse();
            options.onSuccess = (options: base.AjaxMethodOptions<cacheController, entities.cacheResponse>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<cacheController, entities.cacheResponse>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}
