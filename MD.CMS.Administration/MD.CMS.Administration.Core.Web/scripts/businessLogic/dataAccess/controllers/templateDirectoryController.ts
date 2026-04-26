/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/templateDirectory.ts" />
/// <reference path="../entities/template.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
  export class templateDirectoryController extends base.BaseController<templateDirectoryController, entities.templateDirectory> {

    constructor() {
      super('TemplateDirectory/');
    }

    public getTemplateDirectoryByPath(template: entities.template, onSuccess: (obj: entities.templateDirectory) => void, onError: (error: helpers.mdException) => void): void {
      let options: base.AjaxMethodOptions<templateDirectoryController, entities.templateDirectory> = new base.AjaxMethodOptions<templateDirectoryController, entities.templateDirectory>();
      options.includeAuthHeader = true;
      options.address = this.getAddress('GetTemplateDirectoryByPath');
      options.responseData = new entities.templateDirectory();
      options.requestData = template;
      options.onSuccess = (options: base.AjaxMethodOptions<templateDirectoryController, entities.templateDirectory>): void => {
        onSuccess(options.responseData);
      }
      options.onError = (options: base.AjaxMethodOptions<templateDirectoryController, entities.templateDirectory>): void => {
        onError(options.exception);
      }
      this._post(options);
    }
  }
}
