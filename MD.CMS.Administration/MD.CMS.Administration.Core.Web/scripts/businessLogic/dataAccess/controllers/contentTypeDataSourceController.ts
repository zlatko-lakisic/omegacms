/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentTypeDefinition.ts" />
/// <reference path="../entities/contentTypeDefinitionField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class contentTypeDataSourceController extends base.BaseController<contentTypeDataSourceController, entities.contentTypeDataSource | entities.primitiveType<object>> {

        constructor() {
            super('ContentTypeDefinitionDatasource/');
        }

        public getById(id: number, onSuccess: (obj: entities.contentTypeDataSource) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource> = new base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.contentTypeDataSource();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByContentTypeDefinitionId(id: number, onSuccess: (obj: Array<entities.contentTypeDataSource>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource> = new base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByContentTypeDefinitionId', [id]);
            options.responseData = new entities.contentTypeDataSource();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(contentTypeDataSource: entities.contentTypeDataSource, onSuccess: (obj: entities.contentTypeDataSource) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource> = new base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.contentTypeDataSource();
            options.requestData = contentTypeDataSource;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.contentTypeDataSource) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource> = new base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.contentTypeDataSource();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.contentTypeDataSource>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public getDataStructure(contentTypeDataSource: entities.contentTypeDataSource, onSuccess: (obj: entities.primitiveType<object>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.primitiveType<object>> = new base.AjaxMethodOptions<contentTypeDataSourceController, entities.primitiveType<object>>();
            options.includeAuthHeader = true;
            options.isJsonArray = false;
            options.address = this.getAddress('GetDataStructure');
            options.responseData = new entities.primitiveType<object>();
            options.requestData = contentTypeDataSource;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.primitiveType<object>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.primitiveType<object>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getAllDatabaseTypes(onSuccess: (obj: entities.primitiveType<object>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.primitiveType<object>> = new base.AjaxMethodOptions<contentTypeDataSourceController, entities.primitiveType<object>>();
            options.includeAuthHeader = true;
            options.isJsonArray = false;
            options.address = this.getAddress('GetAllDatabaseTypes');
            options.responseData = new entities.primitiveType<object>();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.primitiveType<object>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDataSourceController, entities.primitiveType<object>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}
