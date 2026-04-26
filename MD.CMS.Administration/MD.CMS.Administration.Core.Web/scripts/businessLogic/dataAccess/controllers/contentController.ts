/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/content.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="./options/iContentRequestOptions.ts" />
/// <reference path="./options/v2/iContentRequestOptions.ts" />
namespace mdBusinessLogic.dataAccess.controllers {
    export class contentController extends base.BaseController<contentController, entities.content | entities.primitiveType<any> | entities.paginationEntity<entities.content>> {

        constructor(controllerBase: string = 'Content/') {
            super(controllerBase);
        }

        public get(opts: options.v2.iContentRequestOptions, onSuccess: (obj: entities.paginationEntity<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>> = new base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('', opts);
            options.responseData = new entities.paginationEntity<entities.content>(entities.content);
            options.lcid = opts.Lcid;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getById(id: string, loadAuthor: boolean, lcid: number, fillFields: boolean = true, isDataBound: boolean = false, contentTypeDefinitionId: number = 0, onSuccess: (obj: entities.content) => void, onError: (error: helpers.mdException) => void): void {
            this.get({
                ContentIds: [id],
                FillFields: fillFields,
                FillMetaData: loadAuthor,
                Lcid: lcid
            }, (result) => {
                onSuccess(result.Items[0]);
            }, (error) => {
                onError(error);
            });

            /*let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id, fillFields, isDataBound, contentTypeDefinitionId]);
            options.responseData = new entities.content;
            options.lcid = lcid;
            options.headers.push(new base.AjaxMethodHeader('loadAuthor', loadAuthor.toString()));
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);*/
        }

        public getByIds(ids: Array<string>, loadAuthor: boolean, lcid: number, fillFields: boolean = true, isDataBound: boolean = false, contentTypeDefinitionId: number = 0, onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            this.get({
                ContentIds: ids,
                FillFields: fillFields,
                LoadAuthor: loadAuthor,
                Lcid: lcid
            }, (result) => {
                onSuccess(result.Items);
            }, (error) => {
                onError(error);
            });

            /*let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByIds', [ids.join(';'), fillFields, isDataBound, contentTypeDefinitionId]);
            options.responseData = new entities.content;
            options.lcid = lcid;
            options.headers.push(new base.AjaxMethodHeader('loadAuthor', loadAuthor.toString()));
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);*/
        }

        public getByRequest(request: options.iContentRequestOptions, onSuccess: (obj: entities.paginationEntity<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>> = new base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByRequest');
            options.requestData = request;
            options.responseData = new entities.paginationEntity<entities.content>(entities.content);
            options.headers.push(new base.AjaxMethodHeader('loadAuthor', request.LoadAuthor.toString()));
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public taxonomyContentGetContentByTaxonomy(id: number, onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('TaxonomyContentGetContentByTaxonomy/', [id]);
            options.responseData = new entities.content;
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public taxonomyContentGetContentByTaxonomyFullMeta(id: number, onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('TaxonomyContentGetContentByTaxonomy/', [id, true, false]);
            options.responseData = new entities.content;
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public menuContentGetContentByMenu(id: number, onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('MenuContentGetContentByMenu/', [id]);
            options.responseData = new entities.content;
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public translate(content: entities.content, targetLcid: number, lcid: number, onSuccess: (obj: entities.content) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Translate/');
            options.requestData = content;
            options.responseData = new entities.content;
            options.lcid = lcid;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public selectAllCount(id: number, onSuccess: (obj: entities.content) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('SelectAllCount/', [id]);
            options.responseData = new entities.content;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFolderId(id: number, loadAuthor: boolean, lcid: number, loadFields: boolean, loadMetaDataFields: boolean, onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByFolderId/', [id, loadAuthor, lcid, loadFields, loadMetaDataFields]);
            options.responseData = new entities.content;
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public paginationGetByFolderId(paginationData: options.iFolderPaginatedRequestOptions, onSuccess: (obj: entities.paginationEntity<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>> = new base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('PaginationGetByFolderId/', paginationData);
            options.responseData = new entities.paginationEntity<entities.content>(entities.content);
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.paginationEntity<entities.content>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByFolderIdCount(countData: number, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.primitiveType<number>> = new base.AjaxMethodOptions<contentController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByFolderIdCount/', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (response: base.AjaxMethodOptions<contentController, entities.primitiveType<number>>): void => {
                onSuccess((response.responseData as entities.primitiveType<number>).Value);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAll/');
            options.responseData = new entities.content;
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllVersion(id: number, onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllVersion/', [id]);
            options.responseData = new entities.content;
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByAll(obj: entities.content, onSuccess: (obj: entities.content) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByAll/');
            options.responseData = new entities.content;
            options.requestData = obj;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public search(searchData: any, onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.content();
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getBySearchTerm(searchTerm: string, loadAuthor: boolean, lcid: number, onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetBySearchTerm/', [searchTerm]);
            options.responseData = new entities.content;
            options.isJsonArray = true;
            options.lcid = lcid;
            options.headers.push(new base.AjaxMethodHeader('loadAuthor', loadAuthor.toString()));
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(obj: entities.content, onSuccess: (obj: entities.content) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save/');
            options.responseData = new entities.content;
            options.requestData = obj;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.content) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete/', [id]);
            options.responseData = new entities.content;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public deleteByAll(id: number, onSuccess: (obj: entities.content) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('DeleteByAll/', [id]);
            options.responseData = new entities.content;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public selectByContentTypeDefinitionCount(id: number, onSuccess: (obj: Array<entities.content>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<contentController, entities.content> = new base.AjaxMethodOptions<contentController, entities.content>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('SelectByContentTypeDefinitionCount/', [id]);
            options.responseData = new entities.content;
            options.isJsonArray = true;
            options.onSuccess = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<contentController, entities.content>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public doesContentExist(contents: Array<entities.content>, content: entities.content): number {
            let contentIndex: number = -1;
            for (let i: number = 0; i < contents.length; i++) {
                if (contents[i].Id == content.Id) {
                    contentIndex = i;
                    break;
                }
            }
            return contentIndex;
        }
    }
}
