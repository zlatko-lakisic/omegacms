/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/profileType.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
  export class profileController extends base.BaseController<profileController, entities.profileType> {

    constructor() {
      super('Profile/');
    }

    public assignProfileTypeToUser(assignData: any, onSuccess: (obj: entities.profileType) => void, onError: (error: helpers.mdException) => void): void {
      let options: base.AjaxMethodOptions<profileController, entities.profileType> = new base.AjaxMethodOptions<profileController, entities.profileType>();
      options.includeAuthHeader = true;
      options.address = this.getAddress('AssignProfileTypeToUser', assignData);
      options.responseData = new entities.profileType();
      options.onSuccess = (options: base.AjaxMethodOptions<profileController, entities.profileType>): void => {
        onSuccess(options.responseData);
      }
      options.onError = (options: base.AjaxMethodOptions<profileController, entities.profileType>): void => {
        onError(options.exception);
      }
      this._get(options);
    }
  }
}
