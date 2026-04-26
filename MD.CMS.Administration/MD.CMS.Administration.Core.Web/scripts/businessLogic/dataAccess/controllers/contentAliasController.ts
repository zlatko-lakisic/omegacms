/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentAlias.ts" />
/// <reference path="../entities/content.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
  export class contentAliasController extends base.BaseController<contentAliasController, entities.contentAlias | entities.primitiveType<any>> {

    constructor() {
      super('ContentAlias/');
    }

    public getById(id: number, lcid: number, onSuccess: (obj: entities.contentAlias) => void, onError: (error: helpers.mdException) => void): void {
      let options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias> = new base.AjaxMethodOptions<contentAliasController, entities.contentAlias>();
      options.includeAuthHeader = true;
      options.lcid = lcid;
      options.address = this.getAddress('GetById', [id]);
      options.responseData = new entities.contentAlias();
      options.onSuccess = (options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias>): void => {
        onSuccess(options.responseData);
      }
      options.onError = (options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias>): void => {
        onError(options.exception);
      }
      this._get(options);
    }

    public getAll(onSuccess: (obj: Array<entities.contentAlias>) => void, onError: (error: helpers.mdException) => void): void {
      let options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias> = new base.AjaxMethodOptions<contentAliasController, entities.contentAlias>();
      options.includeAuthHeader = true;
      options.isJsonArray = true;
      options.address = this.getAddress('GetAll');
      options.responseData = new entities.contentAlias();
      options.onSuccess = (options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias>): void => {
        onSuccess(options.responseDataArray);
      }
      options.onError = (options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias>): void => {
        onError(options.exception);
      }
      this._get(options);
    }

    public getAllByContent(content: entities.content, onSuccess: (obj: Array<entities.primitiveType<string>>) => void, onError: (error: helpers.mdException) => void): void {
      let options: base.AjaxMethodOptions<contentAliasController, entities.primitiveType<string>> = new base.AjaxMethodOptions<contentAliasController, entities.primitiveType<string>>();
      options.includeAuthHeader = true;
      options.isJsonArray = true;
      options.address = this.getAddress('GetAllAliasesByContent');
      options.responseData = new entities.primitiveType<string>();
      options.requestData = content;
      options.onSuccess = (options: base.AjaxMethodOptions<contentAliasController, entities.primitiveType<string>>): void => {
        onSuccess(options.responseDataArray);
      }
      options.onError = (options: base.AjaxMethodOptions<contentAliasController, entities.primitiveType<string>>): void => {
        onError(options.exception);
      }
      this._post(options);
    }

    public del(id: number, onSuccess: (obj: entities.contentAlias) => void, onError: (error: helpers.mdException) => void): void {
      let options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias> = new base.AjaxMethodOptions<contentAliasController, entities.contentAlias>();
      options.includeAuthHeader = true;
      options.address = this.getAddress('Delete', [id]);
      options.responseData = new entities.contentAlias();
      options.onSuccess = (options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias>): void => {
        onSuccess(options.responseData);
      }
      options.onError = (options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias>): void => {
        onError(options.exception);
      }
      this._delete(options);
    }

    public save(contentAlias: entities.contentAlias, onSuccess: (obj: entities.contentAlias) => void, onError: (error: helpers.mdException) => void): void {
      let options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias> = new base.AjaxMethodOptions<contentAliasController, entities.contentAlias>();
      options.includeAuthHeader = true;
      options.address = this.getAddress('Save');
      options.responseData = new entities.contentAlias();
      options.requestData = contentAlias;
      options.onSuccess = (options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias>): void => {
        onSuccess(options.responseData);
      }
      options.onError = (options: base.AjaxMethodOptions<contentAliasController, entities.contentAlias>): void => {
        onError(options.exception);
      }
      this._post(options);
    }
  }
}
