/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/messageFolder.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class messageFolderController extends base.BaseController<messageFolderController, entities.messageFolder> {

        constructor() {
            super('MessageFolder/');
        }

        public getById(id: number, onSuccess: (obj: entities.messageFolder) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder> = new base.AjaxMethodOptions<messageFolderController, entities.messageFolder>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.messageFolder();
            options.onSuccess = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByIdAndAuthorId(id: number, onSuccess: (obj: entities.messageFolder) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder> = new base.AjaxMethodOptions<messageFolderController, entities.messageFolder>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByIdAndAuthorId', [id]);
            options.responseData = new entities.messageFolder();
            options.onSuccess = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(onSuccess: (obj: Array<entities.messageFolder>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder> = new base.AjaxMethodOptions<messageFolderController, entities.messageFolder>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.messageFolder();
            options.onSuccess = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllSystemFolders(onSuccess: (obj: Array<entities.messageFolder>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder> = new base.AjaxMethodOptions<messageFolderController, entities.messageFolder>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAllSystemFolders');
            options.responseData = new entities.messageFolder();
            options.onSuccess = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByAuthorId(onSuccess: (obj: Array<entities.messageFolder>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder> = new base.AjaxMethodOptions<messageFolderController, entities.messageFolder>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByAuthorId');
            options.responseData = new entities.messageFolder();
            options.onSuccess = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(messageFolder: entities.messageFolder, onSuccess: (obj: Array<entities.messageFolder>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder> = new base.AjaxMethodOptions<messageFolderController, entities.messageFolder>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.messageFolder();
            options.requestData = messageFolder;
            options.onSuccess = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public 'delete'(id: number, onSuccess: (obj: Array<entities.messageFolder>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder> = new base.AjaxMethodOptions<messageFolderController, entities.messageFolder>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.messageFolder();
            options.onSuccess = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<messageFolderController, entities.messageFolder>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }
    }
}