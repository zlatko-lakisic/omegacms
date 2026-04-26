/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/taxonomy.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class taxonomyController extends base.BaseController<taxonomyController, entities.taxonomy | entities.primitiveType<any> | entities.paginationEntity<entities.taxonomy>> {

        constructor() {
            super('Taxonomy/');
        }

        public getById(id: number, onSuccess: (obj: entities.taxonomy) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.taxonomy();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByParentId(id: number, depth: string, onSuccess: (obj: Array<entities.taxonomy>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByParentId', [id]);
            options.responseData = new entities.taxonomy();
            options.headers.push(new base.AjaxMethodHeader("depth", depth));
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByParentIdCount(countData: any, depth: string, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyController, entities.primitiveType<number>> = new base.AjaxMethodOptions<taxonomyController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByParentIdCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public paginationGetTaxonomyByPath(paginationData: any, onSuccess: (obj: entities.taxonomy) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetTaxonomyWithPaginationByPath', paginationData);
            options.responseData = new entities.taxonomy();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public GetByParentIdWithPagination(paginationData: any, onSuccess: (obj: entities.paginationEntity<entities.taxonomy>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyController, entities.paginationEntity<entities.taxonomy>> = new base.AjaxMethodOptions<taxonomyController, entities.paginationEntity<entities.taxonomy>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByParentIdWithPagination', paginationData);
            options.responseData = new entities.paginationEntity<entities.taxonomy>(entities.taxonomy);
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.paginationEntity<entities.taxonomy>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.paginationEntity<entities.taxonomy>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public updateChildren(taxonomy: entities.taxonomy, orderStart: number, onSuccess: (obj: entities.taxonomy) => void, onError: (error: helpers.mdException) => void): void {
            orderStart = mdBusinessLogic.helpers.typeConversion.toInt(orderStart);

            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('UpdateChildren', [orderStart]);
            options.responseData = new entities.taxonomy();
            options.requestData = taxonomy;
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public search(searchTerm: string, taxonomyId: number, recursive: boolean, onSuccess: (obj: Array<entities.taxonomy>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', [searchTerm, taxonomyId, recursive]);
            options.responseData = new entities.taxonomy();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByContent(id: number, onSuccess: (obj: Array<entities.taxonomy>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByContent', [id]);
            options.responseData = new entities.taxonomy();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public taxonomyContentGetTaxonomyByContent(content: entities.content, onSuccess: (obj: Array<entities.taxonomy>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('TaxonomyContentGetTaxonomyByContent');
            options.responseData = new entities.taxonomy();
            options.requestData = content;
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getAll(lcid: number, onSuccess: (obj: Array<entities.taxonomy>) => void, onError: (error: helpers.mdException) => void): void {
            lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);

            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.lcid = lcid;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.taxonomy();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getHierarchyByParentId(id: number, depth: string, onSuccess: (obj: Array<entities.taxonomy>) => void, onError: (error: helpers.mdException) => void): void {
            this.getHierarchyByParentIdWithContents(id, depth, false, onSuccess, onError);
        }

        public getHierarchyByParentIdWithContents(id: number, depth: string, loadContents: boolean, onSuccess: (obj: Array<entities.taxonomy>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetHierarchyByParentId', [id, loadContents]);
            options.responseData = new entities.taxonomy();
            options.headers.push(new base.AjaxMethodHeader("depth", depth));
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(taxonomy: entities.taxonomy, onSuccess: (obj: entities.taxonomy) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.taxonomy();
            options.requestData = taxonomy;
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.taxonomy) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.taxonomy();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public assignContentToTaxonomy(id: number, contentId: string, onSuccess: (obj: entities.taxonomy) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('AssignContentToTaxonomy', [id, contentId]);
            options.responseData = new entities.taxonomy();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public assignContentToTaxonomies(taxonomyIds: Array<number>, contentId: string, onSuccess: (obj: entities.taxonomy) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('AssignContentToTaxonomies');
            options.responseData = new entities.taxonomy();
            options.isJsonArray = true;
            options.requestData = {
                contentId: contentId,
                taxonomyIds: taxonomyIds
            };
            options.contentType = new base.AjaxMethodHeader('Content-Type', 'application/json');
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getByTaxonomyPath(path: string, onSuccess: (obj: entities.taxonomy) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetTaxonomyByPath');
            options.responseData = new entities.taxonomy();
            options.requestData = {
                ValueName: path
            }
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public delContent(id: number, path: string, onSuccess: (obj: entities.taxonomy) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy> = new base.AjaxMethodOptions<taxonomyController, entities.taxonomy>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('DeleteContent', [id]);
            options.responseData = new entities.taxonomy();
            options.requestData = {
                ValueName: path
            }
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyController, entities.taxonomy>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }
    }
}
