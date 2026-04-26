/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentTypeDefinition.ts" />
/// <reference path="../entities/contentTypeDefinitionField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class contentTypeDefinitionControllerGeneric<T extends entities.genericContent.genericContentField & entities.base.IBaseEntity<T>> extends base.BaseController<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T> | entities.primitiveType<any> | entities.paginationEntity<entities.contentTypeDefinition<T>>> {

        constructor() {
            super('ContentTypeDefinition/');
        }

        public getById(id: number, onSuccess: (obj: entities.contentTypeDefinition<T>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>> = new base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.contentTypeDefinition<T>();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(onSuccess: (obj: Array<entities.contentTypeDefinition<T>>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>> = new base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.contentTypeDefinition<T>();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFolder(folderId: number, onSuccess: (obj: Array<entities.contentTypeDefinition<T>>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>> = new base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByFolder', [folderId]);
            options.responseData = new entities.contentTypeDefinition<T>();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public contentTypeDefinitionsByFolder(folderId: number, onSuccess: (obj: Array<entities.contentTypeDefinition<T>>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>> = new base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('ContentTypeDefinitionsByFolder', [folderId]);
            options.responseData = new entities.contentTypeDefinition<T>();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(contentTypeDefinition: entities.contentTypeDefinition<T>, onSuccess: (obj: entities.contentTypeDefinition<T>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>> = new base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.contentTypeDefinition<T>();
            options.requestData = contentTypeDefinition;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.contentTypeDefinition<T>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>> = new base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.contentTypeDefinition<T>();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.contentTypeDefinition<T>>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public paginationGetAll(data: any, onSuccess: (obj: entities.paginationEntity<entities.contentTypeDefinition<T>>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.paginationEntity<entities.contentTypeDefinition<T>>> = new base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.paginationEntity<entities.contentTypeDefinition<T>>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('PaginationGetAll', data);
            options.responseData = new entities.paginationEntity<entities.contentTypeDefinition<T>>(entities.contentTypeDefinition);
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.paginationEntity<entities.contentTypeDefinition<T>>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.paginationEntity<entities.contentTypeDefinition<T>>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.primitiveType<number>> = new base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionControllerGeneric<T>, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }

    export class contentTypeDefinitionController extends contentTypeDefinitionControllerGeneric<entities.contentTypeDefinitionField> {

    }

    export class contentTypeDefinitionControllerValue extends contentTypeDefinitionControllerGeneric<entities.contentTypeDefinitionFieldValue> {

    }
}
