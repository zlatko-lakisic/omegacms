/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentAlias.ts" />
/// <reference path="../entities/content.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class cultureController extends base.BaseController<cultureController, entities.culture> {

        constructor() {
            super('Culture/');
        }

        public selectCulture(onSuccess: (obj: entities.culture) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<cultureController, entities.culture> = new base.AjaxMethodOptions<cultureController, entities.culture>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('SelectCulture');
            options.responseData = new entities.culture();
            options.onSuccess = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByLCID(id: number, onSuccess: (obj: entities.culture) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<cultureController, entities.culture> = new base.AjaxMethodOptions<cultureController, entities.culture>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByLCID', [id]);
            options.responseData = new entities.culture();
            options.onSuccess = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(onSuccess: (obj: Array<entities.culture>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<cultureController, entities.culture> = new base.AjaxMethodOptions<cultureController, entities.culture>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.culture();
            options.onSuccess = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getApproved(onSuccess: (obj: Array<entities.culture>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<cultureController, entities.culture> = new base.AjaxMethodOptions<cultureController, entities.culture>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetApproved');
            options.responseData = new entities.culture();
            options.onSuccess = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllForContentId(id: number, onSuccess: (obj: Array<entities.culture>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<cultureController, entities.culture> = new base.AjaxMethodOptions<cultureController, entities.culture>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAllForContentId', [id]);
            options.responseData = new entities.culture();
            options.onSuccess = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(culture: entities.culture, onSuccess: (obj: entities.culture) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<cultureController, entities.culture> = new base.AjaxMethodOptions<cultureController, entities.culture>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.culture();
            options.requestData = culture;
            options.onSuccess = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(culture: entities.culture, onSuccess: (obj: entities.culture) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<cultureController, entities.culture> = new base.AjaxMethodOptions<cultureController, entities.culture>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete');
            options.responseData = new entities.culture();
            options.requestData = culture;
            options.onSuccess = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<cultureController, entities.culture>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }
    }
}
