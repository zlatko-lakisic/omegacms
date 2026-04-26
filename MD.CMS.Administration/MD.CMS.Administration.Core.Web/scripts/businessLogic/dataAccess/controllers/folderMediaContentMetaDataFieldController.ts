/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/folderMediaContentMetaDataField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class folderMediaContentMetaDataFieldController extends base.BaseController<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField> {

        constructor() {
            super('FolderMediaContentMetaDataField/');
        }

        public getByIds(folderId: number, metaDataFieldId: number, onSuccess: (obj: entities.folderMediaContentMetaDataField) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField> = new base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByIds', [folderId, metaDataFieldId]);
            options.responseData = new entities.folderMediaContentMetaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getUsed(folderId: number, onSuccess: (obj: Array<entities.folderMediaContentMetaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField> = new base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetUsedFolderMediaContentMetaDataField', [folderId]);
            options.responseData = new entities.folderMediaContentMetaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(onSuccess: (obj: Array<entities.folderMediaContentMetaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField> = new base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.folderMediaContentMetaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFolderId(folderId: number, onSuccess: (obj: Array<entities.folderMediaContentMetaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField> = new base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByFolderId', [folderId]);
            options.responseData = new entities.folderMediaContentMetaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getMediaContentMetaDataFieldByFolder(folderId: number, onSuccess: (obj: Array<entities.folderMediaContentMetaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField> = new base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetMediaContentMetaDataFieldByFolder', [folderId]);
            options.responseData = new entities.folderMediaContentMetaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderMediaContentMetaDataFieldController, entities.folderMediaContentMetaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}