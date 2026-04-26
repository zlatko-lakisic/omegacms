/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/reportScheduler.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class reportSchedulerController extends base.BaseController<reportSchedulerController, entities.reportScheduler | entities.primitiveType<any> | entities.paginationEntity<entities.reportScheduler>> {

        constructor() {
            super('ReportScheduler/');
        }

        public getById(id: number, onSuccess: (obj: entities.reportScheduler) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler> = new base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetById', [id]);
            options.responseData = new entities.reportScheduler();
            options.onSuccess = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getByReportDefinitionId(id: number, onSuccess: (obj: Array<entities.reportScheduler>) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler> = new base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>();
            options.address = this.getAddress('GetByReportDefinitionId', [id]);
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.responseData = new entities.reportScheduler();
            options.onSuccess = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAll(searchTerm: string, onSuccess: (obj: Array<entities.reportScheduler>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler> = new base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAll', [searchTerm]);
            options.responseData = new entities.reportScheduler();
            options.onSuccess = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public save(reportScheduler: entities.reportScheduler, onSuccess: (obj: entities.reportScheduler) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler> = new base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Save');
            options.responseData = new entities.reportScheduler();
            options.requestData = reportScheduler;
            options.onSuccess = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public del(id: number, onSuccess: (obj: entities.reportScheduler) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler> = new base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.reportScheduler();
            options.onSuccess = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportSchedulerController, entities.reportScheduler>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }

        public getReportSchedulerActionTypes(onSuccess: (obj: Array<entities.primitiveType<String>>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportSchedulerController, entities.primitiveType<String>> = new base.AjaxMethodOptions<reportSchedulerController, entities.primitiveType<String>>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetReportSchedulerActionTypes');
            options.responseData = new entities.primitiveType<String>();
            options.onSuccess = (options: base.AjaxMethodOptions<reportSchedulerController, entities.primitiveType<String>>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<reportSchedulerController, entities.primitiveType<String>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllWithPagination(paginationData: any, onSuccess: (obj: entities.paginationEntity<entities.reportScheduler>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportSchedulerController, entities.paginationEntity<entities.reportScheduler>> = new base.AjaxMethodOptions<reportSchedulerController, entities.paginationEntity<entities.reportScheduler>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllWithPagination', paginationData);
            options.responseData = new entities.paginationEntity<entities.reportScheduler>(entities.reportScheduler);
            options.onSuccess = (options: base.AjaxMethodOptions<reportSchedulerController, entities.paginationEntity<entities.reportScheduler>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportSchedulerController, entities.paginationEntity<entities.reportScheduler>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportSchedulerController, entities.primitiveType<number>> = new base.AjaxMethodOptions<reportSchedulerController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<reportSchedulerController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<reportSchedulerController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}