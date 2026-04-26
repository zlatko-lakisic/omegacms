/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/attributeTypeDefinition.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
  export class attributeTypeDefinitionController extends base.BaseController<attributeTypeDefinitionController, entities.attributeTypeDefinition> {

    constructor() {
      super('AttributeTypeDefinition/');
      }

      public getById(id: number, onSuccess: (obj: entities.attributeTypeDefinition) => void, onError: (error: helpers.mdException) => void): void {
          let options: base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition> = new base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition>();
          options.includeAuthHeader = true;
          options.address = this.getAddress('GetById', [id]);
          options.responseData = new entities.attributeTypeDefinition();
          options.onSuccess = (options: base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition>): void => {
              onSuccess(options.responseData);
          }
          options.onError = (options: base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition>): void => {
              onError(options.exception);
          }
          this._get(options);
      }

      public getByInputTypeId(id: number, onSuccess: (obj: entities.attributeTypeDefinition) => void, onError: (error: helpers.mdException) => void): void {
          let options: base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition> = new base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition>();
          options.includeAuthHeader = true;
          options.address = this.getAddress('GetByInputTypeId', [id]);
          options.responseData = new entities.attributeTypeDefinition();
          options.onSuccess = (options: base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition>): void => {
              onSuccess(options.responseData);
          }
          options.onError = (options: base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition>): void => {
              onError(options.exception);
          }
          this._get(options);
      }

    public getAll(onSuccess: (obj: Array<entities.attributeTypeDefinition>) => void, onError: (error: helpers.mdException) => void): void {
      let options: base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition> = new base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition>();
      options.includeAuthHeader = true;
      options.isJsonArray = true;
      options.address = this.getAddress('GetAll');
      options.responseData = new entities.attributeTypeDefinition();
      options.onSuccess = (options: base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition>): void => {
        onSuccess(options.responseDataArray);
      }
      options.onError = (options: base.AjaxMethodOptions<attributeTypeDefinitionController, entities.attributeTypeDefinition>): void => {
        onError(options.exception);
      }
      this._get(options);
    }
  }
}
