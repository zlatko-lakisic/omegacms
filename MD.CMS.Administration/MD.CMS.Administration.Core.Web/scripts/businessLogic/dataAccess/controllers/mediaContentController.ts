/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/mediaContent.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />
/// <reference path="./options/iUploadOptions.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class mediaContentController extends base.BaseController<mediaContentController, entities.mediaContent | entities.primitiveType<any> | entities.paginationEntity<entities.mediaContent>> {

        constructor() {
            super('MediaContent/');
        }

        public getById(id: number, lcid: number, onSuccess: (obj: entities.mediaContent) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);
            lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);

            let options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent> = new base.AjaxMethodOptions<mediaContentController, entities.mediaContent>();
            options.includeAuthHeader = true;
            options.lcid = lcid;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.mediaContent();
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByIdWithMetaData(id: number, lcid: number, onSuccess: (obj: entities.mediaContent) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);
            lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);

            let options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent> = new base.AjaxMethodOptions<mediaContentController, entities.mediaContent>();
            options.includeAuthHeader = true;
            options.lcid = lcid;
            options.address = this.getAddress('GetByIdWithMetaData', [id]);
            options.responseData = new entities.mediaContent();
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFolderId(id: number, lcid: number, onSuccess: (obj: entities.mediaContent) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);
            lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);

            let options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent> = new base.AjaxMethodOptions<mediaContentController, entities.mediaContent>();
            options.includeAuthHeader = true;
            options.lcid = lcid;
            options.address = this.getAddress('GetByFolderId', [id, lcid]);
            options.responseData = new entities.mediaContent();
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFileType(id: number, lcid: number, onSuccess: (obj: Array<entities.mediaContent>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);
            lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);

            let options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent> = new base.AjaxMethodOptions<mediaContentController, entities.mediaContent>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.lcid = lcid;
            options.address = this.getAddress('GetByFileType', [id, lcid]);
            options.responseData = new entities.mediaContent();
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public search(searchData: any, onSuccess: (obj: Array<entities.mediaContent>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent> = new base.AjaxMethodOptions<mediaContentController, entities.mediaContent>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.mediaContent();
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public searchByFileType(searchText: string, fileType: number, lcid: number, onSuccess: (obj: Array<entities.mediaContent>) => void, onError: (error: helpers.mdException) => void): void {
            lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);

            let options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent> = new base.AjaxMethodOptions<mediaContentController, entities.mediaContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('SearchByFileType', [searchText, fileType, lcid]);
            options.responseData = new entities.mediaContent();
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public del(id: number, onSuccess: (obj: entities.mediaContent) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent> = new base.AjaxMethodOptions<mediaContentController, entities.mediaContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.mediaContent();
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public getAll(onSuccess: (obj: Array<entities.mediaContent>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent> = new base.AjaxMethodOptions<mediaContentController, entities.mediaContent>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.mediaContent();
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public paginationGetByFolderId(paginationData: options.iFolderPaginatedRequestOptions, onSuccess: (obj: entities.paginationEntity<entities.mediaContent>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<mediaContentController, entities.paginationEntity<entities.mediaContent>> = new base.AjaxMethodOptions<mediaContentController, entities.paginationEntity<entities.mediaContent>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetWithPaginationByFolderId', paginationData);
            options.responseData = new entities.paginationEntity<entities.mediaContent>(entities.mediaContent);
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.paginationEntity<entities.mediaContent>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.paginationEntity<entities.mediaContent>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFolderIdCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<mediaContentController, entities.primitiveType<number>> = new base.AjaxMethodOptions<mediaContentController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByFolderIdCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public SavePermissions(mediaContent: entities.mediaContent, onSuccess: (obj: entities.mediaContent) => void, onError: (error: helpers.mdException) => void): void {
            this.save(mediaContent, onSuccess, onError);
        }

        public save(mediaContent: entities.mediaContent, onSuccess: (obj: entities.mediaContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent> = new base.AjaxMethodOptions<mediaContentController, entities.mediaContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.mediaContent();
            options.requestData = mediaContent;
            options.onSuccess = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<mediaContentController, entities.mediaContent>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}
