/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/reportDefinition.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class reportDefinitionController extends base.BaseController<reportDefinitionController, entities.reportDefinition | entities.primitiveType<any> | entities.innerReportDefinitionEntity | entities.paginationEntity<entities.reportDefinition>> {

        constructor() {
            super('ReportDesigner/');
        }

        public getEntities(onSuccess: (obj: Array<entities.innerReportDefinitionEntity>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDefinitionController, entities.innerReportDefinitionEntity> = new base.AjaxMethodOptions<reportDefinitionController, entities.innerReportDefinitionEntity>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetEntities');
            options.responseData = new entities.innerReportDefinitionEntity();
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.innerReportDefinitionEntity>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.innerReportDefinitionEntity>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getReportPreview(data: any, onSuccess: (obj: entities.primitiveType<object>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDefinitionController, entities.primitiveType<object>> = new base.AjaxMethodOptions<reportDefinitionController, entities.primitiveType<object>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GenerateSampleReportdata');
            options.responseData = new entities.primitiveType<object>();
            options.requestData = data;
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.primitiveType<object>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.primitiveType<object>>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public save(reportDefinition: entities.reportDefinition, onSuccess: (obj: entities.reportDefinition) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition> = new base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('SaveDefinition');
            options.responseData = new entities.reportDefinition();
            options.requestData = reportDefinition;
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getReportColumns(reportDefinition: entities.reportDefinition, onSuccess: (obj: entities.reportDefinition) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition> = new base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllColumns');
            options.responseData = new entities.reportDefinition();
            options.requestData = reportDefinition;
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onError(options.exception);
            }
            this._post(options);
        }

        public getAll(sortData: any, onSuccess: (obj: Array<entities.reportDefinition>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition> = new base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>();
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAllDefinitions', [sortData.sort]);
            options.responseData = new entities.reportDefinition();
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getById(id: number, onSuccess: (obj: entities.reportDefinition) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition> = new base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetDefinitionById', [id]);
            options.responseData = new entities.reportDefinition();
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public search(searchData: any, onSuccess: (obj: Array<entities.reportDefinition>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition> = new base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Search', searchData);
            options.responseData = new entities.reportDefinition();
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onSuccess(options.responseDataArray);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllWithPagination(paginationData: any, onSuccess: (obj: entities.paginationEntity<entities.reportDefinition>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDefinitionController, entities.paginationEntity<entities.reportDefinition>> = new base.AjaxMethodOptions<reportDefinitionController, entities.paginationEntity<entities.reportDefinition>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllWithPagination', paginationData);
            options.responseData = new entities.paginationEntity<entities.reportDefinition>(entities.reportDefinition);
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.paginationEntity<entities.reportDefinition>>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.paginationEntity<entities.reportDefinition>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public getAllCount(countData: any, onSuccess: (obj: entities.primitiveType<number>) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<reportDefinitionController, entities.primitiveType<number>> = new base.AjaxMethodOptions<reportDefinitionController, entities.primitiveType<number>>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('GetAllCount', countData);
            options.responseData = new entities.primitiveType<number>();
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.primitiveType<number>>): void => {
                onSuccess(options.responseData.Value);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.primitiveType<number>>): void => {
                onError(options.exception);
            }
            this._get(options);
        }

        public del(id: number, onSuccess: (obj: entities.reportDefinition) => void, onError: (error: helpers.mdException) => void): void {
            id = mdBusinessLogic.helpers.typeConversion.toInt(id);

            let options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition> = new base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Delete', [id]);
            options.responseData = new entities.reportDefinition();
            options.onSuccess = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<reportDefinitionController, entities.reportDefinition>): void => {
                onError(options.exception);
            }
            this._delete(options);
        }
    }
}
