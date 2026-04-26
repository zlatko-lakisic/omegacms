/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/profileTypeFieldValue.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
  export class profileTypeFieldValueController extends base.BaseController<profileTypeFieldValueController, entities.profileTypeFieldValue> {

    constructor() {
      super('ProfileTypeFieldValue/');
    }

      public getByUser(id: number, onSuccess: (obj: Array<entities.profileTypeFieldValue>) => void, onError: (error: helpers.mdException) => void): void {
          id = mdBusinessLogic.helpers.typeConversion.toInt(id);

      let options: base.AjaxMethodOptions<profileTypeFieldValueController, entities.profileTypeFieldValue> = new base.AjaxMethodOptions<profileTypeFieldValueController, entities.profileTypeFieldValue>();
      options.includeAuthHeader = true;
      options.isJsonArray = true;
      options.address = this.getAddress('GetByUser', [id]);
      options.responseData = new entities.profileTypeFieldValue();
      options.onSuccess = (options: base.AjaxMethodOptions<profileTypeFieldValueController, entities.profileTypeFieldValue>): void => {
        onSuccess(options.responseDataArray);
      }
      options.onError = (options: base.AjaxMethodOptions<profileTypeFieldValueController, entities.profileTypeFieldValue>): void => {
        onError(options.exception);
      }
      this._get(options);
    }

    public save(profileTypeFieldValue: entities.profileTypeFieldValue, onSuccess: (obj: entities.profileTypeFieldValue) => void, onError: (error: helpers.mdException) => void): void {
      let options: base.AjaxMethodOptions<profileTypeFieldValueController, entities.profileTypeFieldValue> = new base.AjaxMethodOptions<profileTypeFieldValueController, entities.profileTypeFieldValue>();
      options.includeAuthHeader = true;
      options.address = this.getAddress('Save');
      options.responseData = new entities.profileTypeFieldValue();
      options.requestData = profileTypeFieldValue;
      options.onSuccess = (options: base.AjaxMethodOptions<profileTypeFieldValueController, entities.profileTypeFieldValue>): void => {
        onSuccess(options.responseData);
      }
      options.onError = (options: base.AjaxMethodOptions<profileTypeFieldValueController, entities.profileTypeFieldValue>): void => {
        onError(options.exception);
      }
      this._post(options);
    }
  }
}
