/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/taxonomyContent.ts" />
/// <reference path="../entities/taxonomy.ts" />
/// <reference path="../entities/content.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class taxonomyContentController extends base.BaseController<taxonomyContentController, entities.taxonomyContent | entities.primitiveType<any> | entities.paginationEntity<entities.taxonomyContent>> {

        constructor() {
            super('TaxonomyContent/');
        }

        public getByTaxonomyId(id: number, onSuccess: (obj: Array<entities.taxonomyContent>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent> = new base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByTaxonomyId', [id]);
            options.responseData = new entities.taxonomyContent();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public paginationGetByTaxonomyId(paginationData: any, onSuccess: (obj: entities.paginationEntity<entities.taxonomyContent>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyContentController, entities.paginationEntity<entities.taxonomyContent>> = new base.AjaxMethodOptions<taxonomyContentController, entities.paginationEntity<entities.taxonomyContent>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('PaginationGetByTaxonomyId', paginationData);
            options.responseData = new entities.paginationEntity<entities.taxonomyContent>(entities.taxonomyContent);
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.paginationEntity<entities.taxonomyContent>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.paginationEntity<entities.taxonomyContent>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByTaxonomyIdCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyContentController, entities.primitiveType<number>> = new base.AjaxMethodOptions<taxonomyContentController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByTaxonomyIdCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public del(deleteData: any, onSuccess: (obj: entities.taxonomyContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent> = new base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [deleteData.Id, deleteData.TaxonomyId]);
            options.responseData = new entities.taxonomyContent();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public save(taxonomyContent: entities.taxonomyContent, onSuccess: (obj: entities.taxonomyContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent> = new base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.taxonomyContent();
            options.requestData = taxonomyContent;
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public deletetaxonomy(content: entities.content, onSuccess: (obj: entities.taxonomyContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent> = new base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('DeleteTaxonomyContent');
            options.responseData = new entities.taxonomyContent();
            options.requestData = content;
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public savecontent(taxonomy: entities.taxonomy, onSuccess: (obj: entities.taxonomyContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent> = new base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('SaveTaxonomyContent');
            options.responseData = new entities.taxonomyContent();
            options.requestData = taxonomy;
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public update(taxonomy: entities.taxonomy, pageIndex: number, onSuccess: (obj: entities.taxonomyContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent> = new base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Update', [pageIndex]);
            options.responseData = new entities.taxonomyContent();
            options.requestData = taxonomy;
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public deletecontent(taxonomy: entities.taxonomy, onSuccess: (obj: entities.taxonomyContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent> = new base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('DeleteContent');
            options.responseData = new entities.taxonomyContent();
            options.requestData = taxonomy;
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public search(searchTerm: string, taxonomyId: number, lcid: number, onSuccess: (obj: Array<entities.taxonomyContent>) => void, onError: (error: helpers.mdException) => void): void {
            taxonomyId = mdBusinessLogic.helpers.typeConversion.toInt(taxonomyId);
            lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);

            let options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent> = new base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', [searchTerm, taxonomyId, lcid]);
            options.responseData = new entities.taxonomyContent();
            options.onSuccess = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<taxonomyContentController, entities.taxonomyContent>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}