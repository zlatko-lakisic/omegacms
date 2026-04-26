/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/menuContent.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class menuContentController extends base.BaseController<menuContentController, entities.menuContent | entities.primitiveType<number> | entities.paginationEntity<entities.menuContent>> {

        constructor() {
            super('MenuContent/');
        }

        public getById(id: number, lcid: number, onSuccess: (obj: entities.menuContent) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);
            lcid = mdBusinessLogic.helpers.typeConversion.toInt(lcid);

            let options: base.AjaxMethodOptions<menuContentController, entities.menuContent> = new base.AjaxMethodOptions<menuContentController, entities.menuContent>();
            options.includeAuthHeader = true;
            options.lcid = lcid;
            options.address = this.getAddress('GetByMenuId', [id]);
            options.responseData = new entities.menuContent();
            options.onSuccess = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public del(data: any, onSuccess: (obj: entities.menuContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuContentController, entities.menuContent> = new base.AjaxMethodOptions<menuContentController, entities.menuContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [data.Id, data.MenuId]);
            options.responseData = new entities.menuContent();
            options.onSuccess = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public save(menuContent: entities.menuContent, onSuccess: (obj: entities.menuContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuContentController, entities.menuContent> = new base.AjaxMethodOptions<menuContentController, entities.menuContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.menuContent();
            options.requestData = menuContent;
            options.onSuccess = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public update(menu: entities.menu, orderStart: boolean, onSuccess: (obj: entities.menuContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuContentController, entities.menuContent> = new base.AjaxMethodOptions<menuContentController, entities.menuContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Update', [orderStart]);
            options.responseData = new entities.menuContent();
            options.requestData = menu;
            options.onSuccess = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public deletemenu(menuContent: entities.menuContent, onSuccess: (obj: entities.menuContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuContentController, entities.menuContent> = new base.AjaxMethodOptions<menuContentController, entities.menuContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete');
            options.responseData = new entities.menuContent();
            options.requestData = menuContent;
            options.onSuccess = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public getByMenuIdCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuContentController, entities.primitiveType<number>> = new base.AjaxMethodOptions<menuContentController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByMenuIdCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<menuContentController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<menuContentController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public paginationGetByMenuId(paginationData: any, onSuccess: (obj: entities.paginationEntity<entities.menuContent>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuContentController, entities.paginationEntity<entities.menuContent>> = new base.AjaxMethodOptions<menuContentController, entities.paginationEntity<entities.menuContent>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('PaginationGetByMenuId', paginationData);
            options.responseData = new entities.paginationEntity<entities.menuContent>(entities.menuContent);
            options.onSuccess = (options: base.AjaxMethodOptions<menuContentController, entities.paginationEntity<entities.menuContent>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuContentController, entities.paginationEntity<entities.menuContent>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public search(searchData: any, onSuccess: (obj: entities.menuContent) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuContentController, entities.menuContent> = new base.AjaxMethodOptions<menuContentController, entities.menuContent>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.menuContent();
            options.onSuccess = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuContentController, entities.menuContent>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}