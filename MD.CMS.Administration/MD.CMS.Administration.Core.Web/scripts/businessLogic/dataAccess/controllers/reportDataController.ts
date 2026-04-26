/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/reportData.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class reportDataController extends base.BaseController<reportDataController, entities.reportData | entities.primitiveType<object>> {

        constructor() {
            super('ReportData/');
        }

        public getAll(onSuccess: (obj: Array<entities.reportData>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDataController, entities.reportData> = new base.AjaxMethodOptions<reportDataController, entities.reportData>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll');
            options.responseData = new entities.reportData();
            options.onSuccess = (options: base.AjaxMethodOptions<reportDataController, entities.reportData>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDataController, entities.reportData>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByReportSchedulerId(id: number, onSuccess: (obj: Array<entities.reportData>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<reportDataController, entities.reportData> = new base.AjaxMethodOptions<reportDataController, entities.reportData>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetByReportSchedulerId', [id]);
            options.responseData = new entities.reportData();
            options.onSuccess = (options: base.AjaxMethodOptions<reportDataController, entities.reportData>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDataController, entities.reportData>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(reportData: entities.reportData, onSuccess: (obj: entities.reportData) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDataController, entities.reportData> = new base.AjaxMethodOptions<reportDataController, entities.reportData>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetByReportSchedulerId');
            options.responseData = new entities.reportData();
            options.requestData = reportData;
            options.onSuccess = (options: base.AjaxMethodOptions<reportDataController, entities.reportData>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDataController, entities.reportData>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getReportData(data: any, onSuccess: (obj: entities.primitiveType<object>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDataController, entities.primitiveType<object>> = new base.AjaxMethodOptions<reportDataController, entities.primitiveType<object>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GenerateReportdata');
            options.responseData = new entities.primitiveType<object>();
            options.requestData = data;
            options.onSuccess = (options: base.AjaxMethodOptions<reportDataController, entities.primitiveType<object>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDataController, entities.primitiveType<object>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }
    }
}