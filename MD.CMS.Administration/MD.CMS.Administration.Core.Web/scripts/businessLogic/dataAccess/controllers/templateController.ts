/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/template.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class templateController extends base.BaseController<templateController, entities.template | entities.templateScreenshot | entities.primitiveType<any> | entities.paginationEntity<entities.template>> {

        constructor() {
            super('Template/');
        }

        public getByFolder(folderId: number, onSuccess: (obj: Array<entities.template>) => void, onError: (error: helpers.mdException) => void): void {
            folderId = mdBusinessLogic.helpers.typeConversion.toInt(folderId);

            let options: base.AjaxMethodOptions<templateController, entities.template> = new base.AjaxMethodOptions<templateController, entities.template>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByFolder', [folderId]);
            options.responseData = new entities.template();
            options.onSuccess = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(sort: string, onSuccess: (obj: Array<entities.template>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<templateController, entities.template> = new base.AjaxMethodOptions<templateController, entities.template>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll', [sort]);
            options.responseData = new entities.template();
            options.onSuccess = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllWithPagination(paginationData: any, onSuccess: (obj: entities.paginationEntity<entities.template>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<templateController, entities.paginationEntity<entities.template>> = new base.AjaxMethodOptions<templateController, entities.paginationEntity<entities.template>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllWithPagination', paginationData);
            options.responseData = new entities.paginationEntity<entities.template>(entities.template);
            options.onSuccess = (options: base.AjaxMethodOptions<templateController, entities.paginationEntity<entities.template>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<templateController, entities.paginationEntity<entities.template>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<templateController, entities.primitiveType<number>> = new base.AjaxMethodOptions<templateController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<templateController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<templateController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getById(id: number, onSuccess: (obj: entities.template) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<templateController, entities.template> = new base.AjaxMethodOptions<templateController, entities.template>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.template();
            options.onSuccess = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(template: entities.template, onSuccess: (obj: entities.template) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<templateController, entities.template> = new base.AjaxMethodOptions<templateController, entities.template>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.template();
            options.requestData = template;
            options.onSuccess = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.template) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<templateController, entities.template> = new base.AjaxMethodOptions<templateController, entities.template>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.template();
            options.onSuccess = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public search(searchData: string, onSuccess: (obj: Array<entities.template>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<templateController, entities.template> = new base.AjaxMethodOptions<templateController, entities.template>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.template();
            options.onSuccess = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<templateController, entities.template>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getScreenshot(templateScreenshot: entities.templateScreenshot, onSuccess: (obj: entities.templateScreenshot) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<templateController, entities.templateScreenshot> = new base.AjaxMethodOptions<templateController, entities.templateScreenshot>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetTemplateScreenshot');
            options.responseData = new entities.templateScreenshot();
            options.requestData = templateScreenshot;
            options.onSuccess = (options: base.AjaxMethodOptions<templateController, entities.templateScreenshot>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<templateController, entities.templateScreenshot>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}