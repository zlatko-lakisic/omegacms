/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/reportDirectory.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class reportDirectoryController extends base.BaseController<reportDirectoryController, entities.reportDirectory> {

        constructor() {
            super('ReportDirectory/');
        }

        public getReportDirectoryByPath(path: string, onSuccess: (obj: entities.reportDirectory) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDirectoryController, entities.reportDirectory> = new base.AjaxMethodOptions<reportDirectoryController, entities.reportDirectory>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetReportDirectoryByPath');
            options.responseData = new entities.reportDirectory();
            options.requestData = { ValueName: path };
            options.onSuccess = (options: base.AjaxMethodOptions<reportDirectoryController, entities.reportDirectory>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDirectoryController, entities.reportDirectory>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}