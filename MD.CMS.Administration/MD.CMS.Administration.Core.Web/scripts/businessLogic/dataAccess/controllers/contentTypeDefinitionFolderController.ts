/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentTypeDefinitionFolder.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class contentTypeDefinitionFolderController extends base.BaseController<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder> {

        constructor() {
            super('ContentTypeDefinitionFolder/');
        }

        public save(folder: entities.contentTypeDefinitionFolder, onSuccess: (obj: entities.contentTypeDefinitionFolder) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder> = new base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.contentTypeDefinitionFolder();
            options.requestData = folder;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(folder: entities.contentTypeDefinitionFolder, onSuccess: (obj: entities.contentTypeDefinitionFolder) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder> = new base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete');
            options.responseData = new entities.contentTypeDefinitionFolder();
            options.requestData = folder;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getByFolder(id: number, onSuccess: (obj: Array<entities.contentTypeDefinitionFolder>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder> = new base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.contentTypeDefinitionFolder();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderController, entities.contentTypeDefinitionFolder>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}
