/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/folderMetaDataField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class folderMetaDataFieldController extends base.BaseController<folderMetaDataFieldController, entities.folderMetaDataField> {

        constructor() {
            super('FolderMetaDataField/');
        }

        public getByIds(folderId: number, metaDataFieldId: number, onSuccess: (obj: entities.folderMetaDataField) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField> = new base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [folderId, metaDataFieldId]);
            options.responseData = new entities.folderMetaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getUsed(folderId: number, onSuccess: (obj: Array<entities.folderMetaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField> = new base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetUsedFolderMetaDataField', [folderId]);
            options.responseData = new entities.folderMetaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(onSuccess: (obj: Array<entities.folderMetaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField> = new base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.folderMetaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderMetaDataFieldController, entities.folderMetaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}