/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/folder.ts" />
/// <reference path="../entities/content.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class folderController extends base.BaseController<folderController, entities.folder<entities.content> | entities.primitiveType<any> | entities.paginationEntity<entities.folder<entities.content>>> {

        constructor() {
            super('Folder/');
        }

        public get(opts: options.v2.iFolderRequestOptions, onSuccess: (obj: entities.paginationEntity<entities.folder<entities.content>>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<folderController, entities.paginationEntity<entities.folder<entities.content>>> = new base.AjaxMethodOptions<folderController, entities.paginationEntity<entities.folder<entities.content>>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('');
            options.requestData = opts;
            options.responseData = new entities.paginationEntity<entities.folder<entities.content>>(entities.folder);
            options.lcid = opts.Lcid;
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.paginationEntity<entities.folder<entities.content>>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.paginationEntity<entities.folder<entities.content>>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getByFolderPath(path: string, loadContents: boolean, onSuccess: (obj: entities.folder<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            this.get({
                Paths: [path],
                FillContents: loadContents,
                MaxNumberOfRows: 1
            }, (result) => {
                onSuccess(result.Items[0]);
            }, (error) => {
                onError(error);
            });

            /*let options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>> = new base.AjaxMethodOptions<folderController, entities.folder<entities.content>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetFolderByPath', { id: path, loadContents: loadContents });
            options.responseData = new entities.folder<entities.content>();
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onError(options.exception);
            }
            this._get(options);*/
        }

        public search(searchTerm: string, parentId: number, recursive: boolean, onSuccess: (obj: Array<entities.folder<entities.content>>) => void, onError: (error: helpers.mdException) => void): void {
            parentId = mdBusinessLogic.helpers.typeConversion.toInt(parentId);

            let options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>> = new base.AjaxMethodOptions<folderController, entities.folder<entities.content>>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', [searchTerm, parentId, recursive]);
            options.responseData = new entities.folder<entities.content>();
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public paginationGetByFolderPath(paginationData: options.iFolderPaginatedRequestOptions, onSuccess: (obj: entities.folder<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            this.get({
                Paths: [paginationData.path],
                FillContents: paginationData.fillContents,
                MaxNumberOfRows: 1,
                CurrentPageIndex: 0,
                SearchTerm: paginationData.searchTerm,
                ContentRequestOptions: paginationData.fillContents ? ({
                    LoadAuthor: true,
                    MaxNumberOfRows: paginationData.pageSize,
                    CurrentPageIndex: paginationData.pageIndex,
                    FillFields: false,
                    FillMetaData: false
                }) : null,
                FillChildren: true,
                ChildFolderRequestOptions: {
                    FillContents: false,
                    MaxNumberOfRows: paginationData.pageSize,
                    CurrentPageIndex: paginationData.pageIndex
                }
            }, (result) => {
                onSuccess(result.Items[0]);
            }, onError);

            /*let options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>> = new base.AjaxMethodOptions<folderController, entities.folder<entities.content>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetFolderWithPaginationByPath', paginationData);
            options.responseData = new entities.folder<entities.content>();
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onError(options.exception);
            }
            this._get(options);*/
        }

        public paginationGetByParentId(paginationData: options.iFolderPaginatedRequestOptions, onSuccess: (obj: entities.paginationEntity<entities.folder<entities.content>>) => void, onError: (error: helpers.mdException) => void): void {
            this.get({
                ParentId: paginationData.parentId,
                FillContents: paginationData.fillContents,
                MaxNumberOfRows: paginationData.pageSize,
                CurrentPageIndex: paginationData.pageIndex,
                SearchTerm: paginationData.searchTerm,
                ContentRequestOptions: paginationData.fillContents ? ({
                    LoadAuthor: true,
                    MaxNumberOfRows: paginationData.pageSize,
                    CurrentPageIndex: paginationData.pageIndex,
                    FillFields: false,
                    FillMetaData: false
                }) : null,
                FillChildren: true,
                ChildFolderRequestOptions: {
                    FillContents: false,
                    MaxNumberOfRows: paginationData.pageSize,
                    CurrentPageIndex: paginationData.pageIndex
                }
            }, onSuccess, onError);


            /*let options: base.AjaxMethodOptions<folderController, entities.paginationEntity<entities.folder<entities.content>>> = new base.AjaxMethodOptions<folderController, entities.paginationEntity<entities.folder<entities.content>>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByParentIdWithPagination', paginationData);
            options.responseData = new entities.paginationEntity<entities.folder<entities.content>>(entities.folder);
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.paginationEntity<entities.folder<entities.content>>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.paginationEntity<entities.folder<entities.content>>>): void => {
                onError(options.exception);
            }
            this._get(options);*/
        }

        public getByParentId(parentId: number, onSuccess: (obj: Array<entities.folder<entities.content>>) => void, onError: (error: helpers.mdException) => void): void {
            parentId = mdBusinessLogic.helpers.typeConversion.toInt(parentId);

            let options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>> = new base.AjaxMethodOptions<folderController, entities.folder<entities.content>>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByParentId', [parentId]);
            options.responseData = new entities.folder<entities.content>();
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getHierarchyByParentId(parentId: number, depth: any, onSuccess: (obj: Array<entities.folder<entities.content>>) => void, onError: (error: helpers.mdException) => void): void {
            parentId = mdBusinessLogic.helpers.typeConversion.toInt(parentId);

            let options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>> = new base.AjaxMethodOptions<folderController, entities.folder<entities.content>>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            let params: Array<any> = new Array<any>();
            params.push(parentId);
            if (typeof (depth) !== "boolean") {
                params.push(depth);
            }
            options.address = this.getAddress('GetHierarchyByParentId', params);
            options.responseData = new entities.folder<entities.content>();
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByParentIdCount(countData: any, onSuccess: (obj: number) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<folderController, entities.primitiveType<number>> = new base.AjaxMethodOptions<folderController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByParentIdCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getById(id: number, onSuccess: (obj: entities.folder<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>> = new base.AjaxMethodOptions<folderController, entities.folder<entities.content>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.folder<entities.content>();
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(folder: number, onSuccess: (obj: entities.folder<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>> = new base.AjaxMethodOptions<folderController, entities.folder<entities.content>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.folder<entities.content>();
            options.requestData = folder;
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.folder<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>> = new base.AjaxMethodOptions<folderController, entities.folder<entities.content>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.folder<entities.content>();
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public getByRequest(request: options.iFolderPaginatedRequestOptions, onSuccess: (obj: Array<entities.folder<entities.content>>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>> = new base.AjaxMethodOptions<folderController, entities.folder<entities.content>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByRequest');
            options.requestData = request;
            options.responseData = new entities.folder<entities.content>();
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.folder<entities.content>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}