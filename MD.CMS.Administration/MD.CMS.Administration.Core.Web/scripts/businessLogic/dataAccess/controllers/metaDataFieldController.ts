/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/metaDataField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class metaDataFieldController extends base.BaseController<metaDataFieldController, entities.metaDataField | entities.primitiveType<any> | entities.paginationEntity<entities.metaDataField>> {

        constructor() {
            super('MetaDataField/');
        }

        public getAll(onSuccess: (obj: Array<entities.metaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField> = new base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.metaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getById(id: number, onSuccess: (obj: entities.metaDataField) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField> = new base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.metaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public paginationGetAll(paginationData: any, onSuccess: (obj: entities.paginationEntity<entities.metaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<metaDataFieldController, entities.paginationEntity<entities.metaDataField>> = new base.AjaxMethodOptions<metaDataFieldController, entities.paginationEntity<entities.metaDataField>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('PaginationGetAll', paginationData);
            options.responseData = new entities.paginationEntity<entities.metaDataField>(entities.metaDataField);
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.paginationEntity<entities.metaDataField>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.paginationEntity<entities.metaDataField>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<metaDataFieldController, entities.primitiveType<number>> = new base.AjaxMethodOptions<metaDataFieldController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFolderId(folderId: number, onSuccess: (obj: Array<entities.metaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField> = new base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByFolderId', [folderId]);
            options.responseData = new entities.metaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public metadatagetByFolder(folderId: number, onSuccess: (obj: Array<entities.metaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField> = new base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('MetaDataMediaContentGetByFolderId', [folderId]);
            options.responseData = new entities.metaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFolder(folderId: number, onSuccess: (obj: Array<entities.metaDataField>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField> = new base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByFolder', [folderId]);
            options.responseData = new entities.metaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(metaDataField: entities.metaDataField, onSuccess: (obj: entities.metaDataField) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField> = new base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.metaDataField();
            options.requestData = metaDataField;
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.metaDataField) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField> = new base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.metaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public assignMetaDataFieldToFolder(folderId: number, metaDataFieldId: number, onSuccess: (obj: entities.metaDataField) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);
            metaDataFieldId = mdBusinessLogic.helpers.typeConversion.toInt(metaDataFieldId);

            let options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField> = new base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('AssignMetaDataFieldToFolder', [folderId, metaDataFieldId]);
            options.responseData = new entities.metaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public search(searchData: any, onSuccess: (obj: entities.metaDataField) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField> = new base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.metaDataField();
            options.onSuccess = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<metaDataFieldController, entities.metaDataField>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}