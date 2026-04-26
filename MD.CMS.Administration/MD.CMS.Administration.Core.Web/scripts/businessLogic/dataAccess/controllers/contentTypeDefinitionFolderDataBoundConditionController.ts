/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentTypeDefinition.ts" />
/// <reference path="../entities/contentTypeDefinitionField.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class contentTypeDefinitionFolderDataBoundConditionController extends base.BaseController<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition> {

        constructor() {
            super('ContentTypeDefinitionFolderDataBoundCondition/');
        }

        public getByFolderAndContentTypeDefinitionId(folderId: number, contentTypeDefinitionId: number, onSuccess: (obj: Array<entities.contentTypeDefinitionFolderDataBoundCondition>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition> = new base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByFolderAndContentTypeDefinitionId', [folderId, contentTypeDefinitionId]);
            options.responseData = new entities.contentTypeDefinitionFolderDataBoundCondition();
            options.responseDataArray = new Array<entities.contentTypeDefinitionFolderDataBoundCondition>();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(contentTypeDefinitionFolderDataBoundCondition: entities.contentTypeDefinitionFolderDataBoundCondition, onSuccess: (obj: entities.contentTypeDefinitionFolderDataBoundCondition) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition> = new base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.contentTypeDefinitionFolderDataBoundCondition();
            options.requestData = contentTypeDefinitionFolderDataBoundCondition;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public saveAll(contentTypeDefinitionFolderDataBoundConditions: Array<entities.contentTypeDefinitionFolderDataBoundCondition>, onSuccess: (obj: Array<entities.contentTypeDefinitionFolderDataBoundCondition>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition> = new base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.contentType = new base.AjaxMethodHeader('Content-Type', 'application/json; charset=UTF-8');
            options.address = this.getAddress('SaveAll');
            options.responseDataArray = new Array<entities.contentTypeDefinitionFolderDataBoundCondition>();
            options.requestData = contentTypeDefinitionFolderDataBoundConditions;
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(obj: entities.contentTypeDefinitionFolderDataBoundCondition, onSuccess: () => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition> = new base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [obj.FolderId, obj.ContentTypeDefinitionId, obj.ContentTypeDefinitionFieldId]);
            options.responseData = new entities.contentTypeDefinitionFolderDataBoundCondition();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onSuccess();
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public deleteAll(folderId: number, contentTypeDefinitionId: number, onSuccess: () => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition> = new base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('DeleteAll', [folderId, contentTypeDefinitionId]);
            options.responseData = new entities.contentTypeDefinitionFolderDataBoundCondition();
            options.onSuccess = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onSuccess();
            }
            options.onError = (options: base.AjaxMethodOptions<contentTypeDefinitionFolderDataBoundConditionController, entities.contentTypeDefinitionFolderDataBoundCondition>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }
    }
}
