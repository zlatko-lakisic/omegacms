/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentTypeDefinition.ts" />
/// <reference path="../entities/contentTypeDefinitionField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class contentTypeDataSourceJoinController extends base.BaseController<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin> {

        constructor() {
            super('ContentTypeDefinitionDatasource/');
        }

        public getById(id: number, onSuccess: (obj: entities.contentTypeDataSourceJoin) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin> = new base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.contentTypeDataSourceJoin();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(contentTypeDataSourceJoin: entities.contentTypeDataSourceJoin, onSuccess: (obj: entities.contentTypeDataSourceJoin) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin> = new base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.contentTypeDataSourceJoin();
            options.requestData = contentTypeDataSourceJoin;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(contentTypeDataSourceJoin: entities.contentTypeDataSourceJoin, onSuccess: (obj: entities.contentTypeDataSourceJoin) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin> = new base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete');
            options.responseData = new entities.contentTypeDataSourceJoin();
            options.requestData = contentTypeDataSourceJoin;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDataSourceJoinController, entities.contentTypeDataSourceJoin>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}
