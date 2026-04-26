/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentTypeDefinition.ts" />
/// <reference path="../entities/contentTypeDefinitionField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class contentTypeDefinitionFolderDataBoundSyncController extends base.BaseController<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync> {

        constructor() {
            super('contentTypeDefinitionFolderDataBoundSync/');
        }

        public getByFolderAndContentTypeDefinitionId(folderId: number, contentTypeDefinitionId: number, onSuccess: (obj: entities.contentTypeDefinitionFolderDataBoundSync) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
            contentTypeDefinitionId = mdBusinessLogic.helpers.typeConversion.toInt(contentTypeDefinitionId);

            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync> = new base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByFolderAndContentTypeDefinitionId', [folderId, contentTypeDefinitionId]);
            options.responseData = new entities.contentTypeDefinitionFolderDataBoundSync();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(contentTypeDefinitionFolderDataBoundSync: entities.contentTypeDefinitionFolderDataBoundSync, onSuccess: (obj: entities.contentTypeDefinitionFolderDataBoundSync) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync> = new base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.contentTypeDefinitionFolderDataBoundSync();
            options.requestData = contentTypeDefinitionFolderDataBoundSync;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(obj: entities.contentTypeDefinitionFolderDataBoundSync, onSuccess: () => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync> = new base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [obj.FolderId, obj.ContentTypeDefinitionId]);
            options.responseData = new entities.contentTypeDefinitionFolderDataBoundSync();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync>): void => {
                onSuccess();
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundSyncController, entities.contentTypeDefinitionFolderDataBoundSync>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }
    }
}
