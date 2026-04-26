/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/hardwareInfoPerformance.ts" />
/// <reference path="../entities/pluginJob.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class systemInfoController extends base.BaseController<systemInfoController, entities.hardwareInfoPerformance | entities.pluginJob | entities.models.initModel> {

        private static _isInitialized: boolean = false;
        private static _initCallInProgress: boolean = false;

        public static processPreInit(callback: any): void {
            function deferer(callback: any) {
                if (!systemInfoController._isInitialized) {
                    if (systemInfoController._initCallInProgress) {
                        setTimeout(function () {
                            deferer(callback);
                        }, 500);
                    } else {
                        systemInfoController._initCallInProgress = true;
                        (new systemInfoController()).getInit(function (data) {
                            systemInfoController._initCallInProgress = false;
                            systemInfoController._isInitialized = data.Initiated;
                            callback();
                        }, function (error) {
                            systemInfoController._initCallInProgress = false;
                            throw new helpers.mdException("Failed to initialize the API, please look at the server logs or contact your administrator!");
                        });
                    }
                } else {
                    callback();
                }
            }

            deferer(callback);
        }

        public static getIsInitialized(): boolean {
            return systemInfoController._isInitialized;
        }

        constructor() {
            super('SystemInfo/');
        }

        public getPerformance(requestId: string, delay: number, onSuccess: (obj: entities.hardwareInfoPerformance, socket: WebSocket) => void, onError: (error: helpers.mdException, socket: WebSocket) => void): void {
            delay = mdBusinessLogic.helpers.typeConversion.toInt(delay);

            let options: base.AjaxMethodOptions<systemInfoController, entities.hardwareInfoPerformance> = new base.AjaxMethodOptions<systemInfoController, entities.hardwareInfoPerformance>(requestId);
            options.includeAuthHeader = true;
            options.address = this.getAddress('Performance', [delay]);
            options.responseData = new entities.hardwareInfoPerformance();
            options.onSuccess = (options: base.AjaxMethodDataSocket<systemInfoController, entities.hardwareInfoPerformance>): void => {
                onSuccess(options.responseData, options.socket);
            }
            options.onError = (options: base.AjaxMethodDataSocket<systemInfoController, entities.hardwareInfoPerformance>): void => {
                onError(options.exception, options.socket);
            }
            this._socket(options);
        }

        public getPluginJobs(requestId: string, onSuccess: (obj: Array<entities.pluginJob>, socket: WebSocket) => void, onError: (error: helpers.mdException, socket: WebSocket) => void): string {
            let options: base.AjaxMethodOptions<systemInfoController, entities.pluginJob> = new base.AjaxMethodOptions<systemInfoController, entities.pluginJob>(requestId);
            options.includeAuthHeader = true;
            options.isJsonArray = true;
            options.address = this.getAddress('GetAllJobs');
            options.responseData = new entities.pluginJob();
            options.onSuccess = (options: base.AjaxMethodDataSocket<systemInfoController, entities.pluginJob>): void => {
                onSuccess(options.responseDataArray, options.socket);
            }
            options.onError = (options: base.AjaxMethodDataSocket<systemInfoController, entities.pluginJob>): void => {
                onError(options.exception, options.socket);
            }
            this._socket(options);
            return options.getRequestId();
        }

        public getInit(onSuccess: (obj: entities.models.initModel) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<systemInfoController, entities.models.initModel> = new base.AjaxMethodOptions<systemInfoController, entities.models.initModel>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('Init');
            options.responseData = new entities.models.initModel();
            options.isInitCall = true;
            options.onSuccess = (options: base.AjaxMethodDataSocket<systemInfoController, entities.models.initModel>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodDataSocket<systemInfoController, entities.models.initModel>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}
