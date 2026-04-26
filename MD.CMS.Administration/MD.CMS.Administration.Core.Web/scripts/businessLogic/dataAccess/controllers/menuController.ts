/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/menu.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class menuController extends base.BaseController<menuController, entities.menu | entities.primitiveType<any> | entities.paginationEntity<entities.menu>> {

        constructor() {
            super('Menu/');
        }

        public getById(id: number, onSuccess: (obj: entities.menu) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.menu();
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByParentId(id: number, depth: string, onSuccess: (obj: Array<entities.menu>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByParentId', [id]);
            options.responseData = new entities.menu();
            options.headers.push(new base.AjaxMethodHeader("depth", depth));
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByParentIdCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuController, entities.primitiveType<number>> = new base.AjaxMethodOptions<menuController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByParentIdCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public paginationGetMenuByPath(paginationData: any, onSuccess: (obj: entities.menu) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('PaginationGetMenuByPath', paginationData);
            options.responseData = new entities.menu();
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public GetByParentIdWithPagination(paginationData: any, onSuccess: (obj: entities.paginationEntity<entities.menu>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuController, entities.paginationEntity<entities.menu>> = new base.AjaxMethodOptions<menuController, entities.paginationEntity<entities.menu>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByParentIdWithPagination', paginationData);
            options.responseData = new entities.paginationEntity<entities.menu>(entities.menu);
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.paginationEntity<entities.menu>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.paginationEntity<entities.menu>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(onSuccess: (obj: Array<entities.menu>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.menu();
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getHierarchyByParentId(id: number, depth: string, onSuccess: (obj: Array<entities.menu>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetHierarchyByParentId', [id]);
            options.responseData = new entities.menu();
            options.headers.push(new base.AjaxMethodHeader("depth", depth));
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(menu: entities.menu, onSuccess: (obj: entities.menu) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.menu();
            options.requestData = menu;
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public updateChildren(menu: entities.menu, orderStart: number, onSuccess: (obj: entities.menu) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('UpdateChildren', [orderStart]);
            options.responseData = new entities.menu();
            options.requestData = menu;
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.menu) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.menu();
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public delContent(id: number, path: string, onSuccess: (obj: entities.menu) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.address = this.getAddress(this.getAddress('DeleteContent', [id], false), { ValueName: path });
            options.responseData = new entities.menu();
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public assignContentToMenu(menuId: number, contentId: string, onSuccess: (obj: entities.menu) => void, onError: (error: helpers.mdException) => void): void {
            menuId = mdBusinessLogic.helpers.typeConversion.toInt(menuId);

            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('AssignContentToMenu', [menuId, contentId]);
            options.responseData = new entities.menu();
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getByMenuPath(path: string, onSuccess: (obj: entities.menu) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetMenuByPath');
            options.requestData = { ValueName: path };
            options.responseData = new entities.menu();
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public search(searchData: any, onSuccess: (obj: Array<entities.menu>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<menuController, entities.menu> = new base.AjaxMethodOptions<menuController, entities.menu>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.menu();
            options.onSuccess = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<menuController, entities.menu>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}