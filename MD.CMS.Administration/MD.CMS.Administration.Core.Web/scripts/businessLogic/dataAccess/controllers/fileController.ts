/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/file.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class fileController extends base.BaseController<folderController, entities.primitiveType<any>> {

        constructor() {
            super('Upload/');
        }

        public upload(file: entities.file, onSuccess: (obj: any) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<folderController, entities.primitiveType<any>> = new base.AjaxMethodOptions<folderController, entities.primitiveType<any>>();
            options.includeAuthHeader = true;

            options.address = this.getAddress('PostFormData');

            var formData = new FormData();
            formData.append('file', file.data);
            formData.append('path', file.path);
            formData.append('fileType', file.fileType.toString());

            options.isFormData = true;
            options.requestData = formData;
            options.responseData = new entities.primitiveType<any>();
            options.onSuccess = (options: base.AjaxMethodOptions<folderController, entities.primitiveType<any>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<folderController, entities.primitiveType<any>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}
